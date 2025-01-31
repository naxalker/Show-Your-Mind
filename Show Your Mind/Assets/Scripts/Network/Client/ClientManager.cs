using Network.Client.Services;
using Network.Shared;
using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine;

namespace Network.Client
{
    public class ClientManager : IDisposable
    {
        public User User { get; private set; }

        private NetworkClient _networkClient;
        private MatchmakerService _matchmaker;
        private LeaderboardService _leaderboard;
        private SceneLoaderMediator _sceneLoaderMeditator;

        public ClientManager(SceneLoaderMediator sceneLoader)
        {
            Debug.Log(GetType().Name);

            _sceneLoaderMeditator = sceneLoader;
            
            User = new User();

#pragma warning disable 4014
            InitAsync();
#pragma warning restore 4014
        }

        public LeaderboardService Leaderboard => _leaderboard;

        private async Task InitAsync()
        {
            await UnityServices.InitializeAsync();

            _networkClient = new NetworkClient(_sceneLoaderMeditator);
            _matchmaker = new MatchmakerService();
            _leaderboard = new LeaderboardService();

            var authenticationResult = await AuthenticationWrapper.DoAuth();

            if (authenticationResult == AuthState.Authenticated)
            {
                User.Name = await AuthenticationWrapper.PlayerName();
                User.AuthId = AuthenticationWrapper.PlayerID();

                _sceneLoaderMeditator.LoadMenu();
            }
            else
            {
                Debug.LogError("Authentication Error");
            }
        }

        public void SetGameMode(GameMode gameType)
        {
            User.GameModePreferences = gameType;
        }

        public void BeginConnection(string ip, int port)
        {
            Debug.Log($"Starting networkClient @ {ip}:{port}\nWith : {User}");

            _networkClient.StartClient(ip, port);
        }

        public void Disconnect()
        {
            _networkClient.DisconnectClient();
        }

        public async Task MatchmakeAsync(Action<MatchmakerPollingResult> onMatchmakerResponse = null)
        {
            if (_matchmaker.IsMatchmaking)
            {
                Debug.LogWarning("Already matchmaking, please wait or cancel.");
                return;
            }

            var matchResult = await GetMatchAsync();
            onMatchmakerResponse?.Invoke(matchResult);
        }

        public async Task CancelMatchmaking()
        {
            await _matchmaker.CancelMatchmaking();
        }

        public void ToMainMenu()
        {
            _sceneLoaderMeditator.LoadMenu();
        }

        private async Task<MatchmakerPollingResult> GetMatchAsync()
        {
            Debug.Log($"Beginning Matchmaking with {User}");

            var matchmakingResult = await _matchmaker.Matchmake(User.Data);

            if (matchmakingResult.result == MatchmakerPollingResult.Success)
                BeginConnection(matchmakingResult.ip, matchmakingResult.port);
            else
                Debug.LogWarning($"{matchmakingResult.result} : {matchmakingResult.resultMessage}");

            return matchmakingResult.result;
        }

        public void Dispose()
        {
            _networkClient?.Dispose();
            _matchmaker?.Dispose();
        }
    }
}
