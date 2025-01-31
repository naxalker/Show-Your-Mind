using System;
using System.Text;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    public event Action OnFieldCompleted;
    public event Action OnIncorrectPlaced;

    private const float CellSize = 1f;

    private MobileInput _input;
    private SudokuGameManager _gameManager;

    private Cell[,] _cells = new Cell[9, 9];
    private int[,] _correctField;
    private int[,] _userField;

    [Inject]
    private void Construct(MobileInput input, SudokuGameManager gameManager)
    {
        _input = input;
        _gameManager = gameManager;
    }

    private void Start()
    {
        (_correctField, _userField) 
            = FieldGenerator.GenerateField(Random.Range(_gameManager.Config.MinCellsToRemove, _gameManager.Config.MaxCellsToRemove));

        Debug.Log("Correct Field");
        LogMatrix(_correctField);

        Debug.Log("User Field");
        LogMatrix(_userField);

        foreach (Cell cell in GetComponentsInChildren<Cell>())
        {
            int column = (int)(cell.transform.position.x + 4 * CellSize);
            int row = (int)(4 * CellSize - cell.transform.position.y);

            _cells[row, column] = cell;
            _cells[row, column].InitCell(_userField[row, column]);
        }

        _input.OnTouchPerformed += OnTouchPerformedHandler;
    }

    private void OnDestroy()
    {
        _input.OnTouchPerformed -= OnTouchPerformedHandler;
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

                    var isCorrect = _gameManager.SelectedDigit == _correctField[row, column];
                    _cells[row, column].SetCell(_gameManager.SelectedDigit, isCorrect);

                    if (isCorrect)
                    {
                        if (IsFieldCompleted(_userField))
                        {
                            OnFieldCompleted?.Invoke();
                        }
                    }
                    else
                    {
                        OnIncorrectPlaced?.Invoke();
                    }
                }
            }
        }
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

#if UNITY_EDITOR
    [ContextMenu("CompleteField")]
    private void CompleteField()
    {
        OnFieldCompleted?.Invoke();
    }
#endif
}
