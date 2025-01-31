using UnityEngine;
using Zenject;

public class GlobalInstaller : MonoInstaller
{
    [SerializeField] private ColorPalette _colorPalette;
    [SerializeField] private AllGameConfigs _allGameConfigs;

    public override void InstallBindings()
    {
        BindInput();
        BindLoader();
        BindConfigs();
    }

    private void BindConfigs()
    {
        Container.Bind<ColorPalette>().FromScriptableObject(_colorPalette).AsSingle();

        Container.Bind<AllGameConfigs>().FromScriptableObject(_allGameConfigs).AsSingle();
    }

    private void BindLoader()
    {
        Container.Bind<ZenjectSceneLoaderWrapper>().AsSingle();
        Container.Bind<SceneLoader>().AsSingle();
        Container.Bind<SceneLoaderMediator>().AsSingle();
    }

    private void BindInput()
    {
        Container.BindInterfacesAndSelfTo<MobileInput>().AsSingle();
    }
}
