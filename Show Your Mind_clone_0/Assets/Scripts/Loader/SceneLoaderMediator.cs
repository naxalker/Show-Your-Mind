public class SceneLoaderMediator
{
    private SceneLoader _sceneLoader;

    private SudokuConfig _sudokuConfig;

    public SceneLoaderMediator(SceneLoader sceneLoader, SudokuConfig sudokuConfig)
    {
        _sceneLoader = sceneLoader;
        _sudokuConfig = sudokuConfig;
    }

    public void LoadSudokuClient()
    {
        _sceneLoader.Load<SudokuClientGameManager>();
    }

    public void LoadSudokuServer()
    {
        _sceneLoader.Load<SudokuServerGameManager>(_sudokuConfig);
    } 
}
