using System;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using Object = UnityEngine.Object;

public class SudokuServerGameManager : ServerGameManager
{
    private SudokuConfig _config;

    public SudokuServerGameManager(SudokuConfig config)
    {
        _config = config;

        var fieldInstance = Object.Instantiate(_config.FieldPrefab);
        fieldInstance.GetComponent<NetworkObject>().Spawn();
    }

    public override void Init()
    {
    }
}
