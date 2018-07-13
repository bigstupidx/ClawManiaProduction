using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;



public class GL1Connector : MonoBehaviour 
{
	public static string GAME_ID = "CLAW";
	public static string GAME_VER = "1.7.1";

	//public UILabel Testlbl;

	static GL1Connector instance;
	System.Text.Encoding encoding = new System.Text.UTF8Encoding();
	static string URL = "https://www.gemugemu.com/api/";
	//static private string sToken;
	//static private string sCurrUser;
	//string param = "{\"full_name\":\"Aji Pamungkas\", \"email\":\"aji.rifa.pamungkas@gmail.com\", \"nat_id\":\"787665567\", \"address_1\":\"cisitu\", \"address_2\":\"bandung\", \"zip\":\"40135\", \"phone\":\"\", \"mobile\":\"085286443061\", \"username\":\"ajipamungkas\", \"password\":\"testgl1\", \"fb_id\":\"\", \"ggl_id\":\"\", \"ip\":\"127.0.0.1\"}";
	WWW www;
	GameObject requestByGameObject;
	// Use this for initialization

	void Start () 
	{
		//StartCoroutine (Sending ());
		//if ( PlayerPrefs.HasKey(PlayerPrefHandler.keyUserName) == false )
		//	PlayerPrefs.SetString(PlayerPrefHandler.keyUserName, "" );
		//if ( PlayerPrefs.HasKey(PlayerPrefHandler.keyToken) == false )
		//	PlayerPrefs.SetString(PlayerPrefHandler.keyToken, "" );

		/*
		if ( PlayerPrefs.HasKey("gemu_username") )
			sCurrUser = PlayerPrefs.GetString("gemu_username" );
		if ( PlayerPrefs.HasKey("gemu_token") )
			sToken = PlayerPrefs.GetString("gemu_token" );
		*/

	}

	void Awake()
	{
		instance = this;
	}

	public static GL1Connector GetInstance()
	{
		return instance;
	}
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void Register
				(
			GameObject obsender,
			string sFullName,
			string sEmail,
			string sNationalID,
			string sAddress1,
			string sAddress2,
			string sZipCode,
			string sPhoneNumber,
			string sMobileNumber,
			string sPassword,
			string sFBID,
			string sGoogleID,
			string sIP,
			string sTiket,
			string sReferalCode,
			string sDeviceID
				)
	{
		requestByGameObject = obsender;
		string sParam = "{";
		sParam += "\"full_name\":\"" + sFullName + "\",";
		sParam += "\"email\":\"" + sEmail + "\",";
		sParam += "\"nat_id\":\"" + sNationalID + "\",";
		sParam += "\"address_1\":\"" + sAddress1 + "\",";
		sParam += "\"address_2\":\"" + sAddress2 + "\",";
		sParam += "\"zip\":\"" + sZipCode + "\",";
		sParam += "\"phone\":\"" + sPhoneNumber + "\",";
		sParam += "\"mobile\":\"" + sMobileNumber + "\",";
		sParam += "\"password\":\"" + sPassword + "\",";
		sParam += "\"fb_id\":\"" + sFBID + "\",";
		sParam += "\"ggl_id\":\"" + sGoogleID + "\",";
		sParam += "\"ip\":\"" + sIP + "\",";
		sParam += "\"ref_code\":\"" + sReferalCode + "\",";
		sParam += "\"deviceid\":\"" + sDeviceID + "\"";
		sParam += "}";
		StartCoroutine (Sending (URL+"registeremail.php", sParam));
	}

	public void Login
		(
			GameObject obsender,
			string sEmail,
			string sPassword,
			string sIP
			)
	{
		requestByGameObject = obsender;
		Debug.LogError ("[GL1Connector] Login email=" + sEmail + " pass=" + sPassword);
		string sParam = "{";
		sParam += "\"email\":\"" + sEmail + "\",";
		sParam += "\"password\":\"" + sPassword + "\",";
		sParam += "\"gameid\":\"" + GAME_ID + "\",";
		sParam += "\"ip\":\"" + sIP + "\",";
		sParam += "\"gamever\":\"" + GAME_VER + "\"";
		sParam += "}";

		Debug.Log (sParam);
		StartCoroutine (Sending (URL+"loginemail.php", sParam));
	}

