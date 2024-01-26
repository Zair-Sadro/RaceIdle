using UnityEngine;
public class RaceIdleGame : MonoBehaviour, ISaveLoad<RaceIdleData>
{
    #region  controllers/managers

    private TileSetter _tileSetter => InstantcesContainer.Instance.TileSetter;

    private WalletSystem _walletsystem => InstantcesContainer.Instance.WalletSystem;

    private BuildSaver _buildSaver => InstantcesContainer.Instance.BuildSaver;

    [SerializeField] private MachineUpgrade _ironMachine;
    [SerializeField] private MachineUpgrade _plasticMachine;
    [SerializeField] private MachineUpgrade _rubberMachine;

    [SerializeField] private AutoMachineUpgrade _ironAutoMachine;
    [SerializeField] private AutoMachineUpgrade _plasticAutoMachine;
    [SerializeField] private AutoMachineUpgrade _rubberAutoMachine;

    [SerializeField] private TilesLoaderInStorages _tilesLoaderInStorages;

    [SerializeField] private RaceTrackManager _raceTrackManager;
    [SerializeField] private PlayerUpgrade _playerUpgrade;



    #endregion

    #region Data fields

    public TileSetterData TileSetterData
    {
        get => _tileSetter.GetData();
        set
        {
            if (value != null) _tileSetter.Initialize(value);
        }
    }
    public BuildingsData BuildsData
    {
        get => _buildSaver.GetData();
        set => _buildSaver.Initialize(value); 
    }

    public RaceData RaceTrackData
    {
        get => _raceTrackManager.GetData();
        set
        {
            if (value != null) _raceTrackManager.Initialize(value);
        }
    }

    public TilesInAllStoragesData TilesInAllStoragesData
    {
        get => _tilesLoaderInStorages.GetData();
        set => _tilesLoaderInStorages.Initialize(value);
    }


    #region Machines
    public MachineUpgradeData IronMachineData
    {
        get => _ironMachine.GetData();
        set
        {
            if (value != null) _ironMachine.Initialize(value);
        }
    }
    public MachineUpgradeData PlasticMachineData
    {
        get => _plasticMachine.GetData();
        set
        {
            if (value != null) _plasticMachine.Initialize(value);
        }
            
    }
    public MachineUpgradeData RubberMachineData
    {
        get => _rubberMachine.GetData();
        set
        {
            if (value != null) _rubberMachine.Initialize(value);
        }
    }


    #endregion

    #region AutoMachines
    public MachineUpgradeData IronAutoMachineData
    {
        get => _ironAutoMachine.GetData();
        set
        {
            if (value != null) _ironAutoMachine.Initialize(value);
        }
    }
    public MachineUpgradeData PlasticAutoMachineData
    {
        get => _plasticAutoMachine.GetData();
        set
        {
            if (value != null) _plasticAutoMachine.Initialize(value);
        }
    }
    public MachineUpgradeData RubberAutoMachineData
    {
        get => _rubberAutoMachine.GetData();
        set
        {
            if (value != null) _rubberAutoMachine.Initialize(value);
        }
    }
    #endregion

    #endregion

    #region DataSaveLoad
    [SerializeField] private RaceIdleData _data;
    public RaceIdleData GetData()
    {

        _data.tileSetterData = TileSetterData;
        _data.buildSaver = BuildsData;
        _data.raceData = RaceTrackData;

        _data.ironMachine = IronMachineData;
        _data.plasticMachine = PlasticMachineData;
        _data.rubberMachine = RubberMachineData;

        _data.ironAutoMachine = IronAutoMachineData;
        _data.plasticAutoMachine = PlasticAutoMachineData;
        _data.rubberAutoMachine = RubberAutoMachineData;

        _data.tilesInAllStoragesData = TilesInAllStoragesData;

        _data.money =  _walletsystem.TotalMoney;
        _data.playerData = _playerUpgrade.GetData();

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

        TilesInAllStoragesData = data.tilesInAllStoragesData;

        RaceTrackData = data.raceData;

        _walletsystem.Init(data.money);
        _playerUpgrade.Initialize(data.playerData);
    }
    #endregion




}
