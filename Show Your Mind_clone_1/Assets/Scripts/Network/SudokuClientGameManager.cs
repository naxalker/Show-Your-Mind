using UnityEngine;

public class SudokuClientGameManager : ClientGameManager
{
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

    public override void Init()
    {
        
    }
}
