using Zenject;

public class GlobalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        BindLoader();
    }

    private void BindLoader()
    {
        Container.Bind<ZenjectSceneLoaderWrapper>().AsSingle();
        Container.Bind<SceneLoader>().AsSingle();
    }
}