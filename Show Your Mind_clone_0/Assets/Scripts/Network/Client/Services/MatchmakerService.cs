using Network.Shared;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Services.Matchmaker;
using Unity.Services.Matchmaker.Models;
using UnityEngine;

namespace Network.Client.Services
{
    public enum MatchmakerPollingResult
    {
        Success,
        TicketCreationError,
        TicketCancellationError,
        TicketRetrievalError,
        MatchAssignmentError
    }

    public class MatchmakingResult
    {
        public string ip;
        public int port;
        public MatchmakerPollingResult result;
        public string resultMessage;
    }

    public class MatchmakerService : IDisposable
    {
        private const int TicketCooldown = 1000;

        public bool IsMatchmaking { get; private set; }

        private string _lastUsedTicket;
        private CancellationTokenSource _cancelToken;

        public async Task<MatchmakingResult> Matchmake(UserData data)
        {
            _cancelToken = new CancellationTokenSource();

            string queueName = data.UserGamePreferences.ToMultiplayQueue();
            CreateTicketOptions createTicketOptions = new CreateTicketOptions(queueName);

            List<Player> players = new List<Player>
            {
                new Player(data.UserAuthId, data.UserGamePreferences)
            };

            try
            {
                IsMatchmaking = true;
                CreateTicketResponse createResult = await Unity.Services.Matchmaker.MatchmakerService.Instance.CreateTicketAsync(players, createTicketOptions);

                _lastUsedTicket = createResult.Id;

                try
                {
                    while (!_cancelToken.IsCancellationRequested)
                    {
                        TicketStatusResponse checkTicket = await Unity.Services.Matchmaker.MatchmakerService.Instance.GetTicketAsync(_lastUsedTicket);

                        if (checkTicket.Type == typeof(MultiplayAssignment))
                        {
                            MultiplayAssignment matchAssignment = (MultiplayAssignment)checkTicket.Value;

                            if (matchAssignment.Status == MultiplayAssignment.StatusOptions.Found)
                            {
                                return ReturnMatchResult(MatchmakerPollingResult.Success, "", matchAssignment);
                            }
                            if (matchAssignment.Status == MultiplayAssignment.StatusOptions.Timeout ||
                                matchAssignment.Status == MultiplayAssignment.StatusOptions.Failed)
                            {
                                return ReturnMatchResult(MatchmakerPollingResult.MatchAssignmentError,
                                    $"Ticket: {_lastUsedTicket} - {matchAssignment.Status} - {matchAssignment.Message}", null);
                            }
                            Debug.Log($"Polled Ticket: {_lastUsedTicket} Status: {matchAssignment.Status} ");
                        }

                        await Task.Delay(TicketCooldown);
                    }
                }
                catch (MatchmakerServiceException e)
                {
                    return ReturnMatchResult(MatchmakerPollingResult.TicketRetrievalError, e.ToString(), null);
                }
            }
            catch (MatchmakerServiceException e)
            {
                return ReturnMatchResult(MatchmakerPollingResult.TicketCreationError, e.ToString(), null);
            }

            return ReturnMatchResult(MatchmakerPollingResult.TicketRetrievalError, "Cancelled Matchmaking", null);
        }

        public async Task CancelMatchmaking()
        {
            if (!IsMatchmaking) { return; }

            IsMatchmaking = false;

            if (_cancelToken.Token.CanBeCanceled)
            {
                _cancelToken.Cancel();
            }

            if (string.IsNullOrEmpty(_lastUsedTicket)) { return; }

            Debug.Log($"Cancelling {_lastUsedTicket}");

            await Unity.Services.Matchmaker.MatchmakerService.Instance.DeleteTicketAsync(_lastUsedTicket);
        }

        private MatchmakingResult ReturnMatchResult(MatchmakerPollingResult resultErrorType, string message, MultiplayAssignment assignment)
        {
            IsMatchmaking = false;

            if (assignment != null)
            {
                string parsedIp = assignment.Ip;
                int? parsedPort = assignment.Port;
                if (parsedPort == null)
                {
                    return new MatchmakingResult
                    {
                        result = MatchmakerPollingResult.MatchAssignmentError,
                        resultMessage = $"Port missing? - {assignment.Port}\n-{assignment.Message}"
                    };
                }

                return new MatchmakingResult
                {
                    result = MatchmakerPollingResult.Success,
                    ip = parsedIp,
                    port = (int)parsedPort,
                    resultMessage = assignment.Message
                };
            }

            return new MatchmakingResult
            {
                result = resultErrorType,
                resultMessage = message
            };
        }

        public void Dispose()
        {
            _ = CancelMatchmaking();

            _cancelToken?.Dispose();
        }
    }
}
