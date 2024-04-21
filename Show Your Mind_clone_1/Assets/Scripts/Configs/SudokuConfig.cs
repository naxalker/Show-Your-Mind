using UnityEngine;

[CreateAssetMenu()]
public class SudokuConfig : ScriptableObject
{
    [field: SerializeField] public Field FieldPrefab { get; private set; }
}
