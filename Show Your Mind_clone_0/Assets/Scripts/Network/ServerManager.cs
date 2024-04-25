using Unity.Netcode;
using UnityEngine;
using Zenject;

public class ServerManager : IInitializable
{
    private GameConfig _config;
    private DiContainer _container;

    public ServerManager(DiContainer container, GameConfig config)
    {
        _config = config;
        _container = container;
    }

    public void Initialize()
    {
        NetworkManager.Singleton.StartServer();
        
        var gameManagerInstance = Object.Instantiate(_config.GameManagerPrefab);
        _container.Inject(gameManagerInstance);
        var gameManagerNetworkObject = gameManagerInstance.GetComponent<NetworkObject>();
        gameManagerNetworkObject.Spawn();
    }
}
