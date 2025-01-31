using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _resultLabel;

    public void Show(string resultText)
    {
        gameObject.SetActive(true);
        GetComponent<RectTransform>().DOScale(1f, .5f);

        _resultLabel.text = resultText;
    }

    public void Hide()
    {
        GetComponent<RectTransform>().DOScale(0f, .5f)
            .OnComplete(() => gameObject.SetActive(false));
    }
}
