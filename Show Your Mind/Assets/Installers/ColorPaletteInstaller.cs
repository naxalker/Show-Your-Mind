using UnityEngine;
using Zenject;

public class ColorPaletteInstaller : MonoInstaller
{
    [SerializeField] private ColorPalette _colorPalette;

    public override void InstallBindings()
    {
        Container.Bind<ColorPalette>().FromScriptableObject(_colorPalette).AsSingle();
    }
}