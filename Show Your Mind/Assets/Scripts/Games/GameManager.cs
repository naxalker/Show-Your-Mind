using System;
using System.Collections;
using UnityEngine;
using Zenject;

public abstract class GameManager : MonoBehaviour, IGameResultValidator, ITimeCounter
{
    public event Action<GameOverResultType> OnGameOver;
    public event Action<float> GameTimeChanged;

    public bool IsGameActive { get; private set; }
    public float GameTime { get; private set; }
    
    protected DiContainer Container { get; private set; }

    [Inject]
    private void Construct(DiContainer container)
    {
        Container = container;
    }

    public virtual void Initialize(GameConfig config)
    {
        Container.Bind<ITimeCounter>().FromInstance(this);
    }

    protected virtual void Start()
    {
        Debug.Log(GetType().Name);

        IsGameActive = true;
        GameTime = 0;
        StartCoroutine(UpdateGameTime());
    }

    public void ProcessGameOver(GameOverResultType resultType)
    {
        IsGameActive = false;
        StopAllCoroutines();

        OnGameOver?.Invoke(resultType);
    }

    private IEnumerator UpdateGameTime()
    {
        var delay = new WaitForSecondsRealtime(1f);

        while (true)
        {
            yield return delay;

            GameTime += 1f;

            GameTimeChanged?.Invoke(GameTime);
        }
    }
}
