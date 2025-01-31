#if UNITY_SERVER
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

using Unity.Services.Multiplay;

namespace Network.Server.Services
{
    public class MultiplayServerQueryService : IDisposable
    {
        private IMultiplayService _multiplayService;
        private IServerQueryHandler _serverQueryHandler;
        private CancellationTokenSource _serverCheckCancel;

        public MultiplayServerQueryService()
        {
            try
            {
                _multiplayService = MultiplayService.Instance;
                _serverCheckCancel = new CancellationTokenSource();
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Error creating Multiplay allocation service.\n{ex}");
            }
        }

        public async Task BeginServerQueryHandler()
        {
            if (_multiplayService == null)
                return;

            _serverQueryHandler = await _multiplayService.StartServerQueryHandlerAsync((ushort)2, "ServerName", "GameType", "0", "MapName");

#pragma warning disable 4014
            ServerQueryLoop(_serverCheckCancel.Token);
#pragma warning restore 4014
        }

        public void SetServerName(string name)
        {
            _serverQueryHandler.ServerName = name;
        }

        public void SetBuildID(string id)
        {
            _serverQueryHandler.BuildId = id;
        }

        public void SetMaxPlayers(ushort players)
        {
            _serverQueryHandler.MaxPlayers = players;
        }

        public void SetPlayerCount(ushort count)
        {
            _serverQueryHandler.CurrentPlayers = count;
        }

        public void AddPlayer()
        {
            _serverQueryHandler.CurrentPlayers += 1;
        }

        public void RemovePlayer()
        {
            _serverQueryHandler.CurrentPlayers -= 1;
        }

        public void SetMode(string mode)
        {
            _serverQueryHandler.GameType = mode;
        }

        async Task ServerQueryLoop(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _serverQueryHandler.UpdateServerCheck();
                await Task.Delay(100);
            }
        }

        public void Dispose()
        {
            if (_serverCheckCancel != null)
                _serverCheckCancel.Cancel();
        }
    }
}
#endif