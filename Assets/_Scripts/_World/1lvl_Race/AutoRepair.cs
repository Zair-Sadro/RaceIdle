using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class AutoRepair :MonoBehaviour, IUpgradable
{
    [SerializeField] private AutoRepairData _data;

    [Space(5)]
    [Header("Points & Storages")]

    [SerializeField] private PlayerDetector _detectorForRes;

    [SerializeField] private float delayMachineTakeTile;
    [SerializeField] private int repairLevel;

    [SerializeField] private GameObject _car;

    private List<TileType> _requiredTypes=new();
    private Dictionary<TileType,ProductRequierment> _productRequierments=new();
    private Dictionary<TileType,int> _tileCountByType=new();

    private bool _carRiding;

    

    [Zenject.Inject] private WalletSystem _walletSystem;
    [Zenject.Inject] private TileSetter _playerTilesBag;
    private void Collect()
    {
        if (_playerTilesBag._isGivingTiles|| _carRiding) return;

        StartCoroutine(CollectCor());

    }
    private IEnumerator CollectCor()
    {

        for (int i = 0; i < _requiredTypes.Count; i++)
        {
            var req = _requiredTypes[i];
            Debug.Log($"{req.ToString()}collecting");
            _playerTilesBag.RemoveTiles(req, _detectorForRes.transform.position, RecieveTile, true);
            yield return new WaitForSeconds(delayMachineTakeTile);
        }
    }
    private IEnumerator Repair() 
    {
        _carRiding = true;
        CancelInvoke(nameof(Collect));
        StopCoroutine(CollectCor());

        _detectorForRes.OnPlayerEnter -= Collect;
        _detectorForRes.OnPlayerExit -= StopCollect;

        _car.SetActive(true);
        yield return new WaitForSeconds(2f);

        GetRequired();
        _carRiding = false;
        

    }
    #region Init&SaveLoad

    private void OnEnable()
    {
        GetRequired();

        _detectorForRes.OnPlayerEnter += Collect;
        _detectorForRes.OnPlayerExit += StopCollect;


    }
    private void OnDisable()
    {
        _detectorForRes.OnPlayerEnter -= Collect;
        _detectorForRes.OnPlayerExit -= StopCollect;

    }
    #endregion
    private void RecieveTile(Tile tile)
    {
        var type = tile.Type;
        var _ = ++_tileCountByType[type];

        _counterUI.InfoToUI(type, _);

        if (_tileCountByType[type] >= _productRequierments[type].Amount)
        {
            StopCollect();

            _requiredTypes.Remove(type);

            if (_requiredTypes.Count < 1) 
                StartCoroutine(Repair());
        }
        

    }

    [SerializeField] private MultuCounterView _counterUI;
    private void GetRequired()
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
            _tileCountByType.Add(colcount.Type,0);

            _counterUI.InitUI(colcount.Type,colcount.Amount);
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

public class MultuCounterView : MonoBehaviour
{
    [SerializeField] private CounterView _goldView;
    [SerializeField] private CounterView _junkView;
    [SerializeField] private CounterView _ironView;
    [SerializeField] private CounterView _plasticView;
    [SerializeField] private CounterView _rubberView;

    public void InfoToUI(TileType type,int count)
    {
        switch (type)
        {
            case TileType.Junk:
                _junkView.TextCountVisual(count);
                break;
            case TileType.Iron:
                _ironView.TextCountVisual(count);
                break;
            case TileType.Rubber:
                _rubberView.TextCountVisual(count);
                break;
            case TileType.Plastic:
                _plasticView.TextCountVisual(count);
                break;
        }
    }
    public void InitUI(TileType type,int max)
    {
        switch (type)
        {
            case TileType.Junk:
                _junkView.InitText(max);
                break;
            case TileType.Iron:
                _ironView.InitText(max);
                break;
            case TileType.Rubber:
                _rubberView.InitText(max);
                break;
            case TileType.Plastic:
                _plasticView.InitText(max);
                break;
        }
    }
    public void InitGoldUI(int max)
    {
        _goldView.InitText(max);
    }
    public void InfoGoldUI(int gold)
    {
        _goldView.TextCountVisual(gold);
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

