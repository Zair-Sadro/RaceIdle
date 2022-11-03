using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductStorage : MonoBehaviour
{
    private Stack<Tile> _tilesInStorage = new Stack<Tile>();
    [Zenject.Inject] private TileSetter _tileSetter;
    public IReadOnlyCollection<Tile> TilesInStorage => _tilesInStorage;


    public void TileToStack(Tile t)
    {
        _tilesInStorage.Push(t);
        t.transform.parent = transform;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (PlayerDetector.IsPlayer(other.gameObject) && _tilesInStorage.Count>=1)
        {
            ThrowTilesToPlayer();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (PlayerDetector.IsPlayer(other.gameObject) && _tilesInStorage.Count >= 1)
        {
            ThrowTilesToPlayer();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (PlayerDetector.IsPlayer(other.gameObject))
        {
            StopThrow();
        }
    }
     private void ThrowTilesToPlayer()
     {
        StartCoroutine(ThrowingTileCor());
     }
    private void StopThrow()
    {
        StopAllCoroutines();
    }
    private IEnumerator ThrowingTileCor()
    {

        _tileSetter.AddTile(_tilesInStorage.Pop());
        yield return new WaitForSeconds(StaticValues.tileThrowDelay);
    }

}
