using UnityEngine;
using Zenject;

public class PlayerInputInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<MobileInput>().AsSingle();
    }
}