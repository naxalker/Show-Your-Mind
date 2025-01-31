using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;
using UnityEngine;
using Network.Shared;

namespace Network.Server
{
    public class NetworkServer : IDisposable
    {
        public Action<UserData> OnPlayerLeft;
        public Action<UserData> OnPlayerJoined;

        private const int MaxConnectPayload = 1024;

        private NetworkManager _networkManager;
        private SceneLoaderMediator _sceneLoaderMediator;

        private Dictionary<string, UserData> _clientData = new Dictionary<string, UserData>();
        private Dictionary<ulong, string> _networkIdToAuth = new Dictionary<ulong, string>();

        public NetworkServer(NetworkManager networkManager, SceneLoaderMediator sceneLoaderMediator)
        {
            _networkManager = networkManager;
            _sceneLoaderMediator = sceneLoaderMediator;

            //_networkManager.ConnectionApprovalCallback += ApprovalCheck;
            _networkManager.OnServerStarted += OnNetworkReady;
        }

        public int PlayerCount => _networkManager.ConnectedClients.Count;

        public bool OpenConnection(string ip, int port, GameInfo gameInfo)
        {
            var unityTransport = _networkManager.gameObject.GetComponent<UnityTransport>();
            _networkManager.NetworkConfig.NetworkTransport = unityTransport;
            unityTransport.SetConnectionData(ip, (ushort)port);

            Debug.Log($"Starting server at {ip}:{port}\nWith: {gameInfo}");

            return _networkManager.StartServer();
        }

        public void ConfigureServer(GameInfo gameInfo)
        {
            _sceneLoaderMediator.LoadMultiplayerScene(
                gameInfo.Game, 
                (DifficultyType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(DifficultyType)).Length));
        }

        void OnNetworkReady()
        {
            _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
        }

        //void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        //{
        //    if (request.Payload.Length > MaxConnectPayload)
        //    {
        //        response.Approved = false;
        //        response.CreatePlayerObject = false;
        //        response.Position = null;
        //        response.Rotation = null;
        //        response.Pending = false;

        //        Debug.LogError($"Connection payload was too big! : {request.Payload.Length} / {MaxConnectPayload}");
        //        return;
        //    }

        //    var payload = System.Text.Encoding.UTF8.GetString(request.Payload);
        //    var userData = JsonUtility.FromJson<UserData>(payload);
        //    userData.NetworkId = request.ClientNetworkId;
        //    Debug.Log($"Host ApprovalCheck: connecting client: ({request.ClientNetworkId}) - {userData}");

        //    if (_clientData.ContainsKey(userData.UserAuthId))
        //    {
        //        ulong oldClientId = _clientData[userData.UserAuthId].NetworkId;
        //        Debug.Log($"Duplicate ID Found : {userData.UserAuthId}, Disconnecting Old user");

        //        WaitToDisconnect(oldClientId);
        //    }

        //    _networkIdToAuth[request.ClientNetworkId] = userData.UserAuthId;
        //    _clientData[userData.UserAuthId] = userData;
        //    OnPlayerJoined?.Invoke(userData);

        //    response.Approved = true;
        //    response.CreatePlayerObject = true;
        //    response.Position = Vector3.zero;
        //    response.Rotation = Quaternion.identity;
        //    response.Pending = false;
        //}

        private void OnClientDisconnect(ulong networkId)
        {
            if (_networkIdToAuth.TryGetValue(networkId, out var authId))
            {
                _networkIdToAuth?.Remove(networkId);
                OnPlayerLeft?.Invoke(_clientData[authId]);

                if (_clientData[authId].NetworkId == networkId)
                {
                    _clientData.Remove(authId);
                }
            }
        }

        async void WaitToDisconnect(ulong networkId)
        {
            await Task.Delay(500);
            _networkManager.DisconnectClient(networkId);
        }

        public void Dispose()
        {
            if (_networkManager == null)
                return;
            //_networkManager.ConnectionApprovalCallback -= ApprovalCheck;
            _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;
            _networkManager.OnServerStarted -= OnNetworkReady;
            if (_networkManager.IsListening)
                _networkManager.Shutdown();
        }
    }
}
