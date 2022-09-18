using UnityEngine;
using Zenject;

public class MainPlayerInstaller : MonoInstaller
{
    [SerializeField] private PlayerController _playerController;
    public override void InstallBindings()
    {

        Container.Bind<PlayerController>().FromInstance(_playerController).AsSingle();
        Container.QueueForInject(_playerController);
    }
}