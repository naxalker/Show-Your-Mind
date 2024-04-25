using System;
using TMPro;
using UnityEngine;
using Zenject;

public class GameTime : MonoBehaviour
{
    private SudokuGameManager _gameManager;

    private TMP_Text _timeLabel;

    [Inject]
    private void Construct(SudokuGameManager gameManager)
    {
        Debug.Log(name);
        _gameManager = gameManager;
    }

    private void Start()
    {
        _gameManager.GameTime.OnValueChanged += GameTimeValueChangedHandler;
        _timeLabel = GetComponent<TMP_Text>();
    }

    private void GameTimeValueChangedHandler(float previousValue, float newValue)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(newValue);
        _timeLabel.text = "Время:\n" + timeSpan.ToString(@"mm\:ss");
    }
}
