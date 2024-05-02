using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ConnectionButtons : MonoBehaviour
{
    [SerializeField] private Button _serverButton;
    [SerializeField] private Button _clientButton;
    [SerializeField] private MenuGameConfig _gameConfig;

    private SceneLoader _sceneLoader;

    [Inject]
    private void Construct(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    private void Awake()
    {
        _serverButton.onClick.AddListener(() =>
        {
            _sceneLoader.Load<ServerManager>(_gameConfig);
        });

        _clientButton.onClick.AddListener(() =>
        {
            _sceneLoader.Load<ClientManager>(_gameConfig);
        });
    }
}
