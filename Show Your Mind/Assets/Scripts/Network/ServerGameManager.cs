using Unity.Netcode;

public abstract class ServerGameManager : GameManager
{
    protected ServerGameManager()
    {
        StartServer();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }
}