	public void PlayGame(
		GameObject obsender)
	{
		requestByGameObject = obsender;
		//Debug.LogError ("[GL1Connector] PlayGame username=" + GetCurrUser() + " token=" + GetToken() );
		string sParam = "{";
		sParam += "\"username\":\"" + GetCurrUser() + "\",";
		sParam += "\"token\":\"" + GetToken() + "\"";
		sParam += "}";
		StartCoroutine (Sending ("http://api.gemugemu.com/play/", sParam));
	}

	public void PlayResult(GameObject obsender,string sGameLv,string sTiket,string sScore)
	{
		if ( string.IsNullOrEmpty(GetToken()) )
			return;
		requestByGameObject = obsender;
		//Debug.LogError ("[GL1Connector] PlayResult username=" + GetCurrUser() + " token=" + GetToken() );
		string sParam = "{";
		sParam += "\"email\":\"" + GetCurrUser() + "\",";
		sParam += "\"token\":\"" + GetToken() + "\",";
		sParam += "\"gameid\":\"" + GAME_ID + "\",";
		sParam += "\"gamelevel\":\"" + sGameLv + "\",";
		sParam += "\"gp\":\"" + sTiket + "\",";
		sParam += "\"score\":\"" + sScore + "\"";
		sParam += "}";
		StartCoroutine (Sending (URL+"playresultemail.php", sParam));
	}

	public void AddBalance(GameObject obsender,string sGP,string sCoin,string sIP,string sref)
	{
		string sRef=sref;
		string sSku="";

		if ( string.IsNullOrEmpty(GetToken()) )
			return;
		requestByGameObject = obsender;
		//Debug.LogError ("[GL1Connector] PlayResult username=" + GetCurrUser() + " token=" + GetToken() );
		string sParam = "{";
		sParam += "\"email\":\"" + GetCurrUser() + "\",";
		sParam += "\"token\":\"" + GetToken() + "\",";
		sParam += "\"gameid\":\"" + GAME_ID + "\",";
		/*sParam += "\"gp\":\"" + sGP + "\",";*/
		sParam += "\"gg\":\"" + sCoin + "\",";
		sParam += "\"ip\":\"" + sIP + "\",";
		sParam += "\"ref\":\"" + sRef + "\",";
		sParam += "\"sku\":\"" + sSku + "\"";
		sParam += "}";
		StartCoroutine (Sending (URL+"addbalanceemail.php", sParam));
	}

	public void DecBalance(GameObject obsender,string sCoin,string sIP,string sref)
	{
		string sRef=sref;
		string sSku="";

		if ( string.IsNullOrEmpty(GetToken()) )
			return;
		requestByGameObject = obsender;
		//Debug.LogError ("[GL1Connector] PlayResult username=" + GetCurrUser() + " token=" + GetToken() );
		string sParam = "{";
		sParam += "\"email\":\"" + GetCurrUser() + "\",";
		sParam += "\"token\":\"" + GetToken() + "\",";
		sParam += "\"gameid\":\"" + GAME_ID + "\",";
		/*sParam += "\"gp\":\"" + "0" + "\",";*/
		sParam += "\"gg\":\"" + sCoin + "\",";
		sParam += "\"ip\":\"" + sIP + "\",";
		sParam += "\"ref\":\"" + sRef + "\",";
		sParam += "\"sku\":\"" + sSku + "\"";
		sParam += "}";
		StartCoroutine (Sending (URL+"decbalanceemail.php", sParam));
	}

	public void AddGameBalance(GameObject obsender,string sGP,string sIP,string sref){
		string sRef=sref;
		string sSku="";

		if ( string.IsNullOrEmpty(GetToken()) )
			return;
		requestByGameObject = obsender;
		//Debug.LogError ("[GL1Connector] PlayResult username=" + GetCurrUser() + " token=" + GetToken() );
		string sParam = "{";
		sParam += "\"email\":\"" + GetCurrUser() + "\",";
		sParam += "\"token\":\"" + GetToken() + "\",";
		sParam += "\"gameid\":\"" + "GEMU" + "\",";
		sParam += "\"currency\":\"" + "GP" + "\",";
		sParam += "\"value\":\"" + sGP + "\",";
		sParam += "\"ip\":\"" + sIP + "\",";
		sParam += "\"ref\":\"" + sRef + "\",";
		sParam += "\"sku\":\"" + sSku + "\"";
		sParam += "}";
		StartCoroutine (Sending (URL+"addgamebalanceemail.php", sParam));
	}

