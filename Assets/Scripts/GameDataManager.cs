using UnityEngine;
using UnityEngine.Advertisements;
using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;

public enum PlayAdsState
{
	FAILED,
	FINISHED,
	SKIPPED,
	NOTENOUGHTIME,
	NOTREADY
}

public class GameDataManager : MonoBehaviour {
	//public static string GEMU_APP_ID = "1431032373";
	public static int TIMER_TO_WATCH_ADS = 17280; // in seconds
	//public static int TIMER_TO_WATCH_ADS = 20; // in seconds
	public static GameDataManager instance = null;

	public int gameID;

	public int coins;
	public int tickets;

	public string deviceIP;

	void Awake()
	{
		//PlayerPrefs.DeleteAll ();
		DontDestroyOnLoad(this.gameObject);
		if (instance == null)
		{
			instance = this;

//			Advertisement.Initialize ("34672");

			GetDeviceIP();
		}
		else
		{
			DestroyImmediate(this.gameObject);
			return;
		}

		//SendPlayResult (GEMU_APP_ID, "0", "5", "0", "0");
		this.GetComponent<Gamestate_Gameplay>().enabled=true;
	}

	public static int GetTimerToWatchAds()
	{
		int fTimer = GameManager.getEpochTime();
		if ( PlayerPrefs.HasKey("timerads" ) )
		{
			fTimer = PlayerPrefs.GetInt("timerads");
		}
		else
			PlayerPrefs.SetInt("timerads",fTimer);
		
		return GameManager.getEpochTime() - fTimer;
	}

	public static PlayAdsState unityAdsResult = PlayAdsState.NOTREADY;
	public static bool PlayUnityVideoAd()
	{
		Debug.LogError("[GameDataManager] PlayUnityVideoAd");
		unityAdsResult = PlayAdsState.NOTREADY;
		// check timer
		//int iDiff = GetTimerToWatchAds ();
		//if ( iDiff > TIMER_TO_WATCH_ADS ) // TODO : Set the value using static var
		//{
			// user can now watch the ad
			string adName = "";
			string adString = PlayerPrefs.GetString ("UnityAds" + adName);
			if (Advertisement.IsReady (adString)) 
			{

				
				PlayerPrefs.SetInt("timerads",GameManager.getEpochTime());
				ShowOptions options = new ShowOptions();
				//options.resultCallback = HandleShowResult;
				Advertisement.Show(adString, options);
				
				Debug.LogError("show");
				return true;
				
			}
			else
			{
			
			Debug.LogError("not ready");
			}
		//}
		//else
		//{
		//	Debug.LogError("not enough time");
			// user must wait 
		//	unityAdsResult = PlayAdsState.NOTENOUGHTIME;
		//	return false;
		//}
		return false;
	}

	/*
	public static void HandleShowResult (ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
			unityAdsResult = PlayAdsState.FINISHED;
			break;
		case ShowResult.Skipped:
			unityAdsResult = PlayAdsState.SKIPPED;
			break;
		case ShowResult.Failed:
			unityAdsResult = PlayAdsState.FAILED;
			break;
		}
	}
*/
	void OnDestroy()
	{
		if (instance == this)
			instance = null;
		GemuAPI.OnGetUserResponse -= OnGetUserResponse;
		GemuAPI.OnPlayResultResponse -= OnPlayResultResponse;
	}

	public void SaveData()
	{
		//Debug.Log("Saving game data...");
		string lastLoginTime = System.DateTime.Now.ToString();
		PlayerPrefs.SetString (PlayerPrefHandler.keyLastLogin, lastLoginTime);
		//Debug.Log("Last Login Time: " + lastLoginTime);
		//PlayerPrefs.SetFloat (PlayerPrefHandler.keyWaktuCountLogin, CoinTimerHandler.instance.waktuCount);
		//Debug.Log ("Waktu Count Saat Keluar : " + CoinTimerHandler.instance.waktuCount);
		if ( CoinTimerHandler.instance )
			PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin, CoinTimerHandler.instance.countCoin);
		//Debug.Log ("Total Coin Saat Keluar : " + CoinTimerHandler.instance.countCoin);
		//PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin, CoinTimerHandler.instance.keyUserTiket);
		//Debug.Log ("Total Tickets Saat Keluar : " + CoinTimerHandler.instance.keyUserTiket);
		
