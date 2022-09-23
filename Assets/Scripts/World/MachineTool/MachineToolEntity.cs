using System;
using UnityEngine;
using System.Linq;

public class MachineToolEntity : MonoBehaviour
{
    [SerializeField] private MachineLevelType _level;
    [Header("Product Settings")]
    [SerializeField] private Transform _placedTilesPoint;
    [SerializeField] private Transform _createdTilesPoint;

    private MachineLevel _currentLevel;
    private BaseMachineTool _baseMachine;
    private SimpleTimer _timer;

    private bool isWorking=false;

    #region Properties

    public MachineLevelType Level => _level;
    public MachineLevel CurrentLevel => _currentLevel;
    public Transform PlacedTilesPoint => _placedTilesPoint;
    public Transform CreatedTilesPoint => _createdTilesPoint;

    #endregion

    public void Init(BaseMachineTool baseMachine)
    {
        _baseMachine = baseMachine;
        GetMachineLevel();
        InitTimer();
    }

    private void InitTimer()
    {
        _timer = new SimpleTimer(this);
        _timer.OnTimerEnd += CreateProduct;
        _timer.OnTimeChange += UpdateCreationClock;
    }

    private void GetMachineLevel()
    {
        var machineLevelEntity = _baseMachine.MachineData.Levels.Where(l => l.Level == _level).FirstOrDefault();
        _currentLevel = machineLevelEntity;
    }

    private void UpdateCreationClock(float value)
    {
       //link to machinebase ui
    }

    private void CreateProduct()
    {
        if (isWorking) return;

    }

    private void OnDisable()
    {
        _timer.OnTimerEnd -= CreateProduct;
        _timer.OnTimeChange -= UpdateCreationClock;
    }
}
