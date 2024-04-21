using System.Collections;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationController : MonoBehaviour
{
    //[SerializeField] private ClientSingleton _clientPrefab;
    ////[SerializeField] private HostSingleton _hostPrefab;
    //[SerializeField] private ServerSingleton _serverPrefab;

    private void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {

        }
        else if (NetworkManager.Singleton.IsClient) 
        {
            
        }
    }
}