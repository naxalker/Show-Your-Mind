using UnityEngine;

public class SudokuClientGameManager : ClientGameManager
{
    private bool _isEraseMode;
    private bool _isNotesMode;
    private int _selectedDigit;

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
        
    }
}
