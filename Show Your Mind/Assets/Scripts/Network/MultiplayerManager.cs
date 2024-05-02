using Zenject;

public abstract class MultiplayerManager : IInitializable
{
    protected readonly MenuGameConfig Config;
    protected readonly DiContainer Container;

    public MultiplayerManager(DiContainer container, MenuGameConfig config)
    {
        Config = config;
        Container = container;
    }

    public virtual void Initialize() { }
}
