using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AttemptsUI : MonoBehaviour
{
    [SerializeField] private Image[] _healthImages;

    private IAttemptsCounter _attemptsCounter;

    [Inject]
    private void Construct(IAttemptsCounter attemptsCounter)
    {
        _attemptsCounter = attemptsCounter;
    }

    private void Start()
    {
        _attemptsCounter.OnAttemptSpent += AttemptSpentHandler;
    }

    private void OnDestroy()
    {
        _attemptsCounter.OnAttemptSpent -= AttemptSpentHandler;
    }

    private void AttemptSpentHandler(uint leftAttempts)
    {
        Color color = _healthImages[leftAttempts].color;
        color.a = 0;
        _healthImages[leftAttempts].color = color;
    }
}
