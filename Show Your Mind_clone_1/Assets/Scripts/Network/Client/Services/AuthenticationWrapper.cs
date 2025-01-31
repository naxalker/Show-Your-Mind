using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using WebSocketSharp;

namespace Network.Client.Services
{
    public enum AuthState
    {
        NotAuthenticated,
        Authenticating,
        Authenticated,
        Error,
        TimedOut
    }

    public static class AuthenticationWrapper
    {
        public static AuthState AuthorizationState { get; private set; } = AuthState.NotAuthenticated;

        public static async Task<AuthState> DoAuth(int tries = 5)
        {
            if (AuthorizationState == AuthState.Authenticated)
            {
                Debug.LogWarning("Cant Authenticate if we authenticated");

                return AuthorizationState;
            }

            if (AuthorizationState == AuthState.Authenticating)
            {
                Debug.LogWarning("Cant Authenticate if we are authenticating");

                await Authenticating();
                return AuthorizationState;
            }

            await SignInAnonymouslyAsync(tries);

            Debug.Log($"Auth attempts Finished : {AuthorizationState}");

            return AuthorizationState;
        }

        public static string PlayerID()
        {
            return AuthenticationService.Instance.PlayerId;
        }

        public async static Task<string> PlayerName()
        {
            var playerName = await AuthenticationService.Instance.GetPlayerNameAsync(false);

            if (string.IsNullOrEmpty(playerName))
            {
                playerName = NameGenerator.GenerateName();
                await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
            }

            return playerName;
        }

        public static async Task<AuthState> Authenticating()
        {
            while (AuthorizationState == AuthState.Authenticating || AuthorizationState == AuthState.NotAuthenticated)
            {
                await Task.Delay(200);
            }

            return AuthorizationState;
        }

        static async Task SignInAnonymouslyAsync(int maxRetries)
        {
            AuthorizationState = AuthState.Authenticating;
            var tries = 0;

            while (AuthorizationState == AuthState.Authenticating && tries < maxRetries)
            {
                try
                {
                    await AuthenticationService.Instance.SignInAnonymouslyAsync();

                    if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                    {
                        AuthorizationState = AuthState.Authenticated;
                        break;
                    }
                }
                catch (AuthenticationException ex)
                {
                    Debug.LogError(ex);
                    AuthorizationState = AuthState.Error;
                }
                catch (RequestFailedException exception)
                {
                    Debug.LogError(exception);
                    AuthorizationState = AuthState.Error;
                }

                tries++;
                await Task.Delay(1000);
            }

            if (AuthorizationState != AuthState.Authenticated)
            {
                Debug.LogWarning($"Player was not signed in successfully after {tries} attempts");
                AuthorizationState = AuthState.TimedOut;
            }
        }

        public static void SignOut()
        {
            AuthenticationService.Instance.SignOut(false);
            AuthorizationState = AuthState.NotAuthenticated;
        }
    }
}