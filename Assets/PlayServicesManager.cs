using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class PlayServicesManager : MonoBehaviour {
    public static PlayServicesManager instance;
    [SerializeField]
    public List<basicAchievement> gamesPlayed;
    [SerializeField]
    public List<basicAchievement> scoreAchievement;
    public Text scoreCounts;

    void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

	void Start () {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.Activate();

            if(!GameManager.instance.isFirstTime())
                SignIn();
        }
	}

    void SignIn() {        
        if (GameManager.instance.isFirstTime())
        {
            GameManager.instance.isTryingLogin = true;
            if (!Social.localUser.authenticated)
                Social.localUser.Authenticate(success => 
                {
                    GameManager.instance.isTryingLogin = false;
                    if (success)
                    {                        
                        PlayerPrefs.SetInt("isFirstTime", 1);
                        reportAchievements(DroppingDot.achievement_welcome_);
                    }
                }
                );
            return;
        }        

        if (!Social.localUser.authenticated)
            Social.localUser.Authenticate(success => 
            {
                GameManager.instance.isTryingLogin = false;
                if (success) {
                    reportAchievements(DroppingDot.achievement_welcome_);
                }
            }
            );
    }

    public void addScoreToLeaderboard(int score) {
        if(Social.localUser.authenticated)
            Leaderboard.AddScoreToLeaderboard(DroppingDot.leaderboard_best_score, score);
    }

    public void showLeaderboardUI() {
        if (!Social.localUser.authenticated)
            SignIn();
        Leaderboard.ShowLeaderboardUI();
    }

    public void showAchievementsUI() {
        if (!Social.localUser.authenticated)
            SignIn();
        Achievements.ShowAchievementsUI();
    }

    public void checkGamesAchievement(int gamesCount) {
        if (Social.localUser.authenticated)
        {
            for (int i = 0; i < gamesPlayed.Count; i++)
            {
                if (gamesCount >= gamesPlayed[i].num)
                {
                    Achievements.UnlockAchievement(gamesPlayed[i].id);
                }
            }
        }
    }

    public void checkScoreAchievement(int score)
    {
        if (Social.localUser.authenticated)
        {
            for (int i = 0; i < scoreAchievement.Count; i++)
            {
                if (score >= scoreAchievement[i].num)
                {
                    Achievements.UnlockAchievement(scoreAchievement[i].id);
                }
            }
        }
    }

    public void reportAchievements(string id)
    {
        Achievements.UnlockAchievement(id);
    }

    public void loadScoreOnLeaderboard(string uName, IScore[] score) {
        StartCoroutine(loadScore(uName, score));
    }

    IEnumerator loadScore(string uName, IScore[] score)
    {
        yield return null;
        int highestScore = 0;

        foreach (IScore s in score)
        {
            if (s.userID == uName)
            {
                if (s.value > highestScore)
                    highestScore = (int)s.value;
            }
            yield return null;
        }
        GameManager.instance.saveBestScore(highestScore);
    }

}
[System.Serializable]
public struct basicAchievement {
    public int num;
    public string id;
}
