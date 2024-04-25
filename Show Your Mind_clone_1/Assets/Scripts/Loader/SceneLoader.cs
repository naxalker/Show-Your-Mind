using UnityEngine;

public class SceneLoader
{
    private ZenjectSceneLoaderWrapper _zenjectSceneLoader;

    public SceneLoader(ZenjectSceneLoaderWrapper zenjectSceneLoader)
    {
        _zenjectSceneLoader = zenjectSceneLoader;
    }

    public void LoadServer(GameConfig gameConfig)
    {
        _zenjectSceneLoader.Load(container =>
        {
            container.BindInterfacesAndSelfTo<ServerManager>().AsSingle().WithArguments(gameConfig).NonLazy();
        }, (int)SceneID.Gameplay);
    }

    public void LoadClient(GameConfig gameConfig)
    {
        _zenjectSceneLoader.Load(container =>
        {
            container.BindInterfacesAndSelfTo<ClientManager>().AsSingle().WithArguments(gameConfig).NonLazy();
        }, (int)SceneID.Gameplay);
    }
}
