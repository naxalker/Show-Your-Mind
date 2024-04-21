using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Zenject;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private Field _field;
    [SerializeField] private Transform _UICanvas;
    [SerializeField] private GameObject _bottomUIGroup;
    [SerializeField] private GameObject _topUIGroup;

    [SerializeField] private List<GameObject> _prefabs;

    [Inject]
    private DiContainer _container;

    private void Awake()
    {
        for (int i = 0; i < _prefabs.Count; i++)
        {
            NetworkManager.Singleton.PrefabHandler.AddHandler(_prefabs[i], new ZenjectNetCodeFactory(_prefabs[i], _container));
        }

        if (NetworkManager.Singleton.IsClient)
        {
            if (_bottomUIGroup != null)
                _container.InstantiatePrefab(_bottomUIGroup, _UICanvas);

            if (_topUIGroup != null)
                _container.InstantiatePrefab(_topUIGroup, _UICanvas);
        }
    }
}
