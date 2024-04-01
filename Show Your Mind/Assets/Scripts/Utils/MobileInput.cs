using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MobileInput : IInitializable
{
    private TouchAction _touchAction;

    public void Initialize()
    {
        _touchAction = new TouchAction();
        _touchAction.Enable();
    }
}
