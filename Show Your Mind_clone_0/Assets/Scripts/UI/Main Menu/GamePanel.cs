using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private Image _gameImage;
    [SerializeField] private TMP_Text _gameTitle;

    public void SetPanel(Sprite gameSprite, string gameTitle)
    {
        _gameImage.sprite = gameSprite;
        _gameTitle.text = gameTitle;
    }
}
