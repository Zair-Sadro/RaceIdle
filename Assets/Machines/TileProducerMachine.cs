using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class TileProducerMachine : MonoBehaviour
{
    [Header("Points & Storages")]
    [SerializeField] private ProductStorage productStorage;
    [SerializeField] private Transform tileStartPos;
    [SerializeField] private Transform tileFinishPos;

    [SerializeField] private ProducerFields _producerFields;

    [SerializeField] private int maxTileCount { get; set; }
    [SerializeField] private int currentTilesCount { get; set; }

    [SerializeField] private Vector3 _tileScale;


    protected TileSetter _playerTilesBag => InstantcesContainer.Instance.TileSetter;

    private ResourceTilesSpawn _tilesSpawner => InstantcesContainer.Instance.ResourceTilesSpawn;
    private WalletSystem _walletSystem => InstantcesContainer.Instance.WalletSystem;


    private Action<int, int> OnCountChange;
    private bool producing;

    public TileType typeProduced => _producerFields.ProductType;
    public float machineSpeed => _producerFields.Speed;



    private IEnumerator TileManufacture()
    {
        producing = true;
        var tile = _tilesSpawner.GetTile(typeProduced);
        tile.OnTake();

        yield return StartCoroutine(tile.AppearFromZero(_tileScale, tileStartPos.position, machineSpeed * 0.35f));

        yield return tile.transform.DOMove(tileFinishPos.position, machineSpeed * 0.65f)
                                   .SetEase(Ease.InOutFlash)
                                   .WaitForCompletion();

        yield return tile.transform.DOJump(productStorage.transform.position, 2, 1, 0.5f)
                                   .WaitForCompletion();

        //PuffEffect();
        productStorage.TileToStack(tile);
        _walletSystem.Income(_producerFields.Income);


        producing = false;

       StartCoroutine(TileManufacture());
    }


}
