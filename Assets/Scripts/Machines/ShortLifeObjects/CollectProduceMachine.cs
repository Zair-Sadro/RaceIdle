using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTaskManager;
using DG.Tweening;
using ProductRequierment = MachineLevel.ProductRequierment;

public class CollectProduceMachine : TileCollector
{
    [SerializeField] private MachineLevel machineFields;

    [Space(5)]
    [Header("Points & Storages")]
    [SerializeField] private Transform tileStorage;
    [SerializeField] private Transform productStorage;
    [SerializeField] private Transform tileStartPos;
    [SerializeField] private Transform tileFinishPos;

    [Zenject.Inject] private TileSetter _playerTilesBag;
    [Zenject.Inject] private ResourceTilesSpawn _tilesSpawner;
    [SerializeField] private PlayerDetector _playerDetector;


    private Dictionary<TileType, Stack<Tile>> tileListByType = new Dictionary<TileType, Stack<Tile>>();
    protected override int maxTileCount => machineFields.MaxTiles;


    private byte typesReq;
    private SimpleTimer _timer;

    private Action OnCollect;

    #region "StatesTaskMachine"

    MachineState currentState;
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
                wait = new Task(WaitForEnough());
                break;

            case MachineState.PRODUCE:

                produce = new Task(TileManufacture());

                break;

            case MachineState.GAIN:

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
        yield break;
    }

    // case MachineState.PRODUCE:
    private IEnumerator TileManufacture() 
    {
        yield return new WaitForSeconds(machineFields.CreateTime);

        var tile = _tilesSpawner.GetTile(typeProduced);
        tile.OnTake();
        tile.transform.position = tileStartPos.position;

        yield return tile.transform.DOMove(tileFinishPos.position, 1f)
                                   .SetEase(Ease.InFlash)
                                   .WaitForCompletion();

        //PuffEffect();
        tile.transform.parent = productStorage;
        SetState(MachineState.WAIT_FOR_ENOUGH);

        yield break;
    }

    #endregion

    #region Init&SaveLoad

    private void Awake()
    {

    }
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
        }
        
    }
    protected virtual void IniTimer()
    {
        _timer = new SimpleTimer(this);
        _timer.OnTimerEnd += Remove;
        _timer.OnTimeChange += UpdateCreationClock;
    }

    private void OnEnable()
    {
        _playerDetector.OnPlayerEnter += Collect;
        _playerDetector.OnPlayerExit += StopCollect;
    }
    private void OnDisable()
    {
        _playerDetector.OnPlayerEnter -= Collect;
        _playerDetector.OnPlayerExit -= StopCollect;
        OnCollect += (() => SetState(MachineState.WAIT_FOR_ENOUGH));
    }
    #endregion

    private bool EnoughForProduce()
    {
        bool enough = false;
      
        for (int i = 0; i < typesReq; i++)
        {
            if (OneOfRequredTypeIsEnough(i)) enough = true;
            else enough = false;

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

    #region Collect Stuff

    public override void Collect()
    {
        for (int i = 0; i < typesReq; i++)
        {
            var req = machineFields.Requierments[i].Type;
            _playerTilesBag.RemoveTiles(req, tileStorage.position, RecieveTile);
        }
        OnCollect?.Invoke();
    }

    public virtual void StopCollect()
    {
        _playerTilesBag.StopRemovingTiles();
    }
    private void RecieveTile(Tile tile)
    {
        tileListByType[tile.Type].Push(tile);
        tile.OnStorage(tileStorage);

    }
    public override void Remove()
    {

    }
    #endregion


    private void UpdateCreationClock(float value)
    {

    }

}

