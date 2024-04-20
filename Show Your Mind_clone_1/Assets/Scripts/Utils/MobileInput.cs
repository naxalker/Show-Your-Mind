using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class MobileInput : IInitializable, IDisposable
{
    public event Action<Vector3> OnTouchPerformed;

    private TouchAction _touchAction;

    public void Initialize()
    {
        _touchAction = new TouchAction();
        _touchAction.Enable();

        _touchAction.Gameplay.Touch.performed += ProcessTouch;
    }

    public void Dispose()
    {
        _touchAction.Gameplay.Touch.performed -= ProcessTouch;

        _touchAction.Disable();
    }

    private void ProcessTouch(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = _touchAction.Gameplay.TouchPosition.ReadValue<Vector2>();

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);

        OnTouchPerformed?.Invoke(worldPosition);
    }
}
