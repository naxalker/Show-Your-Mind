using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Core;
using UnityEngine;
using Zenject;

namespace Network.Server
{
    public class ServerSingleton : MonoBehaviour
    {
        private static ServerSingleton _serverSingleton;

#if UNITY_SERVER
        private ServerManager _gameManager;
#endif
        private SceneLoaderMediator _sceneLoaderMediator;

        [Inject]
        private void Construct(SceneLoaderMediator sceneLoaderMediator)
        {
            Debug.Log("Construct");
            _sceneLoaderMediator = sceneLoaderMediator;
        }

        public static ServerSingleton Instance
        {
            get
            {
                if (_serverSingleton != null) return _serverSingleton;

                _serverSingleton = FindObjectOfType<ServerSingleton>();

                if (_serverSingleton == null)
                {
                    Debug.LogError("No ServerSingleton in scene, did you run this from the bootStrap scene?");
                    return null;
                }

                return _serverSingleton;
            }
        }
#if UNITY_SERVER
        public ServerManager Manager
        {
            get
            {
                if (_gameManager != null)
                {
                    return _gameManager;
                }

                Debug.LogError($"Server Manager is missing, did you run OpenConnection?");
                return null;
            }
        }
#endif

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public async Task CreateServer()
        {
            try
            {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
#if UNITY_SERVER
            _gameManager = new ServerManager(
                ApplicationData.IP(),
                ApplicationData.Port(),
                ApplicationData.QPort(),
                NetworkManager.Singleton,
                _sceneLoaderMediator);
#endif
        }

        void OnDestroy()
        {
#if UNITY_SERVER
            _gameManager?.Dispose();
#endif
        }
    }
}
