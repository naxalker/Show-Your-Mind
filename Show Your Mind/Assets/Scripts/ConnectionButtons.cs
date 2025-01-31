using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class ConnectionButtons : MonoBehaviour
{
    [SerializeField] private Button _serverButton;
    [SerializeField] private Button _clientButton;

    private SceneLoaderMediator _sceneLoader;
    private DiContainer _container;

    [Inject]
    private void Construct(SceneLoaderMediator sceneLoader, DiContainer container)
    {
        _sceneLoader = sceneLoader;
        _container = container;
    }

    private void Awake()
    {
        _serverButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartServer();

            _sceneLoader.LoadMultiplayerScene(
                GameMode.Sudoku,
                (DifficultyType)Random.Range(0, Enum.GetValues(typeof(DifficultyType)).Length));
        });

        _clientButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();

            _sceneLoader.LoadMultiplayerScene(
                GameMode.Sudoku,
                (DifficultyType)Random.Range(0, Enum.GetValues(typeof(DifficultyType)).Length));
        });
    }
}
