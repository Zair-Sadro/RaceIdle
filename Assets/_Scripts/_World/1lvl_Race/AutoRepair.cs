using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRepair : MonoBehaviour, IUpgradable
{
    [SerializeField] private AutoRepairData _data;

    [Space(5)]
    [Header("Points & Storages")]

    [SerializeField] private PlayerDetector _detectorForRes;

    [SerializeField] private float delayMachineTakeTile;
    [SerializeField] private int repairLevel;

    [SerializeField] private CarSpawner _carSpawner;
    [SerializeField] private GameObject _needToMergeSign;

    [SerializeField] private RaceTrackManager _raceTrackMan;

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
        for (int i = 0; i < reqtype.Count; i++)
        {
            if (!_playerTilesBag._isGivingTiles)
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
    private IEnumerator Repair()
    {
        _carRiding = true;

        StopCoroutine(CollectCor());
        SubscribeForTilesDetect(false);
        _carSpawner.Spawn(0);

        yield return new WaitForSeconds(2f);

        GetNextTilesRequired();
        _carRiding = false;


    }
    #region Init&SaveLoad

    private void OnEnable()
    {
        GetNextTilesRequired();
        SubscribeForTilesDetect(true);
        _raceTrackMan.NoSpaceOnTrack += CarsCountCheck;
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
        _tileCountByType = new();

        for (int i = 0; i < lenght; i++)
        {
            var colcount = (data.RequiermentsList[i]);
            _productRequierments.Add(colcount.Type, colcount);
            _requiredTypes.Add(colcount.Type);
            _tileCountByType.Add(colcount.Type, 0);

            _counterUI.InitCounerValues(colcount.Type,0, colcount.Amount);
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

    public void UpgradeSpeedCapacity(int level)
    {

    }

    public void UpgradeIncome(int level)
    {

    }
}


