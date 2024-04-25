using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public abstract class GameManager : NetworkBehaviour, IGameResultValidator
{
    protected DiContainer _container;

    protected bool _isGameActive = true;

    public bool IsGameActive => _isGameActive;

    [Inject]
    private void Construct(DiContainer container)
    {
        _container = container;
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log(GetType().Name);
    }

    public void ProcessVictory(ulong clientId)
    {

    }

    public void ProcessDefeat(ulong clientId)
    {

    }
}
