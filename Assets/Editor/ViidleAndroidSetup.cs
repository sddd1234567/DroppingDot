namespace ViidleUnityPlugin
{
	using UnityEngine;
	using UnityEditor;
	using System.IO;
	using System.Xml;
	using System.Linq;

	public class ViidleAndroidSetup : EditorWindow
	{
		private static bool isImportGooglePlayServices = false;
		private static bool isImportGooglePlayServicesLocation = false;
		private static int locationSelect = 0;
		private static int writeExternalStorageSelect = 0;

		private const string AndroidSDKRoot = "AndroidSdkRoot";
		private const string GmsDirectoryPath = "extras/google/m2repository/com/google/android/gms";
		private const string GmsArtifactName = "play-services-basement";
		private const string GmsLocationArtifactName = "play-services-location";
		private const string GmsLocationTypeCoarse = "android.permission.ACCESS_COARSE_LOCATION";
		private const string GmsLocationTypeFine = "android.permission.ACCESS_FINE_LOCATION";
		private const string WriteExternalStorage = "android.permission.WRITE_EXTERNAL_STORAGE";

		private const string AndroidLibraryDirectoryPath = "Plugins/Android";

		private bool m_ShowGooglePlayServiceMenu = true;
		private bool m_ShowWriteExternalStorageMenu = true;

		private Vector2 m_ScrollPosition = Vector2.zero;

		private static class Ja
		{
			internal const string ImportGooglePlayServices = "viidleSDKを組み込んだ際はGoogle Play Servicesライブラリーが必要になります。\n既にGoogle Play Servicesがプロジェクトに追加されている場合はチェックを外してください。";
			internal const string ImportGooglePlayServicesLocation = "viidleSDKでは一部広告では適切な広告ターゲティングを行う為に位置情報を使用します。\n位置情報を利用する場合はチェックして、パーミッション宣言を選択してください。";
			internal const string WriteExternalStorage = "viidleSDKではAPIレベル18(Android4.3)以前の端末での動画広告のキャッシュにパーミッションが必要です。\nAPIレベル19以降の端末にもパーミッションを許可すると広告配信のパフォーマンスが向上します。";
			internal const string WarningAndroidSDKPath = "AndroidSDKのパスが設定されていません。\nUnityのPreferences... > External Toolsより設定を行ってください。";
			internal const string WarningGooglePlayServices = "Google Play Servicesがダウンロードされていません。\nAndroid SDK ManagerでGoogle Repositoryをダウンロードしてください。";
			internal const string WarningGooglePlayServicesLocation = "Google Play Services Locationがダウンロードされていません。\nAndroid SDK ManagerでGoogle Repositoryをダウンロードしてください。";
			internal const string AboutUnityPreferences = "Preferences設定について";
			internal const string AboutAndroidSDKManager = "Android SDK Managerについて";
			internal const string AboutGoogleRepository = "Google Repositoryについて";
			internal const string AboutCoarseLocation = "Access Coarse Locationについて";
			internal const string AboutFineLocation = "Access Fine Locationについて";
			internal const string AboutWriteExternalStorage = "Write External Storageについて";
			internal const string UnityPreferencesURL = "https://docs.unity3d.com/ja/current/Manual/Preferences.html";
			internal const string AndroidSDKManagerURL = "https://developer.android.com/studio/intro/update.html?hl=ja#sdk-manager";
			internal const string GoogleRepositoryURL = "https://developer.android.com/studio/intro/update.html?hl=ja#recommended";
		}

		private static class En
		{
			internal const string ImportGooglePlayServices = "When you incorporate viidleSDK you need a Google Play Services library.\nUncheck this if Google Play Services has already been added to the project.";
			internal const string ImportGooglePlayServicesLocation = "In viidleSDK, some advertisements use location information to do advertisement targeting properly.\nTo use location information, please check and select the permission declaration.";
			internal const string WriteExternalStorage = "ViidleSDK requires permission to cache video ads on devices with API level 18 (Android 4.3) or earlier.\nAllowing permissions on terminals after API level 19 will improve ad delivery performance.";
			internal const string WarningAndroidSDKPath = "The Android SDK path is not set.\nPlease make settings from Unity's \"Preferences ...> External Tools\".";
			internal const string WarningGooglePlayServices = "Google Play Services has not been downloaded.\nDownload Google Repository with Android SDK Manager.";
			internal const string WarningGooglePlayServicesLocation = "Google Play Services Location has not been downloaded.\nDownload Google Repository with Android SDK Manager.";
			internal const string AboutUnityPreferences = "About Preferences";
			internal const string AboutAndroidSDKManager = "About Android SDK Manager";
			internal const string AboutGoogleRepository = "About Google Repository";
			internal const string AboutCoarseLocation = "About Access Coarse Location";
			internal const string AboutFineLocation = "About Access Fine Location";
			internal const string AboutWriteExternalStorage = "About Write External Storage";
			internal const string UnityPreferencesURL = "https://docs.unity3d.com/Manual/Preferences.html";
			internal const string AndroidSDKManagerURL = "https://developer.android.com/studio/intro/update.html#sdk-manager";
			internal const string GoogleRepositoryURL = "https://developer.android.com/studio/intro/update.html#recommended";
			internal const string CoarseLocationURL = "https://developer.android.com/reference/android/Manifest.permission.html#ACCESS_COARSE_LOCATION";
			internal const string FineLocationURL = "https://developer.android.com/reference/android/Manifest.permission.html#ACCESS_FINE_LOCATION";
			internal const string WriteExternalStorageURL = "https://developer.android.com/reference/android/Manifest.permission.html#WRITE_EXTERNAL_STORAGE";
		}

		[MenuItem ("viidleSDK/Android Setup", false, 1)]
		public static void MenuItemAndroidSetup ()
		{
			ViidleAndroidSetup window = (ViidleAndroidSetup)EditorWindow.GetWindow (typeof(ViidleAndroidSetup));
			var titleContent = new GUIContent ();
			titleContent.text = "Android Setup";
			window.titleContent = titleContent;

			var vec2 = new Vector2 (470, 360);
			window.minSize = vec2;
			window.Show ();
		}

		void OnGUI ()
		{
			GUIStyle buttonStyle;
			var isJapanese = IsJapanese ();

			m_ScrollPosition = EditorGUILayout.BeginScrollView (m_ScrollPosition);
			{
				m_ShowGooglePlayServiceMenu = EditorGUILayout.Foldout (m_ShowGooglePlayServiceMenu, "Google Play Services");
				if (m_ShowGooglePlayServiceMenu) {
					EditorGUI.indentLevel = 1;
					EditorGUILayout.HelpBox (isJapanese ? Ja.ImportGooglePlayServices : En.ImportGooglePlayServices, MessageType.Info, true);
					isImportGooglePlayServices = EditorGUILayout.ToggleLeft ("Import Google Play Services", isImportGooglePlayServices);
					if (isImportGooglePlayServices) {
						if (!CheckAndroidSDKPath ()) {
							ShowAndroidSDKWarning (isJapanese);
						} else if (!CheckLibrary (GmsDirectoryPath)) {
							ShowAndroidLibraryWarning (isJapanese ? Ja.WarningGooglePlayServices : En.WarningGooglePlayServices, isJapanese ? Ja.AboutGoogleRepository : En.AboutGoogleRepository, isJapanese);
						}
					}

					EditorGUILayout.HelpBox (isJapanese ? Ja.ImportGooglePlayServicesLocation : En.ImportGooglePlayServicesLocation, MessageType.Info, true);
					isImportGooglePlayServicesLocation = EditorGUILayout.ToggleLeft ("Import Google Play Services Location", isImportGooglePlayServicesLocation);
					if (isImportGooglePlayServicesLocation) {
						if (!CheckAndroidSDKPath ()) {
							ShowAndroidSDKWarning (isJapanese);
						} else if (!CheckLibrary (GmsDirectoryPath)) {
							ShowAndroidLibraryWarning (isJapanese ? Ja.WarningGooglePlayServicesLocation : En.WarningGooglePlayServicesLocation, isJapanese ? Ja.AboutGoogleRepository : En.AboutGoogleRepository, isJapanese);
						} else {
							EditorGUILayout.BeginHorizontal();
							{
								buttonStyle = EditorStyles.radioButton;
								buttonStyle.margin = new RectOffset (35, 0, 0, 0);
								locationSelect = GUILayout.SelectionGrid(locationSelect, new string[]{"ACCESS_COARSE_LOCATION", "ACCESS_FINE_LOCATION"}, 1, buttonStyle);

								EditorGUILayout.BeginVertical();
								buttonStyle = new GUIStyle (GUI.skin.button);
								buttonStyle.margin = new RectOffset (0, 20, 0, 0);
								if (GUILayout.Button (isJapanese ? Ja.AboutCoarseLocation : En.AboutCoarseLocation, buttonStyle, GUILayout.ExpandWidth (true))) {
									Application.OpenURL (En.CoarseLocationURL);
								}
								if (GUILayout.Button (isJapanese ? Ja.AboutFineLocation : En.AboutFineLocation, buttonStyle, GUILayout.ExpandWidth (true))) {
									Application.OpenURL (En.FineLocationURL);
								}
								EditorGUILayout.EndVertical();
							}
							EditorGUILayout.EndHorizontal();
						}
					}
				}
				EditorGUI.indentLevel = 0;

				m_ShowWriteExternalStorageMenu = EditorGUILayout.Foldout (m_ShowWriteExternalStorageMenu, "Write External Storage");
				if (m_ShowWriteExternalStorageMenu) {
					EditorGUI.indentLevel = 1;
					EditorGUILayout.HelpBox (isJapanese ? Ja.WriteExternalStorage : En.WriteExternalStorage, MessageType.Info, true);

					EditorGUILayout.BeginHorizontal();
					{
						buttonStyle = EditorStyles.radioButton;
						buttonStyle.margin = new RectOffset (35, 0, 0, 0);
						writeExternalStorageSelect = GUILayout.SelectionGrid(writeExternalStorageSelect, new string[]{"For Max Sdk Version 18", "For All API Level"}, 1, buttonStyle);
					
						EditorGUILayout.BeginVertical();
						buttonStyle = new GUIStyle (GUI.skin.button);
						buttonStyle.margin = new RectOffset (0, 20, 0, 0);
						if (GUILayout.Button (isJapanese ? Ja.AboutWriteExternalStorage : En.AboutWriteExternalStorage, buttonStyle, GUILayout.ExpandWidth (true))) {
							Application.OpenURL (En.WriteExternalStorageURL);
						}
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndHorizontal();
				}
				EditorGUI.indentLevel = 0;
			}
			EditorGUILayout.EndScrollView ();

			buttonStyle = new GUIStyle (GUI.skin.button);
			buttonStyle.margin = new RectOffset (20, 20, 10, 10);
			if (GUILayout.Button ("Configure", buttonStyle, GUILayout.Height (24))) {
				Configure ();
			}
		}

		public void Configure ()
		{
			Debug.Log ("Processing...");
			if (isImportGooglePlayServices) {
				AddLibrary (GmsDirectoryPath, GmsArtifactName);
			}

			if (isImportGooglePlayServicesLocation) {
				AddLibrary (GmsDirectoryPath, GmsLocationArtifactName);
			}

			ConfigureAndroidManifest ();

			AssetDatabase.Refresh ();
			Debug.Log ("Done!");
			Close ();
		}

		private static bool IsJapanese ()
		{
			return Application.systemLanguage == SystemLanguage.Japanese; 
		}

		private static bool CheckAndroidSDKPath ()
		{
			string androidSDKPath = EditorPrefs.GetString (AndroidSDKRoot, null);
			return !string.IsNullOrEmpty (androidSDKPath);
		}

		private static bool CheckLibrary (string libraryPath)
		{
			string androidSDKPath = EditorPrefs.GetString (AndroidSDKRoot, null);
			string path = Path.Combine (androidSDKPath, ToPlatformDirectorySeparator (libraryPath));
			return Directory.Exists (path);
		}

		private static string ToPlatformDirectorySeparator (string path)
		{
			return path.Replace ("/", Path.DirectorySeparatorChar.ToString ());
		}

		private static void AddLibrary (string path, string artifactName)
		{
			string libraryDirectoryPath = Path.Combine (Application.dataPath, ToPlatformDirectorySeparator (AndroidLibraryDirectoryPath));
			string[] archives = Directory.GetFiles (libraryDirectoryPath, artifactName + "*.?.?.aar");
			if (null != archives && 0 < archives.Length) {
				Debug.Log ("The " + artifactName + " is already exist.");
				return;
			}
			string artifactPath = Path.Combine (EditorPrefs.GetString (AndroidSDKRoot, null), Path.Combine (path, artifactName));
			var directoryInfo = new DirectoryInfo (artifactPath);
			if (directoryInfo.Exists) {
				DirectoryInfo[] infos = directoryInfo.GetDirectories ("*.?.?");

				if (null == infos || 0 == infos.Length) {
					Debug.LogWarning ("The " + artifactName + " is not installed.");
					return;
				}
				var max = infos
					.Select (di => di.Name)
					.Aggregate ((current, next) => {
					int currentVersion = int.Parse (current.Replace (".", ""));
					int nextVersion = int.Parse (next.Replace (".", ""));
					return nextVersion > currentVersion ? next : current;
				});
				string archiveName = string.Format (artifactName + "-{0}.aar", max);
				string aarPathFrom = Path.Combine (artifactPath, Path.Combine (max, archiveName));
				string aarPathTo = Path.Combine (libraryDirectoryPath, archiveName);
				FileUtil.CopyFileOrDirectory (aarPathFrom, aarPathTo);
				Debug.Log ("Added: " + aarPathTo);
			} else {
				Debug.LogWarning ("The " + artifactName + " is not installed.");
			}
		}

		private static void ConfigureAndroidManifest ()
		{
			string manifestPathDest = Path.Combine (Application.dataPath, ToPlatformDirectorySeparator (AndroidLibraryDirectoryPath + "/AndroidManifest.xml"));
			if (!File.Exists (manifestPathDest)) {
				string[] UnityAndroidManifestPathList = {
					Path.Combine (EditorApplication.applicationPath, ToPlatformDirectorySeparator ("../PlaybackEngines/AndroidPlayer/Apk/AndroidManifest.xml")),
					Path.Combine (EditorApplication.applicationContentsPath, ToPlatformDirectorySeparator ("PlaybackEngines/AndroidPlayer/Apk/AndroidManifest.xml")),
					Path.Combine (EditorApplication.applicationContentsPath, ToPlatformDirectorySeparator ("PlaybackEngines/AndroidPlayer/AndroidManifest.xml"))
				};
					
				string defaultManifestPath = null;
				foreach (string path in UnityAndroidManifestPathList) {
					if (File.Exists (path)) {
						defaultManifestPath = path;
						Debug.Log ("Found AndroidManifest at " + path);
						break;
					}
				}
				if (null == defaultManifestPath) {
					Debug.LogWarning ("Couldn't find default AndroidManifest.");
					return;
				}
				FileUtil.CopyFileOrDirectory (defaultManifestPath, manifestPathDest);
			} else {
				Debug.Log ("The AndroidManifest is already exist.");
			}
		
			var doc = new XmlDocument ();
			doc.Load (manifestPathDest);
		
			XmlNode applicationNode = doc.SelectSingleNode ("manifest/application");
			if (null == applicationNode) {
				Debug.LogWarning ("The application tag is not found.");
				return;
			}
		
			string ns = applicationNode.GetNamespaceOfPrefix ("android");
			var nsManager = new XmlNamespaceManager (doc.NameTable);
			nsManager.AddNamespace ("android", ns);

			XmlElement rootElement = doc.DocumentElement;
			XmlNodeList nodelist = rootElement.GetElementsByTagName("uses-permission");

			for (int i = 0; i < nodelist.Count; i++)
			{
				XmlElement deleteElement = (XmlElement)nodelist.Item(i);
				string permission = deleteElement.GetAttribute("name", ns);
				if (GmsLocationTypeCoarse.Equals(permission) || GmsLocationTypeFine.Equals(permission) || WriteExternalStorage.Equals(permission)) {
					rootElement.RemoveChild (deleteElement);
				}
			}

			if (isImportGooglePlayServicesLocation) {
				XmlElement locationElement;
				if (locationSelect == 0) {
					locationElement = CreateLocaionPermissionElement (doc, ns, GmsLocationTypeCoarse);
				} else {
					locationElement = CreateLocaionPermissionElement (doc, ns, GmsLocationTypeFine);
				}
				doc.DocumentElement.AppendChild (locationElement);
				Debug.Log ("Added: " + locationElement.OuterXml);
			}

			XmlElement storageElement;
			storageElement = CreateStoragePermissionElement (doc, ns, WriteExternalStorage);
			doc.DocumentElement.AppendChild (storageElement);
			Debug.Log ("Added: " + storageElement.OuterXml);

			doc.Save (manifestPathDest);
		}

		private static bool SearchChildNode (XmlNode parentNode, string xpath, XmlNamespaceManager nsManager)
		{
			XmlNodeList nodes = parentNode.SelectNodes (xpath, nsManager);
			return null != nodes && 0 < nodes.Count;
		}

		private static XmlElement CreateLocaionPermissionElement (XmlDocument doc, string ns, string permission)
		{
			XmlElement element = doc.CreateElement ("uses-permission");
			element.SetAttribute ("name", ns, permission);
			return element;
		}

		private static XmlElement CreateStoragePermissionElement (XmlDocument doc, string ns, string permission)
		{
			XmlElement element = doc.CreateElement ("uses-permission");
			element.SetAttribute ("name", ns, permission);
			if (writeExternalStorageSelect == 0) {
				element.SetAttribute ("maxSdkVersion", ns, "18");
			}
			return element;
		}
			
		private static void ShowAndroidSDKWarning (bool isJapanese)
		{
			GUIStyle buttonStyle;

			EditorGUILayout.HelpBox (isJapanese ? Ja.WarningAndroidSDKPath : En.WarningAndroidSDKPath, MessageType.Warning, true);
			buttonStyle = new GUIStyle (GUI.skin.button);
			buttonStyle.margin = new RectOffset (20, 0, 0, 0);
			if (GUILayout.Button (isJapanese ? Ja.AboutUnityPreferences : En.AboutUnityPreferences, buttonStyle, GUILayout.ExpandWidth (false))) {
				Application.OpenURL (isJapanese ? Ja.UnityPreferencesURL : En.UnityPreferencesURL);
			}
		}

		private static void ShowAndroidLibraryWarning (string WarningLibrary, string WarningRepository, bool isJapanese)
		{
			GUIStyle buttonStyle;

			EditorGUILayout.HelpBox (WarningLibrary, MessageType.Warning, true);
			EditorGUILayout.BeginHorizontal ();
			{
				buttonStyle = new GUIStyle (GUI.skin.button);
				buttonStyle.margin = new RectOffset (20, 0, 0, 0);
				if (GUILayout.Button (isJapanese ? Ja.AboutAndroidSDKManager : En.AboutAndroidSDKManager, buttonStyle, GUILayout.ExpandWidth (false))) {
					Application.OpenURL (isJapanese ? Ja.AndroidSDKManagerURL : En.AndroidSDKManagerURL);
				}
				buttonStyle = new GUIStyle (GUI.skin.button);
				buttonStyle.margin = new RectOffset (10, 0, 0, 0);
				if (GUILayout.Button (WarningRepository, buttonStyle, GUILayout.ExpandWidth (false))) {
					Application.OpenURL (isJapanese ? Ja.GoogleRepositoryURL : En.GoogleRepositoryURL);
				}
			}
			EditorGUILayout.EndHorizontal ();
		}
	}
}