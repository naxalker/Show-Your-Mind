using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public abstract class GameManager : NetworkBehaviour, IGameResultValidator
{
    public NetworkVariable<bool> IsGameActive = new NetworkVariable<bool>();

    protected DiContainer _container;
    protected GameHUD GameHUD;

    [Inject]
    private void Construct(DiContainer container)
    {
        _container = container;
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log(GetType().Name);

        if (IsServer)
        {
            IsGameActive.Value = true;
        }

        GameHUD = FindObjectOfType<GameHUD>();
    }

    public virtual void ProcessVictory(ulong clientId)
    {
        IsGameActive.Value = false;
        ShowGameOverUIClientsRpc(clientId, true);
    }

    public virtual void ProcessDefeat(ulong clientId)
    {
        IsGameActive.Value = false;
        ShowGameOverUIClientsRpc(clientId, false);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ShowGameOverUIClientsRpc(ulong clientID, bool isVictory)
    {
        if (NetworkManager.LocalClientId == clientID)
        {
            if (isVictory)
            {
                GameHUD.ShowGameOverUI("�����������!!! �� ��������.");
            }
            else
            {
                GameHUD.ShowGameOverUI("� ���������, �� ���������...");
            }
        }
        else
        {
            if (isVictory)
            {
                GameHUD.ShowGameOverUI("� ���������, �� ���������...");
            }
            else
            {
                GameHUD.ShowGameOverUI("�����������!!! �� ��������.");
            }
        }
    }
}
