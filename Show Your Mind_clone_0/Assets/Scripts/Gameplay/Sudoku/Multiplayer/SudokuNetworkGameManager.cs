using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SudokuNetworkGameManager : NetworkGameManager, ISudokuModeHandler, IAttemptsCounter
{
    public event Action<uint> OnAttemptSpent;
    public event Action<float> OnOpponentProgressChanged;

    private const uint MaxAttempts = 3;

    [SerializeField] private FieldNetwork _fieldPrefab;
    [SerializeField] private Transform _topUIGroup;
    [SerializeField] private Transform _bottomUIGroup;

    private FieldNetwork _field;
    private SudokuDifficultyConfig _config;

    private Dictionary<ulong, uint> _clientAttempts = new Dictionary<ulong, uint>();
    private Dictionary<ulong, float> _clientProgress = new Dictionary<ulong, float>();

    private bool _isEraseMode;
    private bool _isNotesMode;
    private int _selectedDigit;

    public override void Initialize(DifficultyConfig config)
    {
        base.Initialize(config);

        _config = base.Config as SudokuDifficultyConfig;
    }

    public new SudokuDifficultyConfig Config => _config;

    public bool IsEraseMode
    {
        get { return _isEraseMode; }
        set { _isEraseMode = value; }
    }

    public bool IsNotesMode
    {
        get { return _isNotesMode; }
        set { _isNotesMode = value; }
    }

    public int SelectedDigit
    {
        get { return _selectedDigit; }
        set { _selectedDigit = value; }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsServer)
        {
            SpawnField();

            _field.OnFieldCompleted += FieldCompletedHandler;
            _field.OnIncorrectPlaced += IncorrectPlacedHandler;

            NetworkManager.Singleton.OnClientConnectedCallback += ClientConnectedHandler;
        }

        if (IsClient)
        {
            SpawnUI();
        }
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        if (IsServer)
        {
            _field.OnFieldCompleted -= FieldCompletedHandler;
            _field.OnIncorrectPlaced -= IncorrectPlacedHandler;

            NetworkManager.Singleton.OnClientConnectedCallback -= ClientConnectedHandler;
        }
    }

    public void SetClientProgress(ulong clientId, float value)
    {
        if (_clientProgress.ContainsKey(clientId))
        {
            _clientProgress[clientId] = value;
            UpdateOpponentProgressRpc(clientId, value);
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateOpponentProgressRpc(ulong clientId, float value)
    {
        if (clientId != NetworkManager.LocalClientId)
        {
            OnOpponentProgressChanged?.Invoke(value);
        }
    }

    private void SpawnField()
    {
        _field = Instantiate(_fieldPrefab);
        Container.Inject(_field);
        var fieldNetworkObject = _field.GetComponent<NetworkObject>();
        fieldNetworkObject.Spawn();
    }

    private void SpawnUI()
    {
        var topUIInstance = Container.InstantiatePrefab(_topUIGroup, GameHUD.transform);
        topUIInstance.transform.SetAsFirstSibling();

        var bottomUIInstance = Container.InstantiatePrefab(_bottomUIGroup, GameHUD.transform);
        bottomUIInstance.transform.SetAsFirstSibling();
    }

    private void FieldCompletedHandler(ulong clientId)
    {
        ProcessGameOver(GameOverResultType.UserWon, clientId);
    }

    private void ClientConnectedHandler(ulong clientId)
    {
        _clientAttempts[clientId] = MaxAttempts;
        _clientProgress[clientId] = 0f;
    }

    private void IncorrectPlacedHandler(ulong clientId)
    {
        _clientAttempts[clientId]--;

        SpendAttemptRpc(_clientAttempts[clientId], RpcTarget.Single(clientId, RpcTargetUse.Temp));

        if (_clientAttempts[clientId] == 0)
        {
            ProcessGameOver(GameOverResultType.UserLost, clientId);
        }
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void SpendAttemptRpc(uint leftAttempts, RpcParams rpcParams = default)
    {
        OnAttemptSpent?.Invoke(leftAttempts);
    }
}
