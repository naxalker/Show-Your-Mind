using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AttemptsUI : NetworkBehaviour
{
    [SerializeField] private Image[] _healthImages;

    private SudokuGameManager _gameManager;

    [Inject]
    private void Construct(SudokuGameManager gameManager)
    {
        Debug.Log(name);
        _gameManager = gameManager;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            _gameManager.OnAttemptSpent += AttemptSpentHandler;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            _gameManager.OnAttemptSpent -= AttemptSpentHandler;
        }
    }

    private void AttemptSpentHandler(ulong clientId, uint leftAttempts)
    {
        UpdateAttemptsRpc(leftAttempts, RpcTarget.Single(clientId, RpcTargetUse.Temp));
    }

    [Rpc(SendTo.SpecifiedInParams)]
    public void UpdateAttemptsRpc(uint leftAttemptsAmount, RpcParams rpcParams = default)
    {
        Color color = _healthImages[leftAttemptsAmount].color;
        color.a = 0;
        _healthImages[leftAttemptsAmount].color = color;
    }
}
