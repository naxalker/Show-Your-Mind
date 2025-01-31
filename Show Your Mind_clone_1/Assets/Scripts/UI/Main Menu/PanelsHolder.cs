using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static MainMenu;

public class PanelsHolder : MonoBehaviour
{
    [SerializeField] private List<RectTransform> _panels;

    private RectTransform _holderRectTransform;
    private MenuPanelType _currentPanelType;

    private void Start()
    {
        _holderRectTransform = GetComponent<RectTransform>();

        for (int i = 0; i < _panels.Count; i++)
        {
            _panels[i].DOAnchorPosX(i * _panels[0].rect.width, 0f);
            _panels[i].gameObject.SetActive(true);
        }
    }

    public void Move(MenuPanelType panelType)
    {
        if (_currentPanelType != panelType)
        {
            _currentPanelType = panelType;
            _holderRectTransform.DOAnchorPosX(-(int)panelType * _panels[0].rect.width, .5f);
        }
    }
}
