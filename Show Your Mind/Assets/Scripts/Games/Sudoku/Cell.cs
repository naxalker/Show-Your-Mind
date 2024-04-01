using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private TMP_Text _label;

    public void SetText(string text)
    {
        _label.text = text;
        _label.enabled = true;
    }
}
