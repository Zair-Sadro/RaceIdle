
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderFromTiles : TileCollector, ITilesSave
{
    [Tooltip("Off")]
    public GameObject[] collidersAfterBuild, collidersBeforeBuild;
    [Tooltip("On")]
    public GameObject[] collidersAfterBuildOn, collidersBeforeBuildOn;

    public GameObject building;
    private IBuildable buildingContract;

    [SerializeField] private Transform _buildEffectPosition;
    [SerializeField] private byte _buildid;
    [SerializeField] protected bool forceToBuild = true;

    public BuildType buildType;
    public byte BuildID => _buildid;

    [SerializeField] protected PlayerDetector _playerDetector;
    protected BuildSaver _buildSaver => InstantcesContainer.Instance.BuildSaver;

    protected int minCountForCheck;
    protected Action OnEnoughForBuild;
    protected Dictionary<TileType, int> _tilesCountByType = new();

    [SerializeField] private AudioName _audioName = AudioName.BUILD;

    internal bool IsBuilt;

    private void Start()
    {

        if (forceToBuild)
        {
            StopCollect();
            BuildEffects(building);
            AfterBuildAction();
        }
        else
        {
            BeforeBuildAction();
            building.SetActive(false);
            InitLocalDictionary();

            for (int i = 0; i < _requiredTypesCount; i++)
            {
                var count = productRequierments[i].Amount;
                var type = productRequierments[i].Type;


                if (type == TileType.Gold)
                {
                    _counterView.InitCounerValues(type, _tilesCountByType[type], count);

                }
                else
                {
                    currentTilesCount += _tilesCountByType[type];
                    minCountForCheck += count;
                    _counterView.InitCounerValues(type, _tilesCountByType[type], count);
                }
            }


        }


    }
    protected void InitLocalDictionary()
    {
        _requiredTypesCount = (byte)productRequierments.Count;
        _requiredTypes = new List<TileType>();

        for (int i = 0; i < _requiredTypesCount; i++)
        {
            var type = productRequierments[i].Type;
            _requiredTypes.Add(type);

            if (type != TileType.Gold)
            {
                tileListByType.Add(type, new Stack<Tile>());
            }


        }
        for (int i = 0; i < _requiredTypesCount; i++)
        {
            var type = productRequierments[i].Type;

            if (!_tilesCountByType.ContainsKey(type))
                _tilesCountByType.Add(type, 0);



        }
    }


    protected virtual void AfterBuildAction()
    {
        if (collidersAfterBuild.Length > 0)
            for (int i = 0; i < collidersAfterBuild.Length; i++)
            {
                collidersAfterBuild[i].SetActive(false);

            }


        if (collidersAfterBuildOn.Length > 0)
            for (int i = 0; i < collidersAfterBuildOn.Length; i++)
            {

                collidersAfterBuildOn[i].SetActive(true);
            }

        OnBuild?.Invoke();

    }
    protected virtual void BeforeBuildAction()
    {
        if (collidersBeforeBuild.Length > 0)
            for (int i = 0; i < collidersBeforeBuild.Length; i++)
            {
                collidersBeforeBuild[i].SetActive(false);

            }

        if (collidersBeforeBuildOn.Length > 0)
            for (int i = 0; i < collidersBeforeBuildOn.Length; i++)
            {
                collidersBeforeBuildOn[i].SetActive(true);
            }
    }

    protected virtual void OnEnable()
    {
        _playerDetector.OnPlayerEnter += Collect;
        _playerDetector.OnPlayerExit += StopCollect;

        OnEnoughForBuild += Build;

        if (_counterView == null) return;
        OnCountChange += _counterView.ChangeCount;

    }
    protected virtual void OnDisable()
    {
        _playerDetector.OnPlayerEnter -= Collect;
        _playerDetector.OnPlayerExit -= StopCollect;

        OnCountChange -= _counterView.ChangeCount;
        OnEnoughForBuild -= Build;
    }
    protected override void Collect()
    {
        if (_playerTilesBag._isGivingTiles) return;

        _stopCollect = false;
        StartCoroutine(CollectCor());
    }
    private IEnumerator CollectCor()
    {
        var reqtype = new List<TileType>(_requiredTypes);
        for (int i = 0; i < reqtype.Count; i++)
        {
            if (_stopCollect)
                yield break;

            var req = reqtype[i];


            if (req == TileType.Gold)
            {
                var countneed = productRequierments[i].Amount - goldCount;

                if (countneed <= 0)
                    continue;

                yield return StartCoroutine(_playerTilesBag.RemoveGoldWthCount
                                           (countneed, tileStorage.position, RecieveGold));
            }
            else
            {
                var countneed = productRequierments[i].Amount - _tilesCountByType[req];

                if (countneed == 0)
                    continue;

                yield return StartCoroutine(_playerTilesBag.RemoveTilesWthCount
                                           (req, countneed, tileStorage.position, RecieveTile, true));
            }



        }
    }
    public event Action OnBuild;
    protected virtual void Build()
    {
        if (EnoughForBuild())
        {
            IsBuilt = true;
            StopCollect();
            BuildEffects(building);

            _buildSaver.GetBuildInfo(this);
            AfterBuildAction();
            OnBuild?.Invoke();
            InstantcesContainer.Instance.AudioService.PlayAudo(_audioName);
            GameEventSystem.NeedToSaveProgress.Invoke();
        }
    }
    public virtual void BuildBySaver()
    {
        forceToBuild = true;
        IsBuilt = true;
    }
    protected virtual void BuildEffects(GameObject b)
    {
        StartCoroutine(BuildCor(b));

    }
    IEnumerator BuildCor(GameObject b)
    {
        if (_buildEffectPosition)
            _buildSaver.BuildEffect(_buildEffectPosition.position);

        yield return new WaitForSeconds(0.2f);

        var normalscale = b.transform.localScale;
        b.transform.localScale = Vector3.zero;
        b.SetActive(true);
        buildingContract?.Build();
        b.transform.DOScale(normalscale, 0.4f);
        Destroy(this, 2f);

    }

    protected override void RecieveTile(Tile T)
    {
        base.RecieveTile(T);
        _tilesCountByType[T.Type]++;

        ++currentTilesCount;
        OnCountChange?.Invoke(T.Type, _tilesCountByType[T.Type]);

        if (currentTilesCount >= minCountForCheck)
            OnEnoughForBuild();
    }
    protected override void RecieveGold(int goldCount)
    {
        base.RecieveGold(goldCount);

        OnCountChange?.Invoke(TileType.Gold, this.goldCount);

        if (currentTilesCount >= minCountForCheck)
            OnEnoughForBuild();
    }

    protected bool EnoughForBuild()
    {
        for (int i = 0; i < _requiredTypesCount; i++)
        {
            if (productRequierments[i].Type == TileType.Gold)
                if (productRequierments[i].Amount > goldCount)
                {
                    return false;
                }
                else 
                {
                    continue;
                }


            if (!OneOfRequredTypeIsEnough(i))
                return false;


        }

        return true;

        bool OneOfRequredTypeIsEnough(int i)
        {
            var type = productRequierments[i].Type;
            int tileCount = 0;

            if (type != TileType.Gold)
            {
                if (!_tilesCountByType.TryGetValue(type, out tileCount))
                    return false;
            }


            var requiredAmount = productRequierments[i].Amount;

            if (tileCount >= requiredAmount)
                return true;

            return false;
        }

    }

    public void SetTiles(List<ProductRequierment> tilesList)
    {
        for (int i = 0; i < tilesList.Count; i++)
        {
            IFGold(tilesList[i]);

            var type = tilesList[i].Type;
            _tilesCountByType[type] = tilesList[i].Amount;
        }

        bool IFGold(ProductRequierment item)
        {
            if (item.Type == TileType.Gold)
            {
                goldCount = item.Amount;
                return true;
            }
            return false;
        }
    }

    public List<ProductRequierment> GetTiles()
    {
        List<ProductRequierment> list = new();

        for (int i = 0; i < _requiredTypesCount; i++)
        {
            if (IFGold(productRequierments[i]))
                continue;


            var type = productRequierments[i].Type;
            var count = _tilesCountByType[type];

            ProductRequierment item = new ProductRequierment(type, count);
            list.Add(item);

        }
        return list;

        bool IFGold(ProductRequierment item)
        {
            if (item.Type == TileType.Gold)
            {
                ProductRequierment goldReq = new(TileType.Gold, goldCount);
                list.Add(goldReq);

                return true;
            }
            return false;
        }
    }
}
