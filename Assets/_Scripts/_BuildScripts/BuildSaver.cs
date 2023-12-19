using System;
using System.Collections.Generic;
using UnityEngine;

public class BuildSaver : MonoBehaviour, ISaveLoad<BuildingsData>
{
    [SerializeField] private BuildingsData data;
    [SerializeField] private List<BuilderFromTiles> _buildings;
    [SerializeField] private ParticleSystem _buildVFX;

    private Dictionary<byte, BuilderFromTiles> _buildingsById = new();
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

            case BuildType.AutoMachine:
                data.autoMachineTilesCount++;
                data.Buildings.Add(builder.BuildID);
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
        if (data == null)
            return;
        this.data = data;
        foreach (var id in data.Buildings)
        {
            _buildingsById[id].BuildBySaver();
        }
    }
    public void BuildEffect(Vector3 globalpos)
    {
        _buildVFX.transform.position = globalpos;
        _buildVFX.Play();
    }
    private void FillIDs()
    {
        if (_buildingsById.Count == 0)
            foreach (var building in _buildings)
            {
                _buildingsById.Add(building.BuildID, building);
            }
    }

}
public interface IBuildable 
{
    public void Build();
}
public enum BuildType
{
    SimpleBuild,
    MachineTile,
    AutoMachine
}
[Serializable]
public class BuildingsData
{
    public int machineTileCount;
    public int autoMachineTilesCount;
    public List<byte> Buildings;

    public BuildingsData() 
    {
        machineTileCount = 0;
        autoMachineTilesCount = 0;
        Buildings= new List<byte>();
    }

}
