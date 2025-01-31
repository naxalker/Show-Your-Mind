using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameOverUI _gameOverUI;
    [SerializeField] private PauseUI _pauseUI;

    private Image _panelImage;

    private void Awake()
    {
        _panelImage = GetComponent<Image>();
    }

    public void ShowGameOverUI(string resultText)
    {
        Show();

        _gameOverUI.Show(resultText);
    }

    public void ShowPauseUI()
    {
        Show();

        _pauseUI.Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);

        Color halfBlack = Color.black;
        halfBlack.a = .5f;
        _panelImage.DOColor(halfBlack, .5f);
    }

    private void Hide()
    {
        Color transparent = Color.black;
        transparent.a = .0f;
        _panelImage.DOColor(transparent, .5f)
            .OnComplete(() => gameObject.SetActive(false));

        if (_gameOverUI.gameObject.activeSelf)
        {
            _gameOverUI.Hide();
        }
        else if (_pauseUI.gameObject.activeSelf)
        {
            _pauseUI.Hide();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_gameOverUI.gameObject.activeSelf == false)
        {
            Hide();
        }
    }
}
