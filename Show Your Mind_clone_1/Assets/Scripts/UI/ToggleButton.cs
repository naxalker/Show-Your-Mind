using System;
using UnityEngine;
using UnityEngine.UI;

public class ToggleButton : MonoBehaviour
{
    public event Action<bool> OnToggled;

    [field: SerializeField] public Button Button {  get; private set; }
    private bool _isSelected;

    private void Awake()
    {
        Button.onClick.AddListener(() =>
        {
            _isSelected = !_isSelected;

            if (_isSelected)
            {
                Button.image.color = Color.green;
            }
            else
            {
                Button.image.color = Color.white;
            }

            OnToggled?.Invoke(_isSelected);
        });
    }

    public void Unselect()
    {
        _isSelected = false;
        Button.image.color = Color.white;
    }
}
