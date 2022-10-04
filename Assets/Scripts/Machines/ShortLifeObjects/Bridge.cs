using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : TileCollector
{

    public int fullBridgeCount;
    public TileType reqType;

    public Transform tilePos;

    public GameObject[] collidersAfterBuild,collidersBeforeBuild;
    public GameObject fullBridge;

    [Zenject.Inject] private TileSetter _playerTilesBag;
    [SerializeField] private PlayerDetector _playerDetector;


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

    private void AfterBuildAction()
    {
        for (int i = 0; i < collidersAfterBuild.Length; i++)
        {
            collidersAfterBuild[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        _playerDetector.OnPlayerEnter += Collect;
        _playerDetector.OnPlayerExit += StopCollect;

        if (counter == null) return;
        OnCountChange += TextCountVisual;

    }
    private void OnDisable()
    {
        _playerDetector.OnPlayerEnter -= Collect;
        _playerDetector.OnPlayerExit -= StopCollect;
        OnCountChange -= TextCountVisual;
    }

    public override void Collect()
    {
         _playerTilesBag.RemoveTiles(reqType, tilePos.position, TilePlus,true);
       
    }
    private void StopCollect()
    {
        _playerTilesBag.StopRemovingTiles();
    }

    private void TilePlus(Tile t)
    {
        ++currentTilesCount;
        OnCountChange?.Invoke(fullBridgeCount);
        BuildBridge();

    }
    private void BuildBridge()
    {
        if(currentTilesCount >= fullBridgeCount)
        {
            StopCollect();
            BuildAndEffect(fullBridge);
            AfterBuildAction();
        }
    }
    private void BuildAndEffect(GameObject b)
    {
        b.SetActive(true);
    }

    public override void Remove()
    {
        base.Remove();
    }

}
