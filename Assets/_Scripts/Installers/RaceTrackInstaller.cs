using UnityEngine;
using Zenject;

public class RaceTrackInstaller : MonoInstaller
{
    [SerializeField] private RaceTrackManager _raceTrackManager;

    public override void InstallBindings()
    {

        Container.Bind<RaceTrackManager>().FromInstance(_raceTrackManager).AsSingle();
        Container.QueueForInject(_raceTrackManager);
    }
}
