using UnityEngine;
public class RaceIdleGame : MonoBehaviour, ISaveLoad<RaceIdleData>
{
    #region Injected controllers/managers

    private TileSetter _tileSetter => InstantcesContainer.Instance.TileSetter;
    private BuildSaver _buildSaver => InstantcesContainer.Instance.BuildSaver;

    [SerializeField] private MachineUpgrade _ironMachine;

    [SerializeField] private MachineUpgrade _plasticMachine;

    [SerializeField] private MachineUpgrade _rubberMachine;

    [SerializeField] private AutoMachineUpgrade _ironAutoMachine;

    [SerializeField] private AutoMachineUpgrade _plasticAutoMachine;

    [SerializeField] private AutoMachineUpgrade _rubberAutoMachine;


    #endregion

    #region Data fields

    public TileSetterData TileSetterData
    {
        get => _tileSetter.GetData();
        set => _tileSetter.Initialize(value);
    }
    public BuildingsData BuildsData
    {
        get => _buildSaver.GetData();
        set => _buildSaver.Initialize(value);
    }

    #region Machines
    public MachineUpgradeData IronMachineData
    {
        get => _ironMachine.GetData();
        set => _ironMachine.Initialize(value);
    }
    public MachineUpgradeData PlasticMachineData
    {
        get => _plasticMachine.GetData();
        set => _plasticMachine.Initialize(value);
    }
    public MachineUpgradeData RubberMachineData
    {
        get => _rubberMachine.GetData();
        set => _rubberMachine.Initialize(value);
    }

    public MachineUpgradeData IronAutoMachineData
    {
        get => _ironAutoMachine.GetData();
        set => _ironAutoMachine.Initialize(value);
    }
    public MachineUpgradeData PlasticAutoMachineData
    {
        get => _plasticAutoMachine.GetData();
        set => _plasticAutoMachine.Initialize(value);
    }
    public MachineUpgradeData RubberAutoMachineData
    {
        get => _rubberAutoMachine.GetData();
        set => _rubberAutoMachine.Initialize(value);
    }
    #endregion

    #region AutoMachines
    #endregion

    #endregion

    #region DataSaveLoad
    [SerializeField] private RaceIdleData _data;
    public RaceIdleData GetData()
    {

        _data.tileSetterData = TileSetterData;
        _data.buildSaver = BuildsData;

        _data.ironMachine = IronMachineData;
        _data.plasticMachine = PlasticMachineData;
        _data.rubberMachine = RubberMachineData;

        _data.ironAutoMachine = IronAutoMachineData;
        _data.plasticAutoMachine = PlasticAutoMachineData;
        _data.rubberAutoMachine = RubberAutoMachineData;

        return _data;
    }

    public void Initialize(RaceIdleData data)
    {
        _data = data;

        BuildsData = data.buildSaver;
        TileSetterData = data.tileSetterData;

        IronMachineData = data.ironMachine;
        PlasticMachineData = data.plasticMachine;
        RubberMachineData = data.rubberMachine;

        IronAutoMachineData = data.ironAutoMachine;
        PlasticAutoMachineData = data.plasticAutoMachine;
        RubberAutoMachineData = data.rubberAutoMachine;



    }
    #endregion




}
