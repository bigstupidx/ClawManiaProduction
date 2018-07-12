using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AssetBundleManager : MonoBehaviour {
	static AssetBundleManager instance;
	static string sURL_Server = "http://www.gemugemu.com/bundles/";


	void OnDestroy()
	{
		instance = null;
	}

	public static AssetBundleManager GetInstance()
	{
		if ( instance == null )
		{
			GameObject ob = new GameObject();
			AssetBundleManager abm = ob.AddComponent<AssetBundleManager>();
			instance = abm;
		}

		return instance;
	}
	// A dictionary to hold the AssetBundle references
	static private Dictionary<string, AssetBundleRef> dictAssetBundleRefs;
	
	public static bool IsAssetExists(string keyName)
	{
		return dictAssetBundleRefs.ContainsKey(keyName);
	}
	
	public static void AddAsset(string keyName,string url,int version,AssetBundle assetBundle)
	{
		
		AssetBundleRef abRef = new AssetBundleRef (url, version);
		abRef.assetBundle = assetBundle;
		dictAssetBundleRefs.Add (keyName, abRef);
	}
	
	
	// Class with the AssetBundle reference, url and version
	private class AssetBundleRef {
		public AssetBundle assetBundle = null;
		public int version;
		public string url;
		public AssetBundleRef(string strUrlIn, int intVersionIn) {
			url = strUrlIn;
			version = intVersionIn;
		}
	};
	// Get an AssetBundle
	public static AssetBundle getAssetBundle (string name, int version){
		//string keyName = url + version.ToString();
		string keyName = name;
		AssetBundleRef abRef;
		if (dictAssetBundleRefs.TryGetValue(keyName, out abRef))
			return abRef.assetBundle;
		else
			return null;
	}

	public void PlayAssetBundle( string name)
	{
		if (bDownloadingAsset)
			return;
		if ( dictAssetBundleRefs == null )
			dictAssetBundleRefs = new Dictionary<string, AssetBundleRef>();
		StartCoroutine(downloadAssetBundle ( name, 1));
	}
	
	public bool IsDownloading()
	{
		return bDownloadingAsset;
	}
	
	public WWW getWWW()
	{
		return www;
	}

	static WWW www;
	static bool bDownloadingAsset = false;
	public static IEnumerator downloadAssetBundle (string name, int version)
	{
		bDownloadingAsset = true;
		//string keyName = name + version.ToString();
		string keyName = name;
		if (dictAssetBundleRefs.ContainsKey(keyName))
			yield return null;
		else 
		{
			Debug.LogError("[downloadAssetBundle] name="+name);
			
			string sCRC = "";
			if ( PlayerPrefs.HasKey(name+".crc") )
			{
				sCRC = PlayerPrefs.GetString(name+".crc");
			}
			else
				Debug.LogError("Can not find crc in playerprefs");
			string sURL = sURL_Server;
			if ( Application.platform == RuntimePlatform.IPhonePlayer )
				sURL += "iPhone";
			else if ( Application.platform == RuntimePlatform.Android )
				sURL += "Android";
			else if ( Application.platform == RuntimePlatform.WindowsPlayer )
				sURL += "StandaloneWindows";
			else if ( Application.isEditor )
				sURL += "StandaloneWindows";

			string sURL2 = sURL;
			sURL2 += "/download.php";
			sURL += "/"+name.Replace(" ","%20");
			// download crc
			www = new WWW(sURL+".crc");
			Debug.LogError("Downloading CRC Info "+(sURL+".crc"));
			while ( www.isDone == false )
			{
				yield return null;
			}
			string sTargetCRC = "";
			if ( string.IsNullOrEmpty(www.error) )
			{
				sTargetCRC = www.text;
				sTargetCRC = sTargetCRC.Replace("\n","");
				Debug.LogError("sTargetCRC="+sTargetCRC);
			}
			else
			{
				Debug.LogError("Downloading CRC Fail");
			}
			www.Dispose();
			www = null;
			Debug.LogError("downloadAssetBundle "+sURL+" sCRC="+sCRC+" sTarget="+sTargetCRC);
			//sURL = "http://www.gemugemu.com/bundles/"+platfo
			//GUI_Debug.Log("[ResourceManager] downloadAssetBundle: "+url);
			if ( sCRC != "" )
				www = WWW.LoadFromCacheOrDownload (sURL2, version, uint.Parse(sCRC) );
			else
				www = WWW.LoadFromCacheOrDownload (sURL2, version );
			while ( www != null && www.isDone == false )
			{
				yield return www;
				Debug.Log("[ResourceManager] downloadAssetBundle: Done downloading "+name+".unity3d");
				
				AssetBundleRef abRef = new AssetBundleRef (sURL, version);
				abRef.assetBundle = www.assetBundle;
				dictAssetBundleRefs.Add (keyName, abRef);
				www.assetBundle.LoadAllAssets();	
				PlayerPrefs.SetString(name+".crc",sTargetCRC);
				
			}
			/*
			sURL = "D:/___aji_projects2/GemuGemu/Bundles/StandaloneWindows/"+name;
			Debug.LogError(sURL);
			AssetBundle bundle = AssetBundle.CreateFromFile(sURL);
			*/
		}
		
		Debug.LogError("downloadAssetBundle "+name+ " Done");
		bDownloadingAsset = false;
		Application.LoadLevel("SceneGameplay");
	}
}
