using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class BuildSaver: MonoBehaviour,ISaveLoad<BuildingsData>
{
    [SerializeField] private BuildingsData data;

    #region Builders in scene
    [SerializeField] private BuilderFromTiles[] machineTools;
    [SerializeField] private BuilderFromTiles[] levelBridges;
    #endregion

    private int _machineTileCount;
    public int TileInvented => _machineTileCount;
    public void GetBuildInfo(BuilderFromTiles builder)
    {
        switch (builder.buildType)
        {
            case BuildType.SimpleBuild:
                data.simpleBuilders.Add(builder); 
                break;

            case BuildType.MachineTile:
                _machineTileCount++;
                data.machineToolsBuilders.Add(builder);
                break;

            case BuildType.NextLvlMachineTile:
                break;
            default:
                break;
        }
    }

    public BuildingsData GetData()
    {
        return data;
    }

    public void Initialize(BuildingsData data)
    {
       
    }
}
public enum BuildType
{
    SimpleBuild,
    MachineTile,
    NextLvlMachineTile
}
[Serializable]
public class BuildingsData
{
   public List<BuilderFromTiles> machineToolsBuilders;
   public List<BuilderFromTiles> simpleBuilders;

}
