using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductStorage : MonoBehaviour
{
    private Stack<Tile> _tilesInStorage = new Stack<Tile>();
    private TileSetter _tileSetter => InstantcesContainer.Instance.TileSetter;

    public IReadOnlyCollection<Tile> TilesInStorage => _tilesInStorage;

    public event Action OnFreeSpaceInStorage;
    
    public void TileToStorage(Tile t)
    {
        _tilesInStorage.Push(t);
        t.transform.parent = transform;
        t.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public bool IsFreeForNextTiles(int maximum) 
    {
        if (_tilesInStorage.Count >= maximum)
        {
            return false;
        }
        else 
        {
            return true;
        }

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
        if(!_tileSetter.MaxCapacity)
        StartCoroutine(ThrowingTileCor());
    }
    private void StopThrow()
    {
        StopAllCoroutines();
    }
    private IEnumerator ThrowingTileCor()
    {
        _tileSetter.TryAddTile(_tilesInStorage.Pop());
        InstantcesContainer.Instance.AudioService.PlayAudo(AudioName.TILE);
        yield return new WaitForSeconds(StaticValues.tileThrowDelay);

    }
    
}
