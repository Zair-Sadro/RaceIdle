using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : TileCollector,IProduce
{
    public int tilesNeeded { get; set; }

    public int fullBridgeCount;
    public TileType reqType;
    public int productMaxCount { get; set; }
    public Transform tilePos;

    public GameObject[] collidersAfterBuild,collidersBeforeBuild;
    public GameObject fullBridge;

    [Zenject.Inject] private TileSetter _playerTilesBag;

    private void Start()
    {
        if (fullBridge.activeInHierarchy) 
            AfterBuildAction();
        else
            for (int i = 0; i < collidersBeforeBuild.Length; i++)
            {
                collidersBeforeBuild[i].SetActive(true);
            }
    }
    public override void Collect()
    {
        _playerTilesBag.RemoveTiles(reqType, tilePos.position, TilePlus);
       
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
        OnCountChange?.Invoke(fullBridgeCount);
        BuildBridge();

    }
    private void BuildBridge()
    {


        if(currentTilesCount >= fullBridgeCount)
        {
            BuildAndEffect(fullBridge);
            AfterBuildAction();
        }
    }
    private void BuildAndEffect(GameObject b)
    {
        b.SetActive(true);
    }
    private void AfterBuildAction()
    {
        for (int i = 0; i < collidersAfterBuild.Length; i++)
        {
            collidersAfterBuild[i].SetActive(false);
        }
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
