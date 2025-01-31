using System;
using System.Collections;
using System.Reflection;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public abstract class NetworkGameManager : NetworkBehaviour, IGameResultValidator, ITimeCounter
{
    public event Action<float> OnGameTimeChanged;
    public event Action<GameOverResultType, ulong> OnGameOver;

    public NetworkVariable<bool> IsGameActive { get; private set; } = new NetworkVariable<bool>();
    public float GameTime { get; private set; }

    protected DiContainer Container { get; private set; }
    protected DifficultyConfig Config { get; private set; }
    protected GameHUD GameHUD { get; private set; }

    [Inject]
    private void Construct(DiContainer container)
    {
        Container = container;

        BindSelfAndInterfaces();
    }

    public virtual void Initialize(DifficultyConfig config)
    {
        Config = config;
    }

    public override void OnNetworkSpawn()
    {
        Debug.Log(GetType().Name);

        if (IsServer)
        {
            IsGameActive.Value = true;
            GameTime = 0;
            StartCoroutine(UpdateGameTime());

            NetworkManager.Singleton.OnClientDisconnectCallback += ClientDisconnectedHandler;
        }

        GameHUD = FindObjectOfType<GameHUD>();
        GameHUD.Initalize(this);
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback -= ClientDisconnectedHandler;
        }
    }

    public void ProcessGameOver(GameOverResultType resultType, ulong clientId = 0)
    {
        IsGameActive.Value = false;
        StopAllCoroutines();

        ProcessGameOverUIClientsRpc(resultType, clientId);
    }

    private void BindSelfAndInterfaces()
    {
        Type derivedType = GetType();
        MethodInfo bindMethod = typeof(DiContainer)
            .GetMethod("BindInterfacesAndSelfTo", Type.EmptyTypes)
            .MakeGenericMethod(derivedType);

        var binding = bindMethod.Invoke(Container, null);
        var fromInstanceMethod = binding.GetType().GetMethod("FromInstance", new Type[] { derivedType });
        fromInstanceMethod.Invoke(binding, new object[] { this });

        var asSingleMethod = binding.GetType().GetMethod("AsSingle", Type.EmptyTypes);
        asSingleMethod.Invoke(binding, null);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void ProcessGameOverUIClientsRpc(GameOverResultType resultType, ulong clientId)
    {
        OnGameOver?.Invoke(resultType, clientId);
    }

    private IEnumerator UpdateGameTime()
    {
        var delay = new WaitForSecondsRealtime(1f);

        while (true)
        {
            yield return delay;

            GameTime += 1f;
            ChangeGameTimeRpc(GameTime);
        }
    }

    [Rpc(SendTo.Everyone)]
    private void ChangeGameTimeRpc(float newTime)
    {
        OnGameTimeChanged?.Invoke(newTime);
    }

    private void ClientDisconnectedHandler(ulong clientId)
    {
        ProcessGameOver(GameOverResultType.UserLeft, clientId);
    }
}
