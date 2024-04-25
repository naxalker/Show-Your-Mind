using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _resultLabel;

    public void SetResultText(string resultText)
    {
        _resultLabel.text = resultText;
    }
}
