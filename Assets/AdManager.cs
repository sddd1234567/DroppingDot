using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour {
    public bool canWatch = false;
    public DateTime nextVideoTime;
    public TimeSpan timeSpan;
    float timeCount = 1;

    [TextArea]
    public string canWatchText;

    [SerializeField]
    string gameId;
    [SerializeField]
    bool isTesting;

    [SerializeField]
    string adUnitId;

  //  public Ad unityAds;
    public GoogleAd admob;
    public ViidleAds unityAds;

    bool isRequestUnityAds;
    bool isRequestAdmob;
    public bool isRequestShowingAd;
    public bool isShowingAd;

    void Start () {
        admobIdJudge();
        loadNextVideoTime();
        if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            requestAllAds();
    }

    public void saveNextVideoTime() {
        PlayerPrefs.SetString("NextVideoTime", nextVideoTime.ToString());
    }

    public void loadNextVideoTime() {
        string t = PlayerPrefs.GetString("NextVideoTime", DateTime.Now.ToString());
        nextVideoTime = DateTime.Parse(t);
    }

    public void admobIdJudge() {
        adUnitId = "ca-app-pub-9674557703156077/9719279149";
    }


    void Update () {
        if (timeCount >= 1 && !canWatch && !GameManager.instance.isGameStart)
        {
            timeCount = 0;
            judgeVideo();
        }
        else
            timeCount += Time.deltaTime;
    }

    public void showRewardAd() {
        GameManager.instance.seManager.playSE(GameManager.instance.seManager.buttonSE);
        if (Application.internetReachability != NetworkReachability.NotReachable && canWatch)
        {
            Debug.Log("Request AD");
            GameManager.instance.uiManager.setUIActive(GameManager.instance.uiManager.loadingAdsUI, true);
            StartCoroutine(KeepShowingAd());
        }
        else
            Debug.Log("Internet Not Reachable");
    }

    void showRewardedVideo() {
        if (!GameManager.instance.isGameStart)
        {            
            if (UnityEngine.Random.Range(0, 2) == 0)
            {
                if (unityAds.isReady())
                {
                    unityAds.ShowRewardedVideo();
                }
                else if (admob.isReady())
                {
                    Debug.Log("show admob");
                    admob.ShowRewardBasedVideo();                    
                }
                else
                {
                    requestAllAds();
                }
            }
            else
            {
                if (admob.isReady())
                {
                    admob.ShowRewardBasedVideo();     
                }
                else if (unityAds.isReady())
                {
                    unityAds.ShowRewardedVideo();
                }
                else
                {
                    requestAllAds();
                }
            }
            
        }
    }

    public void onVideoStop() {
        isShowingAd = false;
    }


    void judgeVideo()
    {
        DateTime nowDateTime = DateTime.Now;
        timeSpan = nextVideoTime.Subtract(nowDateTime);
        if (timeSpan.TotalSeconds < 0)
        {
            canWatchVideo();
        }
        else
        {
            canNotWatchVideo();
        }
    }


    public void canWatchVideo()
    {
        canWatch = true;        

        GameManager.instance.uiManager.setText(GameManager.instance.uiManager.videoWaitTime, canWatchText);
        GameManager.instance.uiManager.videoWaitTime.fontSize = 12;
        GameManager.instance.uiManager.setUIActive(GameManager.instance.uiManager.adBlueHeart, true);

        if (!unityAds.isReady())
        {
            StartCoroutine(KeepRequestUnityAds());
        }

        if (!admob.isReady())
        {
            StartCoroutine(KeepRequestAdmob());
        }
    }

    void requestAllAds() {
        StartCoroutine(KeepRequestAdmob());
        StartCoroutine(KeepRequestUnityAds());
    }

    public void requestUnityAds() {
        //unityAds.RequestRewardedVideo(gameId, isTesting);
        unityAds.RequestRewardedVideo(isTesting);
    }

    public void requestAdmob() {        
        admob.RequestRewardBasedVideo(adUnitId, isTesting);
    }

    public void canNotWatchVideo()
    {
        canWatch = false;
        GameManager.instance.uiManager.setUIActive(GameManager.instance.uiManager.adBlueHeart, false);
        GameManager.instance.uiManager.videoWaitTime.fontSize = 16;
        GameManager.instance.uiManager.setText(GameManager.instance.uiManager.videoWaitTime, timeSpan.Minutes + ":" + timeSpan.Seconds);
    }

    void rewardHeart()
    {
        GameManager.instance.givePlayerHeart(10);
    }

    public void finishRewardedVideo() {        
        nextVideoTime = DateTime.Now.AddMinutes(5);
        canWatch = false;
        saveNextVideoTime();
        judgeVideo();
        rewardHeart();
        onVideoStop();
    }

    IEnumerator KeepRequestUnityAds() {
        if (!isRequestUnityAds)
        {
            isRequestUnityAds = true;
            yield return null;
            while (!unityAds.isReady())
            {
                requestUnityAds();
                yield return new WaitForSeconds(1);
            }
            isRequestUnityAds = false;
        }
        else
            yield return null;
    }

    IEnumerator KeepRequestAdmob()
    {
        if (!isRequestAdmob)
        {
            isRequestAdmob = true;
            yield return null;
            while (!admob.isReady())
            {
                requestAdmob();
                yield return new WaitForSeconds(1);
            }
            isRequestAdmob = false;
        }
        else
            yield return null;
    }

    IEnumerator KeepShowingAd() {
        if (!isRequestShowingAd)
        {
            isRequestShowingAd = true;
            yield return null;
            while (!admob.isReady() && !unityAds.isReady())
            {
                yield return null;
                Debug.Log("Requiring AD");
                requestAllAds();
            }
            Debug.Log(admob.isReady());
            Debug.Log(unityAds.isReady());
            isRequestShowingAd = false;
            showRewardedVideo();
        }
        else
        {
            Debug.Log(isRequestShowingAd);
            yield return null;
        }
    }

    public void onVideoDisplayed() {
        GameManager.instance.uiManager.setUIActive(GameManager.instance.uiManager.loadingAdsUI, false);
        isShowingAd = true;
    }

    public void showVideoFailed() {
        isRequestShowingAd = false;
        StartCoroutine(KeepShowingAd());
    }

    public void stopShowingAds() {
        StopCoroutine(KeepShowingAd());
        isRequestShowingAd = false;
        GameManager.instance.uiManager.setUIActive(GameManager.instance.uiManager.loadingAdsUI, false);
    }
}
