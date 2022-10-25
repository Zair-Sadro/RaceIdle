using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIMemmoryInstaller : MonoInstaller
{
    [SerializeField] private UIMemmory _UIManager;
    public override void InstallBindings()
    {

        Container.Bind<UIMemmory>().FromInstance(_UIManager).AsSingle();
        Container.QueueForInject(_UIManager);
    }
}
