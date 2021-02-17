/*using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;


public class Ad : MonoBehaviour{
    public void RequestRewardedVideo(string gameId, bool isTesting) {
        Advertisement.Initialize(gameId, isTesting);
    }

    public bool isReady() {
        return Advertisement.IsReady("rewardedVideo");
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }
    
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                GameManager.instance.adManager.finishRewardedVideo();             
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                GameManager.instance.adManager.onVideoStop();
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                break;
        }

    }
}
*/
