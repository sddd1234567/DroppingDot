#if UNITY_ANDROID
namespace ViidleUnityPlugin.Platform.android
{
	using ViidleUnityPlugin.AD;
	using System;
	using UnityEngine;
	using System.Runtime.InteropServices;
	using Log = ViidleUnityPlugin.Common.ViidleAdLogger;

	internal class AndroidViidleAd : ViidleAd
	{
		private const string ViidleAdClassName = "net.viidle.unity.plugin.ViidleUnityAd";
		private const string UnityPlayerClassName = "com.unity3d.player.UnityPlayer";
		private AndroidJavaObject m_JavaObject;
		private Listener m_listener;

		internal AndroidViidleAd (string unitId) : base ()
		{
			m_listener = new Listener (this);
			using (var unityPlayer = new AndroidJavaClass (UnityPlayerClassName)) {
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity")) {
					m_JavaObject = new AndroidJavaObject (ViidleAdClassName, unitId, activity, m_listener);
				}
			}
		}

		internal override void LoadInternal ()
		{
			m_JavaObject.Call ("loadViidleAd");
		}

		internal override void ShowInternal ()
		{
			using (var unityPlayer = new AndroidJavaClass (UnityPlayerClassName)) {
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity")) {
					m_JavaObject.Call ("showViidleAd", activity);
				}
			}
		}

		internal override void StartTestModeInternal ()
		{
			using (var unityPlayer = new AndroidJavaClass (UnityPlayerClassName)) {
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity")) {
					m_JavaObject.Call ("testMode", activity);
				}
			}
		}

		internal override void SetAutoReloadInternal (bool isAutoReload)
		{
			m_JavaObject.Call ("setAutoReload", isAutoReload);
		}

		internal override bool IsReadyInternal ()
		{
			return m_JavaObject.Call<bool> ("isReady");
		}

		internal override void SetUserIdInternal (string userId)
		{
			m_JavaObject.Call ("setUserId", userId);
		}

		internal override void ReleaseInternal ()
		{
			using (var unityPlayer = new AndroidJavaClass (UnityPlayerClassName)) {
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity")) {
					m_JavaObject.Call ("release", activity);
				}
			}
			m_JavaObject.Dispose ();
			m_JavaObject = null;
			m_listener.release ();
		}
			
		internal override void ResumeInternal ()
		{
			Log.D ("Resume activity.");

			using (var unityPlayer = new AndroidJavaClass (UnityPlayerClassName)) {
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity")) {
					m_JavaObject.Call ("resume", activity);
				}
			}
		}

		internal override void PauseInternal ()
		{
			Log.D ("Pause activity.");

			using (var unityPlayer = new AndroidJavaClass (UnityPlayerClassName)) {
				using (var activity = unityPlayer.GetStatic<AndroidJavaObject> ("currentActivity")) {
					m_JavaObject.Call ("pause", activity);
				}
			}
		}

		private class Listener : AndroidJavaProxy
		{
			private const string ViidleAdListenerClassName = "net.viidle.unity.plugin.ViidleUnityAdListener";
			private AndroidViidleAd m_viidleAd;

			internal Listener (AndroidViidleAd viidleAd) : base (ViidleAdListenerClassName)
			{
				m_viidleAd = viidleAd;
			}

			void onLoadCompleted ()
			{
				m_viidleAd.LoadCompletedCallback ();
			}

			void onLoadFailed (int errorCode, string errorMessage)
			{
				m_viidleAd.LoadFailedCallback (errorCode, errorMessage);
			}

			void onShowFailed ()
			{
				m_viidleAd.ShowVideoFailedCallback ();
			}

			void onVideoDisplayed ()
			{
				m_viidleAd.VideoDisplayedCallback ();
			}

			void onRewardCompleted (string currencyName, int currencyAmount)
			{
				m_viidleAd.RewardCompletedCallback (currencyName, currencyAmount);
			}

			void onVideoCompleted ()
			{
				m_viidleAd.VideoClosedCallback ();
			}

			public void release ()
			{
				m_viidleAd = null;
			}
		}

		~AndroidViidleAd ()
		{
			Dispose ();
		}

		public override void Dispose ()
		{
			Log.D ("Dispose AndroidViddleAd.");
		}
	}
}
#endif