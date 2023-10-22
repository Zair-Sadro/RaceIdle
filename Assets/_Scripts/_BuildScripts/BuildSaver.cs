using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http.Headers;
using UnityEngine;

public class BuildSaver: MonoBehaviour,ISaveLoad<BuildingsData>
{
    [SerializeField] private BuildingsData data;
    [SerializeField] private List<BuilderFromTiles> _buildings;

    private Dictionary<byte, BuilderFromTiles> _buildingsById=new();
    public int TileInvented => data.machineTileCount;

    private void Start()
    {
        FillIDs();
    }
    public void GetBuildInfo(BuilderFromTiles builder)
    {
        switch (builder.buildType)
        {
            case BuildType.SimpleBuild:
                data.Buildings.Add(builder.BuildID); 
                break;

            case BuildType.MachineTile:
                data.machineTileCount++;
                data.Buildings.Add(builder.BuildID);
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
        FillIDs();
        foreach (var id in data.Buildings)
        {
            _buildingsById[id].BuildBySaver();
        }
    }
    private void FillIDs()
    {
     if(_buildingsById.Count==0)   
        foreach (var building in _buildings)
        {
            _buildingsById.Add(building.BuildID, building);
        }
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
   public int machineTileCount;
   public List<byte> Buildings;


}
