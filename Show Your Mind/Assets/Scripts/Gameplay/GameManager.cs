using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using Zenject;

public abstract class GameManager : MonoBehaviour, IGameResultValidator, ITimeCounter, IDifficultyProvider
{
    public event Action<GameOverResultType, ulong> OnGameOver;
    public event Action<float> OnGameTimeChanged;

    public bool IsGameActive { get; private set; }
    public float GameTime { get; private set; }
    public DifficultyType DifficultyType { get; private set; }

    protected DiContainer Container { get; private set; }
    protected DifficultyConfig Config { get; private set; }
    protected GameHUD GameHUD { get; private set; }
    
    [Inject]
    private void Construct(DiContainer container)
    {
        Container = container;

        BindSelfAndInterfaces();
    }

    public virtual void Initialize(DifficultyConfig config, GameHUD gameHUD)
    {
        Config = config;
        DifficultyType = Config.DifficultyType;

        GameHUD = gameHUD;
        GameHUD.Initalize(this);
    }

    protected virtual void Start()
    {
        Debug.Log(GetType().Name);

        IsGameActive = true;
        GameTime = 0;
        StartCoroutine(UpdateGameTime());
    }

    public void ProcessGameOver(GameOverResultType resultType, ulong clientId = 0)
    {
        IsGameActive = false;
        StopAllCoroutines();

        OnGameOver?.Invoke(resultType, clientId);
    }

    private void BindSelfAndInterfaces()
    {
        Type derivedType = GetType();
        MethodInfo bindMethod = typeof(DiContainer)
            .GetMethod("BindInterfacesAndSelfTo", Type.EmptyTypes)
            .MakeGenericMethod(derivedType);

        var binding = bindMethod.Invoke(Container, null);
        var fromInstanceMethod = binding.GetType().GetMethod("FromInstance", new Type[] { derivedType });
        fromInstanceMethod.Invoke(binding, new object[] { this });

        var asSingleMethod = binding.GetType().GetMethod("AsSingle", Type.EmptyTypes);
        asSingleMethod.Invoke(binding, null);
    }

    private IEnumerator UpdateGameTime()
    {
        var delay = new WaitForSecondsRealtime(1f);

        while (true)
        {
            yield return delay;

            GameTime += 1f;

            OnGameTimeChanged?.Invoke(GameTime);
        }
    }
}
