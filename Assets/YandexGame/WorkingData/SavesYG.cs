
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
        public BuildingsData BuildData;
        public TileSetterData TileSetterData;
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
            _data.tileSetterData = TileSetterData;
            _data.buildSaver = BuildData;

            _data.ironMachine = ironMachine;
            _data.plasticMachine = plasticMachine;
            _data.rubberMachine = rubberMachine;

            _data.ironAutoMachine = ironAutoMachine;
            _data.plasticAutoMachine = plasticAutoMachine;
            _data.rubberAutoMachine = rubberAutoMachine;

            return _data;
        }

        public SavesYG()
        {
        }
    }
}
