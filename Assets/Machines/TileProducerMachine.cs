using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TileProducerMachine : MonoBehaviour
{
    [Header("Points & Storages")]
    [SerializeField] private ProductStorage productStorage;
    [SerializeField] private Transform tileStartPos;
    [SerializeField] private Transform tileFinishPos;

    [SerializeField] private ProducerFields _producerFields;

    [SerializeField] private Vector3 _tileScale;

    private ResourceTilesSpawn _tilesSpawner => InstantcesContainer.Instance.ResourceTilesSpawn;
    private WalletSystem _walletSystem =>
      InstantcesContainer.Instance.WalletSystem;

    private bool producing;

    public TileType typeProduced => _producerFields.ProductType;
    public float MachineSpeed => _producerFields.Speed;
    public int MaxTileCount => _producerFields.MaxTiles;
    public int currentTilesCount => productStorage.TilesInStorage.Count;


    private void Start()
    {
        StartProduce();
        productStorage.OnFreeSpaceInStorage += StartProduce;

    }

    private void StartProduce()
    {
        StartCoroutine(TileProduceProcess());
    }

    private IEnumerator TileProduceProcess()
    {
        producing = true;
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
        _walletSystem.Income(_producerFields.Income);


        producing = false;

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


}
