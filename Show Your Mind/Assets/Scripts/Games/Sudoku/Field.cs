using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class Field : MonoBehaviour
{
    private const float CellSize = 1f;

    private MobileInput _input;

    private Cell[,] _cells = new Cell[9, 9];
    private int[,] _correctField = new int[9, 9];
    private int[,] _userField = new int[9, 9];

    [Inject]
    private void Construct(MobileInput input)
    {
        _input = input;
    }

    private void Awake()
    {
        foreach (Cell cell in GetComponentsInChildren<Cell>())
        {
            int xIndex = (int)(cell.transform.position.x + 4 * CellSize);
            int yIndex = (int)(4 * CellSize - cell.transform.position.y);

            _cells[xIndex, yIndex] = cell;
        }

        (_correctField, _userField) = FieldGenerator.GenerateField(39);

        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                _cells[i, j].InitCell(_correctField[i, j], _userField[i, j] == 0);
            }
        }
    }

    private void OnEnable()
    {
        _input.OnTouchPerformed += OnTouchPerformedHandler;
    }

    private void OnDisable()
    {
        _input.OnTouchPerformed -= OnTouchPerformedHandler;
    }

    private void OnTouchPerformedHandler(Vector3 pos)
    {
        float minBound = -4 * CellSize - CellSize / 2;
        float maxBound = 4 * CellSize + CellSize / 2;

        if (pos.x >= minBound && pos.x < maxBound && pos.y >= minBound && pos.y < maxBound)
        {
            int xIndex = Mathf.FloorToInt(pos.x + maxBound);
            int yIndex = Mathf.FloorToInt(maxBound - pos.y);
            _cells[xIndex, yIndex].SetCell(_correctField[xIndex, yIndex]);
        }
    }
}
