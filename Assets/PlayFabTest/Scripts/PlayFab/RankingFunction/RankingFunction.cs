using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Cysharp.Threading.Tasks;
using System.Threading;


public class RankingFunction
{
    public enum RankingState
    {
        Level,
        Score
    }
    
    public async UniTask SetRankingValue(RankingState rankingState, int value, CancellationToken cancellationToken)
    {
        UpdatePlayerStatisticsResult result = null;
        PlayFabError error = null;

        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = rankingState.ToString(),
                    Value = value
                }
            }
        },
        r => { Debug.Log("Result SetLevel"); result = r; },
        e =>
        {
            Debug.Log("Erorr SetLevel:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }

    public async UniTask GetRankingValue(RankingState rankingState, CancellationToken cancellationToken)
    {
        GetLeaderboardResult result = null;
        PlayFabError error = null;
        var player = new Player();
        
        PlayFabClientAPI.GetLeaderboard(new GetLeaderboardRequest
        {
            StatisticName = rankingState.ToString()
        },
        r => 
        { 
            foreach (var item in r.Leaderboard)
            {
                Debug.Log($"{item.Position + 1}位:{item.DisplayName} " + $"スコア {item.StatValue}");
            }

            Debug.Log("Result GetLevel"); 
            result = r; 
        },
        e =>
        {
            Debug.Log("Erorr GetLevel:" + e.GenerateErrorReport());
            error = e;
        });

        await new WaitUntil(() => result != null || error != null || cancellationToken.IsCancellationRequested);
        cancellationToken.ThrowIfCancellationRequested();
    }
}
