using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GameConfig : ScriptableObject
{
    [Serializable]
    public class DifficultyByType
    {
        public DifficultyType DifficultyType;
        public DifficultyConfig GameDifficultyConfig;
    }

    [field: SerializeField] public GameMode GameMode { get; private set; }
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public GameManager SingleplayerGameManager { get; private set; }
    [field: SerializeField] public NetworkGameManager MultiplayerGameManager { get; private set; }
    [field: SerializeField] public List<DifficultyByType> GameConfigsByDifficulty { get; private set; }
}
