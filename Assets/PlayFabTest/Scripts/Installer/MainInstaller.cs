using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ICustomIdLogin>().To<CustomIdLogin>().AsSingle();
        Container.Bind<PlayerData>().AsSingle();
        Container.Bind<FriendFunction>().AsSingle();
        Container.Bind<CloudScriptFunction>().AsSingle();
        Container.Bind<RankingFunction>().AsSingle();
        Container.Bind<GroupFunction>().AsSingle();
        Container.Bind<PlayerPrefsUtility>().AsSingle();
        Container.Bind<EntityKeyMaker>().AsSingle();
        Container.BindInterfacesAndSelfTo<AsyncToken>().AsTransient();
    }
}