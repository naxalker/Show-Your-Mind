public class SceneLoader
{
    private ZenjectSceneLoaderWrapper _zenjectSceneLoader;

    public SceneLoader(ZenjectSceneLoaderWrapper zenjectSceneLoader)
    {
        _zenjectSceneLoader = zenjectSceneLoader;
    }

    public void Load(SceneID sceneId)
    {
        _zenjectSceneLoader.Load((int)sceneId);
    }

    public void Load(GameMode gameType, DifficultyType difficultyType, SceneID sceneId)
    {
        _zenjectSceneLoader.Load(container =>
        {
            container.BindInstances(gameType, difficultyType);
        }, (int)sceneId);
    }
}
