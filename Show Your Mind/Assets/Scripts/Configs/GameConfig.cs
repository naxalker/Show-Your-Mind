using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu()]
public class GameConfig : ScriptableObject
{
    [field: SerializeField] public GameManager GameManagerPrefab { get; set; }
    [field: SerializeField] public DifficultyType Difficulty { get; set; }
}
