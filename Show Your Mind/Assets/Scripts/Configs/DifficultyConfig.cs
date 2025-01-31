using UnityEngine;

public abstract class DifficultyConfig : ScriptableObject
{
    [field: SerializeField] public DifficultyType DifficultyType { get; private set; }
}
