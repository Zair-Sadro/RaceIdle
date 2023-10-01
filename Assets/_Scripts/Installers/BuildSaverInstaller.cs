using UnityEngine;

public class BuildSaverInstaller : Zenject.MonoInstaller
{
    [SerializeField] private BuildSaver _buildsaver;
    public override void InstallBindings()
    {

        Container.Bind<BuildSaver>().FromInstance(_buildsaver).AsSingle().NonLazy();
        Container.QueueForInject(_buildsaver);

    }
}

