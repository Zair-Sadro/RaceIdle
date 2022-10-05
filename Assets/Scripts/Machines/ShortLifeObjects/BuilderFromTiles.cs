using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderFromTiles : TileCollector
{


    public GameObject[] collidersAfterBuild,collidersBeforeBuild;
    public GameObject building;

    [SerializeField] private PlayerDetector _playerDetector;

    private int minCountForCheck;

    private void Start()
    {
        if (building.activeInHierarchy) 
            AfterBuildAction();
        else
            for (int i = 0; i < collidersBeforeBuild.Length; i++)
            {
                collidersBeforeBuild[i].SetActive(true);
            }

     
        InitDictionary();

        for (int i = 0; i < _requiredTypesCount; i++)
        {
            minCountForCheck += productRequierments[i].Amount;
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

    private void Build()
    {
        if(EnoughForBuild())
        {
            StopCollect();
            BuildAndEffect(building);
            AfterBuildAction();
        }
    }
    private void BuildAndEffect(GameObject b)
    {
        b.SetActive(true);
    }

    protected override void RecieveTile(Tile T)
    {
        base.RecieveTile(T);
        ++currentTilesCount;
        OnCountChange?.Invoke(_requiredTypesCount);
       if (currentTilesCount >= minCountForCheck)
        Build();
    }
    private bool EnoughForBuild()
    {
        for (int i = 0; i < _requiredTypesCount; i++)
        {
            if (!OneOfRequredTypeIsEnough(i))
                return false;

        }

        return true;

        bool OneOfRequredTypeIsEnough(int i)
        {
            var type = productRequierments[i].Type;

            if (!tileListByType.TryGetValue(type, out Stack<Tile> tileStack))  //Check if stack exist
                return false;

            var requiredAmount = productRequierments[i].Amount;

            if (tileStack.Count >= requiredAmount)
                return true;

            return false;
        }

    }

}
