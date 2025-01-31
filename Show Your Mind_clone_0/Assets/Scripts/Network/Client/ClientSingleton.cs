using UnityEngine;
using Zenject;

namespace Network.Client
{
    public class ClientSingleton : MonoBehaviour
    {
        private static ClientSingleton _clientGameManager;

        private ClientManager _gameManager;
        private SceneLoaderMediator _sceneLoaderMediator;

        [Inject]
        private void Construct(SceneLoaderMediator sceneLoaderMediator)
        {
            _sceneLoaderMediator = sceneLoaderMediator;
        }

        public static ClientSingleton Instance
        {
            get
            {
                if (_clientGameManager != null) return _clientGameManager;

                _clientGameManager = FindObjectOfType<ClientSingleton>();

                if (_clientGameManager == null)
                {
                    Debug.LogError("No ClientSingleton in scene, did you run this from the bootStrap scene?");
                    return null;
                }

                return _clientGameManager;
            }
        }

        public ClientManager Manager
        {
            get
            {
                if (_gameManager != null) return _gameManager;

                Debug.LogError($"ClientGameManager is missing, did you run StartClient()?", gameObject);

                return null;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void CreateClient()
        {
            _gameManager = new ClientManager(_sceneLoaderMediator);
        }

        private void OnDestroy()
        {
            Manager?.Dispose();
        }
    }
}