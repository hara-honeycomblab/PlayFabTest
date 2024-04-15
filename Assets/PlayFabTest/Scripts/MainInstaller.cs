using System;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayFabCustomIdLogin>().AsSingle();
        Container.Bind<PlayerData>().AsSingle();
        Container.Bind<FriendFunction>().AsSingle();
        Container.BindInterfacesAndSelfTo<AsyncToken>().AsTransient();
    }
}