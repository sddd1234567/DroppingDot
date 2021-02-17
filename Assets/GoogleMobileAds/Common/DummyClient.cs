// Copyright (C) 2015 Google, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Reflection;

using GoogleMobileAds.Api;
using UnityEngine;

namespace GoogleMobileAds.Common
{
    public class DummyClient : IBannerClient, IInterstitialClient, IRewardBasedVideoAdClient,
            IAdLoaderClient, INativeExpressAdClient, IMobileAdsClient
    {
        public DummyClient()
        {

        }

        // Disable warnings for unused dummy ad events.
#pragma warning disable 67

        public event EventHandler<EventArgs> OnAdLoaded;

        public event EventHandler<AdFailedToLoadEventArgs> OnAdFailedToLoad;

        public event EventHandler<EventArgs> OnAdOpening;

        public event EventHandler<EventArgs> OnAdStarted;

        public event EventHandler<EventArgs> OnAdClosed;

        public event EventHandler<Reward> OnAdRewarded;

        public event EventHandler<EventArgs> OnAdLeavingApplication;

        public event EventHandler<CustomNativeEventArgs> OnCustomNativeTemplateAdLoaded;

#pragma warning restore 67

        public string UserId
        {
            get
            {

                return "UserId";
            }

            set
            {
            }
        }

        public void Initialize(string appId)
        {
        }

        public void CreateBannerView(string adUnitId, AdSize adSize, AdPosition position)
        {
        }

        public void CreateBannerView(string adUnitId, AdSize adSize, int positionX, int positionY)
        {
        }

        public void LoadAd(AdRequest request)
        {
        }

        public void ShowBannerView()
        {
        }

        public void HideBannerView()
        {
        }

        public void DestroyBannerView()
        {
        }

        public void CreateInterstitialAd(string adUnitId)
        {
        }

        public bool IsLoaded()
        {
            return true;
        }

        public void ShowInterstitial()
        {
        }

        public void DestroyInterstitial()
        {
        }

        public void CreateRewardBasedVideoAd()
        {
        }

        public void SetUserId(string userId)
        {
        }

        public void LoadAd(AdRequest request, string adUnitId)
        {
        }

        public void DestroyRewardBasedVideoAd()
        {
        }

        public void ShowRewardBasedVideoAd()
        {
        }

        public void CreateAdLoader(AdLoader.Builder builder)
        {
        }

        public void Load(AdRequest request)
        {
        }

        public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, AdPosition position)
        {
        }

        public void CreateNativeExpressAdView(string adUnitId, AdSize adSize, int positionX, int positionY)
        {
        }

        public void SetAdSize(AdSize adSize)
        {
        }

        public void ShowNativeExpressAdView()
        {
        }

        public void HideNativeExpressAdView()
        {
        }

        public void DestroyNativeExpressAdView()
        {
        }
    }
}
