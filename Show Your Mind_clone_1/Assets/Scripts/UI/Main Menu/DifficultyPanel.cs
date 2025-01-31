using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Image))]
public class DifficultyPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RectTransform _buttonsPanel;
    [SerializeField] private Button _easyModeButton;
    [SerializeField] private Button _normalModeButton;
    [SerializeField] private Button _hardModeButton;

    private Image _panelImage;

    private SceneLoaderMediator _sceneLoaderMediator;
    private ColorPalette _palette;

    [Inject]
    private void Construct(SceneLoaderMediator sceneLoaderMediator, ColorPalette palette)
    {
        _sceneLoaderMediator = sceneLoaderMediator;
        _palette = palette;
    }

    private void Awake()
    {
        _panelImage = GetComponent<Image>();
    }

    public void Show(GameConfig gameConfig)
    {
        gameObject.SetActive(true);

        SetButtons(gameConfig);

        Color halfBlack = Color.black;
        halfBlack.a = .5f;
        _panelImage.DOColor(halfBlack, .5f);

        _buttonsPanel.DOAnchorPosY(100, .5f);
    }

    public void Hide()
    {
        Color transparent = Color.black;
        transparent.a = .0f;
        _panelImage.DOColor(transparent, .5f);

        _buttonsPanel.DOAnchorPosY(-850, .5f)
            .OnComplete(() => gameObject.SetActive(false));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Hide();
    }

    private void SetButtons(GameConfig gameConfig)
    {
        SetButton(gameConfig, _easyModeButton, DifficultyType.Easy);
        SetButton(gameConfig, _normalModeButton, DifficultyType.Normal);
        SetButton(gameConfig, _hardModeButton, DifficultyType.Hard);
    }

    private void SetButton(GameConfig gameConfig, Button button, DifficultyType difficultyType)
    {
        button.onClick.RemoveAllListeners();

        var config = gameConfig.GameConfigsByDifficulty.FirstOrDefault(x => x.DifficultyType == difficultyType);

        if (config != null)
        {
            button.interactable = true;
            button.GetComponentInChildren<TMP_Text>().color = _palette.MainColor;

            button.onClick.AddListener((() =>
            {
                _sceneLoaderMediator.LoadSingleplayerScene(gameConfig.GameMode, difficultyType);
            }));
        }
        else
        {
            button.interactable = false;
            button.GetComponentInChildren<TMP_Text>().color = _palette.DarkFontColor;
        }
    }
}
