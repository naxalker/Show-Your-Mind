using Unity.Netcode;

public abstract class GameManager : NetworkBehaviour
{
    protected bool _isGameActive = true;

    public bool IsGameActive => _isGameActive;

    public abstract void Init();
}
