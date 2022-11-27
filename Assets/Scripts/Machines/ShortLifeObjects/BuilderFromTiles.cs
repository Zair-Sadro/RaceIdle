using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderFromTiles : TileCollector
{


    public GameObject[] collidersAfterBuild,collidersBeforeBuild;
    public GameObject building;

    [SerializeField] protected PlayerDetector _playerDetector;

    protected int minCountForCheck;
    protected Action OnEnoughForBuild;
    private void Start()
    {

        if (building.activeInHierarchy)
        {
            StopCollect();
            BuildAndEffect(building);
            AfterBuildAction();
        }
        else
            BeforeBuildAction();

     
        InitDictionary();

        for (int i = 0; i < _requiredTypesCount; i++)
        {
            minCountForCheck += productRequierments[i].Amount;
        }

        _counterView.InitText(minCountForCheck);
    }

    protected virtual void AfterBuildAction()
    {
        if (collidersAfterBuild.Length <= 0) return;

        for (int i = 0; i < collidersAfterBuild.Length; i++)
        {
            collidersAfterBuild[i].SetActive(false);
        }
    }
    protected virtual void BeforeBuildAction()
    {
        if (collidersBeforeBuild.Length <= 0) return;

        for (int i = 0; i < collidersBeforeBuild.Length; i++)
        {
            collidersBeforeBuild[i].SetActive(false);
        }
    }

    private void OnEnable()
    {
        _playerDetector.OnPlayerEnter += Collect;
        _playerDetector.OnPlayerExit += StopCollect;

        OnEnoughForBuild += Build;

        if (_counterView == null) return;
        OnCountChange += _counterView.TextCountVisual;

    }
    private void OnDisable()
    {
        _playerDetector.OnPlayerEnter -= Collect;
        _playerDetector.OnPlayerExit -= StopCollect;

        OnCountChange -= _counterView.TextCountVisual;
        OnEnoughForBuild -= Build;
    }
   
    protected virtual void Build()
    {
        if(EnoughForBuild())
        {
            StopCollect();
            BuildAndEffect(building);
            AfterBuildAction();
        }
    }
    protected virtual void BuildAndEffect(GameObject b)
    {
        b.SetActive(true);
    }

    protected override void RecieveTile(Tile T)
    {
        base.RecieveTile(T);
        ++currentTilesCount;
        OnCountChange?.Invoke(currentTilesCount, minCountForCheck);
        if (currentTilesCount >= minCountForCheck)
            OnEnoughForBuild();
    }
    protected bool EnoughForBuild()
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
