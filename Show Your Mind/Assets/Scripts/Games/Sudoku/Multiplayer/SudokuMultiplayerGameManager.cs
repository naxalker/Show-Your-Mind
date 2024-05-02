using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SudokuMultiplayerGameManager : NetworkGameManager, ISudokuModeHandler
{
    public event Action<ulong, uint> OnAttemptSpent;

    private const uint MaxAttempts = 3;

    [SerializeField] private FieldNetwork _fieldPrefab;
    [SerializeField] private Transform _topUIGroup;
    [SerializeField] private Transform _bottomUIGroup;

    private FieldNetwork _field;

    private Dictionary<ulong, uint> _clientAttempts = new Dictionary<ulong, uint>();

    private bool _isEraseMode;
    private bool _isNotesMode;
    private int _selectedDigit;

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

        _container.BindInstance(this).AsSingle();

        NetworkManager.PrefabHandler.AddHandler(_fieldPrefab.gameObject, new ZenjectNetCodeFactory(_fieldPrefab.gameObject, _container));
        NetworkManager.PrefabHandler.AddHandler(_topUIGroup.gameObject, new ZenjectNetCodeFactory(_topUIGroup.gameObject, _container));

        if (IsServer)
        {
            SpawnField();

            _field.OnFieldCompleted += FieldCompletedHandler;
            _field.OnIncorrectPlaced += IncorrectPlacedHandler;

            NetworkManager.Singleton.OnClientConnectedCallback += ClientConnectedHandler;
        }

        SpawnUI();
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

    private void SpawnField()
    {
        _field = Instantiate(_fieldPrefab);
        _container.Inject(_field);
        var fieldNetworkObject = _field.GetComponent<NetworkObject>();
        fieldNetworkObject.Spawn();
    }

    private void SpawnUI()
    {
        if (IsServer)
        {
            var topUIInstance = Instantiate(_topUIGroup);
            _container.InjectGameObject(topUIInstance.gameObject);
            var topUINetworkObject = topUIInstance.GetComponent<NetworkObject>();
            topUINetworkObject.Spawn();
            topUINetworkObject.TrySetParent(GameHUD.transform, false);
        }

        if (IsClient)
        {
            var topyUIInstance = _container.InstantiatePrefab(_bottomUIGroup);
            topyUIInstance.transform.SetParent(GameHUD.transform, false);
        }
    }

    private void FieldCompletedHandler(ulong clientId)
    {
        ProcessVictory(clientId);
    }

    private void ClientConnectedHandler(ulong clientId)
    {
        _clientAttempts[clientId] = MaxAttempts;
    }

    private void IncorrectPlacedHandler(ulong clientId)
    {
        _clientAttempts[clientId]--;

        OnAttemptSpent?.Invoke(clientId, _clientAttempts[clientId]);

        if (_clientAttempts[clientId] == 0)
        {
            ProcessDefeat(clientId);
        }
    } 
}
