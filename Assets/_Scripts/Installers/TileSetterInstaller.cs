using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TileSetterInstaller : MonoInstaller
{
    [SerializeField] private TileSetter _tileSetter;
    public override void InstallBindings()
    {

        Container.Bind<TileSetter>().FromInstance(_tileSetter).AsSingle();
        Container.QueueForInject(_tileSetter);

    }
}


