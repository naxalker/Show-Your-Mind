using System;
using System.Text;

namespace Network.Shared
{
    public class User
    {
        public User()
        {
            Data = new UserData(
                "PlayerName", 
                Guid.NewGuid().ToString(), 
                0, 
                new GameInfo());
        }

        public UserData Data { get; }

        public string Name
        {
            get => Data.UserName;
            set => Data.UserName = value;
        }

        public string AuthId
        {
            get => Data.UserAuthId;
            set => Data.UserAuthId = value;
        }

        public GameMode GameModePreferences
        {
            get => Data.UserGamePreferences.Game;
            set => Data.UserGamePreferences.Game = value;
        }

        public override string ToString()
        {
            var userData = new StringBuilder("User: ");
            userData.AppendLine($"- {Data}");
            return userData.ToString();
        }
    }

    [Serializable]
    public class UserData
    {
        public string UserName;
        public string UserAuthId;
        public ulong NetworkId;
        public GameInfo UserGamePreferences;

        public UserData(string userName, string userAuthId, ulong networkId, GameInfo userGamePreferences)
        {
            UserName = userName;
            UserAuthId = userAuthId;
            NetworkId = networkId;
            UserGamePreferences = userGamePreferences;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UserData: ");
            sb.AppendLine($"- User Name:             {UserName}");
            sb.AppendLine($"- User Auth Id:          {UserAuthId}");
            sb.AppendLine($"- User Game Preferences: {UserGamePreferences}");
            return sb.ToString();
        }
    }

    [Serializable]
    public class GameInfo
    {
        public GameMode Game;

        public string ToMultiplayQueue()
        {
            return "default-queue";
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("GameInfo: ");
            sb.AppendLine($"- gameMode:   {Game}");
            return sb.ToString();
        }
    }
}