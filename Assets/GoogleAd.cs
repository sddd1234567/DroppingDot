using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds;
using GoogleMobileAds.Android;
using System;

public class GoogleAd : MonoBehaviour {
    public static bool rewardBasedEventHandlersSet;
    RewardBasedVideoAd rewardBasedVideo;
    int requestTime = 0;
    // Use this for initialization
    void Start () {
        rewardBasedVideo = RewardBasedVideoAd.Instance;
        setRewardedVideoHandlers();
	}

    void setRewardedVideoHandlers() {
        // Reward based video instance is a singleton. Register handlers once to
        // avoid duplicate events.
        if (!rewardBasedEventHandlersSet)
        {
            // Ad event fired when the rewarded video ad
            // has been received.
            rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
            // has failed to load.
            rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
            // is opened.
            rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
            // has started playing.
      //      rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;
            // has rewarded the user.
            rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
            // is closed.
            rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
            // is leaving the application.
     //       rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

            rewardBasedEventHandlersSet = true;
        }
    }

    public void HandleRewardBasedVideoOpened(object sender, EventArgs args) {
        GameManager.instance.adManager.onVideoDisplayed();
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        GameManager.instance.adManager.onVideoStop();
        GameManager.instance.adManager.requestAdmob();
    }

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args) {
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        requestTime++;
        GameManager.instance.adManager.requestAdmob();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        GameManager.instance.adManager.finishRewardedVideo();
    }

    void showBannerAd() {
    #if UNITY_EDITOR
        string adUnitId = "unused";
    #elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-9674557703156077/9719279149";
    #elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-9674557703156077/9719279149";
    #else
        string adUnitId = "unexpected_platform";
    #endif
        AdRequest request = new AdRequest.Builder().AddTestDevice(AdRequest.TestDeviceSimulator).AddTestDevice("51C37C35FF1E25E0BB6272FB40BC1E83").Build();

        BannerView bannerAd = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Bottom);
        bannerAd.LoadAd(request);
        bannerAd.Show();
    }


    public void RequestRewardBasedVideo(string adUnitId, bool isTesting)
    {
        AdRequest request;
        request = new AdRequest.Builder().AddTestDevice("51C37C35FF1E25E0BB6272FB40BC1E83").AddTestDevice("DED0214753EBC9603CD1DCB8E99BE0B1").Build();
        rewardBasedVideo.LoadAd(request, adUnitId);
    }

    public void ShowRewardBasedVideo() {
        if (rewardBasedVideo.IsLoaded())
            rewardBasedVideo.Show();
    }

    public bool isReady() {
        return rewardBasedVideo.IsLoaded();
    }
}
