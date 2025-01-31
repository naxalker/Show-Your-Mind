using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(Slider))]
public class OpponentProgressBar : MonoBehaviour
{
    private Slider _slider;

    private SudokuNetworkGameManager _gameManager;

    [Inject]
    private void Construct(SudokuNetworkGameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _gameManager.OnOpponentProgressChanged += OpponentProgressChangedHandler;
    }

    private void OnDestroy()
    {
        _gameManager.OnOpponentProgressChanged -= OpponentProgressChangedHandler;
    }

    private void OpponentProgressChangedHandler(float value)
    {
        SetSlider(value);
    }

    private void SetSlider(float value)
    {
        value = Mathf.Clamp01(value);
        _slider.DOValue(value, .5f);
    }
}
