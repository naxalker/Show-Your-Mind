using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ConnectionButtons : MonoBehaviour
{
    [SerializeField] private Button _serverButton;
    [SerializeField] private Button _clientButton;

    private SceneLoaderMediator _sceneLoaderMediator;

    [Inject]
    private void Construct(SceneLoaderMediator sceneLoaderMediator)
    {
        _sceneLoaderMediator = sceneLoaderMediator;
    }

    private void Awake()
    {
        _serverButton.onClick.AddListener(() =>
        {
            _sceneLoaderMediator.LoadSudokuServer();
        });

        _clientButton.onClick.AddListener(() =>
        {
            _sceneLoaderMediator.LoadSudokuClient();
        });
    }
}
