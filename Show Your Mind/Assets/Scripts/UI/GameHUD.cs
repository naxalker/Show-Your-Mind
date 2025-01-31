using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private PausePanel _pausePanel;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private Button _muteButton;

    private SceneLoaderMediator _sceneLoaderMediator;
    private IGameResultValidator _gameResultValidator;

    [Inject]
    private void Construct(SceneLoaderMediator sceneLoaderMediator)
    {
        _sceneLoaderMediator = sceneLoaderMediator;
    }

    private void Awake()
    {
        _pauseButton.onClick.AddListener(() => _pausePanel.ShowPauseUI());
    }

    public void Initalize(IGameResultValidator gameResultValidator)
    {
        _gameResultValidator = gameResultValidator;

        _gameResultValidator.OnGameOver += GameOverHandler;
    }

    private void OnDestroy()
    {
        _gameResultValidator.OnGameOver -= GameOverHandler;
    }

    private void ShowGameOverUI(string resultText = "")
    {
        _pausePanel.ShowGameOverUI(resultText);
    }

    public void ToMainMenu()
    {
        _sceneLoaderMediator.LoadMenu();
    }

    private void GameOverHandler(GameOverResultType type, ulong clientId)
    {
        Debug.Log("Game Over");

        var resultText = type switch
        {
            GameOverResultType.UserWon => clientId == NetworkManager.Singleton.LocalClientId ? "�����������, �� ��������!!!" : "� ���������, �� ���������...",
            GameOverResultType.UserLost => clientId == NetworkManager.Singleton.LocalClientId ? "� ���������, �� ���������..." : "�����������, �� ��������!!!",
            GameOverResultType.UserLeft => "�����������, �� ��������!!! ��� �������� ������� ����.",
            _ => ""
        };

        ShowGameOverUI(resultText);
    }
}
