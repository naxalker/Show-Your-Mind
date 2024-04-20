using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;
    [SerializeField] private TMP_Text[] _notesDigits;

    private int _currentValue;
    private bool _isInteractable;

    public void InitCell(int value)
    {
        if (value != 0)
        {
            _label.text = value.ToString();
            _label.enabled = true;
        }
    }

    public void SetCell(int value, bool isCorrect = false)
    {
        if (_isInteractable == false) { return; }

        _currentValue = value;

        if (value != 0)
        {
            _label.text = value.ToString();

            if (isCorrect)
            {
                _label.color = Color.green;
                _isInteractable = false;
            }
            else
            {
                _label.color = Color.red;
            }

            _label.enabled = true;

            HideNoteDigits();
        }
        else
        {
            _label.enabled = false;
        }
    }

    public void SetNoteDigit(int digit)
    {
        if (_isInteractable == false) { return; }

        if (_currentValue != 0) { return; }

        _notesDigits[digit - 1].enabled = !_notesDigits[digit - 1].enabled;
    }

    private void HideNoteDigits()
    {
        foreach (var noteDigit in  _notesDigits)
        {
            noteDigit.enabled = false;
        }
    }
}
