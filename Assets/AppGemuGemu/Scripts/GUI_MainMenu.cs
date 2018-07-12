using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GUI_MainMenu : MonoBehaviour {
	static string sURL_Server = "http://www.ajipamungkas.com/pub/";
	// Use this for initialization
	void Start () {
		//Caching.CleanCache ();
		//PlayerPrefs.DeleteAll ();
		dictAssetBundleRefs = new Dictionary<string, AssetBundleRef>();
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if ( bDownloadingAsset && www != null && www.isDone == false )
		{
			if ( currGameContent != null )
			{
				currGameContent.progressBar.gameObject.SetActive (true);
				currGameContent.progressBar.value = www.progress;
			}
		}
		*/
	}


	Game_Content currGameContent;
	public void OnClickGame()
	{
		UIButton button = UIButton.current;
		if ( button == null )
			return;

		if ( bDownloadingAsset )
			return;

		Game_Content gameContent = button.GetComponent<Game_Content> ();
		if ( gameContent )
		{
			currGameContent = gameContent;

			string url = sURL_Server;
			if ( Application.platform == RuntimePlatform.IPhonePlayer )
				url += "iPhone";
			else if ( Application.platform == RuntimePlatform.Android )
				url += "Android";
			else if ( Application.isEditor )
				url += "Android";

			url += "/"+gameContent.sGameID.Replace(" ","%20");
			StartCoroutine(downloadAssetBundle(url,gameContent.sGameID,1));
		}
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

	static WWW www;
	static bool bDownloadingAsset = false;
	public static IEnumerator downloadAssetBundle (string sURL, string name, int version)
	{
		bDownloadingAsset = true;
		//string keyName = name + version.ToString();
		string keyName = name;
		if (dictAssetBundleRefs.ContainsKey(keyName))
			yield return null;
		else 
		{


			string sCRC = "";
			if ( PlayerPrefs.HasKey(name+".crc") )
			{
				sCRC = PlayerPrefs.GetString(name+".crc");
			}
			
			// download crc
			www = new WWW(sURL+".crc");
			Debug.LogError("Downloading CRC Info "+(sURL+".crc"));
			while ( www.isDone == false )
			{
				yield return null;
			}
			string sTargetCRC = "";
			if ( www.error != null )
			{
				sTargetCRC = www.text;
			}
			www.Dispose();
			www = null;

			Debug.LogError("downloadAssetBundle "+sURL+" sCRC="+sCRC);
			//GUI_Debug.Log("[ResourceManager] downloadAssetBundle: "+url);
			if ( sCRC != "" )
				www = WWW.LoadFromCacheOrDownload (sURL, version, uint.Parse(sCRC) );
			else
				www = WWW.LoadFromCacheOrDownload (sURL, version );
			while ( www != null && www.isDone == false )
			{
				yield return www;
				Debug.Log("[ResourceManager] downloadAssetBundle: Done downloading "+name+".unity3d");

				AssetBundleRef abRef = new AssetBundleRef (sURL, version);
				abRef.assetBundle = www.assetBundle;
				dictAssetBundleRefs.Add (keyName, abRef);
				
				PlayerPrefs.SetString(sURL+".crc.",sTargetCRC);

			}
			Application.LoadLevel("Loading");

		}
		
		Debug.LogError("downloadAssetBundle "+name+ " Done");
		bDownloadingAsset = false;
	}
}
