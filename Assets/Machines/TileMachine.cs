﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTaskManager;
public class TileMachine : TileCollector,IBuildable
{

    public MachineFields machineFields;
    [SerializeField] private ParticleSystem _puffVFX;

    [Space(5)]
    [Header("Points & Storages")]
    [SerializeField] private ProductStorage productStorage;
    [SerializeField] private AutoLayout3D.GridLayoutGroup3D _layoutGroupResource, _layoutGroupProduct;
    [SerializeField] private Transform tileStartPos;
    [SerializeField] private Transform tileFinishPos;

    private ResourceTilesSpawn _tilesSpawner => InstantcesContainer.Instance.ResourceTilesSpawn;
    private WalletSystem _walletSystem => InstantcesContainer.Instance.WalletSystem;
    [SerializeField] private PlayerDetector _detectorForRes;
    public IReadOnlyDictionary<TileType, Stack<Tile>> TilesListsByType => tileListByType;

    protected override int maxTileCount => machineFields.MaxTiles;
    protected virtual float machineSpeed => machineFields.Speed;
    public float delayMachineTakeTile;

    private Action OnCollect;
    public event Action<TileType> TypeInvented;

    #region "StatesTaskMachine"

    MachineState currentState;

    private bool producing;
    private bool gaining;

    public Task wait { get; private set; }
    public Task produce { get; private set; }
    public Task gainTiles { get; private set; }
    public TileType typeProduced => machineFields.ProductType;


    private void SetState(MachineState state)
    {
        currentState = state;


        switch (state)
        {
            case MachineState.WAIT_FOR_ENOUGH:
                if (wait != null)
                    if (wait.Running|| producing) return;
                wait = new Task(WaitForEnough());
                break;

            case MachineState.PRODUCE:
                if (producing) return;
                produce = new Task(TileManufacture());

                break;

            case MachineState.GAIN:
                if (gaining) return;
                gainTiles = new Task(GainTiles());

                break;

        }
    }
    // case MachineState.WAIT_FOR_ENOUGH:
    private IEnumerator WaitForEnough()
    {
        if (!EnoughForProduce())
        {
            yield break; // спокойно выходим ибо подписаны на ивент OnCollect
                         // при получении новыйх тайлов сново вызовется wait
        }

        SetState(MachineState.GAIN);
    }

    // case MachineState.GAIN:
    private IEnumerator GainTiles()
    {
        gaining = true;
        var delay = delayMachineTakeTile;

        if (_playerTilesBag._isGivingTiles) yield return new WaitForSeconds(0.7f);


        for (int i = 0; i < _requiredTypesCount; i++)
        {
            var type = machineFields.Requierments[i].Type;
            var amount = machineFields.Requierments[i].Amount;


            for (int j = 0; j < amount; j++)
            {
                PuffEffect(tileListByType[type].Peek().transform.position);

                _tilesSpawner.ToPool(tileListByType[type].Pop());// переносим родителя Пула  (выкидываем с стака)


                yield return new WaitForSeconds(delay);
            }

        }

        SetState(MachineState.PRODUCE);
        gaining = false;
        yield break;
    }

    // case MachineState.PRODUCE:

    private IEnumerator TileManufacture()
    {
        producing = true;
        var tile = _tilesSpawner.GetTile(typeProduced);
        tile.OnTake();

        yield return StartCoroutine(tile.AppearFromZero(Vector3.one, tileStartPos.position, machineSpeed * 0.35f));

        yield return tile.transform.DOMove(tileFinishPos.position, machineSpeed * 0.65f)
                                   .SetEase(Ease.InOutFlash)
                                   .WaitForCompletion();

        yield return tile.transform.DOJump(productStorage.transform.position, 2, 1, 0.5f)
                                   .WaitForCompletion();

        productStorage.TileToStorage(tile);
        _layoutGroupProduct.UpdateLayout();
        _walletSystem.Income(machineFields.Income);

        producing = false;
        SetState(MachineState.WAIT_FOR_ENOUGH);

      
        yield break;
    }

    private void PuffEffect(Vector3 pos)
    {
        _puffVFX.transform.position = pos;
        _puffVFX.Play();
    }

    #endregion
    protected override void Collect()
    {
        if (_playerTilesBag._isGivingTiles) return;

        _stopCollect = false;
        _playerTilesBag.RemoveTiles(_requiredTypes[0], tileStorage.position, RecieveTile);
    }
    #region Init&SaveLoad
    private void Awake()
    {
        Init();
        productStorage.OnFreeSpaceInStorage += () => SetState(MachineState.WAIT_FOR_ENOUGH);
    }
    protected virtual void Init()
    {
        productRequierments = machineFields.Requierments;

        InitDictionary();

        for (int i = 0; i < _requiredTypesCount; i++)
        {
            minCountForCheck += productRequierments[i].Amount;
        }

    }


    private void OnEnable()
    {
        hideAfterCollect = false;
        OnCollect += (() => SetState(MachineState.WAIT_FOR_ENOUGH));
        SetState(MachineState.WAIT_FOR_ENOUGH);

        _detectorForRes.OnPlayerEnter += Collect;
        _detectorForRes.OnPlayerExit += StopCollect;
    }
    private void OnDisable()
    {
        _detectorForRes.OnPlayerEnter -= Collect;
        _detectorForRes.OnPlayerExit -= StopCollect;

    }
    #endregion

    private int minCountForCheck;
    protected override void RecieveTile(Tile tile)
    {
        tileListByType[tile.Type].Push(tile);
        tile.OnStorage(tileStorage);

        if (tileStorage.transform.childCount >= minCountForCheck
            && currentState == MachineState.WAIT_FOR_ENOUGH)

            OnCollect?.Invoke();
       StartCoroutine(UpdateLay());
    }
    IEnumerator UpdateLay()
    {
        yield return new WaitForSeconds(0.9f);
        _layoutGroupResource.UpdateLayout();
    }
    private bool EnoughForProduce()
    {
        if (productStorage.IsFreeForNextTiles(maxTileCount) == false)
            return false;

        for (int i = 0; i < _requiredTypesCount; i++)
        {
            if (!OneOfRequredTypeIsEnough(i))
                return false;

        }

        return true;

        bool OneOfRequredTypeIsEnough(int i)
        {
            var type = machineFields.Requierments[i].Type;

            if (!tileListByType.TryGetValue(type, out Stack<Tile> tileStack))  //Check if stack exist
                return false;

            var requiredAmount = machineFields.Requierments[i].Amount;

            if (tileStack.Count >= requiredAmount)
                return true;

            return false;
        }

    }

    public void Build()
    {
        TypeInvented?.Invoke(machineFields.ProductType);
    }
}


