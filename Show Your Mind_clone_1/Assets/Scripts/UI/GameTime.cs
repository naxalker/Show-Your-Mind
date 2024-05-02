using System;
using TMPro;
using UnityEngine;
using Zenject;

public class GameTime : MonoBehaviour
{
    private ITimeCounter _timeCounter;

    private TMP_Text _timeLabel;

    [Inject]
    private void Construct(ITimeCounter timeCounter)
    {
        _timeCounter = timeCounter;
    }

    private void Start()
    {
        _timeCounter.GameTimeChanged += GameTimeValueChangedHandler;
        _timeLabel = GetComponent<TMP_Text>();
    }

    private void OnDestroy()
    {
        _timeCounter.GameTimeChanged -= GameTimeValueChangedHandler;
    }

    private void GameTimeValueChangedHandler(float newTime)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(newTime);
        _timeLabel.text = "Время:\n" + timeSpan.ToString(@"mm\:ss");
    }
}
