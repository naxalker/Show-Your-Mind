using System;
using Unity.Netcode;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

public class SudokuServerGameManager : ServerGameManager, IInitializable, IDisposable
{
    private SudokuConfig _config;

    private int[,] _correctField, _userField;

    private Field _field;

    public SudokuServerGameManager(SudokuConfig config)
    {
        _config = config;
    }

    public void Initialize()
    {
        (_correctField, _userField) = FieldGenerator.GenerateField(39);

        _field = Object.Instantiate(_config.FieldPrefab);
        _field.Init(_correctField, _userField);

        _field.OnFieldCompleted += FieldCompletedHandler;

        var fieldNetworkObject = _field.GetComponent<NetworkObject>();
        fieldNetworkObject.Spawn();
    }

    public void Dispose()
    {
        _field.OnFieldCompleted -= FieldCompletedHandler;
    }

    private void FieldCompletedHandler(ulong clientId)
    {
        ProcessVictory(clientId);
    }

    public override void Init()
    {
    }
}
