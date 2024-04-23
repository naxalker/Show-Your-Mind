public class SceneLoader
{
    private ZenjectSceneLoaderWrapper _zenjectSceneLoader;

    public SceneLoader(ZenjectSceneLoaderWrapper zenjectSceneLoader)
    {
        _zenjectSceneLoader = zenjectSceneLoader;
    }

    public void Load<T>(params object[] arguments) where T : GameManager
    {
        _zenjectSceneLoader.Load(container =>
        {
            container.BindInterfacesAndSelfTo<T>().AsSingle().WithArguments(arguments).NonLazy();
        }, (int)SceneID.Gameplay);
    }
}
