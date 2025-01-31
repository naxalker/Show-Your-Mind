internal interface ISudokuModeHandler
{
    public bool IsEraseMode { get; set; }
    public bool IsNotesMode { get; set; }
    public int SelectedDigit { get; set; }
}
