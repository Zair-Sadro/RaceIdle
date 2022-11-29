using UnityEngine;
using Zenject;

public class StatsValueInfoInstaller : MonoInstaller
{
    [SerializeField] private StatsValuesInformator _statsValieInf;
    public override void InstallBindings()
    {
        Container.Bind<StatsValuesInformator>().FromInstance(_statsValieInf).AsSingle();
        Container.QueueForInject(_statsValieInf);
    }
}