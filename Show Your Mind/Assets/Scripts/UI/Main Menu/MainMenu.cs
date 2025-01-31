using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public enum MenuPanelType
    {
        Main,
        Leaderboard,
        Chat
    }

    [SerializeField] private PanelsHolder _panelsHodler;

    [SerializeField] private Button _homeButton;
    [SerializeField] private Button _leaderboardButton;
    [SerializeField] private Button _chatButton;

    private void Awake()
    {
        _homeButton.onClick.AddListener(() => _panelsHodler.Move(MenuPanelType.Main));
        _leaderboardButton.onClick.AddListener(() => _panelsHodler.Move(MenuPanelType.Leaderboard));
        _chatButton.onClick.AddListener(() => _panelsHodler.Move(MenuPanelType.Chat));
    }
}
