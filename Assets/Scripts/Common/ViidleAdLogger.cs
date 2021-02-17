namespace ViidleUnityPlugin.Common
{
	public class ViidleAdLogger
	{
		public enum ViidleAdLogLevel : int
		{
			Debug = 0,
			Warn = 1,
			Error = 2,
			None = int.MaxValue
		}

		private static ViidleAdLogLevel s_LogLevel = ViidleAdLogLevel.None;

		public static ViidleAdLogLevel LogLevel {
			set {
				s_LogLevel = value;
			}
		}

		internal static void D (string format, params object[] args)
		{
			Log (ViidleAdLogLevel.Debug, format, args);
		}

		internal static void W (string format, params object[] args)
		{
			Log (ViidleAdLogLevel.Warn, format, args);
		}

		internal static void E (string format, params object[] args)
		{
			Log (ViidleAdLogLevel.Error, format, args);
		}

		private static void Log (ViidleAdLogLevel level, string format, params object[] args)
		{
			if (level >= s_LogLevel) {
				switch (level) {
				case ViidleAdLogLevel.Debug:
					UnityEngine.Debug.LogFormat (format, args);
					break;
				case ViidleAdLogLevel.Warn:
					UnityEngine.Debug.LogWarningFormat (format, args);
					break;
				case ViidleAdLogLevel.Error:
					UnityEngine.Debug.LogErrorFormat (format, args);
					break;
				}
			}
		}
	}
}