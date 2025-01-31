#if UNITY_SERVER
using Network.Shared;
using System;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Matchmaker.Models;
using Network.Server.Services;
using UnityEngine;

namespace Network.Server
{
    public class ServerManager : IDisposable
    {
        const int MultiplayServiceTimeout = 20000;

        private NetworkServer _networkServer;
        private MultiplayAllocationService _multiplayAllocationService;
        private MultiplayServerQueryService _multiplayServerQueryService;

        private string _serverIP = "0.0.0.0";
        private int _serverPort = 7777;
        private int _queryPort = 7787;
        private string _serverName = "My Server";
        private bool _startedServices;

        public ServerManager(string serverIP, int serverPort, int serverQPort, NetworkManager manager, SceneLoaderMediator sceneLoaderMediator)
        {
            Debug.Log(GetType().Name);

            _serverIP = serverIP;
            _serverPort = serverPort;
            _queryPort = serverQPort;

            _networkServer = new NetworkServer(manager, sceneLoaderMediator);
            _multiplayAllocationService = new MultiplayAllocationService();
            _multiplayServerQueryService = new MultiplayServerQueryService();
        }

        public async Task StartGameServerAsync()
        {
            await _multiplayServerQueryService.BeginServerQueryHandler();

            try
            {
                var matchmakerPayload = await GetMatchmakerPayload(MultiplayServiceTimeout);

                if (matchmakerPayload != null)
                {
                    Debug.Log($"Got payload: {matchmakerPayload}");

                    var gameInfo = matchmakerPayload.MatchProperties.Players[0].CustomData.GetAs<GameInfo>();

                    MatchServerQuery(gameInfo);

                    if (!_networkServer.OpenConnection(_serverIP, _serverPort, gameInfo))
                    {
                        Debug.LogError("NetworkServer did not start as expected.");
                        return;
                    }

                    _networkServer.ConfigureServer(gameInfo);

                    //_multiplayServerQueryService.SetPlayerCount((ushort)_networkServer.PlayerCount);

                    _networkServer.OnPlayerJoined += UserJoined;
                    _networkServer.OnPlayerLeft += UserLeft;
                    _startedServices = true;
                }
                else
                {
                    Debug.LogWarning("Matchmaker Payload timed out");
                }
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        async Task<MatchmakingResults> GetMatchmakerPayload(int timeout)
        {
            if (_multiplayAllocationService == null)
                return null;

            var matchmakerPayloadTask = _multiplayAllocationService.SubscribeAndAwaitMatchmakerAllocation();

            if (await Task.WhenAny(matchmakerPayloadTask, Task.Delay(timeout)) == matchmakerPayloadTask)
            {
                return matchmakerPayloadTask.Result;
            }

            return null;
        }

        private void MatchServerQuery(GameInfo gameInfo)
        {
            _multiplayServerQueryService.SetServerName(_serverName);
            _multiplayServerQueryService.SetMaxPlayers(2);
            _multiplayServerQueryService.SetBuildID("0");
            _multiplayServerQueryService.SetMode(gameInfo.Game.ToString());
        }

        private void UserJoined(UserData joinedUser)
        {
            Debug.Log($"{joinedUser} joined the game");
            _multiplayServerQueryService.AddPlayer();
        }

        private void UserLeft(UserData leftUser)
        {
            var playerCount = _networkServer.PlayerCount;
            _multiplayServerQueryService.RemovePlayer();

            Debug.Log($"player '{leftUser?.UserName}' left the game, {playerCount} players left in game.");

            if (playerCount <= 0)
            {
                CloseServer();
                return;
            }
        }

        private void CloseServer()
        {
            Debug.Log($"Closing Server");

            Dispose();

            Application.Quit();
        }

        public void Dispose()
        {
            if (!_startedServices)
            {
                if (_networkServer.OnPlayerJoined != null) _networkServer.OnPlayerJoined -= UserJoined;
                if (_networkServer.OnPlayerLeft != null) _networkServer.OnPlayerLeft -= UserLeft;
            }

            _multiplayAllocationService?.Dispose();
            _networkServer?.Dispose();
        }
    }
}
#endif