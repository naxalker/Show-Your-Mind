using DG.Tweening;
using TMPro;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    public void Show()
    {
        gameObject.SetActive(true);
        GetComponent<RectTransform>().DOScale(1f, .5f);
    }

    public void Hide()
    {
        GetComponent<RectTransform>().DOScale(0f, .5f)
            .OnComplete(() => gameObject.SetActive(false));
    }
}
