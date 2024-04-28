using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstAutoRepair : MonoBehaviour, ISaveLoad<int>
{
    [SerializeField] private AutoRepairData _data;

    [Space(5)]
    [SerializeField] private PlayerDetector _detectorForRes;
    [SerializeField] private ParticleSystem _buildVFX;
    [SerializeField] private Transform _oldCar;

    [Space(5)]
    [SerializeField] private ParticleSystem _destroyVFX;

    [SerializeField] private BuilderFromTiles _nextAutoRapirBuilder;
    [SerializeField] private float _delayMachineTakeTile;
    [SerializeField] private float _destroyedObjAppDelay;
    [SerializeField] private int _repairLevel;

    [Space(5)]
    [SerializeField] private CarSpawner _carSpawner;

    private List<TileType> _requiredTypes = new();
    private Dictionary<TileType, ProductRequierment> _productRequierments = new();
    private Dictionary<TileType, int> _tileCountByType = new();

    private bool _carRiding;

    private TileSetter _playerTilesBag => InstantcesContainer.Instance.TileSetter;
    private void Collect()
    {
        if (_playerTilesBag._isGivingTiles || _carRiding) return;

        StartCoroutine(CollectCor());

    }
    private IEnumerator CollectCor()
    {
        var reqtype = new List<TileType>(_requiredTypes);

        //if (reqtype.Count == 0)
        //     StartCoroutine(Repair());

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

    public event Action OnCarRepairByPlayer;

    private IEnumerator Repair()
    {
        if (_repairLevel < 3)
        {
            _tileCountByType = new();
            _carRiding = true;

            StopCoroutine(CollectCor());
            SubscribeForTilesDetect(false);

            _oldCar.DOScale(Vector3.zero, 0.4f);
            _buildVFX.Play();

            yield return new WaitForSeconds(0.5f);
            _carSpawner.Spawn(0);
            OnCarRepairByPlayer?.Invoke();

            yield return new WaitForSeconds(2f);
            _oldCar.DOScale(Vector3.one, 0.7f);
            _repairLevel++;
            GetNextTilesRequired();
            _carRiding = false;
        }
        else
        {
            _destroyVFX.Play();
            yield return new WaitForSeconds(_destroyedObjAppDelay);

            CreateNextAutoRepair();

        }
    }

    private void CreateNextAutoRepair()
    {
        if (_nextAutoRapirBuilder != null)
            _nextAutoRapirBuilder.gameObject.SetActive(true);

        gameObject.SetActive(false);
    }
    #region Init&SaveLoad
    private void OnEnable()
    {
        SetFromSave();
        GetNextTilesRequired();
        SubscribeForTilesDetect(true);
    }

    public int GetData()
    {
        return _repairLevel;
    }

    public void Initialize(int level)
    {
        if (level < 3)
        {
            _repairLevel = level;
            _nextAutoRapirBuilder.gameObject.SetActive(false);
        }
        else
        {
            if (_nextAutoRapirBuilder != null)
                _nextAutoRapirBuilder.gameObject.SetActive(true);

            gameObject.SetActive(false);
        }
        GetNextTilesRequired();

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
    private void OnDisable()
    {
        SubscribeForTilesDetect(false);
    }
    #endregion


    [SerializeField] private CounterView _counterUI;
    private void GetNextTilesRequired()
    {
        var data = _data.GetRequierments(_repairLevel);
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

            _counterUI.InitCounterValues(colcount.Type, _tileCountByType[colcount.Type], colcount.Amount);

            if (_tileCountByType[type] >= _productRequierments[type].Amount)
            {
                _requiredTypes.Remove(type);

            }
        }
        void ClearText()
        {
            _counterUI.InitCounterValues(TileType.Junk, 0, 0);
            _counterUI.InitCounterValues(TileType.Iron, 0, 0);
            _counterUI.InitCounterValues(TileType.Plastic, 0, 0);
            _counterUI.InitCounterValues(TileType.Rubber, 0, 0);
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
    
}


