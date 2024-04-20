using System.Collections.Generic;
using UnityEngine;

public static class FieldGenerator
{
    private static int[,] _correctField = new int[9, 9];
    private static int[,] _userField = new int[9, 9];

    public static (int[,], int[,]) GenerateField(int cellsToRemove)
    {
        ResetFields();

        GenerateDiagonal();
        GenerateRemaining(0, 3);
        RemoveNumbers(cellsToRemove);

        return ((int[,])_correctField.Clone(), (int[,])_userField.Clone());
    }

    private static void GenerateDiagonal()
    {
        for (int i = 0; i < 9; i += 3)
        {
            GenerateBox(i, i);
        }
    }

    private static void GenerateBox(int row, int col)
    {
        List<int> availableNumbers = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int num = availableNumbers[Random.Range(0, availableNumbers.Count)];

                _correctField[row + i, col + j] = num;
                availableNumbers.Remove(num);
            }
        }
    }

    private static bool GenerateRemaining(int row, int col)
    {
        if (row == 9 - 1 && col == 9)
            return true;

        if (col == 9)
        {
            row++;
            col = 0;
        }

        if (_correctField[row, col] != 0)
            return GenerateRemaining(row, col + 1);

        for (int num = 1; num <= 9; num++)
        {
            if (IsSafe(row, col, num))
            {
                _correctField[row, col] = num;

                if (GenerateRemaining(row, col + 1))
                    return true;
            }

            _correctField[row, col] = 0;
        }

        return false;
    }

    private static bool IsSafe(int row, int col, int num)
    {
        // Проверка строки
        for (int i = 0; i < _correctField.GetLength(0); i++)
        {
            if (_correctField[row, i] == num)
            {
                return false;
            }
        }

        // Проверка столбца
        for (int i = 0; i < _correctField.GetLength(0); i++)
        {
            if (_correctField[i, col] == num)
            {
                return false;
            }
        }

        // Проверка 3x3 подквадрата
        int sqrt = 3;
        int boxRowStart = row - row % sqrt;
        int boxColStart = col - col % sqrt;

        for (int i = boxRowStart; i < boxRowStart + sqrt; i++)
        {
            for (int j = boxColStart; j < boxColStart + sqrt; j++)
            {
                if (_correctField[i, j] == num)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static void RemoveNumbers(int count)
    {
        int removed = 0;

        _userField = (int[,])_correctField.Clone();

        while (removed < count)
        {
            int row = Random.Range(0, 9);
            int col = Random.Range(0, 9);

            if (_userField[row, col] != 0)
            {
                _userField[row, col] = 0;
                removed++;
            }
        }
    }

    private static void ResetFields()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                _correctField[i, j] = 0;
                _userField[i, j] = 0;
            }
        }
    }
}
