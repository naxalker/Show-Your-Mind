using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConnectionButtons : MonoBehaviour
{
    [SerializeField] private Button _serverButton;
    [SerializeField] private Button _clientButton;

    private void Awake()
    {
        _serverButton.onClick.AddListener(StartServer);
        _clientButton.onClick.AddListener(StartClient);
    }

    public void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
