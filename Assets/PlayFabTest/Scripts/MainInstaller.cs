using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayFabCustomIdLogin>().AsSingle();
        Container.Bind<PlayerData>().AsSingle();
    }
}