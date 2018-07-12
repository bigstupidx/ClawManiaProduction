using UnityEngine;
using System.Collections;
using SimpleJSON;

public class GUI_GL1_UserData : GUI_Dialog {
	public UIScrollView contentContainer;
	public GUI_Share guiShare;
	// Use this for initialization
	
	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}

	public override void OnStart ()  
	{
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickSignout()
	{
		/*Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay>();
		if ( gs )
		{
			print ("to logout");
			gs.LogOut();
		}*/

		Debug.LogError ("Logout");
		PlayerPrefs.SetString (PlayerPrefHandler.keyToken, "");
		PlayerPrefs.SetString (PlayerPrefHandler.keyUserName, "");
		PlayerPrefs.SetInt (PlayerPrefHandler.keyCoin, 0);
		PlayerPrefs.SetInt (PlayerPrefHandler.keyUserTiket, 0);
		//PlayerPrefs.SetInt (GameManager.keyClawMania_Energy, 200);
		PlayerPrefs.SetInt (GameManager.keyClawMania_Level, 1);
		PlayerPrefs.SetInt ("exp", 0);
		
		GameManager.EXP = 0;
		//RefreshAllInfo ();

		guiShare.OnGemuGemuLogout ();
		if(FB.IsLoggedIn)
			guiShare.OnFBLogout ();
		GUI_Dialog.ReleaseTopCanvas();
	}

	public override void OnShow()
	{
		Debug.LogError ("[GUI_GL1_UserData] OnShow");
		for ( int i=0; i<contentContainer.transform.childCount; i++ )
		{
			Transform trchild = contentContainer.transform.GetChild(i);
			if ( trchild && trchild.GetComponent<UIInput>() )
			{
				UIInput input = trchild.GetComponent<UIInput>();
				input.value = "";

			}
		}
		string sFBID = "";
		if (SPFacebook.instance.IsInited && SPFacebook.instance.IsLoggedIn)
			sFBID = SPFacebook.instance.UserId;
		GL1Connector.GetInstance().RequestUserData (this.gameObject,GL1Connector.GetInstance().GetCurrUser(), GL1Connector.GetInstance().GetToken(), SystemInfo.deviceUniqueIdentifier,sFBID);
	}

	public void OnGL1Done(JSONNode N)
	{
		if ( N["errcode"].ToString() == "\"0\"" )
		{
			var userdata = JSONNode.Parse(N["userdata"].ToString());
			if ( userdata != null )
			{
				int Tiket = (int)float.Parse(userdata["tiket"]);
				int GemuCoins = (int)float.Parse(userdata["coin"]);
				if ( string.IsNullOrEmpty(userdata["lastscore"].ToString()) == false )
				{
					int exp = (int)float.Parse(userdata["lastscore"]);
					GameManager.EXP = exp;
				}
				GameManager.GEMUCOINS = GemuCoins;
				GameManager.TICKET = Tiket;
				//Debug.LogError("tiket="+Tiket+" coin="+GemuCoins+" GameManager.GEMUCOINS="+GameManager.GEMUCOINS+" username="+PlayerPrefs.GetString(PlayerPrefHandler.keyUserName));
				PlayerPrefs.SetInt(PlayerPrefHandler.keyUserTiket,Tiket);
				PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,GemuCoins);
				
				Transform trUsername = contentContainer.transform.Find("Field_username");
				if ( trUsername )
				{
					UIInput input = trUsername.GetComponent<UIInput>();
					if ( input )
					{
						//string temp = PlayerPrefs.GetString(PlayerPrefHandler.keyUserName);
						//input.value = temp.Substring(0,temp.IndexOf('@'));
						input.value=userdata["full_name"];
					}
				}

				foreach ( string key in ((JSONClass)userdata).GetKeys() )
				{
					//Debug.LogError(key);
					Transform trField = contentContainer.transform.Find("Field_"+key);
					if ( trField )
					{
						UIInput input = trField.GetComponent<UIInput>();
						if ( input )
						{
							input.value = userdata[key];
						}
					}
				}
				Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay>();
				if ( gs )
				{
					gs.RefreshAllInfo();
				}
			}
		}
		else
		{

		}
	}
}
