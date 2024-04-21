using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ActionButtonsController : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private ToggleButton _eraseButton;
    [SerializeField] private ToggleButton _notesModeButton;
    [SerializeField] private ToggleButton[] _digitButtons;

    private SudokuClientGameManager _gameManager;

    private Action<bool>[] delegates = new Action<bool>[9];

    [Inject]
    private void Construct(SudokuClientGameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Awake()
    {
        _backButton.onClick.AddListener(() =>
        {
        });
    }

    private void OnEnable()
    {
        _eraseButton.OnToggled += EraseButtonToggledHandler;
        _notesModeButton.OnToggled += NotesModeButtonToggledHandler;

        for (int i = 0; i < _digitButtons.Length; i++)
        {
            int localI = i;

            Action<bool> handler = value =>
            {
                DigitButtonToggledHandler(localI, value);
            };

            delegates[i] = handler;
            _digitButtons[i].OnToggled += handler;
        }
    }

    private void OnDisable()
    {
        _eraseButton.OnToggled -= EraseButtonToggledHandler;
        _notesModeButton.OnToggled -= NotesModeButtonToggledHandler;

        for (int i = 0; i < _digitButtons.Length; i++)
        {
            _digitButtons[i].OnToggled -= delegates[i];
        }
    }

    private void EraseButtonToggledHandler(bool isSelected)
    {
        if (isSelected)
        {
            _gameManager.IsEraseMode = true;
            _notesModeButton.Unselect();
        }
        else
        {
            _gameManager.IsEraseMode = false;
            _eraseButton.Unselect();
        }
    }

    private void NotesModeButtonToggledHandler(bool isSelected)
    {
        if (isSelected)
        {
            _gameManager.IsNotesMode = true;
            _eraseButton.Unselect();
        }
        else
        {
            _gameManager.IsNotesMode = false;
            _notesModeButton.Unselect();
        }
    }

    private void DigitButtonToggledHandler(int buttonIndex, bool isSelected)
    {
        if (isSelected)
        {
            for (int i = 0; i < _digitButtons.Length; i++)
            {
                if (i == buttonIndex) continue;

                _digitButtons[i].Unselect();
            }
            _eraseButton.Unselect();

            _gameManager.SelectedDigit = buttonIndex + 1;
        }
        else
        {
            _gameManager.SelectedDigit = 0;
        }
    }
}
