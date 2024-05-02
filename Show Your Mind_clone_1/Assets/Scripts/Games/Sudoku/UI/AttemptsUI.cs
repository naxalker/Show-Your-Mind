using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AttemptsUI : MonoBehaviour
{
    [SerializeField] private Image[] _healthImages;

    private SudokuGameManager _gameManager;

    [Inject]
    private void Construct(SudokuGameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        _gameManager.OnAttemptSpent += AttemptSpentHandler;
    }

    private void OnDestroy()
    {
        _gameManager.OnAttemptSpent -= AttemptSpentHandler;
    }

    private void AttemptSpentHandler(uint leftAttempts)
    {
        Color color = _healthImages[leftAttempts].color;
        color.a = 0;
        _healthImages[leftAttempts].color = color;
    }
}
