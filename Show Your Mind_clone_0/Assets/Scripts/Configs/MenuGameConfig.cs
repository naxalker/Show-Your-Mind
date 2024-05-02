using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MenuGameConfig : ScriptableObject
{
    [Serializable]
    public class DifficultyConfig
    {
        public DifficultyType DifficultyType;
        public GameConfig GameConfig;
    }

    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public Sprite Image { get; private set; }
    [field: SerializeField] public GameManager SingleplayerGameManager { get; private set; }
    [field: SerializeField] public NetworkGameManager MultiplayerGameManager { get; private set; }
    [field: SerializeField] public List<DifficultyConfig> GameConfigsByDifficulty { get; private set; }
}
