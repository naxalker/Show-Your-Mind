using Unity.Netcode;
using UnityEngine;

public abstract class ServerGameManager : GameManager, IGameResultValidator
{
    protected ServerGameManager()
    {
        StartServer();
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    public virtual void ProcessDefeat(ulong clientId)
    {
        
    }

    public virtual void ProcessVictory(ulong clientId)
    {
        Debug.Log($"Client {clientId} has won!");
    }
}
