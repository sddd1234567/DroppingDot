namespace ViidleUnityPlugin.AD
{
	using System;
	using Log = ViidleUnityPlugin.Common.ViidleAdLogger;

	public abstract class ViidleAd : IDisposable
	{
		public delegate void ViidleLoadCompleted (ViidleAd instance);
		public delegate void ViidleLoadFailed (ViidleAd instance, ViidleAdErrorType errorCode, string errorMessage);
		public delegate void ViidleShowVideoFailed (ViidleAd instance);
		public delegate void ViidleVideoDisplayed (ViidleAd instance);
		public delegate void ViidleRewardCompleted (ViidleAd instance, string currencyName, int currencyAmount);
		public delegate void ViidleVideoClosed (ViidleAd instance);

		public event ViidleLoadCompleted LoadCompleted;
		public event ViidleLoadFailed LoadFailed;
		public event ViidleShowVideoFailed ShowVideoFailed;
		public event ViidleVideoDisplayed VideoDisplayed;
		public event ViidleRewardCompleted RewardCompleted;
		public event ViidleVideoClosed VideoClosed;

		public bool IsReady
		{
			get {return !m_isReleased && IsReadyInternal();}
		}

		private bool m_isReleased = false;
		private bool m_isLoading = false;
		private bool m_isShowing = false;
		private bool m_isLoadSuccess = false;

		protected void LoadCompletedCallback ()
		{
			Log.D ("Load completed.");
			m_isLoadSuccess = true;
			m_isLoading = false;
			if (null != LoadCompleted) {
				LoadCompleted (this);
			}
		}

		protected void LoadFailedCallback (int errorCode, string errorMessage)
		{
			Log.E ("Load failed." + " errorCode:" + errorCode.ToString() + " errorMessage:" + errorMessage);
			m_isLoadSuccess = false;
			m_isLoading = false;
			if (null != LoadFailed) {
				LoadFailed (this, (ViidleAdErrorType)errorCode, errorMessage);
			}
		}

		protected void ShowVideoFailedCallback ()
		{
			Log.E ("Show video failed.");
			m_isShowing = false;
			if (null != ShowVideoFailed) {
				ShowVideoFailed (this);
			}
		}

		protected void VideoDisplayedCallback ()
		{
			Log.D ("Video displayed.");
			if (null != VideoDisplayed) {
				VideoDisplayed (this);
			}
		}

		protected void RewardCompletedCallback (string currencyName, int currencyAmount)
		{
			Log.D ("Reward completed." + " currencyName:" + currencyName + " currencyAmount:" + currencyAmount.ToString());
			if (null != RewardCompleted) {
				RewardCompleted (this, currencyName, currencyAmount);
			}
		}

		protected void VideoClosedCallback ()
		{
			Log.D ("Video closed.");
			m_isShowing = false;
			m_isLoadSuccess = false;
			if (null != VideoClosed) {
				VideoClosed (this);
			}
		}

		protected enum ViidleAdCallbackType : int
		{
			LoadCompleted,
			LoadFailed,
			ShowFailed,
			VideoDisplayed,
			VideoCompleted,
			RewardCompleted,
		}

		public enum ViidleAdErrorType : int
		{
			RequiredParameterEmpty = 400,
			CountryCodeIsInvalid = 401,
			AdunitDataIsNotFound = 500,
			AdnetworkIsNotConnected = 501,
			AppIdIsInvalid = 502,
			AdunitIsStopped = 503,
			AdnetworkEmpty = 504,
			MediationServiceThrowsException = 505,
			NetworkError = 600,
			NoFillFromAdServer = 601
		}

		public static ViidleAd NewInstance (string unitId)
		{
			#if !UNITY_EDITOR && UNITY_IPHONE
			return new ViidleUnityPlugin.Platform.iOS.IOSViidleAd(unitId);
			#elif !UNITY_EDITOR && UNITY_ANDROID
			return new ViidleUnityPlugin.Platform.android.AndroidViidleAd(unitId);
			#else
			return null;
			#endif
		}

		public void Load ()
		{
			if (isReleased ()) {
				return;
			}

			if (m_isLoading) {
				Log.W ("An ad is already loading.");
				return;
			}

			m_isLoading = true;
			LoadInternal ();
		}

		public void Show ()
		{
			if (isReleased ()) {
				return;
			}

			if (m_isShowing) {
				Log.W ("An ad is already showing.");
				return;
			}
			if (!m_isLoadSuccess) {
				Log.W ("There is no ad to show.");
				return;
			}

			m_isShowing = true;
			ShowInternal ();
		}

		public void StartTestMode ()
		{
			if (isReleased ()) {
				return;
			}

			StartTestModeInternal ();
		}

		public void SetAutoReload (bool isAutoReload)
		{
			if (isReleased ()) {
				return;
			}

			SetAutoReloadInternal (isAutoReload);
		}

		public void SetUserId (string userId)
		{
			if (isReleased ()) {
				return;
			}

			SetUserIdInternal (userId);
		}

		public void Release ()
		{
			if (isReleased ()) {
				return;
			}

			m_isReleased = true;
			ReleaseInternal ();
		}

		public bool isReleased () {
			if (m_isReleased) {
				Log.W ("ViidleAd is already released.");
			}
			return m_isReleased;
		}

		public void Resume ()
		{
			if (m_isReleased) {
				return;
			}

			ResumeInternal ();
		}

		public void Pause ()
		{
			if (m_isReleased) {
				return;
			}

			PauseInternal ();
		}
			
		internal abstract void LoadInternal ();
		internal abstract void ShowInternal ();
		internal abstract void StartTestModeInternal ();
		internal abstract void SetAutoReloadInternal (bool isAutoReload);
		internal abstract bool IsReadyInternal ();
		internal abstract void SetUserIdInternal (string userId);
		internal abstract void ReleaseInternal ();
		internal abstract void ResumeInternal ();
		internal abstract void PauseInternal ();
	
		public abstract void Dispose ();
	}
}