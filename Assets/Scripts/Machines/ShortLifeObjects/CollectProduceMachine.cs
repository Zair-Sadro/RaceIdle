using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProductRequierment = MachineLevel.ProductRequierment;

public class CollectProduceMachine : TileCollector, IProduce
{

    [SerializeField] private MachineLevel machineFields;

    [Zenject.Inject] private TileSetter _playerTilesBag;

    [SerializeField] private Transform tileStorage;
    [SerializeField] private Transform productStorage;
    [SerializeField] private Transform tileStartPos;
    [SerializeField] private Transform tileFinishPos;

    [SerializeField] private PlayerDetector _playerDetector;

    private SimpleTimer _timer;
    protected override int maxTileCount => machineFields.MaxTiles;

    private byte typesReq;
    private Dictionary<TileType, Stack<Tile>> tileListByType=new Dictionary<TileType, Stack<Tile>>();
    private bool isProducing;

    private void Start()
    {
        Init();
       
    }
    
    protected virtual void Init()
    {
        typesReq = (byte)machineFields.Requierments.Count;

        for (int i = 0; i < typesReq; i++)
        {
            tileListByType.Add(machineFields.Requierments[i].Type, new Stack<Tile>());
        }

        Produce();
        
    }
    protected virtual void IniTimer()
    {
        _timer = new SimpleTimer(this);
        _timer.OnTimerEnd += Remove;
        _timer.OnTimeChange += UpdateCreationClock;
    }
    public override void Collect() 
    {
        for (int i = 0; i < typesReq; i++)
        {
            var req = machineFields.Requierments[i].Type;
            _playerTilesBag.RemoveTiles(req, tileStorage.position,RecieveTile);
        }
        Produce();
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
    private void UpdateCreationClock(float value)
    {

    }
    public override void Remove() 
    {

    }
    public void Produce()
    {

        if (EnoughForProduce())
        {
            StartCoroutine(ProduceCoroutine());

        }
      
        
    }
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
         
            if (tileStack.Count > requiredAmount)  
                return true;

            return false;
        }

    }
    private IEnumerator ProduceCoroutine()
    {
        isProducing = true;
        yield return GainTiles();
        yield return new WaitForSeconds(machineFields.CreateTime);
        yield return TileManufacture();
        isProducing = false;

        Produce();
    }
    private IEnumerator GainTiles()
    {
        for (int i = 0; i < typesReq; i++)
        {
            var type = machineFields.Requierments[i].Type;
            var amount = machineFields.Requierments[i].Amount;
            var delay = machineFields.DelayMachineTakeTile;

            for (int j = 0; j < amount; j++)
            {
                tileListByType[type].Pop();
                yield return new WaitForSeconds(delay);
            }


        }
       
    }
    private IEnumerator TileManufacture()
    {
        
        //yield return 
        yield return null;
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
    }
}

