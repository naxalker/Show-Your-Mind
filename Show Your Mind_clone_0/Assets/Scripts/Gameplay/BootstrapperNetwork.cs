using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class BootstrapperNetwork : MonoBehaviour
{
    [SerializeField] private GameHUD _gameHUD;
    [SerializeField] private List<NetworkObject> _networkObjects;

    private GameManagerFactory _gameManagerFactory;
    private GameMode _gameType;
    private DifficultyType _difficultyType;
    private DiContainer _diContainer;

    [Inject]
    private void Construct(GameManagerFactory gameManagerFactory, GameMode gameType, DifficultyType difficultyType, DiContainer diContainer)
    {
        _gameManagerFactory = gameManagerFactory;
        _gameType = gameType;
        _difficultyType = difficultyType;
        _diContainer = diContainer;
    }

    private void Awake()
    { 
        for (int i = 0; i < _networkObjects.Count; i++)
        {
            NetworkManager.Singleton.PrefabHandler.AddHandler(_networkObjects[i].gameObject, new ZenjectNetCodeFactory(_networkObjects[i].gameObject, _diContainer));
        }

        if (NetworkManager.Singleton.IsServer)
        {
            var gameManager = _gameManagerFactory.GetNetworkGameManager(_gameType, _difficultyType, out DifficultyConfig difficultyConfig);
            gameManager.Initialize(difficultyConfig);
            gameManager.GetComponent<NetworkObject>().Spawn();
        }
    }
}
