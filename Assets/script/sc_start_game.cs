using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class sc_start_game : MonoBehaviour {
	int GameID = 80;
	
	public GemuDialogBoxController dialogBox;
	int gameLevelActive=1;
	Ray ray3d;
	RaycastHit hit3d;

	//public Texture soundActive;
	//public Texture soundNonActive;
	//public Text txtTiketUser;
	private int coinsUserActive=0;

	//public RawImage rImgSound;
	//public Text txtResultLogin;

	public AudioClip soClickButton;
	AudioSource audioSourceSFX;
	public Transition transition;

	// Use this for initialization
	void Start () 
	{
		GUI_Dialog.ClearStack ();
		Screen.orientation = ScreenOrientation.Portrait;
		//GameDataManager.GEMU_APP_ID = "1430386500";
		PlayerPrefs.SetInt(PlayerPrefHandler.keyLevelAktif,gameLevelActive);
		PlayerPrefs.SetInt (PlayerPrefHandler.keyPause, 0);
		PlayerPrefs.SetInt (PlayerPrefHandler.keyGameId, GameID);

		PlayerPrefs.SetInt (PlayerPrefHandler.keyCurrentScore, 0);

		if (PlayerPrefs.HasKey ("highscore") == false) {
			PlayerPrefs.SetInt ("highscore", 0);
		}

		//Handle Tickets
		if (PlayerPrefs.HasKey (PlayerPrefHandler.keyUserTiket) == false) {
			PlayerPrefs.SetInt (PlayerPrefHandler.keyUserTiket, 0);
		}

		//Handle Suara
		audioSourceSFX = GameObject.Find ("Main Camera").GetComponent<AudioSource> ();
		//rImgSound = GameObject.Find ("btnSound").GetComponent<RawImage> ();
	
		if (PlayerPrefs.HasKey (PlayerPrefHandler.keySound) == false) {
			PlayerPrefs.SetInt (PlayerPrefHandler.keySound, 1);
		} else {
			if(PlayerPrefs.GetInt(PlayerPrefHandler.keySound)==1) {
				//audioSourceSFX.enabled = true;
				//rImgSound.texture = soundActive;
			} else {
				//audioSourceSFX.enabled = false;
				//rImgSound.texture = soundNonActive;
			}
		}

		hitung_tiket_user ();
		if (PlayerPrefs.HasKey (PlayerPrefHandler.keyLastLogin) == false) 
		{
			PlayerPrefs.SetString (PlayerPrefHandler.keyLastLogin, System.DateTime.Now.ToString());
		}
		//if (PlayerPrefs.HasKey (PlayerPrefHandler.keyLoginActive) == false) 
		//{
		//	PlayerPrefs.SetString (PlayerPrefHandler.keyLoginActive, "guest");
		//	PlayerPrefs.SetString (PlayerPrefHandler.keyUserName, "guest");
			//PlayerPrefs.SetInt(PlayerPrefHandler.keyCoinGuest,3);
			//GameObject.Find ("txtTimerUser").SendMessage ("updateCoins");
		//}

		//if (PlayerPrefs.GetString (PlayerPrefHandler.keyLoginActive) == "guest") {
		//	GameObject.Find ("txtTimerUser").SendMessage ("updateCoins");
		//}

		//cek status Login
		//Debug.Log("[ Login Name : " + PlayerPrefs.GetString (PlayerPrefHandler.keyUserName) + " | " + PlayerPrefs.GetString (PlayerPrefHandler.keyLoginActive) + " ]");
		/*
		if (PlayerPrefs.GetString(PlayerPrefHandler.keyLoginActive)=="guest") {
			Debug.Log("-- Proses ulang login Guest --");
			PlayerPrefs.SetString (PlayerPrefHandler.keyUserName, "guest");
			coinsUserActive = PlayerPrefs.GetInt(PlayerPrefHandler.keyCoin);
			
			GameObject.Find("FB_gameobj").SendMessage("loadGambarGuest");
			
			//Text txtCoins = GameObject.Find("txtCoinsUser").GetComponent<Text>();
			//txtCoins.text = "X "+coinsUserActive.ToString();

		} else if (PlayerPrefs.GetString(PlayerPrefHandler.keyLoginActive)=="facebook") {
			Debug.Log("-- Proses ulang login Facebook --");

			

			GameObject.Find("FB_gameobj").SendMessage("GetFotoAndName");
			
			//Text txtCoins = GameObject.Find("txtCoinsUser").GetComponent<Text>();
			//txtCoins.text = "X "+coinsUserActive.ToString();
		} else if (PlayerPrefs.GetString(PlayerPrefHandler.keyLoginActive)=="gemu") {
			Debug.Log("-- Proses ulang login Gemu --");
			get_asset_API(PlayerPrefs.GetString (PlayerPrefHandler.keyUserName));
			
		} else if (PlayerPrefs.GetString(PlayerPrefHandler.keyLoginActive)=="gplus") {
			Debug.Log("-- Proses ulang login GPlus --");
			
		}
		PlayerPrefs.Save();
		*/
		GemuAPI.OnGetUserResponse += OnGetUserResponse;
		GemuAPI.OnLoginResponse += OnLoginResponse;


		//GameDataManager.instance.SendPlayResult(GameDataManager.GEMU_APP_ID,"0","10","0","1");
		//GameDataManager.instance.LoadData ();

		//sementara aja
		//Text txtData = GameObject.Find ("txtData").GetComponent<Text> ();
		//txtData.text = "[ Name:" + PlayerPrefs.GetString (PlayerPrefHandler.keyUserName) + " | Login:" + PlayerPrefs.GetString (PlayerPrefHandler.keyLoginActive) + " | GameID:"+ PlayerPrefs.GetInt(PlayerPrefHandler.keyGameId) +" ]";
		//Debug.Log("--- MainMenu : " + PlayerPrefs.GetString(PlayerPrefHandler.keyUserName) + " | statLogin : " + PlayerPrefs.GetString (PlayerPrefHandler.keyLoginActive) + " ---");
	}

	void OnDestroy()
	{
		GemuAPI.OnGetUserResponse -= OnGetUserResponse;
		GemuAPI.OnLoginResponse -= OnLoginResponse;
	}

	void OnGetUserResponse(Restifizer.RestifizerResponse response)
	{
		Hashtable data = response.Resource;
		
		if ( data["errcode"].ToString() == "0")
		{
			Hashtable userdata = (Hashtable)data["userdata"];

			//txtTiketUser.text = userdata["tiket"].ToString();
		}
		else
		{
			PlayerPrefs.SetString(PlayerPrefHandler.keyUserName,"");
			PlayerPrefs.SetString(PlayerPrefHandler.keyToken,"");
		}
	}

	void OnLoginResponse(Restifizer.RestifizerResponse response)
	{
		Hashtable data = response.Resource;
		
		if ( data["errcode"].ToString() == "0")
		{
			Hashtable logindata = (Hashtable)data["logindata"];
			string sUsername = logindata["username"].ToString();
			string sToken = logindata["token"].ToString();
			
			PlayerPrefs.SetString(PlayerPrefHandler.keyUserName,sUsername);
			PlayerPrefs.SetString(PlayerPrefHandler.keyToken,sToken);
			GUI_Dialog.ReleaseTopCanvas();
			GUI_Dialog.InsertStack(sc_mainmenu_canvas_handler.Instance().cvGemuData.gameObject);
			//GameDataManager.instance.LoadData();
		}
		else
		{
			//txtResultLogin.text = data["errdetail"].ToString();
			//txtResultLogin.gameObject.SetActive(true);
			sc_mainmenu_canvas_handler.Instance().ShowDialogBox(data["errdetail"].ToString(),false);
		}
	}

	public void releaseTopCanvas()
	{
		bunyi_click ();
		GUI_Dialog.ReleaseTopCanvas ();
	}

	public void btnPromo_Show()
	{
		bunyi_click ();
		GUI_Dialog.InsertStack (sc_mainmenu_canvas_handler.Instance ().cvGemuPromo.gameObject);
	}

	public void btnPromo_Hide()
	{
		bunyi_click ();
		GUI_Dialog.ReleaseTopCanvas ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) { 
			if ( GUI_Dialog.GetActiveStackAmount() > 0 )
			{
				bunyi_click();
				GUI_Dialog.ReleaseTopCanvas();
			}
			else
				GUI_Dialog.InsertStack(sc_mainmenu_canvas_handler.Instance().cvQuit.gameObject);
		}
	}

	void bunyi_click() {
		//GetComponent<AudioSource>().PlayOneShot (soClickButton, 1f);
		SoundManager.instance.PlayButton();
	}

	#region FACEBOOK
	// public void btnInviteFriends()
	// {
	// 	bunyi_click ();
	// 	string title = "Hello";
	// 	string message = "Play with me";
		
	// 	SPFacebook.instance.SendInvite(title, message);
	// 	SPFacebook.instance.OnAppRequestCompleteAction += OnAppRequestCompleteAction;
	// }

	// void OnAppRequestCompleteAction (FBAppRequestResult result) {
		
	// 	if(result.IsSucceeded) {
	// 		foreach(string UserId in result.Recipients) {
	// 			Debug.Log(UserId);
	// 		}
			
	// 		Debug.Log("Original Facebook Responce: " + result.Result.Text);
	// 	} else {
	// 		Debug.Log("App request has failed");
	// 	}
		
		
	// 	SPFacebook.instance.OnAppRequestCompleteAction -= OnAppRequestCompleteAction;	
	// }
	#endregion

	public void btnYesQuit_clicked() {
		bunyi_click ();

		if (Application.loadedLevel == 0)
			Application.Quit();
		else
			Application.LoadLevel("main");
	}
	public void btnNoQuit_clicked() {
		bunyi_click ();
		GUI_Dialog.ReleaseTopCanvas ();
	}

	public void btnMore_clicked() {
		bunyi_click ();
		Debug.Log("Btn More clicked");
	}

	public void btnPlay_clicked() {
		bunyi_click ();

			int coinUserJml = PlayerPrefs.GetInt(PlayerPrefHandler.keyCoin);
			Debug.Log("Jumlah Coin User ::: " + coinUserJml);
			//PlayerPrefs.SetInt(PlayerPrefHandler.keyCoinGemu);
			if(coinUserJml>0) {
				PlayerPrefs.SetInt (PlayerPrefHandler.keyCurrentScore, 0);
				PlayerPrefs.SetInt(PlayerPrefHandler.keyCombo, 0);

				coinUserJml-=1;
				PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,coinUserJml);

				if ( CoinTimerHandler.instance.isCounting() == false )
					CoinTimerHandler.instance.Reset();
				CoinTimerHandler.instance.countCoin = coinUserJml;
				//GameObject.Find ("txtTimerUser").SendMessage ("updateCoins");
			GameDataManager.instance.SendPlayResult(GameDataManager.instance.gameID.ToString(),"0",coinUserJml.ToString(),"0","1");
				//GameObject.Find("cont_signup").SendMessage("update_asset_coins",PlayerPrefs.GetString(PlayerPrefHandler.keyUserName));

				PlayerPrefs.Save();

				StartCoroutine(WaitingToPlay());
			}
			else
			{
				dialogBox.Show("Info", "You don't have enough coin",false,"",this.gameObject);
			}
	}

	IEnumerator WaitingToPlay()
	{
		transition.Show (Transition.TransitionMode.EaseIn);
		while (transition.IsDone == false)
			yield return null;
		Application.LoadLevel ("3Dbasketball");
		yield break;
	}
	public void setMusicOnOff_click() {
		GameObject.Find ("cont_music_bgm").SendMessage ("atur_music_bgm");
	}


	public void btnHighscore_clicked() {
		bunyi_click ();
		Debug.Log("Btn Highscore clicked");
		//Application.LoadLevel ("scene_hscore");
		sc_mainmenu_canvas_handler.Instance().cvHighscore.GetComponent<sc_hscore>().get_leaderboard_API();
		sc_mainmenu_canvas_handler.Instance().cvHighscore.gameObject.SetActive( true );
	}

	public void btnSetting_clicked() {
		bunyi_click ();
		//sc_mainmenu_canvas_handler.Instance ().cvSetting.Show ();
		GUI_Dialog.InsertStack (sc_mainmenu_canvas_handler.Instance ().cvSetting.gameObject);
		
	}

	public void btnGPlus_clicked() {
		bunyi_click ();
		Debug.Log("Btn GPlus clicked");
	}
	public void btnFacebook_clicked() {
		bunyi_click ();
		Debug.Log("Btn Facebook Login clicked");
	}
	public void btnGemu_clicked() {
		bunyi_click ();
		Debug.Log("Btn Gemu clicked");
		string sCurrUsername = PlayerPrefs.GetString (PlayerPrefHandler.keyUserName);
		string sCurrToken = PlayerPrefs.GetString (PlayerPrefHandler.keyToken);
		Debug.LogError("currUser="+sCurrUsername);
		Debug.LogError("currToken="+sCurrToken);
		//txtResultLogin.text = "";
		if ( string.IsNullOrEmpty(sCurrUsername) || string.IsNullOrEmpty(sCurrToken))
		{
			GUI_Dialog.InsertStack(sc_mainmenu_canvas_handler.Instance().cvLogin.gameObject);
		}
		else
		{
			GUI_Dialog.InsertStack(sc_mainmenu_canvas_handler.Instance().cvGemuData.gameObject);
		}
	}
	public void btnGemu_login_close() {
		bunyi_click ();
		Debug.Log("Btn Gemu Closed clicked");
		sc_mainmenu_canvas_handler.Instance().cvLogin.gameObject.SetActive( false );
	}

	public void btnGemu_singup() {
		bunyi_click ();
		GUI_Dialog.InsertStack(sc_mainmenu_canvas_handler.Instance().cvSignup.gameObject);
	}

	public void btnGemu_singup_close() {
		bunyi_click ();
		GUI_Dialog.ReleaseTopCanvas ();
	}
	public void btnCredits_show() {
		bunyi_click ();
		GUI_Dialog.InsertStack (sc_mainmenu_canvas_handler.Instance ().cvCredits.gameObject);
	}
	public void btnCredits_close() {
		bunyi_click ();
		GUI_Dialog.ReleaseTopCanvas ();
	}
	public void btnHTP_show() {
		bunyi_click ();
		GUI_Dialog.InsertStack (sc_mainmenu_canvas_handler.Instance ().cvHtp.gameObject);
	}
	public void btnHTP_close() {
		bunyi_click ();
		GUI_Dialog.ReleaseTopCanvas ();
	}


	void reset_register() {
		InputField uname = GameObject.Find("signup_username").GetComponent<InputField>();
		InputField pass1 = GameObject.Find("signup_pass").GetComponent<InputField>();
		InputField pass2 = GameObject.Find("signup_pass2").GetComponent<InputField>();
		InputField email = GameObject.Find("signup_email").GetComponent<InputField>();
		Text result = GameObject.Find("txtProgressSignup").GetComponent<Text>();
		uname.text = "";
		pass1.text = "";
		pass2.text = "";
		email.text = "";
		result.text = "";
	}



	public void btnGemu_login_proses() {
		bunyi_click ();

		Debug.Log("Btn Gemu Login clicked Progress");
		InputField txtUsername = GameObject.Find ("InputField_username").GetComponent<InputField> ();
		InputField txtPassword = GameObject.Find ("InputField_password").GetComponent<InputField> ();
		Debug.Log ("====> Login GEMU Masuk : " + txtUsername.text + " => " + txtPassword.text);


		//Text txtResultLogin = GameObject.Find("txtResultLogin").GetComponent<Text>();
		//txtResultLogin.text = "Please wait...";
		//StartCoroutine (login_API_send_JSON(txtUsername.text.ToString(), txtPassword.text.ToString()));
		Hashtable data = new Hashtable();
		data.Add("username", txtUsername.text);
		data.Add("password", txtPassword.text);
		data.Add("gameid", GameDataManager.instance.gameID.ToString());
		data.Add("ip", GameDataManager.instance.deviceIP);
		
		try
		{
			GemuAPI.Login(data);
		}
		catch(GemuAPI_Exception exc)
		{
			Debug.LogError(exc.Message);
		}

	}


	public void btnClose_setting() {
		bunyi_click ();
		GUI_Dialog.ReleaseTopCanvas ();
	}

	public void btnSound_clicked() {
		bunyi_click ();
		if (PlayerPrefs.GetInt(PlayerPrefHandler.keySound)==1) {
			//rImgSound.texture = soundNonActive;
			//audioSourceSFX.enabled = false;
			SoundManager.instance.SoundOn = false;
			PlayerPrefs.SetInt (PlayerPrefHandler.keySound, 0);
		} else {
			//rImgSound.texture = soundActive;
			//audioSourceSFX.enabled = true;
			SoundManager.instance.SoundOn = true;
			PlayerPrefs.SetInt (PlayerPrefHandler.keySound, 1);
		}

		PlayerPrefs.Save();
		Debug.Log ("Sound " + PlayerPrefs.GetInt(PlayerPrefHandler.keySound));
	}

	public void btnPlus_clicked() {
		bunyi_click ();
		Debug.Log("Plus Clicked");
		GUI_Dialog.InsertStack(sc_mainmenu_canvas_handler.Instance().cvShop.gameObject);
	}

	public void buka_canvas_payment() {
		bunyi_click ();
		sc_mainmenu_canvas_handler.Instance().cvPayment.gameObject.SetActive( true );
	}
	public void tutup_canvas_payment() {
		bunyi_click ();
		sc_mainmenu_canvas_handler.Instance().cvPayment.gameObject.SetActive( false );
	}

	public void buka_moregames() {
		bunyi_click ();
		sc_mainmenu_canvas_handler.Instance().cvMoreGames.gameObject.SetActive( true );
	}

	public void tutup_moregames() {
		bunyi_click ();
		sc_mainmenu_canvas_handler.Instance().cvMoreGames.gameObject.SetActive( false );
	}

	public void buka_cvs_tiket() {
		bunyi_click ();
		Debug.Log("Tiket Clicked");
		GUI_Dialog.InsertStack (sc_mainmenu_canvas_handler.Instance ().cvTiket.gameObject);
		hitung_tiket_user ();
	}

	public void tutup_cvs_tiket() {
		bunyi_click ();
		Debug.Log("Tiket Closed Clicked");
		GUI_Dialog.ReleaseTopCanvas();
		//sc_mainmenu_canvas_handler.Instance().cvTiket.Hide();
	}

	private void hitung_tiket_user() {
		if(PlayerPrefs.HasKey(PlayerPrefHandler.keyUserTiket)) {
			//txtTiketUser = GameObject.Find("txtTiketUser").GetComponent<Text>();
			//txtTiketUser.text = "= " + PlayerPrefs.GetInt (PlayerPrefHandler.keyUserTiket).ToString() + " PCS";
			Debug.Log("Hitung Tiket");
		}
	}




	IEnumerator login_API_send_JSON(string uname, string pass) {
		yield break;
		/*
		Text txtResultLogin = GameObject.Find("txtResultLogin").GetComponent<Text>();

		string postScoreURL = PlayerPrefs.GetString("ipaddress") + postURLLogin;
		//string jsonString = "{ \"userId\":\""+ uname +"\", \"password\":\""+ pass +"\" }";
		string jsonString = "{ \"loginUserId\":\""+ uname +"\", " +
				"\"password\":\""+ pass +"\", " +
				"\"loginGameId\":\""+GameID+"\"," +
				"\"loginIpAddress\":\"8.8.8.8\"," +
				"\"loginPort\":\"80\"," +
				"\"status\":\"1\"," +
				"\"accessToken\":\""+ FB.AccessToken +"\" }";

		var encoding = new System.Text.UTF8Encoding();
		var postHeader = new Dictionary<string, string>();
		
		postHeader.Add("Content-Type", "application/json");
		postHeader.Add("Content-Length", jsonString.Length.ToString());
		
		print("jsonString-Login: " + jsonString + " | length : " + jsonString.Length);
		
		WWW requestLogin = new WWW(postScoreURL, encoding.GetBytes(jsonString), postHeader);
		
		yield return requestLogin;
		
		// Print the error to the console
		if (requestLogin.error != null)
		{
			Debug.Log("request error: " + requestLogin.error);
			txtResultLogin.text = "Invalid PlayerPrefHandler.keyUserName or password";
		}
		else
		{
			Debug.Log("request Login gemu success");
			Debug.Log("returned Login gemu data" + requestLogin.text);

			string reqData = requestLogin.text;
			string[] lines = reqData.Split(new string[] {","}, System.StringSplitOptions.None);
			string strLines = lines[0].ToString();
			string[] resultCode = strLines.ToString().Split(new string[]{":"}, System.StringSplitOptions.None);
			string errCode = resultCode[1].ToString();

			Debug.Log("errCode : " + errCode);
			if(errCode=="\"0\"") {
				Debug.Log("Login Sukses!!!!!!!!");
				txtResultLogin.text = "Login Success";

				ubah_profile_user(uname, "gemu");
				get_asset_API(uname);
				PlayerPrefs.SetString (PlayerPrefHandler.keyUserName, uname);
				
				GameObject.Find("FB_gameobj").SendMessage("loadGambarGuest");
				PlayerPrefs.SetString (PlayerPrefHandler.keyLoginActive, "gemu");


				sc_mainmenu_canvas_handler.Instance().cvLogin.gameObject.SetActive( false );
				sc_mainmenu_canvas_handler.Instance().cvSetting.gameObject.SetActive( false );

			} else {
				Debug.Log("Login Failed!!!!!!!!");
				txtResultLogin.text = "Login Failed";
			}
			PlayerPrefs.Save();
			//string[] hasilErrCode = lines[0].ToString().Split(new string[]{":", System.StringSplitOptions.None});
			//Debug.Log(hasilErrCode[0]);
		}
		*/
	}


	private void ubah_profile_user(string uname, string statProfile) {
		Debug.Log("ubah_profile_user : " + uname + ", StatProfile : " + statProfile);
		if (statProfile != "facebook") {
			GameObject.Find ("FB_gameobj").SendMessage ("ubahNamaUser", uname);
		}
		//PlayerPrefs.SetString (PlayerPrefHandler.keyLoginActive, statProfile);
		//PlayerPrefs.Save();
	}



	//GET USER ASSET ================================================================
	public void get_asset_API(string userid) {
		/*
		string url = PlayerPrefs.GetString("ipaddress") + postURLgetAsset + userid;
		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www));
		*/
		//cvsLoading.enabled = true;
	}

}