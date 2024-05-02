public class SceneLoader
{
    private ZenjectSceneLoaderWrapper _zenjectSceneLoader;

    public SceneLoader(ZenjectSceneLoaderWrapper zenjectSceneLoader)
    {
        _zenjectSceneLoader = zenjectSceneLoader;
    }

    public void Load(GameManager gameManager, GameConfig gameConfig)
    {
        _zenjectSceneLoader.Load(container =>
        {
            var gameManagerInstance = container.InstantiatePrefabForComponent<GameManager>(gameManager);
            gameManagerInstance.Initialize(gameConfig);
        }, (int)SceneID.SingleplayerScene);
    }

    public void Load<T>(MenuGameConfig gameConfig) where T : MultiplayerManager
    {
        _zenjectSceneLoader.Load(container =>
        {
            container.BindInterfacesAndSelfTo<T>().AsSingle().WithArguments(gameConfig).NonLazy();
        }, (int)SceneID.MultiplayerScene);
    }
}
