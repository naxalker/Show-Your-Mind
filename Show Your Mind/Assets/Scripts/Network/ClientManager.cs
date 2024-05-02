using Unity.Netcode;
using Zenject;

public class ClientManager : MultiplayerManager
{
    public ClientManager(DiContainer container, MenuGameConfig config) : base(container, config)
    {
    }

    public override void Initialize()
    {
        NetworkManager.Singleton.StartClient();

        NetworkManager.Singleton.PrefabHandler
            .AddHandler(Config.MultiplayerGameManager.gameObject, new ZenjectNetCodeFactory(Config.MultiplayerGameManager.gameObject, Container));
    }
}
