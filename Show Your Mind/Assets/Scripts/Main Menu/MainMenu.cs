using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainMenu : MonoBehaviour
{
    private const string ConfigPath = "Configs";

    [SerializeField] private GamePanel _gamePanelPrefab;
    [SerializeField] private RectTransform _gamePanelsHolder;

    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    [SerializeField] private Button _playSingleplayerButton;
    [SerializeField] private Button _playMultiplayerButton;

    [SerializeField] private DifficultyPanel _difficultyPanel;

    private MenuGameConfig[] _menuGameConfigs;
    private RectTransform _gamePanelRectTransform;
    private int _currentGameIndex = 0;

    private SceneLoader _sceneLoader;

    [Inject]
    private void Construct(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    void Awake()
    {
        _menuGameConfigs = Resources.LoadAll<MenuGameConfig>(ConfigPath);

        _leftButton.onClick.AddListener(() =>
        {
            float newXPos = _gamePanelsHolder.anchoredPosition.x + _gamePanelRectTransform.rect.width;
            _gamePanelsHolder.DOAnchorPosX(newXPos, .5f);

            _rightButton.interactable = true;

            _currentGameIndex--;
            SetPlayButtons();
            if (_currentGameIndex == 0)
            {
                _leftButton.interactable = false;
            }
        });

        _rightButton.onClick.AddListener(() =>
        {
            float newXPos = _gamePanelsHolder.anchoredPosition.x - _gamePanelRectTransform.rect.width;
            _gamePanelsHolder.DOAnchorPosX(newXPos, .5f);

            _leftButton.interactable = true;

            _currentGameIndex++;
            SetPlayButtons();
            if (_currentGameIndex == _menuGameConfigs.Length - 1)
            {
                _rightButton.interactable = false;
            }
        });
    }

    private void Start()
    {
        SetGamePanels();
        SetPlayButtons();
    }

    private void SetGamePanels()
    {
        for (int i = 0; i < _menuGameConfigs.Length; i++)
        {
            var gamePanel = Instantiate(_gamePanelPrefab, _gamePanelsHolder);

            var rectTransform = gamePanel.GetComponent<RectTransform>();

            if (i == 0)
            {
                _gamePanelRectTransform = rectTransform;
            }

            var anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.x += i * rectTransform.rect.width;

            rectTransform.anchoredPosition = anchoredPosition;

            gamePanel.SetPanel(_menuGameConfigs[i].Image, _menuGameConfigs[i].Title);
        }
    }

    private void SetPlayButtons()
    {
        _playSingleplayerButton.onClick.RemoveAllListeners();
        _playMultiplayerButton.onClick.RemoveAllListeners();

        if (_menuGameConfigs[_currentGameIndex].SingleplayerGameManager != null)
        {
            _playSingleplayerButton.interactable = true;

            if (_menuGameConfigs[_currentGameIndex].GameConfigsByDifficulty.Count == 1)
            {
                _playSingleplayerButton.onClick.AddListener(() =>
                {
                    _sceneLoader.Load(
                        _menuGameConfigs[_currentGameIndex].SingleplayerGameManager,
                        _menuGameConfigs[_currentGameIndex].GameConfigsByDifficulty[0].GameConfig);
                });
            }
            else
            {
                _playSingleplayerButton.onClick.AddListener(() =>
                {
                    _difficultyPanel.Show(_menuGameConfigs[_currentGameIndex]);
                });
            }
        }
        else
        {
            _playSingleplayerButton.interactable = false;
        }

        if (_menuGameConfigs[_currentGameIndex].MultiplayerGameManager != null)
        {
            _playMultiplayerButton.interactable = true;

            _sceneLoader.Load(SceneID.Test);
        }
        else
        {
            _playMultiplayerButton.interactable = false;
        }
    }
}
