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

    private SceneLoader _sceneLoader;
    private ColorPalette _palette;

    [Inject]
    private void Construct(SceneLoader sceneLoader, ColorPalette palette)
    {
        _sceneLoader = sceneLoader;
        _palette = palette;
    }

    private void Awake()
    {
        _panelImage = GetComponent<Image>();
    }

    public void Show(MenuGameConfig gameConfig)
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

    private void SetButtons(MenuGameConfig gameConfig)
    {
        SetButton(gameConfig, _easyModeButton, DifficultyType.Easy);
        SetButton(gameConfig, _normalModeButton, DifficultyType.Normal);
        SetButton(gameConfig, _hardModeButton, DifficultyType.Hard);
    }

    private void SetButton(MenuGameConfig gameConfig, Button button, DifficultyType difficultyType)
    {
        button.onClick.RemoveAllListeners();

        var config = gameConfig.GameConfigsByDifficulty.FirstOrDefault(x => x.DifficultyType == difficultyType);

        if (config != null)
        {
            button.interactable = true;
            button.GetComponentInChildren<TMP_Text>().color = _palette.MainColor;

            button.onClick.AddListener(() =>
            {
                _sceneLoader.Load(gameConfig.SingleplayerGameManager, config.GameConfig);
            });
        }
        else
        {
            button.interactable = false;
            button.GetComponentInChildren<TMP_Text>().color = _palette.DarkFontColor;
        }
    }
}
