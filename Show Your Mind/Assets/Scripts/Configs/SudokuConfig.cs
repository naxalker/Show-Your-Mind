using UnityEngine;

[CreateAssetMenu]
public class SudokuConfig : GameConfig
{
    [field: SerializeField, Range(0, 81)] public int MinCellsToRemove { get; private set; }
    [field: SerializeField, Range(0, 81)] public int MaxCellsToRemove { get; private set; }

    private void OnValidate()
    {
        MinCellsToRemove = Mathf.Clamp(MinCellsToRemove, 0, MaxCellsToRemove);
        MaxCellsToRemove = Mathf.Clamp(MaxCellsToRemove, MinCellsToRemove, 81);
    }
}
