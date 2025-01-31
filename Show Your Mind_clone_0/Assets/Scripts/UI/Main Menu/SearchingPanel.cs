using DG.Tweening;
using Network.Client;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchingPanel : MonoBehaviour
{
    [SerializeField] private List<Image> _searchingImages;
    [SerializeField] private Button _cancelButton;

    private void Awake()
    {
        _cancelButton.onClick.AddListener(CancelMatchmaking);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        StartAnimation();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        StopAnimation();
    }

    private async void CancelMatchmaking()
    {
        await ClientSingleton.Instance.Manager.CancelMatchmaking();

        Hide();
    }

    private void StartAnimation()
    {
        for (int i = 0; i < _searchingImages.Count; i++)
        {
            _searchingImages[i].rectTransform.localScale = Vector3.zero;
        }

        for (int i = 0; i <  _searchingImages.Count; i++)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.SetDelay(i * .3f, false);
            sequence.Append(_searchingImages[i].rectTransform.DOScale(1f, .9f).SetEase(Ease.InOutQuad));
            sequence.Append(_searchingImages[i].rectTransform.DOScale(0f, .9f).SetEase(Ease.InOutQuad));
            sequence.SetLoops(-1);
        }
    }

    private void StopAnimation()
    {
        DOTween.KillAll();
    }
    
    private void OnDestroy()
    {
        StopAnimation();
    }
}
