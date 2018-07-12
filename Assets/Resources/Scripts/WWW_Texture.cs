using UnityEngine;
using System.Collections;

public class WWW_Texture : MonoBehaviour {
	private WWW www;
	private string url;
	public GameObject sender;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	bool isDone = false;
	public bool IsDone()
	{
		return isDone;
	}

	IEnumerator DownloadingPic () 
	{
		while( www.isDone == false )
		{
			yield return www;
		}
		
		if ( string.IsNullOrEmpty(www.error) == true )
		{
			//Debug.LogError("[WWW_Texture] DownloadingPic Done");
			UITexture uiTexture = this.gameObject.GetComponent<UITexture>();
			if ( uiTexture )
			{
				uiTexture.mainTexture = www.texture;
				isDone = true;
				if ( sender )
					sender.gameObject.SendMessage("OnDownloadingPicDone");
			}
		}
		else
		{
			Debug.LogError("[WWW_Texture] DownloadingPic Error="+www.error);
		}
		
		UITexture uiTexture2 = this.gameObject.GetComponent<UITexture>();
		if (uiTexture2) 
		{
			//uiTexture2.alpha = 1;
		}
		//www.Dispose();
		yield break;
	}
	
	public void StartDownloadImage( string sURL )
	{
		if (this.gameObject.activeInHierarchy == false)
			return;
		//url = GameManager.alpsMasterServer+sLoc+"/"+sIDKing+"."+sFileExtension;
		//Debug.LogError ("[WWW_Texture] StartDownloadImage url=" + sURL);
		url = sURL;
		www = new WWW (url);
		
		UITexture uiTexture = this.gameObject.GetComponent<UITexture>();
		if (uiTexture) 
		{
			//uiTexture.alpha = 0;
		}
			StartCoroutine("DownloadingPic");
	}
}
