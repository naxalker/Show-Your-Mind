using UnityEngine;

public abstract class GameManager
{
    protected bool _isGameActive = true;

    protected GameManager()
    {
        Debug.Log(GetType().Name);
    }

    public bool IsGameActive => _isGameActive;

    public abstract void Init();
}
