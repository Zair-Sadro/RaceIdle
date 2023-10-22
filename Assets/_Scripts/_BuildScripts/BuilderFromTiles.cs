
using System;
using System.Collections.Generic;
using UnityEngine;

public class BuilderFromTiles : TileCollector
{
    [Tooltip("Off")]
    public GameObject[] collidersAfterBuild, collidersBeforeBuild;
    [Tooltip("On")]
    public GameObject[] collidersAfterBuildOn, collidersBeforeBuildOn;
    public GameObject building;

    [SerializeField] private byte _buildid;
    [SerializeField] protected bool forceToBuild = true;

    public BuildType buildType;
    public byte BuildID => _buildid;

    [SerializeField] protected PlayerDetector _playerDetector;
    protected BuildSaver _buildSaver => InstantcesContainer.Instance.BuildSaver;

    protected int minCountForCheck;
    protected Action OnEnoughForBuild;



    private void Start()
    {

        if (building.activeInHierarchy && forceToBuild)
        {
            StopCollect();
            BuildEffects(building);
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
        if (collidersAfterBuild.Length > 0)
            for (int i = 0; i < collidersAfterBuild.Length; i++)
            {
                collidersAfterBuild[i].SetActive(false);

            }


        if (collidersAfterBuildOn.Length > 0)
            for (int i = 0; i < collidersAfterBuildOn.Length; i++)
            {

                collidersAfterBuildOn[i].SetActive(true);
            }

        Destroy(this, 2f);
    }
    protected virtual void BeforeBuildAction()
    {
        if (collidersBeforeBuild.Length > 0)
            for (int i = 0; i < collidersBeforeBuild.Length; i++)
            {
                collidersBeforeBuild[i].SetActive(false);

            }

        if (collidersBeforeBuildOn.Length > 0)
            for (int i = 0; i < collidersBeforeBuildOn.Length; i++)
            {
                collidersBeforeBuildOn[i].SetActive(true);
            }
    }

    protected virtual void OnEnable()
    {
        _playerDetector.OnPlayerEnter += Collect;
        _playerDetector.OnPlayerExit += StopCollect;

        OnEnoughForBuild += Build;

        if (_counterView == null) return;
        OnCountChange += _counterView.TextCountVisual;

    }
    protected virtual void OnDisable()
    {
        _playerDetector.OnPlayerEnter -= Collect;
        _playerDetector.OnPlayerExit -= StopCollect;

        OnCountChange -= _counterView.TextCountVisual;
        OnEnoughForBuild -= Build;
    }

    protected virtual void Build()
    {
        if (EnoughForBuild())
        {
            StopCollect();
            BuildEffects(building);

            _buildSaver.GetBuildInfo(this);
            AfterBuildAction();
        }
    }
    public virtual void BuildBySaver()
    {
        building.SetActive(true);
        AfterBuildAction();
    }
    protected virtual void BuildEffects(GameObject b)
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
