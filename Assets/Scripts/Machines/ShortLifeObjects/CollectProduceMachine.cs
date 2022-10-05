using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTaskManager;

public class CollectProduceMachine : TileCollector
{
    [SerializeField] private MachineLevel machineFields;

    [Space(5)]
    [Header("Points & Storages")]
    [SerializeField] private Transform tileStorage;
    [SerializeField] private ProductStorage productStorage;
    [SerializeField] private Transform tileStartPos;
    [SerializeField] private Transform tileFinishPos;

    [Zenject.Inject] private TileSetter _playerTilesBag;
    [Zenject.Inject] private ResourceTilesSpawn _tilesSpawner;
    [SerializeField] private PlayerDetector _detectorForRes;

    private Dictionary<TileType, Stack<Tile>> tileListByType = new Dictionary<TileType, Stack<Tile>>();
    protected override int maxTileCount => machineFields.MaxTiles;

    private Action OnCollect;
    private int minCountForCheck;
    private byte typesReq;

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
                if (wait.Running ) return;
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
        var delay = machineFields.DelayMachineTakeTile;

        for (int i = 0; i < typesReq; i++)
        {
            var type   = machineFields.Requierments[i].Type;
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

        yield return new WaitForSeconds(machineFields.CreateTime);

        var tile = _tilesSpawner.GetTile(typeProduced);
        tile.OnTake();
        tile.transform.position = tileStartPos.position;

        yield return tile.transform.DOMove(tileFinishPos.position, 1f)
                                   .SetEase(Ease.InFlash)
                                   .WaitForCompletion();

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

        OnCollect += (() => SetState(MachineState.WAIT_FOR_ENOUGH));
        SetState(MachineState.WAIT_FOR_ENOUGH);
    }
   
    protected virtual void Init()
    {
        typesReq = (byte)machineFields.Requierments.Count;

        for (int i = 0; i < typesReq; i++)
        {
            tileListByType.Add(machineFields.Requierments[i].Type, new Stack<Tile>());
            minCountForCheck += machineFields.Requierments[i].Amount;
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

    #region Collect Stuff

    public override void Collect()
    {

        for (int i = 0; i < typesReq; i++)
        {
            var req = machineFields.Requierments[i].Type;
            _playerTilesBag.RemoveTiles(req, tileStorage.position, RecieveTile);
        }

    }

    public virtual void StopCollect()
    {
        _playerTilesBag.StopRemovingTiles();
        
    }
    private void RecieveTile(Tile tile)
    {
        tileListByType[tile.Type].Push(tile);
        tile.OnStorage(tileStorage);

        if (tileStorage.transform.childCount >= minCountForCheck 
            && currentState == MachineState.WAIT_FOR_ENOUGH) 

            OnCollect?.Invoke();

    }
    #endregion

    private bool EnoughForProduce()
    {
        bool enough = false;

        for (int i = 0; i < typesReq; i++)
        {
            if (OneOfRequredTypeIsEnough(i)) enough = true;
            else { return false; }

        }
        return enough;

        bool OneOfRequredTypeIsEnough(int i)
        {
            var type = machineFields.Requierments[i].Type;
            _ = new Stack<Tile>();
            Stack<Tile> tileStack;

            if (!tileListByType.TryGetValue(type, out tileStack))  //Check if stack exist
                return false;

            var requiredAmount = machineFields.Requierments[i].Amount;

            if (tileStack.Count >= requiredAmount)
                return true;

            return false;
        }

    }
    private void UpdateCreationClock(float value)
    {

    }

}

