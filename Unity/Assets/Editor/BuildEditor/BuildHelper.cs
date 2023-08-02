using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
using UnityEditor.iOS.Xcode.Extensions;
#endif

namespace BuildTool
{
	public static class BuildHelper
	{
		
		[MenuItem("Tools/web资源服务器")]
		public static void OpenFileServer()
		{
			string fileName = "StartServer.bat";
			//string fileName = "test.bat";
			string local = PathUtil.ProjectPath + "../TestServer/";
			string batPath = Path.Combine(local,fileName);
			//ProcessHelper.Run("dotnet", "FileServer.dll", "../FileServer/");
			BatUtil.RunBat(batPath,"",local);
		}


		
		[PostProcessBuild]
		public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
		{
			if (buildTarget == BuildTarget.iOS)
			{
#if UNITY_IOS
				string projPath = PBXProject.GetPBXProjectPath(path);
				PBXProject xcodeProj = new PBXProject();
				xcodeProj.ReadFromString(File.ReadAllText(projPath));
				
				var targetName = xcodeProj.GetUnityFrameworkTargetGuid();//(PBXProject.GetUnityTargetName());

				//添加依赖库
				xcodeProj.AddFrameworkToProject(targetName, "WebKit.framework", true);
				xcodeProj.AddFrameworkToProject(targetName, "StoreKit.framework", true);
				xcodeProj.AddFrameworkToProject(targetName, "SafariServices.framework", true);
				xcodeProj.AddFrameworkToProject(targetName, "MessageUI.framework", true);
				xcodeProj.AddFrameworkToProject(targetName, "MobileCoreServices.framework", true);
				xcodeProj.AddFrameworkToProject(targetName, "MediaPlayer.framework", true);
				xcodeProj.AddFrameworkToProject(targetName, "CoreTelephony.framework", true);
				xcodeProj.AddFrameworkToProject(targetName, "CoreLocation.framework", true);
				//xcodeProj.AddFrameworkToProject(targetName, "SystemConfiguration.framework", true);
				xcodeProj.AddFrameworkToProject(targetName, "JavaScriptCore.framework", true);
				xcodeProj.AddFrameworkToProject(targetName, "CoreFoundation.framework", true);
				xcodeProj.AddFrameworkToProject(targetName, "AppsFlyerLib.framework", true);
			 	xcodeProj.AddFrameworkToProject(targetName, "Accelerate.framework", true);

				xcodeProj.AddFrameworkToProject(targetName, "libc++.tbd", true);
				xcodeProj.AddFrameworkToProject(targetName, "libresolv.9.tbd", true);
				xcodeProj.AddFrameworkToProject(targetName, "libsqlite3.tbd", true);
				xcodeProj.AddFrameworkToProject(targetName, "libxml2.tbd", true);				
				xcodeProj.AddFrameworkToProject(targetName, "libbz2.tbd", true);
				xcodeProj.AddFrameworkToProject(targetName, "libz.tbd", true);
			
				//设置关闭bitcode
				xcodeProj.SetBuildProperty(targetName, "ENABLE_BITCODE", "NO");
				//xcodeProj.SetBuildProperty(targetName, "PRODUCT_NAME", "Zombie Clash");
				xcodeProj.SetBuildProperty(targetName,"GCC_ENABLE_OBJC_EXCEPTIONS","YES");
				xcodeProj.SetBuildProperty(targetName,"Apple Clang-Language-Objective-C","YES");
				
				xcodeProj.AddFileToEmbedFrameworks(targetName,"FBAudienceNetwork.framework");
				//embedded binaries -添加 FBAudienceNetwork.framework
				/*const string defaultLocationInProj = "Plugins/iOS";//framework 存放的路径
				const string coreFrameworkName = "FBAudienceNetwork.framework";// framework 的文件名
				string framework = Path.Combine(defaultLocationInProj, coreFrameworkName);
				string fileGuid = xcodeProj.AddFile(framework, "Frameworks/" + framework, PBXSourceTree.Sdk);*/

				xcodeProj.AddBuildProperty(targetName, "HEADER_SEARCH_PATHS", "$(SRCROOT)/Libraries/Plugins/iOS");
				xcodeProj.AddBuildProperty(targetName, "LIBRARY_SEARCH_PATHS", "$(SRCROOT)/Libraries");
				 

				//PBXProjectExtensions.(xcodeProj, targetName, fileGuid);

				/*
				string str = "";
				xcodeProj.AddShellScriptBuildPhase(targetName, "/bin/sh",str);*/
				
				
				string path2 = path + "/Info.plist";
				PlistDocument plistDocument = new PlistDocument();
				plistDocument.ReadFromFile(path2);
				PlistElementDict dict = plistDocument.root.AsDict();
				
				dict.SetString("AppLovinSdkKey", "9hZx6KZPde6GIbp6OX3zn0M3HzROhgMsNxoTGLtv_-AHZzNzURwFRGJkEl6xQTnoPZFfv7Tv5iAegTb9-WPQni");
				dict.SetBoolean("GADIsAdManagerApp", true);
				dict.SetString("GADApplicationIdentifier", "ca-app-pub-5470400114155059~7251495091");
				dict.SetString("FacebookDisplayName", "Zombie Clash: Survival");
				dict.SetString("FacebookAppID", "1325284034308731");
				dict.SetString("CFBundleIdentifier", PlayerSettings.applicationIdentifier);
				
				//复杂的是，配置urlSchemes（数组里面有字典，字典里面有子数组）
				PlistElementArray urlTypes = plistDocument.root.CreateArray("CFBundleURLTypes");
				PlistElementDict itemDict;
 
				itemDict = urlTypes.AddDict();
				itemDict.SetString("CFBundleTypeRole","Editor");
				//itemDict.SetString("CFBundleURLName", "xxxx");
				PlistElementArray schemesArray1 = itemDict.CreateArray("CFBundleURLSchemes");
				schemesArray1.AddString("rangersapplog.a48e37b6d2aaa764");
				//  
				// PlistElementDict itemDict2;
 			//
			 // 	itemDict2 = urlTypes.AddDict();
			 // 	PlistElementArray schemesArray2= itemDict2.CreateArray("CFBundleURLSchemes");
			 // 	schemesArray2.AddString("fb1325284034308731");

				plistDocument.WriteToFile(path2);

	
				/*var plistPath = Path.Combine(path, "Info.plist");
				PlistDocument plist = new UnityEditor.iOS.Xcode.PlistDocument();
				plist.ReadFromString(File.ReadAllText(plistPath));
				var rootDict = plist.root;
			 
				rootDict.SetString("AppLovinSdkKey", "9hZx6KZPde6GIbp6OX3zn0M3HzROhgMsNxoTGLtv_-AHZzNzURwFRGJkEl6xQTnoPZFfv7Tv5iAegTb9-WPQni");
				rootDict.SetBoolean("GADIsAdManagerApp", true);
				rootDict.SetString("GADApplicationIdentifier", "ca-app-pub-5470400114155059~7251495091");
				rootDict.SetString("FacebookDisplayName", "Zombie Clash - Normandy");*/
				


				var targetNameUnity = xcodeProj.GetUnityMainTargetGuid();
				xcodeProj.AddFrameworkToProject(targetNameUnity, "StoreKit.framework", true);

				xcodeProj.SetBuildProperty(targetNameUnity, "ENABLE_BITCODE", "NO");
				xcodeProj.SetBuildProperty(targetNameUnity,"GCC_ENABLE_OBJC_EXCEPTIONS","YES");
				xcodeProj.SetBuildProperty(targetNameUnity, "PRODUCT_NAME", "Zombie Clash");
				xcodeProj.SetBuildProperty(targetNameUnity,"Apple Clang-Language-Objective-C","YES");
				xcodeProj.SetBuildProperty(targetNameUnity,"PRODUCT_BUNDLE_IDENTIFIER","com.hg.clash.zombie");
				xcodeProj.SetBuildProperty(targetNameUnity, "PRODUCT_NAME", "Zombie Clash");
				
				xcodeProj.SetTeamId(targetNameUnity,"9BFNW9AHMQ");
				 
				xcodeProj.AddCapability (targetNameUnity, PBXCapabilityType.InAppPurchase);//内购
			 
			 	xcodeProj.AddShellScriptBuildPhase(targetNameUnity, "Run Script", "/bin/sh", "APP_PATH=\"${TARGET_BUILD_DIR}/${WRAPPER_NAME}\"\n\n# This script loops through the frameworks embedded in the application and\n# removes unused architectures.\nfind \"$APP_PATH\" -name '*.framework' -type d | while read -r FRAMEWORK\ndo\nFRAMEWORK_EXECUTABLE_NAME=$(defaults read \"$FRAMEWORK/Info.plist\" CFBundleExecutable)\nFRAMEWORK_EXECUTABLE_PATH=\"$FRAMEWORK/$FRAMEWORK_EXECUTABLE_NAME\"\necho \"Executable is $FRAMEWORK_EXECUTABLE_PATH\"\n\nEXTRACTED_ARCHS=()\n\nfor ARCH in $ARCHS\ndo\necho \"Extracting $ARCH from $FRAMEWORK_EXECUTABLE_NAME\"\nlipo -extract \"$ARCH\" \"$FRAMEWORK_EXECUTABLE_PATH\" -o \"$FRAMEWORK_EXECUTABLE_PATH-$ARCH\"\nEXTRACTED_ARCHS+=(\"$FRAMEWORK_EXECUTABLE_PATH-$ARCH\")\ndone\n\necho \"Merging extracted architectures: ${ARCHS}\"\nlipo -o \"$FRAMEWORK_EXECUTABLE_PATH-merged\" -create \"${EXTRACTED_ARCHS[@]}\"\nrm \"${EXTRACTED_ARCHS[@]}\"\n\necho \"Replacing original executable with thinned version\"\nrm \"$FRAMEWORK_EXECUTABLE_PATH\"\nmv \"$FRAMEWORK_EXECUTABLE_PATH-merged\" \"$FRAMEWORK_EXECUTABLE_PATH\"\n\ndone\n");
				
					//proj.AddBuildProperty(target, "OTHER_LDFLAGS", "-ObjC");
				
				string UnityAppControllerMM     = Application.dataPath + "/Editor/CPPCource/UnityAppController.mm";
 
				string tagUnityAppControllerMM  = path + "/Classes/UnityAppController.mm";
  
				if (File.Exists(tagUnityAppControllerMM))
				{
					File.Delete(tagUnityAppControllerMM);
				}
 
				File.Copy(UnityAppControllerMM, tagUnityAppControllerMM);
			 
				
				//保存工程
				xcodeProj.WriteToFile(projPath);
#endif
			}
		}
		

		#region Custom

		public static string DoShellCmd(string cmd,string arg)
		{
			try
			{
				Process p = new Process
				{
					StartInfo =
					{
						FileName = cmd,
						Arguments = arg,
						UseShellExecute = false,
						RedirectStandardInput = true,
						RedirectStandardOutput = true,
						RedirectStandardError = true,
						CreateNoWindow = true
					}
				};
				p.Start();
				p.WaitForExit();
				string strResult = p.StandardOutput.ReadToEnd();
				p.Close();
				return strResult;
			}
			catch (System.Exception ex)
			{
				UnityEngine.Debug.Log(ex);
			}

			return "";
		}		
	
		

		#endregion
	}
 
}
