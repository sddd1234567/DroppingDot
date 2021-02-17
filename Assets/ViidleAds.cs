using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ViidleUnityPlugin.AD;

public class ViidleAds : MonoBehaviour {

    public ViidleAd m_Ad;
    // Use this for initialization
    void Awake() {
        //m_Ad = new ViidleUnityPlugin.Platform.android.AndroidViidleAd("7106cf43d67d4c569955634a364b012c");
        m_Ad = ViidleAd.NewInstance("7106cf43d67d4c569955634a364b012c");
    }

    void Start () {
        if (m_Ad != null)
        {
            m_Ad.SetAutoReload(true);
            setHandlers();
        }
    }

    public void setHandlers() {
        m_Ad.RewardCompleted += onRewardedVideoCompleted;
        m_Ad.ShowVideoFailed += onShowVideoFailed;
        m_Ad.VideoDisplayed += onVideoDisplayed;
    }

    public void onShowVideoFailed(ViidleAd instance)
    {
        GameManager.instance.adManager.showVideoFailed();
    }

    public void onVideoDisplayed(ViidleAd instance)
    {
        GameManager.instance.adManager.onVideoDisplayed();
    }

    public void RequestRewardedVideo(bool isTest) {
        if (m_Ad != null)
        {
            if (isTest)
                m_Ad.StartTestMode();
            m_Ad.Load();
        }
    }

    public bool isReady() {
        if (m_Ad != null)
            return m_Ad.IsReady;
        else
            return false;
    }

    public void ShowRewardedVideo() {        
        if (isReady())
        {
            m_Ad.Show();
        }
    }

    public void onRewardedVideoCompleted(ViidleAd instance, string currencyName, int currencyAmount)
    {
        GameManager.instance.adManager.finishRewardedVideo();
    }
}

