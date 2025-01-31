using Zenject;

public abstract class MultiplayerManager : IInitializable
{
    protected readonly GameConfig Config;
    protected readonly DiContainer Container;

    public MultiplayerManager(DiContainer container, GameConfig config)
    {
        Config = config;
        Container = container;
    }

    public virtual void Initialize() { }
}
