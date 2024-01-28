[System.Serializable]
public class RaceIdleData
{
    public TileSetterData tileSetterData;
    public BuildingsData buildSaver;

    public MachineUpgradeData ironMachine;
    public MachineUpgradeData rubberMachine;
    public MachineUpgradeData plasticMachine;

    public MachineUpgradeData ironAutoMachine;
    public MachineUpgradeData rubberAutoMachine;
    public MachineUpgradeData plasticAutoMachine;

    public TilesInAllStoragesData tilesInAllStoragesData;

    public RaceData raceData;


    public float money;
    public PlayerData playerData;
    public int taskIndx;

    public RaceIdleData()
    {
        money = 0;
        tileSetterData = new();
        buildSaver = new();

        ironMachine = new();
        rubberMachine = new();
        plasticMachine = new();

        ironAutoMachine = new();
        rubberAutoMachine = new();
        plasticAutoMachine = new();

        tilesInAllStoragesData = new();

        raceData = new();
        playerData = new();
        taskIndx = 0;

    }

}
