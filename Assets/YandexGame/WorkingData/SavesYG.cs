
namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        #region Buildings
        public BuildingsData buildData;
        public TileSetterData tileSetterData;
        public RaceData raceData;
        #endregion

        #region Machines
        public MachineUpgradeData ironMachine;
        public MachineUpgradeData rubberMachine;
        public MachineUpgradeData plasticMachine;

        public MachineUpgradeData ironAutoMachine;
        public MachineUpgradeData rubberAutoMachine;
        public MachineUpgradeData plasticAutoMachine;
        #endregion

        public RaceIdleData GetMainGameData()
        {
            RaceIdleData _data = new();
            _data.tileSetterData = tileSetterData;
            _data.buildSaver = buildData;
            _data.raceData = raceData;

            _data.ironMachine = ironMachine;
            _data.plasticMachine = plasticMachine;
            _data.rubberMachine = rubberMachine;

            _data.ironAutoMachine = ironAutoMachine;
            _data.plasticAutoMachine = plasticAutoMachine;
            _data.rubberAutoMachine = rubberAutoMachine;

            return _data;
        }

        public void SetMainGameData(RaceIdleData _data)
        {
            buildData = _data.buildSaver;
            tileSetterData = _data.tileSetterData;
            raceData = _data.raceData;

            ironMachine = _data.ironMachine;
            plasticMachine = _data.plasticMachine;
            rubberMachine = _data.rubberMachine;

            ironAutoMachine = _data.ironAutoMachine;
            plasticAutoMachine = _data.plasticAutoMachine;
            rubberAutoMachine = _data.rubberAutoMachine;


        }

        public SavesYG()
        {
        }
    }
}
