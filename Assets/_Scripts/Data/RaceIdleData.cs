using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public RaceData raceData;


    public float Money;

    public RaceIdleData() 
    {
        Money = 0;
        tileSetterData = new();
        buildSaver = new();

        ironMachine = new();
        rubberMachine = new();
        plasticMachine = new();

        ironAutoMachine = new();
        rubberAutoMachine = new();
        plasticAutoMachine = new();

        raceData = new();



    }

}
