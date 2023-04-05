
using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class AutoRepair : MonoBehaviour, IUpgradable
{
    [SerializeField] private AutoRepairData _data;

    [Space(5)]
    [Header("Points & Storages")]

    [SerializeField] private PlayerDetector _detectorForRes;

    [SerializeField] private float delayMachineTakeTile;
    [SerializeField] private int repairLevel;

    [SerializeField] private GameObject _car;

    private List<TileType> _requiredTypes = new();
    private Dictionary<TileType, ProductRequierment> _productRequierments = new();
    private Dictionary<TileType, int> _tileCountByType = new();

    private bool _carRiding;

    CancellationTokenSource cancelCollect = new CancellationTokenSource();

    [Zenject.Inject] private WalletSystem _walletSystem;
    [Zenject.Inject] private TileSetter _playerTilesBag;
    private void Collect()
    {
        if (_playerTilesBag._isGivingTiles || _carRiding) return;

        CollectCor(cancelCollect);

    }
    private async UniTask CollectCor(CancellationTokenSource cancellationToken)
    {
        var reqtype = new List<TileType>(_requiredTypes);
        for (int i = 0; i < reqtype.Count; i++)
        {
            var req = reqtype[i];
            var countneed = _productRequierments[req].Amount - _tileCountByType[req];

            print($"await{req}");
            await _playerTilesBag.RemoveTiles
                (req, countneed, _detectorForRes.transform.position, RecieveTile, cancellationToken, true);

        }
    }
    private IEnumerator Repair()
    {
        _carRiding = true;
        //StopCoroutine(CollectCor());
        //  cancelCollect.Cancel();

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
            _tileCountByType.Add(colcount.Type, 0);

            _counterUI.InitUI(colcount.Type, colcount.Amount);
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

public class CarRider : MonoBehaviour
{
    [SerializeField] private RiderData _data;
    private float _speed;
    private Animator _animator;

}

public class RiderData : ScriptableObject
{

}

