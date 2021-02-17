#if UNITY_IPHONE
namespace ViidleUnityPlugin.Platform.iOS
{
	using ViidleUnityPlugin.AD;
	using System;
	using System.Runtime.InteropServices;

	internal class IOSViidleAd : ViidleAd
	{
		private delegate void ViidleAdUnityLoadCompletedCallback (IntPtr selfPtr);
		private delegate void ViidleAdUnityLoadFailedCallback (IntPtr selfPtr, int errorCode, string errorMessage);
		private delegate void ViidleAdUnityShowVideoFailedCallback (IntPtr selfPtr);
		private delegate void ViidleAdUnityVideoDisplayedCallback (IntPtr selfPtr);
		private delegate void ViidleAdUnityRewardCompletedCallback (IntPtr selfPtr, string currencyName, int currencyAmount);
		private delegate void ViidleAdUnityVideoClosedCallback (IntPtr selfPtr);

		private IntPtr m_iOSPtr;
		private IntPtr m_selfPtr;

		internal IOSViidleAd (string unitId) : base ()
		{
			m_selfPtr = (IntPtr)GCHandle.Alloc (this);
			m_iOSPtr = _CreateViidleAd (m_selfPtr, unitId, ViidleAdLoadCompletedCallback, ViidleAdLoadFailedCallback, ViidleAdShowVideoFailedCallback, ViidleAdVideoDisplayedCallback, ViidleAdRewardCompletedCallback, ViidleAdVideoClosedCallback);
		}

		internal override void LoadInternal ()
		{
			_LoadViidleAd (m_iOSPtr);
		}

		internal override void ShowInternal ()
		{
			_ShowViidleAd (m_iOSPtr);
		}

		internal override void StartTestModeInternal ()
		{
			_TestMode (m_iOSPtr);
		}

		internal override void SetAutoReloadInternal (bool isAutoReload)
		{
			_SetAutoReload (m_iOSPtr, isAutoReload);
		}

		internal override bool IsReadyInternal ()
		{
			return _IsReady (m_iOSPtr);
		}

		internal override void SetUserIdInternal (string userId)
		{
			_SetUserId (m_iOSPtr, userId);
		}

		internal override void ReleaseInternal ()
		{
			_ReleaseViidleAd (m_iOSPtr);
			GCHandle handle = (GCHandle)m_selfPtr;
			handle.Free ();
		}

		internal override void ResumeInternal ()
		{
		}

		internal override void PauseInternal ()
		{
		}

		[AOT.MonoPInvokeCallbackAttribute (typeof(ViidleAdUnityLoadCompletedCallback))]
		private static void ViidleAdLoadCompletedCallback (IntPtr selfPtr)
		{
			GCHandle handle = (GCHandle)selfPtr;
			IOSViidleAd instance = (IOSViidleAd)handle.Target;
			instance.LoadCompletedCallback ();
		}

		[AOT.MonoPInvokeCallbackAttribute (typeof(ViidleAdUnityLoadFailedCallback))]
		private static void ViidleAdLoadFailedCallback (IntPtr selfPtr, int errorCode, string errorMessage)
		{
			GCHandle handle = (GCHandle)selfPtr;
			IOSViidleAd instance = (IOSViidleAd)handle.Target;
			instance.LoadFailedCallback (errorCode, errorMessage);
		}

		[AOT.MonoPInvokeCallbackAttribute (typeof(ViidleAdUnityShowVideoFailedCallback))]
		private static void ViidleAdShowVideoFailedCallback (IntPtr selfPtr)
		{
			GCHandle handle = (GCHandle)selfPtr;
			IOSViidleAd instance = (IOSViidleAd)handle.Target;
			instance.ShowVideoFailedCallback ();
		}

		[AOT.MonoPInvokeCallbackAttribute (typeof(ViidleAdUnityVideoDisplayedCallback))]
		private static void ViidleAdVideoDisplayedCallback (IntPtr selfPtr)
		{
			GCHandle handle = (GCHandle)selfPtr;
			IOSViidleAd instance = (IOSViidleAd)handle.Target;
			instance.VideoDisplayedCallback ();
		}

		[AOT.MonoPInvokeCallbackAttribute (typeof(ViidleAdUnityRewardCompletedCallback))]
		private static void ViidleAdRewardCompletedCallback (IntPtr selfPtr, string currencyName, int currencyAmount)
		{
			GCHandle handle = (GCHandle)selfPtr;
			IOSViidleAd instance = (IOSViidleAd)handle.Target;
			instance.RewardCompletedCallback (currencyName, currencyAmount);
		}

		[AOT.MonoPInvokeCallbackAttribute (typeof(ViidleAdUnityVideoClosedCallback))]
		private static void ViidleAdVideoClosedCallback (IntPtr selfPtr)
		{
			GCHandle handle = (GCHandle)selfPtr;
			IOSViidleAd instance = (IOSViidleAd)handle.Target;
			instance.VideoClosedCallback ();
		}

		~IOSViidleAd ()
		{
			Dispose ();
		}

		public override void Dispose ()
		{
			ViidleUnityPlugin.Common.ViidleAdLogger.D ("Dispose IOSViidleAd.");
		}

		[DllImport ("__Internal")]
		private static extern IntPtr _CreateViidleAd (IntPtr unityPtr, string unitId, ViidleAdUnityLoadCompletedCallback loadCompletedCallback, ViidleAdUnityLoadFailedCallback loadFailedCallback, ViidleAdUnityShowVideoFailedCallback showVideoFailedCallback, ViidleAdUnityVideoDisplayedCallback videoDisplayedCallback, ViidleAdUnityRewardCompletedCallback rewardCompletedCallback, ViidleAdUnityVideoClosedCallback videoClosedCallback);

		[DllImport ("__Internal")]
		private static extern void _LoadViidleAd (IntPtr iOSPtr);

		[DllImport ("__Internal")]
		private static extern void _ShowViidleAd (IntPtr iOSPtr);

		[DllImport ("__Internal")]
		private static extern void _TestMode (IntPtr iOSPtr);

		[DllImport ("__Internal")]
		private static extern bool _SetAutoReload (IntPtr iOSPtr, bool isAutoReload);

		[DllImport ("__Internal")]
		private static extern bool _IsReady (IntPtr iOSPtr);

		[DllImport ("__Internal")]
		private static extern bool _SetUserId (IntPtr iOSPtr, string userId);

		[DllImport ("__Internal")]
		private static extern void _ReleaseViidleAd (IntPtr iOSPtr);
	}
}
#endif

