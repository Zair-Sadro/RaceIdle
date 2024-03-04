using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRepair : MonoBehaviour, ITilesSave,ISaveLoad<int>
{
    [SerializeField] private AutoRepairData _data;
    [Space(5)]
    [SerializeField] private PlayerDetector _detectorForRes;
    [Space(5)]
    [SerializeField] private float delayMachineTakeTile;
    [SerializeField] private int repairLevel;
    [Space(5)]
    [SerializeField] private CarSpawner _carSpawner;
    [SerializeField] private GameObject _needToMergeSign;
    [SerializeField] private ParticleSystem _buildVFX;
    [SerializeField] private Transform _oldCar;

    [SerializeField] private RaceTrackManager _raceTrackMan;

    private List<TileType> _requiredTypes = new();
    private Dictionary<TileType, ProductRequierment> _productRequierments = new();
    private Dictionary<TileType, int> _tileCountByType = new();

    private bool _carRiding;
    
    private TileSetter _playerTilesBag => InstantcesContainer.Instance.TileSetter;
    private void Collect()
    {
        if (_playerTilesBag._isGivingTiles || _carRiding|| _needToMergeSign.activeInHierarchy) return;

        StartCoroutine(CollectCor());

    }
    private IEnumerator CollectCor()
    {
        var reqtype = new List<TileType>(_requiredTypes);

        if (reqtype.Count == 0)
            StartCoroutine(Repair());

        for (int i = 0; i < reqtype.Count; i++)
        {
            if (_playerTilesBag._isGivingTiles)
                yield break;

            var req = reqtype[i];
            var countneed = _productRequierments[req].Amount - _tileCountByType[req];

            if (countneed == 0)
                continue;

            yield return StartCoroutine(_playerTilesBag.RemoveTilesWthCount
                (req, countneed, _detectorForRes.transform.position, RecieveTile, true));

        }
    }
    private void RecieveTile(Tile tile)
    {
        var type = tile.Type;
        var _ = ++_tileCountByType[type];

        _counterUI.ChangeCount(type, _);

        if (_tileCountByType[type] >= _productRequierments[type].Amount)
        {
            _requiredTypes.Remove(type);

            if (_requiredTypes.Count < 1)
                StartCoroutine(Repair());
        }


    }
    public IEnumerator RepairByBonus() 
    {
        _carRiding = true;

        StopCoroutine(CollectCor());
        SubscribeForTilesDetect(false);

        _oldCar.DOScale(Vector3.zero, 0.4f);
        _buildVFX.Play();

        yield return new WaitForSeconds(0.5f);
        _carSpawner.Spawn(1);

        yield return new WaitForSeconds(2f);
        _oldCar.DOScale(Vector3.one, 0.7f);
        _carRiding = false;
        SubscribeForTilesDetect(true);
    }

    public event Action OnCarRepairByPlayer;

    private IEnumerator Repair()
    {
        _tileCountByType = new();
        _carRiding = true;

        StopCoroutine(CollectCor());
        SubscribeForTilesDetect(false);

        _oldCar.DOScale(Vector3.zero,0.4f);
        _buildVFX.Play();

        yield return new WaitForSeconds(0.5f);
        _carSpawner.Spawn(0);
        OnCarRepairByPlayer?.Invoke();

        yield return new WaitForSeconds(2f);
        _oldCar.DOScale(Vector3.one, 0.7f);
        repairLevel++;
        GetNextTilesRequired();
        _carRiding = false;


    }
    #region Init&SaveLoad
    private void OnEnable()
    {
        SetFromSave();
        GetNextTilesRequired();
        SubscribeForTilesDetect(true);
        _raceTrackMan.NoSpaceOnTrack += CarsCountCheck;
    }

    public int GetData()
    {
        return repairLevel;
    }

    public void Initialize(int level)
    {
        repairLevel = level;
    }
    public void SetTiles(List<ProductRequierment> tilesList)
    {
        saveData = tilesList;
        GetNextTilesRequired();
        if (_tileCountByType.Count > 0)
        {
            SetFromSave();

            var reqcopy = _requiredTypes.ToArray();
            foreach (var req in reqcopy)
            {
                if (_tileCountByType[req] >= _productRequierments[req].Amount)
                {
                    _requiredTypes.Remove(req);

                }
            }

        }

    }
    private void SetFromSave()
    {
        if (saveData == null)
            return;
        if (saveData.Count == 0)
            return;

        _tileCountByType = new();

        for (int i = 0; i < saveData.Count; i++)
        {

            var type = saveData[i].Type;
            _tileCountByType[type] = saveData[i].Amount;
        }
        foreach (var item in _tileCountByType.Keys)
        {
            _counterUI.ChangeCount(item, _tileCountByType[item]);
        }
     

    }

    private void CarsCountCheck(bool noSpace)
    {
        _needToMergeSign.SetActive(noSpace);
        SubscribeForTilesDetect(!noSpace);
    }

    private void OnDisable()
    {
        SubscribeForTilesDetect(false);
        _raceTrackMan.NoSpaceOnTrack -= CarsCountCheck;
    }
    #endregion


    [SerializeField] private CounterView _counterUI;
    private void GetNextTilesRequired()
    {
        var data = _data.GetRequierments(repairLevel);
        var lenght = data.RequiermentsList.Count;

        _requiredTypes = new List<TileType>();
        _productRequierments = new();

        ClearText();

        for (int i = 0; i < lenght; i++)
        {
            var colcount = (data.RequiermentsList[i]);
            var type = colcount.Type;
            _productRequierments.Add(colcount.Type, colcount);
            _requiredTypes.Add(colcount.Type);

            if (!_tileCountByType.ContainsKey(colcount.Type))
                _tileCountByType.Add(colcount.Type, 0);

            _counterUI.InitCounerValues(colcount.Type, _tileCountByType[colcount.Type], colcount.Amount);

            if (_tileCountByType[type] >= _productRequierments[type].Amount)
            {
                _requiredTypes.Remove(type);

            }
        }
        void ClearText()
        {
            _counterUI.InitCounerValues(TileType.Junk, 0, 0);
            _counterUI.InitCounerValues(TileType.Iron, 0, 0);
            _counterUI.InitCounerValues(TileType.Plastic, 0, 0);
            _counterUI.InitCounerValues(TileType.Rubber, 0, 0);
        }
        SubscribeForTilesDetect(true);

    }
    private void SubscribeForTilesDetect(bool value)
    {
        if (value)
        {
            _detectorForRes.OnPlayerEnter += Collect;
            _detectorForRes.OnPlayerExit += StopCollect;
        }
        else
        {
            _detectorForRes.OnPlayerEnter -= Collect;
            _detectorForRes.OnPlayerExit -= StopCollect;
        }

    }
    private void StopCollect()
    {
        _playerTilesBag._isGivingTiles = false;
    }

    private List<ProductRequierment> saveData;


    public List<ProductRequierment> GetTiles()
    {
        List<ProductRequierment> list = new();

        foreach (var tiletype in _tileCountByType.Keys)
        {
            var count = _tileCountByType[tiletype];

            ProductRequierment item = new ProductRequierment(tiletype, count);
            list.Add(item);
        }



        return list;

    }


}


