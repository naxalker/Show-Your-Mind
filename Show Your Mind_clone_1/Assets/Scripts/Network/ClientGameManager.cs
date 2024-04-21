using Unity.Netcode;

public abstract class ClientGameManager : GameManager
{
    protected ClientGameManager()
    {
        StartClient();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
