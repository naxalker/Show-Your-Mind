using Unity.Netcode;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    [SerializeField] private Transform _UICanvas;
    [SerializeField] private GameObject _bottomUIGroup;
    [SerializeField] private GameObject _topUIGroup;

    private void Awake()
    {
        var gameManager = Instantiate(_gameManager);
        gameManager.GetComponent<NetworkObject>().Spawn();
        gameManager.Init();

        if (_bottomUIGroup != null)
            Instantiate(_bottomUIGroup, _UICanvas);

        if (_topUIGroup != null)
            Instantiate(_topUIGroup, _UICanvas);
    }
}
