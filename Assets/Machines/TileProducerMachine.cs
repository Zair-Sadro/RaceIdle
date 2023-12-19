using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class TileProducerMachine : MonoBehaviour, IBuildable
{
    [Header("Points & Storages")]
    [SerializeField] private ProductStorage productStorage;
    [SerializeField] private Transform tileStartPos;
    [SerializeField] private Transform tileFinishPos;
    [SerializeField] private AutoLayout3D.GridLayoutGroup3D _layoutGroupProduct;

    [SerializeField] private ProducerFields _producerFields;
    public ProducerFields ProducerFields => _producerFields;

    [SerializeField] private Vector3 _tileScale;

    public event Action<TileType> TypeInvented;

    private ResourceTilesSpawn _tilesSpawner => InstantcesContainer.Instance.ResourceTilesSpawn;
    private WalletSystem _walletSystem =>
      InstantcesContainer.Instance.WalletSystem;

    public TileType typeProduced => _producerFields.ProductType;
    public float MachineSpeed => _producerFields.Speed;
    public int MaxTileCount => _producerFields.MaxTiles;
    public int currentTilesCount => productStorage.TilesInStorage.Count;


    IEnumerator StartProduceCheck()
    {
        yield return new WaitForSeconds(1f);
        StartProduce();
        productStorage.OnFreeSpaceInStorage += StartProduce;

    }
    private void OnEnable()
    {
        StartCoroutine(StartProduceCheck());
    }
    private void OnDisable()
    {
        productStorage.OnFreeSpaceInStorage -= StartProduce;
        StopAllCoroutines();
    }
    private void StartProduce()
    {
        StartCoroutine(TileProduceProcess());
    }

    private IEnumerator TileProduceProcess()
    {
        var tile = _tilesSpawner.GetTile(typeProduced);
        tile.OnTake();

        yield return StartCoroutine(tile.AppearFromZero(_tileScale, tileStartPos.position, MachineSpeed * 0.35f));

        yield return tile.transform.DOMove(tileFinishPos.position, MachineSpeed * 0.65f)
                                   .SetEase(Ease.InOutFlash)
                                   .WaitForCompletion();

        yield return tile.transform.DOJump(productStorage.transform.position, 2, 1, 0.5f)
                                   .WaitForCompletion();

        //PuffEffect();
        productStorage.TileToStorage(tile);
        _layoutGroupProduct.UpdateLayout();
        _walletSystem.Income(_producerFields.Income);

        yield return StartCoroutine(CheckCountAndProduce());
        
    }
    private IEnumerator CheckCountAndProduce()
    {
        if (productStorage.IsFreeForNextTiles(MaxTileCount) == false)
        {
            yield break;
        }

        StartCoroutine(TileProduceProcess());
    }

    public void Build()
    {
        TypeInvented?.Invoke(_producerFields.ProductType);
    }
}
