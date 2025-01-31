using UnityEngine;
using Zenject;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameHUD _gameHUD;

    private GameManagerFactory _gameManagerFactory;
    private GameMode _gameMode;
    private DifficultyType _difficultyType;

    [Inject]
    private void Construct(GameManagerFactory gameManagerFactory, GameMode gameMode, DifficultyType difficultyType)
    {
        _gameManagerFactory = gameManagerFactory;
        _gameMode = gameMode;
        _difficultyType = difficultyType;
    }

    private void Start()
    {
        var gameManager = _gameManagerFactory.GetGameManager(_gameMode, _difficultyType, out DifficultyConfig difficultyConfig);
        gameManager.Initialize(difficultyConfig, _gameHUD);
    }
}
