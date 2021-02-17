#if UNITY_IPHONE
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;
using System.Collections.Generic;

public class ViidleIOSXcodeSetup : MonoBehaviour
{
	[PostProcessBuild]
	static void OnPostprocessBuild (BuildTarget buildTarget, string path)
	{
		if (buildTarget != BuildTarget.iOS) {
			return;
		}
			
		string projPath = path + "/Unity-iPhone.xcodeproj/project.pbxproj";

		PBXProject proj = new PBXProject();
		proj.ReadFromFile(projPath);

		string target = proj.TargetGuidByName ("Unity-iPhone");

		proj.AddFrameworkToProject (target, "AdSupport.framework", false);
		proj.AddFrameworkToProject (target, "AudioToolbox.framework", false);
		proj.AddFrameworkToProject (target, "AVFoundation.framework", false);
		proj.AddFrameworkToProject (target, "CFNetwork.framework", false);
		proj.AddFrameworkToProject (target, "CoreGraphics.framework", false);
		proj.AddFrameworkToProject (target, "CoreMedia.framework", false);
		proj.AddFrameworkToProject (target, "Foundation.framework", false);
		proj.AddFrameworkToProject (target, "MediaPlayer.framework", false);
		proj.AddFrameworkToProject (target, "QuartzCore.framework", false);
		proj.AddFrameworkToProject (target, "StoreKit.framework", false);
		proj.AddFrameworkToProject (target, "SystemConfiguration.framework", false);
		proj.AddFrameworkToProject (target, "UIKit.framework", false);
		proj.AddFrameworkToProject (target, "CoreData.framework", false);
		proj.AddFrameworkToProject (target, "CoreMotion.framework", false);
		proj.AddFrameworkToProject (target, "ImageIO.framework", false);
		proj.AddFrameworkToProject (target, "MapKit.framework", false);
		proj.AddFrameworkToProject (target, "MobileCoreServices.framework", false);
		proj.AddFrameworkToProject (target, "Security.framework", false);
		proj.AddFrameworkToProject (target, "WebKit.framework", false);
		proj.AddFrameworkToProject (target, "libc++.tbd", false);
		proj.AddFrameworkToProject (target, "libsqlite3.0.tbd", false);
		proj.AddFrameworkToProject (target, "libxml2.tbd", false);
		proj.AddFrameworkToProject (target, "libz.tbd", false);

		proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");

		proj.WriteToFile(projPath);
	}
}
#endif