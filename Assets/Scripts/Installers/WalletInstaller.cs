using UnityEngine;

public class WalletInstaller : Zenject.MonoInstaller
{
    [SerializeField] private WalletSystem _walletSystem;
    public override void InstallBindings()
    {

        Container.Bind<WalletSystem>().FromInstance(_walletSystem).AsSingle().NonLazy();
        Container.QueueForInject(_walletSystem);
    }
}
