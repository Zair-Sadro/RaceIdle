using UnityEngine;
using Zenject;

public class PlayerControllerInstaller : MonoInstaller
{
    [SerializeField] PlayerController _playerController;
    public override void InstallBindings()
    {
        Container.Bind<PlayerController>().FromInstance(_playerController).AsSingle().NonLazy();
        Container.QueueForInject(_playerController);
    }
}