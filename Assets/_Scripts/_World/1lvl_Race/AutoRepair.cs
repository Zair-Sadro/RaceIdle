using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoRepair : TileCollector
{
    [SerializeField] private AutoRepairData _data;

    [Space(5)]
    [Header("Points & Storages")]
    [SerializeField] private Transform carStartPos;
    [SerializeField] private Transform carFinishPos;

    [Zenject.Inject] private WalletSystem _walletSystem;
    [SerializeField] private PlayerDetector _detectorForRes;

    [SerializeField] private float delayMachineTakeTile;
    [SerializeField] private int repairLevel;

    [SerializeField] private GameObject _car;

    private Dictionary<TileType,ProductRequierment> _productRequierments=new();


    private bool _carRiding;
    protected override void Collect()
    {
        if (_playerTilesBag._isGivingTiles|| _carRiding) return;


        for (int i = 0; i < _requiredTypesCount; i++)
        {
            var req = _requiredTypes[i];
            _playerTilesBag.RemoveTiles(req, tileStorage.position, RecieveTile, hideAfterCollect);
        }

    }
    private IEnumerator Repair() 
    {
        _carRiding = true;
        _detectorForRes.OnPlayerEnter -= Collect;
        _detectorForRes.OnPlayerExit -= StopCollect;
        _car.SetActive(true);
        yield return new WaitForSeconds(2f);
        _carRiding = false;

    }
    #region Init&SaveLoad
   
    protected virtual void Init()
    {

        GetRequired();

    }

    private void OnEnable()
    {
        Init();
        hideAfterCollect = true;

        _detectorForRes.OnPlayerEnter += Collect;
        _detectorForRes.OnPlayerExit += StopCollect;


    }
    private void OnDisable()
    {
        _detectorForRes.OnPlayerEnter -= Collect;
        _detectorForRes.OnPlayerExit -= StopCollect;

    }
    #endregion
    protected override void RecieveTile(Tile tile)
    {
        var type = tile.Type;
        tileListByType[type].Push(tile);
        if (tileListByType[type].Count >= _productRequierments[type].Amount)
        {
            StopCollect();
            tileListByType.Remove(type);

            if (tileListByType.Count < 1) Repair();
        }
        

    }

    private void GetRequired()
    {
        var data = _data.GetRequierments(repairLevel);
        var lenght = data.RequiermentsList.Count;


        for (int i = 0; i < lenght; i++)
        {
            var colcount = (data.RequiermentsList[i]);
            _productRequierments.Add(colcount.Type, colcount);
        }
    }


}

public class CarRider: MonoBehaviour
{
    [SerializeField] private RiderData _data;
    private float _speed;
    private Animator _animator;

}

public class RiderData :ScriptableObject
{

}

