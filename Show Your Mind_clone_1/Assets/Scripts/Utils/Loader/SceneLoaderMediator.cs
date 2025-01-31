public class SceneLoaderMediator
{
    private SceneLoader _sceneLoader;

    public SceneLoaderMediator(SceneLoader sceneLoader)
    {
        _sceneLoader = sceneLoader;
    }

    public void LoadMenu()
    {
        _sceneLoader.Load(SceneID.Menu);
    }

    public void LoadSingleplayerScene(GameMode gameType, DifficultyType difficultyType)
    {
        _sceneLoader.Load(gameType, difficultyType, SceneID.SingleplayerScene);
    }

    public void LoadMultiplayerScene(GameMode gameType, DifficultyType difficultyType)
    {
        _sceneLoader.Load(gameType, difficultyType, SceneID.MultiplayerScene);
    }
}
