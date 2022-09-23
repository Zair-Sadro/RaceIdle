using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : TileCollector,IProduce
{
    public int tilesNeeded { get; set; }
    public int halfBridgeCount;
    public TileType reqType;
    public int productMaxCount { get; set; }
    public Transform tilePos;

    [Zenject.Inject] private TileSetter _playerTilesBag;



    public override void Collect()
    {
        _playerTilesBag.RemoveTiles( reqType, tilePos.position);
    }
    private void StopCollect()
    {
        _playerTilesBag.StopRemovingTiles();
    }

    public override void Remove()
    {
        base.Remove();
    }




    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            Collect();
        }
    }
    private void OnTriggerStay(Collider other)
    {
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            StopCollect();
        }
    }

    


    public void Produce(Action action)
    {
        action.Invoke();
    }

    bool IsPlayer(GameObject ob)
    {
        if (ob.CompareTag("Player"))
            return true;
        return false;
    }
}
