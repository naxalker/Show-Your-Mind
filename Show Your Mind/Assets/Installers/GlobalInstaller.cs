using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private SudokuConfig _sudokuConfig;

    public override void InstallBindings()
    {
        BindLoader();
    }

    private void BindLoader()
    {
        Container.Bind<ZenjectSceneLoaderWrapper>().AsSingle();
        Container.Bind<SceneLoader>().AsSingle();
        Container.Bind<SceneLoaderMediator>().AsSingle().WithArguments(_sudokuConfig);
    }
}