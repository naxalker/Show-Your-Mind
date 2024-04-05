using System;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;

    private int _correctValue;
    private bool _isInteractable;

    public void InitCell(int correctValue, bool isInteractable)
    {
        if (_correctValue != 0)
            throw new Exception("Cell is already inited!");

        _correctValue = correctValue;
        _isInteractable = isInteractable;

        if (_isInteractable == false)
        {
            _label.text = _correctValue.ToString();
            _label.enabled = true;
        }
    }

    public void SetCell(int value)
    {
        if (_isInteractable == false) { return; }

        if (value != 0)
        {
            _label.text = value.ToString();

            if (value == _correctValue)
            {
                _label.color = Color.green;
                _isInteractable = false;
            }
            else
            {
                _label.color = Color.red;
            }

            _label.enabled = true;
        }
        else
        {
            _label.enabled = false;
        }
    }
}
