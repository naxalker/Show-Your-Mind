using System;
using UnityEngine;

public class SudokuGameManager : GameManager, IDisposable
{
    [SerializeField] private Field _fieldPrefab;

    private bool _isEraseMode;
    private bool _isNotesMode;
    private int _selectedDigit;
    private int _attempts = 3;

    private Field _field;

    public bool IsEraseMode
    {
        get { return _isEraseMode; }
        set
        {
            _isEraseMode = value;

            _isNotesMode = false;
            _selectedDigit = 0;
        }
    }

    public bool IsNotesMode
    {
        get { return _isNotesMode; }
        set
        {
            _isNotesMode = value;

            _isEraseMode = false;
        }
    }

    public int SelectedDigit
    {
        get { return _selectedDigit; }
        set { _selectedDigit = value; }
    }

    public override void Init()
    {
        _field = Instantiate(_fieldPrefab);
        _field.Init(this);

        _field.OnFieldCompleted += FieldCompletedHandler;
        _field.OnIncorrectPlaced += IncorrectPlacedHandler;
    }

    private void FieldCompletedHandler()
    {
        Debug.Log("Victory!!!");
        _isGameActive = false;
    }

    private void IncorrectPlacedHandler()
    {
        _attempts--;
        if (_attempts <= 0)
        {
            Debug.Log("Game Over...");
            _isGameActive = false;
        }
    }

    public void Dispose()
    {
        if (IsClient)
        {
            _field.OnFieldCompleted -= FieldCompletedHandler;
            _field.OnIncorrectPlaced -= IncorrectPlacedHandler;
        }
    }
}
