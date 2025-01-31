using System;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Network.Client
{
    public class NetworkClient : IDisposable
    {
        private const int TimeoutDuration = 10;

        private NetworkManager _networkManager;
        private SceneLoaderMediator _sceneLoaderMediator;

        public NetworkClient(SceneLoaderMediator sceneLoaderMediator)
        {
            _sceneLoaderMediator = sceneLoaderMediator;

            _networkManager = NetworkManager.Singleton;
            _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
        }

        public void Dispose()
        {
            if (_networkManager != null)
            {
                _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
            }
        }

        public void StartClient(string ipaddress, int port)
        {
            var unityTransport = _networkManager.gameObject.GetComponent<UnityTransport>();
            unityTransport.SetConnectionData(ipaddress, (ushort)port);
            ConnectClient();
        }

        public void DisconnectClient()
        {
            if (SceneManager.GetActiveScene().buildIndex != (int)SceneID.Menu)
            {
                _sceneLoaderMediator.LoadMenu();
            }

            if (_networkManager.IsConnectedClient)
            {
                _networkManager.Shutdown();
            }
        }

        private void ConnectClient()
        {
            var userData = ClientSingleton.Instance.Manager.User.Data;
            var payload = JsonUtility.ToJson(userData);

            var payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);

            _networkManager.NetworkConfig.ConnectionData = payloadBytes;
            _networkManager.NetworkConfig.ClientConnectionBufferTimeout = TimeoutDuration;

            _networkManager.StartClient();
        }

        private void OnClientDisconnect(ulong clientId)
        {
            if (clientId != 0 && clientId != _networkManager.LocalClientId) { return; }

            DisconnectClient();
        }
    }
}