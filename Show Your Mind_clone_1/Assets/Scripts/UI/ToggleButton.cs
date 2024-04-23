using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ToggleButton : MonoBehaviour
{
    public event Action<bool> OnToggled;

    [field: SerializeField] public Button Button {  get; private set; }
    private bool _isSelected;

    private ColorPalette _palette;

    [Inject]
    private void Construct(ColorPalette palette)
    {
        _palette = palette;
    }

    private void Awake()
    {
        Button.onClick.AddListener(() =>
        {
            _isSelected = !_isSelected;

            if (_isSelected)
            {
                Button.image.DOColor(_palette.MainColor, 1f);
            }
            else
            {
                Button.image.DOColor(Color.white, 1f);
            }

            OnToggled?.Invoke(_isSelected);
        });
    }

    public void Unselect()
    {
        _isSelected = false;
        OnToggled?.Invoke(_isSelected);
        Button.image.DOColor(Color.white, 1f);
    }
}
