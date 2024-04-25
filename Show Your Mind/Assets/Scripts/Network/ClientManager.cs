using Unity.Netcode;
using Zenject;

public class ClientManager : IInitializable
{
    private GameConfig _config;
    private DiContainer _container;

    public ClientManager(DiContainer container, GameConfig config)
    {
        _config = config;
        _container = container;
    }

    public void Initialize()
    {
        NetworkManager.Singleton.StartClient();

        NetworkManager.Singleton.PrefabHandler
            .AddHandler(_config.GameManagerPrefab.gameObject, new ZenjectNetCodeFactory(_config.GameManagerPrefab.gameObject, _container));
    }
}
