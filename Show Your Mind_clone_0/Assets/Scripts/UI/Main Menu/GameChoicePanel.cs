using DG.Tweening;
using Network.Client;
using Network.Client.Services;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Random = UnityEngine.Random;

public class GameChoicePanel : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GamePanel _gamePanelPrefab;
    [SerializeField] private RectTransform _gamePanelsHolder;

    [Header("Player Data")]
    [SerializeField] private TMP_Text _playerNameTitle;
    [SerializeField] private TMP_Text _playerScoreTitle;

    [Header("Navigation Buttons")]
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    [Header("Play Buttons")]
    [SerializeField] private Button _playSingleplayerButton;
    [SerializeField] private Button _playMultiplayerButton;

    [Header("Popup Panels")]
    [SerializeField] private DifficultyPanel _difficultyPanel;
    [SerializeField] private SearchingPanel _searchingPanel;

    private ClientManager _clientManager;
    private RectTransform _gamePanelRectTransform;
    private int _currentGameIndex = 0;
    private List<GamePanel> _gamePanels = new List<GamePanel>();

    private SceneLoaderMediator _sceneLoaderMediator;
    private AllGameConfigs _gameConfigs;

    [Inject]
    private void Construct(SceneLoaderMediator sceneLoaderMediator, AllGameConfigs gameConfigs)
    {
        _sceneLoaderMediator = sceneLoaderMediator;
        _gameConfigs = gameConfigs;
    }

    private List<GameConfig> GameConfigs => _gameConfigs.GameConfigs;

    void Awake()
    {
        _rightButton.interactable = GameConfigs.Count > 1;

        _leftButton.onClick.AddListener(() => 
        {
            float newXPos = _gamePanelsHolder.anchoredPosition.x + _gamePanelRectTransform.rect.width;

            _currentGameIndex--;

            _gamePanels[_currentGameIndex].gameObject.SetActive(true);
            _gamePanelsHolder.DOAnchorPosX(newXPos, .5f).OnComplete(() => _gamePanels[_currentGameIndex + 1].gameObject.SetActive(false));

            _rightButton.interactable = true;

            SetPlayButtons();
            if (_currentGameIndex == 0)
            {
                _leftButton.interactable = false;
            }
        });

        _rightButton.onClick.AddListener(() =>
        {
            float newXPos = _gamePanelsHolder.anchoredPosition.x - _gamePanelRectTransform.rect.width;

            _currentGameIndex++;

            _gamePanels[_currentGameIndex].gameObject.SetActive(true);
            _gamePanelsHolder.DOAnchorPosX(newXPos, .5f).OnComplete(() => _gamePanels[_currentGameIndex - 1].gameObject.SetActive(false));

            _leftButton.interactable = true;
            
            SetPlayButtons();
            if (_currentGameIndex == GameConfigs.Count - 1)
            {
                _rightButton.interactable = false;
            }
        });
    }

    private void Start()
    {
        _clientManager = ClientSingleton.Instance.Manager;

        SetGamePanels();
        SetPlayButtons();

        UpdatePlayerData();
    }

    private void SetGamePanels()
    {
        for (int i = 0; i < GameConfigs.Count; i++)
        {
            var gamePanel = Instantiate(_gamePanelPrefab, _gamePanelsHolder);
            _gamePanels.Add(gamePanel);

            var rectTransform = gamePanel.GetComponent<RectTransform>();

            if (i == 0)
            {
                gamePanel.gameObject.SetActive(true);
                _gamePanelRectTransform = rectTransform;
            }
            else
            {
                gamePanel.gameObject.SetActive(false);
            }

            var anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.x += i * rectTransform.rect.width;

            rectTransform.anchoredPosition = anchoredPosition;

            gamePanel.SetPanel(GameConfigs[i].Image, GameConfigs[i].Title);
        }
    }

    private void SetPlayButtons()
    {
        _playSingleplayerButton.onClick.RemoveAllListeners();
        _playMultiplayerButton.onClick.RemoveAllListeners();

        if (GameConfigs[_currentGameIndex].SingleplayerGameManager != null)
        {
            _playSingleplayerButton.interactable = true;

            if (GameConfigs[_currentGameIndex].GameConfigsByDifficulty.Count == 1)
            {
                _playSingleplayerButton.onClick.AddListener(() =>
                {
                    _sceneLoaderMediator.LoadSingleplayerScene(
                        GameConfigs[_currentGameIndex].GameMode, 
                        GameConfigs[_currentGameIndex].GameConfigsByDifficulty[0].DifficultyType);
                });
            }
            else
            {
                _playSingleplayerButton.onClick.AddListener(() =>
                {
                    _difficultyPanel.Show(GameConfigs[_currentGameIndex]);
                });
            }
        }
        else
        {
            _playSingleplayerButton.interactable = false;
        }

        if (GameConfigs[_currentGameIndex].MultiplayerGameManager != null)
        {
            _playMultiplayerButton.interactable = true;

            _playMultiplayerButton.onClick.AddListener(() =>
            {
                _clientManager.SetGameMode(GameConfigs[_currentGameIndex].GameMode);

                _searchingPanel.Show();

#pragma warning disable 4014
                _clientManager.MatchmakeAsync(OnMatchMade);
#pragma warning restore 4014
            });
        }
        else
        {
            _playMultiplayerButton.interactable = false;
        }
    }

    private void UpdatePlayerData()
    {
        _playerNameTitle.text = _clientManager.User.Name;

        //_playerScoreTitle.text = _clientManager.Leaderboard.GetPlayerScore().Result.Score.ToString();
    }

    void OnMatchMade(MatchmakerPollingResult result)
    {
        if (result == MatchmakerPollingResult.Success)
        {
            _sceneLoaderMediator.LoadMultiplayerScene(
                GameConfigs[_currentGameIndex].GameMode,
                (DifficultyType)Random.Range(0, Enum.GetValues(typeof(DifficultyType)).Length));
        }
        else
        {
            _searchingPanel.Hide();
        }
    }
}
