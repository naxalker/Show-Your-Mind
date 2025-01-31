using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BoostrapperTest : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private DifficultyConfig _difficultyConfig;
    [SerializeField] private GameHUD _gameHUD;

    [Inject] private DiContainer _container;

    private void Start()
    {
        var gameManager = _container.InstantiatePrefabForComponent<GameManager>(_gameManager);
        gameManager.Initialize(_difficultyConfig, _gameHUD);
    }
}
