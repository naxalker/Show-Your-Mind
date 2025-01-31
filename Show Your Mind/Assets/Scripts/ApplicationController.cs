using Network;
using Network.Client;
using Network.Server;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ServerSingleton _serverSingletonPrefab;
    [SerializeField] private ClientSingleton _clientSingletonPrefab;

    private ApplicationData _appData;

    private DiContainer _container;
    private SceneLoaderMediator _sceneLoaderMediator;

    [Inject]
    private void Construct(SceneLoaderMediator sceneLoaderMediator, DiContainer container)
    {
        _sceneLoaderMediator = sceneLoaderMediator;
        _container = container;
    }

    private async void Start()
    {
        Application.targetFrameRate = 60;
        
        if (Application.isEditor)
            return;

        await LaunchInMode(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null);
    }

    public void OnParrelSyncStarted(bool isServer, string cloneName)
    {
#pragma warning disable 4014
        LaunchInMode(isServer);
#pragma warning restore 4014
    }

    private async Task LaunchInMode(bool isServer)
    {
        _appData = new ApplicationData();

        if (isServer)
        {
#if UNITY_SERVER
            await StartServer();
#endif
        }
        else
        {
            StartClient();
        }
    }

#if UNITY_SERVER
    private async Task StartServer()
    {
        var serverSingleton = _container.InstantiatePrefabForComponent<ServerSingleton>(_serverSingletonPrefab);
        await serverSingleton.CreateServer();

        await serverSingleton.Manager.StartGameServerAsync();
    }
#endif

    private void StartClient()
    {
        var clientSingleton = _container.InstantiatePrefabForComponent<ClientSingleton>(_clientSingletonPrefab);
        clientSingleton.CreateClient();

        //clientSingleton.Manager.ToMainMenu();
    }
}