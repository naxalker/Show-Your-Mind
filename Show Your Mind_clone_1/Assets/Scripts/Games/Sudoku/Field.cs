using System;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class Field : NetworkBehaviour
{
    public event Action<ulong> OnFieldCompleted;
    public event Action<ulong> OnIncorrectPlaced;

    private const float CellSize = 1f;

    private MobileInput _input;
    private SudokuGameManager _gameManager;

    private Cell[,] _cells = new Cell[9, 9];
    private int[,] _correctField;
    private int[,] _userField;

    [Inject]
    private void Construct(MobileInput input, SudokuGameManager gameManager)
    {
        Debug.Log("Construct");
        _input = input;
        _gameManager = gameManager;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            (_correctField, _userField) = FieldGenerator.GenerateField(39);

            Debug.Log("Correct Field");
            LogMatrix(_correctField);

            Debug.Log("User Field");
            LogMatrix(_userField);
        }

        if (IsClient)
        {
            foreach (Cell cell in GetComponentsInChildren<Cell>())
            {
                int column = (int)(cell.transform.position.x + 4 * CellSize);
                int row = (int)(4 * CellSize - cell.transform.position.y);

                _cells[row, column] = cell;
            }

            SubmitFieldServerRpc();

            _input.OnTouchPerformed += OnTouchPerformedHandler;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            _input.OnTouchPerformed -= OnTouchPerformedHandler;
        }
    }
    
    [Rpc(SendTo.Server)]
    private void SubmitFieldServerRpc(RpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        Debug.Log($"Request to generate a field from the Client ID: {clientId}");

        SubmitFieldClientRpc(
            ConvertTwoDimensionalArray(_userField), 
            RpcTarget.Single(rpcParams.Receive.SenderClientId, RpcTargetUse.Temp));
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void SubmitFieldClientRpc(int[] convertedUserField, RpcParams rpcParams)
    {
        Debug.Log("Initializing the field");

        _userField = ConvertOneDimensionalArray(convertedUserField);

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                _cells[i, j].InitCell(_userField[i, j]);
            }
        }
    }

    private void OnTouchPerformedHandler(Vector3 pos)
    {
        if (_gameManager.IsGameActive == false) return;

        float minBound = -4 * CellSize - CellSize / 2;
        float maxBound = 4 * CellSize + CellSize / 2;

        if (pos.x >= minBound && pos.x < maxBound && pos.y >= minBound && pos.y < maxBound)
        {
            int row = Mathf.FloorToInt(maxBound - pos.y);
            int column = Mathf.FloorToInt(pos.x + maxBound);

            if (_cells[row, column].IsInteractable == false) return;

            if (_gameManager.IsEraseMode)
            {
                if (_userField[row, column] != 0)
                {
                    _userField[row, column] = 0;
                    _cells[row, column].SetCell(0);
                }
            }
            else if (_gameManager.IsNotesMode)
            {
                _cells[row, column].SetNoteDigit(_gameManager.SelectedDigit);
            }
            else if (_gameManager.SelectedDigit != 0)
            {
                if (_userField[row, column] == _gameManager.SelectedDigit)
                {
                    _userField[row, column] = 0;
                    _cells[row, column].SetCell(0);
                }
                else
                {
                    _userField[row, column] = _gameManager.SelectedDigit;
                    ValidateCellServerRpc(row, column, ConvertTwoDimensionalArray(_userField));
                }
            }
        }
    }

    [Rpc(SendTo.Server)]
    private void ValidateCellServerRpc(int line, int column, int[] convertedField, RpcParams rpcParams = default)
    {
        Debug.Log($"Validating cell ({line}, {column}) from Client Id: {rpcParams.Receive.SenderClientId}");

        int[,] field = ConvertOneDimensionalArray(convertedField);

        bool isCorrect = _correctField[line, column] == field[line, column];

        if (isCorrect)
        {
            if (IsFieldCompleted(field))
            {
                OnFieldCompleted?.Invoke(rpcParams.Receive.SenderClientId);
            }
        }
        else
        {
            OnIncorrectPlaced?.Invoke(rpcParams.Receive.SenderClientId);
        }

        ValidateCellClientRpc(
            line,
            column,
            field[line, column],
            isCorrect, 
            RpcTarget.Single(rpcParams.Receive.SenderClientId, RpcTargetUse.Temp));
    }

    [Rpc(SendTo.SpecifiedInParams)]
    private void ValidateCellClientRpc(int line, int column, int value, bool isCorrect, RpcParams rpcParams)
    {
        Debug.Log($"Cell ({line}, {column}) {(isCorrect == true ? "has" : "has NOT")} been validated with value {value}.");

        _cells[line, column].SetCell(value, isCorrect);
    }

    private bool IsFieldCompleted(int[,] field)
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (field[i, j] != _correctField[i, j])
                    return false;
            }
        }

        return true;
    }

    private void LogMatrix(int[,] matrix)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                sb.Append(matrix[i, j] + " ");
            }
            sb.AppendLine();
        }

        Debug.Log(sb.ToString());
    }

    private int[] ConvertTwoDimensionalArray(int[,] twoDimensionalArray)
    {
        int rows = twoDimensionalArray.GetLength(0);
        int cols = twoDimensionalArray.GetLength(1);

        int[] oneDimensionalArray = new int[rows * cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                oneDimensionalArray[i * cols + j] = twoDimensionalArray[i, j];
            }
        }

        return oneDimensionalArray;
    }

    private int[,] ConvertOneDimensionalArray(int[] oneDimensionalArray)
    {
        int[,] twoDimensionalArray = new int[9, 9];

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                twoDimensionalArray[i, j] = oneDimensionalArray[i * 9 + j];
            }
        }

        return twoDimensionalArray;
    }
}
