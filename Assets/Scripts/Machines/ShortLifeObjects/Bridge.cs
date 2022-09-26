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

    public GameObject colliderForCollect;
    public GameObject halfBridge;
    public GameObject fullBridge;

    private int fullBridgeCount;
    private void Start()
    {
        fullBridgeCount = halfBridgeCount * 2;
    }
    [Zenject.Inject] private TileSetter _playerTilesBag;


    private void OnEnable()
    {
        
    }
   
    public override void Collect()
    {
        _playerTilesBag.RemoveTiles( reqType, tilePos.position, TilePlus);
       
    }
    private void StopCollect()
    {
        _playerTilesBag.StopRemovingTiles();
    }

    public override void Remove()
    {
        base.Remove();
    }

    private void TilePlus()
    {
        ++currentTilesCount;
        BuildBridge();
    }
    private void BuildBridge()
    {
        if (fullBridge.activeInHierarchy) return;

        if (currentTilesCount >= halfBridgeCount && !halfBridge.activeInHierarchy)
        {
            BuildAndEffect(halfBridge);
            return;
        }

        if(currentTilesCount >= fullBridgeCount)
        {
            BuildAndEffect(fullBridge);
        }
    }
    private void BuildAndEffect(GameObject b)
    {
        b.SetActive(true);
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
