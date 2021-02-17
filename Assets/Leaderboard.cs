using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class Leaderboard{

    public static void AddScoreToLeaderboard(string leaderboardId, long score) {
        Social.ReportScore(score, leaderboardId, success => {
            PlayerPrefs.SetInt(Social.localUser.userName + "BestScore", (int)score);
        });        
    }    

    public static void ShowLeaderboardUI()
    {
        if(Social.localUser.authenticated)
            Social.ShowLeaderboardUI();
    }

}
