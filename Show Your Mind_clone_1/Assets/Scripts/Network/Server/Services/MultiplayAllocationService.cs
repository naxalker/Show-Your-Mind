#if UNITY_SERVER
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

using Unity.Services.Multiplay;

namespace Network.Server.Services
{
    public class MultiplayAllocationService : IDisposable
    {
        private IMultiplayService _multiplayService;
        private MultiplayEventCallbacks _serverCallbacks;
        private IServerEvents _serverEvents;
        private string _allocationId;

        public MultiplayAllocationService()
        {
            try
            {
                _multiplayService = MultiplayService.Instance;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error creating Multiplay allocation service.\n{ex}");
            }
        }

        public async Task<MatchmakingResults> SubscribeAndAwaitMatchmakerAllocation()
        {
            if (_multiplayService == null)
                return null;

            _allocationId = null;

            _serverCallbacks = new MultiplayEventCallbacks();
            _serverCallbacks.Allocate += OnMultiplayAllocation;
            _serverCallbacks.Deallocate += OnMultiplayDeAllocation;
            _serverCallbacks.Error += OnMultiplayError;

            _serverEvents = await _multiplayService.SubscribeToServerEventsAsync(_serverCallbacks);

            var allocationID = await AwaitAllocationID();
            var mmPayload = await GetMatchmakerAllocationPayloadAsync();

            return mmPayload;
        }

        async Task<string> AwaitAllocationID()
        {
            var config = _multiplayService.ServerConfig;
            Debug.Log($"Awaiting Allocation. Server Config is:\n" +
                $"-ServerID: {config.ServerId}\n" +
                $"-AllocationID: {config.AllocationId}\n" +
                $"-Port: {config.Port}\n" +
                $"-QPort: {config.QueryPort}\n" +
                $"-logs: {config.ServerLogDirectory}");

            //Waiting on OnMultiplayAllocation() event (Probably wont ever happen in a matchmaker scenario)
            while (string.IsNullOrEmpty(_allocationId))
            {
                var configID = config.AllocationId;

                if (!string.IsNullOrEmpty(configID) && string.IsNullOrEmpty(_allocationId))
                {
                    Debug.Log($"Config had AllocationID: {configID}");
                    _allocationId = configID;
                }

                await Task.Delay(100);
            }

            return _allocationId;
        }

        async Task<MatchmakingResults> GetMatchmakerAllocationPayloadAsync()
        {
            var payloadAllocation = await MultiplayService.Instance.GetPayloadAllocationFromJsonAs<MatchmakingResults>();
            var modelAsJson = JsonConvert.SerializeObject(payloadAllocation, Formatting.Indented);
            Debug.Log(nameof(GetMatchmakerAllocationPayloadAsync) + ":" + Environment.NewLine + modelAsJson);
            return payloadAllocation;
        }

        void OnMultiplayAllocation(MultiplayAllocation allocation)
        {
            Debug.Log($"OnAllocation: {allocation.AllocationId}");
            if (string.IsNullOrEmpty(allocation.AllocationId))
                return;
            _allocationId = allocation.AllocationId;
        }

        void OnMultiplayDeAllocation(MultiplayDeallocation deallocation)
        {
            Debug.Log(
                $"Multiplay Deallocated : ID: {deallocation.AllocationId}\nEvent: {deallocation.EventId}\nServer{deallocation.ServerId}");
        }

        void OnMultiplayError(MultiplayError error)
        {
            Debug.Log($"MultiplayError : {error.Reason}\n{error.Detail}");
        }

        public void Dispose()
        {
            if (_serverCallbacks != null)
            {
                _serverCallbacks.Allocate -= OnMultiplayAllocation;
                _serverCallbacks.Deallocate -= OnMultiplayDeAllocation;
                _serverCallbacks.Error -= OnMultiplayError;
            }

            _serverEvents?.UnsubscribeAsync();
        }
    }

    public static class AllocationPayloadExtensions
    {
        public static string ToString(this MatchmakingResults payload)
        {
            StringBuilder payloadDescription = new StringBuilder();
            payloadDescription.AppendLine("Matchmaker Allocation Payload:");
            payloadDescription.AppendFormat("-QueueName: {0}\n", payload.QueueName);
            payloadDescription.AppendFormat("-PoolName: {0}\n", payload.PoolName);
            payloadDescription.AppendFormat("-ID: {0}\n", payload.BackfillTicketId);
            payloadDescription.AppendFormat("-Teams: {0}\n", payload.MatchProperties.Teams.Count);
            payloadDescription.AppendFormat("-Players: {0}\n", payload.MatchProperties.Players.Count);
            payloadDescription.AppendFormat("-Region: {0}\n", payload.MatchProperties.Region);
            return payloadDescription.ToString();
        }
    }
}

#endif