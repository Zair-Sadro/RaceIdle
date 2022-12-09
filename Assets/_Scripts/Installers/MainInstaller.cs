using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInstaller : Zenject.MonoInstaller
{
    [SerializeField] private RaceIdleGame _raceIdleGame;
    public override void InstallBindings()
    {

        Container.Bind<RaceIdleGame>().FromInstance(_raceIdleGame).AsSingle().NonLazy();
        Container.QueueForInject(_raceIdleGame);
    }
}