		PlayerPrefs.Save();
	}

	void OnApplicationFocus(bool focusStatus) 
	{
		if (!focusStatus)
		{
			SaveData();
		}
	}
	
	void OnApplicationQuit()
	{
		SaveData();
	}

	// Use this for initialization
	void Start () {

		if (instance == this)
		{
			/*
			double waktuCount = 0.0;
			//Calculate bonus coin since last opening the game
			string lastLogin = PlayerPrefs.GetString (PlayerPrefHandler.keyLastLogin, "");
			if (!string.IsNullOrEmpty( lastLogin )) 
			{
				DateTime curTime = DateTime.Now;
				TimeSpan ts = curTime - Convert.ToDateTime (PlayerPrefs.GetString (PlayerPrefHandler.keyLastLogin)); 
				Debug.Log ("LAST LOGIN : " + ts.ToString());
				double totalSecondsLastLogin = ts.TotalSeconds;
				
				//PlayerPrefs.SetFloat (PlayerPrefHandler.keyWaktuCountLogin, waktuCount);
				waktuCount = (double) PlayerPrefs.GetFloat(PlayerPrefHandler.keyWaktuCountLogin);
				waktuCount -= totalSecondsLastLogin;
				
				int totalDapetCoins = 0;

				if (waktuCount < 0)
				{
					waktuCount *= -1;
					int waktuCountInt = (int) waktuCount;
					totalDapetCoins = waktuCountInt / ((int)CoinTimerHandler.instance.waktuResetAwal);
					waktuCount = waktuCount - (((float) totalDapetCoins) * CoinTimerHandler.instance.waktuResetAwal);
					waktuCount = CoinTimerHandler.instance.waktuResetAwal - waktuCount;

					
					//waktuCount = waktuCount;
					
					Debug.Log("Total Seconds Last Login : " + totalSecondsLastLogin.ToString());
					Debug.Log("Berarti dapet Coins nya ... " + totalDapetCoins.ToString());
					Debug.Log("Waktu count waktu login: " + waktuCount.ToString());
				}
				else
				{
					totalDapetCoins = 0;
				}
				
				CoinTimerHandler.instance.bonusCoinAtLogin = totalDapetCoins;
				CoinTimerHandler.instance.waktuCount = (float) waktuCount;
			}
			else
			{
				CoinTimerHandler.instance.bonusCoinAtLogin = 0;
			}
			*/
		}
		
		GemuAPI.OnGetUserResponse += OnGetUserResponse;
		GemuAPI.OnPlayResultResponse += OnPlayResultResponse;
		//LoadData ();
		//Tickets
		//tickets = PlayerPrefs.GetInt(PlayerPrefHandler.keyUserTiket, 0);
	}
	
	void OnPlayResultResponse(Restifizer.RestifizerResponse response)
	{
		if ( response != null)
		{
		Hashtable data = response.Resource;
		
		if ( data["errcode"].ToString() == "0")
		{
			//Debug.LogError("[GameDataManager] OnPlayResultResponse success");
		}
		else
		{
			Debug.LogError("[GameDataManager] OnPlayResultResponse fail");
		}
		}
	}

	void OnGetUserResponse(Restifizer.RestifizerResponse response)
	{
		if (PlayerPrefs.GetString (PlayerPrefHandler.keyUserName) == "")
			return;

		if (response != null && response.Resource != null)
			return;

		Hashtable data = response.Resource;
		
		if ( data["errcode"].ToString() == "0")
		{
			Debug.Log("[GameDataManager] OnGetUserResponse Success");
			Hashtable userdata = (Hashtable)data["userdata"];

			int iCoin = (int)float.Parse(userdata["coin"].ToString());
			PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,iCoin);

			int iTiket = (int) float.Parse(userdata["tiket"].ToString());
			PlayerPrefs.SetInt(PlayerPrefHandler.keyUserTiket,iTiket);

			coins = iCoin;
			tickets = iTiket;
			if ( CoinTimerHandler.instance )
				CoinTimerHandler.instance.countCoin = coins;

		}
		else
		{
			PlayerPrefs.SetString(PlayerPrefHandler.keyUserName,"");
			PlayerPrefs.SetString(PlayerPrefHandler.keyToken,"");
		}
	}

	
	private void FindInSelected(GameObject g,string parent)
	{
		parent += g.name+"/";
		Component[] components = g.GetComponents<Component>();
		Debug.Log("[ToolFindMissingScript] FindIn " + g.name+" amount="+components.Length);
		for (int i = 0; i < components.Length; i++)
		{
			if (components[i] == null)
			{
				Debug.Log("[ToolFindMissingScript] Missing script found at '"+parent+"' on "+i.ToString());
			}
			else
			{
				Debug.Log("[ToolFindMissingScript]    script at "+i.ToString()+" is '"+components[i].ToString()+"'");

			}
		}
		
		for ( int i=0 ; i<g.transform.childCount ; i++ )
		{
			FindInSelected(g.transform.GetChild(i).gameObject,parent);
		}
	}

	// Update is called once per frame
	void Update () {
		/*
		if ( Input.GetKeyUp(KeyCode.Space) )
		{
			string[] check = new string[] {"Gamestate","UI Root (3D)","AchievementManager","Debug"};
			for ( int i=0; i<check.Length; i++ )
			{
				GameObject ob = GameObject.Find(check[i]);
				FindInSelected(ob,"");
			}
		}
		*/
	}

	public void SendPlayResult(string sGameID, string sTiket, string sCoin, string sScore, string sGameLevel)
	{
		Hashtable data = new Hashtable();
		string sUsername = PlayerPrefs.GetString (PlayerPrefHandler.keyUserName);
		string sToken = PlayerPrefs.GetString (PlayerPrefHandler.keyToken);
		//int iCoin = PlayerPrefs.GetInt (PlayerPrefHandler.keyCoin);
		//int iTicket = PlayerPrefs.GetInt (PlayerPrefHandler.keyUserTiket);
		
		Debug.Log ("[GameDataManager] SendPlayResult username=" + sUsername + " token=" + sToken + " coin="+sCoin+" tiket="+sTiket);
		if (string.IsNullOrEmpty (sUsername) || string.IsNullOrEmpty (sToken))
			return;
		
		data.Add("username", sUsername);
		data.Add("token", sToken);
		data.Add("tiket", sTiket);
		data.Add("score", sScore);
		data.Add("coin", sCoin);
		data.Add("gameid", sGameID);
		data.Add("gamelevel", sGameLevel);
		
		try
		{
			GemuAPI.PlayResult(data);
		}
		catch(GemuAPI_Exception exc)
		{
			Debug.LogError(exc.Message);
		}
	}

	void GetDeviceIP()
	{
		if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
		{
			IPHostEntry host;
			string localIP = "";
			host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					localIP = ip.ToString();
					break;
				}
			}
			deviceIP = localIP;
		}
	}

	/*public void LoadData()
	{
		Hashtable data = new Hashtable();
		string sUsername = PlayerPrefs.GetString (PlayerPrefHandler.keyUserName);
		string sToken = PlayerPrefs.GetString (PlayerPrefHandler.keyToken);

		Debug.Log ("[GameDataManager] LoadData username=" + sUsername + " token=" + sToken);
		if (string.IsNullOrEmpty (sUsername) || string.IsNullOrEmpty (sToken))
			return;

		data.Add("username", sUsername);
		data.Add("token", sToken);
		
		try
		{
			GemuAPI.GetUser(data);
		}
		catch(GemuAPI_Exception exc)
		{
			Debug.LogError(exc.Message);
		}
	}*/
}
