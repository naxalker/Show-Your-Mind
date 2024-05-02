using Unity.Netcode;
using UnityEngine;
using Zenject;

public class ServerManager : MultiplayerManager
{
    public ServerManager(DiContainer container, MenuGameConfig config) : base(container, config)
    {
    }

    public override void Initialize()
    {
        NetworkManager.Singleton.StartServer();
        
        var gameManagerInstance = Object.Instantiate(Config.MultiplayerGameManager.gameObject);
        Container.Inject(gameManagerInstance);
        var gameManagerNetworkObject = gameManagerInstance.GetComponent<NetworkObject>();
        gameManagerNetworkObject.Spawn();
    }
}
