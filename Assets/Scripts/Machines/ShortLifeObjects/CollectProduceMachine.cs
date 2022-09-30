using System;
using System.Collections.Generic;
using UnityEngine;

public class CollectProduceMachine : TileCollector, IProduce
{

    [SerializeField] private MachineLevel machineFields;

    [Zenject.Inject] private TileSetter _playerTilesBag;
    [SerializeField] private  Transform tilePos;

    [SerializeField] private PlayerDetector _playerDetector;

    private SimpleTimer _timer;
    protected override int maxTileCount => machineFields.MaxTiles;

    private int typesReq;
    private Dictionary<TileType, List<Tile>> tileListByType;

    private void Start()
    {
        Init();
       
    }
    

    protected virtual void Init()
    {
        typesReq = machineFields.Requierments.Count;
        for (int i = 0; i < typesReq; i++)
        {
            tileListByType.Add(machineFields.Requierments[i].Type, new List<Tile>());
        }
        
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
            _playerTilesBag.RemoveTiles(req, tilePos.position, (t) => tileListByType[req].Add(t));
        }
    }
   
    public virtual void StopCollect()
    {

    }
    private void UpdateCreationClock(float value)
    {

    }
    public override void Remove() 
    {

    }
    public void Produce()
    {
        
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

