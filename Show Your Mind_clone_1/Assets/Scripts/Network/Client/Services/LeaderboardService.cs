using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;

namespace Network.Client.Services
{
    public class LeaderboardService
    {
        private const string LeaderboardId = "Global_Leaderboard";

        public async void AddScore(int score)
        {
            await LeaderboardsService.Instance.AddPlayerScoreAsync(LeaderboardId, score);
        }

        public async Task<List<LeaderboardEntry>> GetScores()
        {
            var scoresResponse =
                await LeaderboardsService.Instance.GetScoresAsync(LeaderboardId);
            return scoresResponse.Results;
        }

        public async Task<LeaderboardEntry> GetPlayerScore()
        {
            return await LeaderboardsService.Instance.GetPlayerScoreAsync(LeaderboardId);
        }
    }
}
