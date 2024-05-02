using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public abstract class NetworkGameManager : NetworkBehaviour, INetworkGameResultValidator, ITimeCounter
{
    public event Action<float> GameTimeChanged;

    public NetworkVariable<bool> IsGameActive { get; private set; } = new NetworkVariable<bool>();
    public NetworkVariable<float> Time { get; private set; } = new NetworkVariable<float>();

    public float GameTime
    {
        get => Time.Value;
        set
        {
            Time.Value = value;
            GameTimeChanged?.Invoke(value);
        }
    }

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
            Time.Value = 0;
            StartCoroutine(UpdateGameTime());
        }

        GameHUD = FindObjectOfType<GameHUD>();
    }

    public virtual void ProcessVictory(ulong clientId)
    {
        IsGameActive.Value = false;
        StopAllCoroutines();

        ShowGameOverUIClientsRpc(clientId, true);
    }

    public virtual void ProcessDefeat(ulong clientId)
    {
        IsGameActive.Value = false;
        StopAllCoroutines();

        ShowGameOverUIClientsRpc(clientId, false);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ShowGameOverUIClientsRpc(ulong clientID, bool isVictory)
    {
        if (NetworkManager.LocalClientId == clientID)
        {
            if (isVictory)
            {
                GameHUD.ShowGameOverUI("Поздравляем!!! Вы победили.");
            }
            else
            {
                GameHUD.ShowGameOverUI("К сожалению, вы проиграли...");
            }
        }
        else
        {
            if (isVictory)
            {
                GameHUD.ShowGameOverUI("К сожалению, вы проиграли...");
            }
            else
            {
                GameHUD.ShowGameOverUI("Поздравляем!!! Вы победили.");
            }
        }
    }

    private IEnumerator UpdateGameTime()
    {
        var delay = new WaitForSecondsRealtime(1f);

        while (true)
        {
            yield return delay;

            Time.Value += 1f;
        }
    }
}
