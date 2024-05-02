using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ActionButtonsController : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Toggle _eraseButton;
    [SerializeField] private Toggle _notesModeButton;
    [SerializeField] private Toggle[] _digitButtons;

    private ISudokuModeHandler _modeHandler;
    private ColorPalette _palette;

    private Toggle _currentDigitButton;

    [Inject]
    private void Construct(ISudokuModeHandler modeHandler, ColorPalette palette)
    {
        _modeHandler = modeHandler;
        _palette = palette;
    }

    private void Awake()
    {
        _backButton.onClick.AddListener(() =>
        {
        });
    }

    private void OnEnable()
    {
        _eraseButton.onValueChanged.AddListener((isOn) =>
        {
            if (isOn && _currentDigitButton != null)
            {
                _currentDigitButton.isOn = false;
            }

            _modeHandler.IsEraseMode = isOn;
            ChangeButtonColor(_eraseButton);
        });

        _notesModeButton.onValueChanged.AddListener((isOn) =>
        {
            _modeHandler.IsNotesMode = isOn;
            ChangeButtonColor(_notesModeButton);
        });

        for (int i = 0; i < _digitButtons.Length; i++)
        {
            int localI = i;

            _digitButtons[i].onValueChanged.AddListener((isOn) =>
            {
                if (isOn)
                {
                    _currentDigitButton = _digitButtons[localI];
                    _modeHandler.SelectedDigit = localI + 1;
                    _eraseButton.isOn = false;
                }
                else
                {
                    _modeHandler.SelectedDigit = 0;
                }

                ChangeButtonColor(_digitButtons[localI]);
            });
        }
    }

    private void ChangeButtonColor(Toggle button)
    {
        if (button.isOn)
        {
            button.image.DOColor(_palette.MainColor, .3f);
        }
        else
        {
            button.image.DOColor(Color.white, .3f);
        }
    }
}
