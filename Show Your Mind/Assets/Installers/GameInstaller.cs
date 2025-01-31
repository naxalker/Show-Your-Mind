using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallFactory();
    }

    private void InstallFactory() 
    {
        Container.Bind<GameManagerFactory>().AsSingle();
    }
}