	public void DecGameBalance(GameObject obsender,string sGP,string sIP,string sref){
		string sRef=sref;
		string sSku="";

		if ( string.IsNullOrEmpty(GetToken()) )
			return;
		requestByGameObject = obsender;
		//Debug.LogError ("[GL1Connector] PlayResult username=" + GetCurrUser() + " token=" + GetToken() );
		string sParam = "{";
		sParam += "\"email\":\"" + GetCurrUser() + "\",";
		sParam += "\"token\":\"" + GetToken() + "\",";
		sParam += "\"gameid\":\"" + "GEMU" + "\",";
		sParam += "\"currency\":\"" + "gp" + "\",";
		sParam += "\"value\":\"" + sGP + "\",";
		sParam += "\"sku\":\"" + "2" + "\",";
		sParam += "\"ip\":\"" + sIP + "\",";
		sParam += "\"ref\":\"" + sRef + "\",";
		sParam += "\"sku\":\"" + sSku + "\"";
		sParam += "}";
		StartCoroutine (Sending (URL+"decgamebalanceemail.php", sParam));
	}

	public void GetReward(GameObject obsender)
	{
		Debug.LogError ("[GL1Connector] GetReward ");
		//if ( this.gameObject.activeInHierarchy )
		requestByGameObject = obsender;
		StartCoroutine (Sending ("http://api.gemugemu.com/reward/", ""));
	}

	public void Redeem(GameObject obsender,string sRewardID, GameObject sender)
	{
		if ( string.IsNullOrEmpty(GetToken()) )
			return;
		requestByGameObject = obsender;
		Debug.LogError ("[GL1Connector] Redeem ");
		string sParam = "{";
		sParam += "\"username\":\"" + GetCurrUser() + "\",";
		sParam += "\"token\":\"" + GetToken() + "\",";
		sParam += "\"rewardid\":\"" + sRewardID + "\"";
		sParam += "}";
		StartCoroutine (Sending ("http://api.gemugemu.com/redeem/", sParam));
	}

	public void Promo(GameObject obsender,string sKodeKupon)
	{
		if ( string.IsNullOrEmpty(GetToken()) )
			return;
		requestByGameObject = obsender;
		Debug.LogError ("[GL1Connector] Promo ");
		string sParam = "{";
		sParam += "\"username\":\"" + GetCurrUser() + "\",";
		sParam += "\"token\":\"" + GetToken() + "\",";
		sParam += "\"kdkupon\":\"" + sKodeKupon + "\"";
		sParam += "}";
		StartCoroutine (Sending ("http://api.gemugemu.com/promo/", sParam));
	}
	
	public void RequestUserData
		(
			GameObject obsender,
			string sUserName,
			string sToken,
			string sDeviceID,
			string sFBID
			)
	{
		//Debug.LogError ("[GL1Connector] RequestUserData username=" + GetCurrUser () + " token=" + GetToken ());
		if ( string.IsNullOrEmpty(GetToken()) )
		{
			//Debug.LogError("empty");
			return;
		}
		requestByGameObject = obsender;
		//Debug.LogError ("[GL1Connector] RequestUserData ");
		string sParam = "{";
		sParam += "\"email\":\"" + sUserName + "\",";
		sParam += "\"token\":\"" + sToken + "\",";
		sParam += "\"gameid\":\"" + GAME_ID + "\",";
		sParam += "\"deviceid\":\"" + sDeviceID + "\",";
		sParam += "\"fb_id\":\"" + sFBID + "\"";
		sParam += "}";
		StartCoroutine (Sending (URL+"getuseremail.php", sParam));
	}

	public bool IsLoggedIn()
	{
		if ( string.IsNullOrEmpty(GetToken()) )
			return false;
		else
			return true;
	}

	public string GetToken()
	{
		if( PlayerPrefs.HasKey(PlayerPrefHandler.keyToken) )
		{
			string sToken = PlayerPrefs.GetString(PlayerPrefHandler.keyToken);
			return sToken;
		}
		else
			return "";
	}
	
	public string GetCurrUser()
	{
		if( PlayerPrefs.HasKey(PlayerPrefHandler.keyUserName) )
		{
			string sCurrUser = PlayerPrefs.GetString(PlayerPrefHandler.keyUserName);
			return sCurrUser;
		}
		else
			return "";
	}

	
	#region FACEBOOK && GEMU API
	// public void LoginViaFB(string email){
		
	// 	//change this values according to user input
	// 	string fbId = FB.UserId;
	// 	string userEmail = email;
		
