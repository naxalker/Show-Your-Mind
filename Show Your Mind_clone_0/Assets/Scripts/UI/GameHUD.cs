using UnityEngine;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;

    [SerializeField] private GameOverUI _gameOverUI;

    public void ShowGameOverUI(string resultText = "")
    {
        _gameOverUI.gameObject.SetActive(true);
        _gameOverUI.SetResultText(resultText);

        _pausePanel.SetActive(true);
    }

    public void SetGameOverResult(string resultText)
    {
        _gameOverUI.SetResultText(resultText);
    }
}
