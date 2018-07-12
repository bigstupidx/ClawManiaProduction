// Builds an asset bundle from the selected objects in the project view,
// and changes the texture format using an AssetPostprocessor.

using UnityEngine;
using UnityEditor;
using System.IO;

public class ExportAssetBundles:EditorWindow {
	/*
	private static string sNameTarget = "Test";
	private static string sTargetPath = "";

	private static bool bDependencies = false;
	private static bool bCompleteAssets = false;
	private static bool bDeterministicAssetBundle = false;
	private static bool bUncompressed = false;


*/

	static BuildAssetBundleOptions options =  
		BuildAssetBundleOptions.CollectDependencies 
			| BuildAssetBundleOptions.CompleteAssets 
			//| BuildAssetBundleOptions.DeterministicAssetBundle 
			//| BuildAssetBundleOptions.UncompressedAssetBundle
			;



	private static BuildTarget[] platforms = new BuildTarget[] 
	{ 
		//BuildTarget.StandaloneWindows,
		BuildTarget.Android
		//BuildTarget.iPhone
	};
	[MenuItem("Tools/Build AssetBundles")]
	static void BuildAssetBundle () 
	{

		string sTargetFolder = ".\\Bundles";
		string sCurrentFolder = "";

		for ( int i=0; i<platforms.Length; i++ )
		{
			BuildTarget target = platforms[i];
			Debug.Log("target="+target.ToString());
			string sAssetName = "Assets/GameClawMania/Scenes/";
			string[] levels = new string[]{	
				//"Assets/GameClawMania/Scenes/Loading.unity",
				//"Assets/GameClawMania/Scenes/Menu.unity",
				"Assets/GameClawMania/Scenes/SceneGameplay.unity"
			};
			/*
			string[] names = new string[]{
				"Loading","Menu","SceneGameplay"
			};
			
			for ( int j=0; j<levels.Length; j++ )
			{
				string sLevel = levels[j];
				string sName = names[j];
				string[] targetLevel = new string[] { sLevel };
				string sTargetFile = sTargetFolder+"\\"+target.ToString()+"\\"+sName+".unity3d";
				Debug.LogError(sTargetFile);
				uint crc = 0;
				BuildPipeline.BuildStreamedSceneAssetBundle(targetLevel, sTargetFile, target, out crc);
				var sr = File.CreateText(sTargetFile+".crc");
				sr.Write (crc.ToString());
				sr.Close();
			}
			*/
			
			uint crc = 0;
			string sTargetFile = sTargetFolder+"\\"+target.ToString()+"\\"+"ClawMania.unity3d";
			BuildPipeline.BuildStreamedSceneAssetBundle(levels, sTargetFile, target, out crc);
			var sr = File.CreateText(sTargetFile+".crc");
			sr.Write (crc.ToString());
			sr.Close();
		}
	}
}