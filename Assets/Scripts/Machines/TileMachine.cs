using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTaskManager;
public class TileMachine : TileCollector
{
    public MachineFields machineFields;

    [Space(5)]
    [Header("Points & Storages")]
    [SerializeField] private ProductStorage productStorage;
    [SerializeField] private Transform tileStartPos;
    [SerializeField] private Transform tileFinishPos;

    [Zenject.Inject] private ResourceTilesSpawn _tilesSpawner;
    [Zenject.Inject] private WalletSystem _walletSystem;
    [SerializeField] private PlayerDetector _detectorForRes;


    protected override int maxTileCount => machineFields.MaxTiles;
    protected virtual float machineSpeed => machineFields.Speed;
    public float delayMachineTakeTile;

    private Action OnCollect;
    private int minCountForCheck;


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
                    if (wait.Running) return;
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
                //PuffEffect();
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

        yield return new WaitForSeconds(machineSpeed);

        var tile = _tilesSpawner.GetTile(typeProduced);
        tile.OnTake();
        tile.transform.position = tileStartPos.position;

        yield return tile.transform.DOMove(tileFinishPos.position, 1f)
                                   .SetEase(Ease.InFlash)
                                   .WaitForCompletion();

        _walletSystem.Income(machineFields.Income);
        //PuffEffect();
        productStorage.TileToStack(tile);
        SetState(MachineState.WAIT_FOR_ENOUGH);

        producing = false;
        yield break;
    }

    #endregion

    #region Init&SaveLoad

    private void Start()
    {
        Init();
        hideAfterCollect = false;
        OnCollect += (() => SetState(MachineState.WAIT_FOR_ENOUGH));
        SetState(MachineState.WAIT_FOR_ENOUGH);
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
        _detectorForRes.OnPlayerEnter += Collect;
        _detectorForRes.OnPlayerExit += StopCollect;


    }
    private void OnDisable()
    {
        _detectorForRes.OnPlayerEnter -= Collect;
        _detectorForRes.OnPlayerExit -= StopCollect;


        OnCollect += (() => SetState(MachineState.WAIT_FOR_ENOUGH));
    }
    #endregion

    protected override void RecieveTile(Tile tile)
    {
        tileListByType[tile.Type].Push(tile);
        tile.OnStorage(tileStorage);

        if (tileStorage.transform.childCount >= minCountForCheck
            && currentState == MachineState.WAIT_FOR_ENOUGH)

            OnCollect?.Invoke(); 

    }

    private bool EnoughForProduce() 
    {
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

 
}

