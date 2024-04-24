using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ICustomIdLogin>().To<CustomIdLogin>().AsSingle();
        Container.Bind<IEmailPasswordLogin>().To<EmailPasswordLogin>().AsSingle();
        Container.Bind<PlayerData>().AsSingle();
        Container.Bind<FriendFunction>().AsSingle();
        Container.Bind<CloudScriptFunction>().AsSingle();
        Container.Bind<RankingFunction>().AsSingle();
        Container.Bind<GroupFunction>().AsSingle();
        Container.Bind<IPlayerInventory>().To<PlayerInventory>().AsSingle();
        Container.Bind<PlayerPrefsUtility>().AsSingle();
        Container.Bind<AesProcess>().AsSingle();
        Container.BindInterfacesAndSelfTo<AsyncToken>().AsTransient();
    }
}