	// 	//string tempGameid = gameID;
	// 	string tempIp = "";
		
	// 	string postURL = "https://www.gemugemu.com/api/loginfb.php";
		
	// 	string jsonString = "{\"fb_id\":\"" + fbId + "\"," +
	// 		"\"fb_email\":\"" + userEmail + "\"," +
	// 			"\"gameid\":\""   + GAME_ID   + "\"," +
	// 			"\"ip\":\""       + tempIp       + "\"," +
	// 			"\"gamever\":\""       + GAME_VER       + "\"" +
	// 			"}";

	// 	StartCoroutine (Sending (postURL, jsonString));
	// }
	#endregion


	string sLastURL;
	public string GetLastURL()
	{
		return sLastURL;
	}

	IEnumerator Sending(string sUrl,string sParam)
	{
		sLastURL = sUrl;
		
		//Debug.LogError ("[GL1Connector] Done Sending ");
		if ( string.IsNullOrEmpty(sParam) == false )
		{
			Dictionary<string,string> postHeader = new Dictionary<string,string> ();
			postHeader.Add("Content-Type", "text/json");
			postHeader.Add("Content-Length", sParam.Length.ToString());
			//www = new WWW("http://api.gemugemu.com/register/",encoding.GetBytes(param),postHeader);
			Debug.LogError ("[GL1Connector] Sending : " + sUrl + " \n"+ sParam);

			www = new WWW (sUrl, encoding.GetBytes (sParam), postHeader);
		}
		else
		{
			www = new WWW (sUrl);
		}

		if ( www == null )
			yield break;

		while ( www!= null && www.isDone == false )
		{
			yield return null;
		}

		
		if ( www == null )
			yield break;
		bool bError = false;
		//Debug.LogError ("[GL1Connector] Done Sending ");
		if (string.IsNullOrEmpty (www.error)) 
		{
			//Testlbl.text = www.text;
			Debug.LogError (www.text);
			var N = JSONNode.Parse (www.text);
			if (sUrl.Contains ("login") ) 
			{
				if ( N ["errcode"].ToString () == "\"0\"" )
				{
					var logindata = JSONNode.Parse (N ["logindata"].ToString ());
					if (logindata != null) {
						string sToken = logindata ["token"];
						string sCurrUser = logindata ["email"];
						print (sToken);
						print (sCurrUser);
						PlayerPrefs.SetString (PlayerPrefHandler.keyUserName, sCurrUser);
						PlayerPrefs.SetString (PlayerPrefHandler.keyToken, sToken);


						#region FACEBOOK && GEMU API
						// if(FB.IsLoggedIn){
						// 	GameObject.Find ("GUI_Share").GetComponent<GUI_Share>().OnFBLoggedIn();
						// 	GameObject.Find ("GUI_Share").GetComponent<GUI_Share>().OnGemuGemuLoggedIn();
						// }
						#endregion
					}
				}
				else
				{
					bError = true;
				}
			} 
			else if (sUrl != null && sUrl.Contains ("getuser") && N != null && N["errcode"] != null && N ["errcode"].ToString () == "\"0\"") 
			{
				var userdata = JSONNode.Parse (N ["userdata"].ToString ());
				if (userdata != null) {
					string sCoin = userdata ["coin"];
					string sTiket = userdata ["tiket"];
					string sLastLevel = userdata ["lastlevel"];
					string sLastScore = userdata ["lastscore"];
					if ( string.IsNullOrEmpty(sLastScore) == false )
						GameManager.EXP = (int)(float.Parse (sLastScore));
					GameManager.TICKET = (int)(float.Parse (sTiket));
					GameManager.GEMUCOINS = (int)(float.Parse (sCoin));
				}
			}

			if ( requestByGameObject )
			{
				//Debug.LogError("sending to "+requestByGameObject.name+" for '"+sLastURL+"'");
				requestByGameObject.gameObject.SendMessage ("OnGL1Done", N);
			}
			else
				this.gameObject.SendMessage ("OnGL1Done", N);
		} else 
			bError = true;

		if ( bError )
		{
			Debug.LogError("error "+www.error.ToString());
			Gamestate gs = this.gameObject.GetComponent<Gamestate>();
			if ( gs )
			{
				//gs.ShowDialogBox("Info","Error Sending data to server",false,"",this.gameObject);
			}
		}
		www.Dispose ();
		www = null;
		yield break;
	}
}
