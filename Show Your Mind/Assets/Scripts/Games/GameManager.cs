using System.Collections;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public abstract class GameManager : NetworkBehaviour, IGameResultValidator
{
    public NetworkVariable<bool> IsGameActive = new NetworkVariable<bool>();
    public NetworkVariable<float> GameTime = new NetworkVariable<float>();

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
            GameTime.Value = 0;
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

    private IEnumerator UpdateGameTime()
    {
        var delay = new WaitForSecondsRealtime(1f);

        while (true)
        {
            yield return delay;

            GameTime.Value += 1f;
        }
    }
}
