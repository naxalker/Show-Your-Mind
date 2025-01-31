using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class GameManagerFactory
{
    private DiContainer _diContainer;
    private AllGameConfigs _gameConfigs;

    public GameManagerFactory(DiContainer diContainer, AllGameConfigs gameConfigs)
    {
        _diContainer = diContainer;
        _gameConfigs = gameConfigs;
    }

    private List<GameConfig> GameConfigs => _gameConfigs.GameConfigs;

    public GameManager GetGameManager(GameMode gameMode, DifficultyType difficultyType, out DifficultyConfig difficultyConfig)
    {
        var gameConfig = GameConfigs.FirstOrDefault(x => x.GameMode == gameMode);
        var managerPrefabToSpawn = gameConfig.SingleplayerGameManager;
        difficultyConfig = gameConfig.GameConfigsByDifficulty.FirstOrDefault(x => x.DifficultyType == difficultyType).GameDifficultyConfig;
        var gameManager = _diContainer.InstantiatePrefabForComponent<GameManager>(managerPrefabToSpawn);

        return gameManager;
    }

    public NetworkGameManager GetNetworkGameManager(GameMode gameMode, DifficultyType difficultyType, out DifficultyConfig difficultyConfig)
    {
        var gameConfig = GameConfigs.FirstOrDefault(x => x.GameMode == gameMode);
        var managerPrefabToSpawn = gameConfig.MultiplayerGameManager;
        difficultyConfig = gameConfig.GameConfigsByDifficulty.FirstOrDefault(x => x.DifficultyType == difficultyType).GameDifficultyConfig;

        var gameManager = Object.Instantiate(managerPrefabToSpawn);
        _diContainer.InjectGameObject(gameManager.gameObject);

        return gameManager;
    }
}
