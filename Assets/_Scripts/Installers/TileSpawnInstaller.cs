using UnityEngine;
using Zenject;

public class TileSpawnInstaller : MonoInstaller
{
    [SerializeField] private ResourceTilesSpawn _junkTileSpawner;
    public override void InstallBindings()
    {
        Container.Bind<ResourceTilesSpawn>().FromInstance(_junkTileSpawner)
                 .AsSingle().NonLazy();


    }
}