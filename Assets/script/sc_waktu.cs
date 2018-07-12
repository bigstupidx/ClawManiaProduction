using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class sc_waktu : MonoBehaviour {
	public float waktu_main=60;
	public Transform target_ring_pindah;
	int waktu3Point = 20;
	sc_stage scriptStage;
	public bool stat3Point=false;
	Text textWaktu;
	public Text txtTiketGet;
	public Text txtHighestCombo;
	public Text txtHighestScore;
	public Text txtYourScore;
	public Text txtScorePass;

	public Image imgNewHighScore;

	public bool statOpeningDone=false;
	float timerOpening=6.0f;
	public Text textOpening;
	public Text textOpeningBonusStage;

	sc_score scriptScore;
	sc_target scriptTarget;

	public Canvas canvasGameOver;
	public Canvas cvsPassLevel;
	public Canvas cvsQuit;
	public Canvas cvsNoCoin;

	bool statPaused=false;

	public AudioClip soTimesUp;
	bool statBunyiTimesUp=false;

	public AudioClip soReady;
	public AudioClip soSaySet;
	public AudioClip soSayGo;
	bool statSayReady=true;
	bool statSaySet=true;
	bool statSayGo=true;
	float tscale;

	public GameObject buttonContinue;
	int hscore =0;
	//string postURLgetAsset = "felix/api/getUsersAsset/";
	//string postURLupdateAsset = "felix/api/UpdateAsset";
	//string postURLinsertLeaderboard = "felix/api/insertLeaderboard";

	//BAWE
	public Canvas canvasOpening;
	enum enumHasTween
	{
		NONE,
		STAGE,
		READY,
		SET,
		GO,
		REMOVE_OPENING
	};

	enumHasTween hasTween = enumHasTween.NONE;
	Vector3 canvasOpeningScale;

	public void OnClickAds()
	{
		if (GameDataManager.PlayUnityVideoAd ()) {
			StartCoroutine(ShowingAds());
		} else {
			//sc_mainmenu_canvas_handler.Instance..GetInstance ().Show ("Info", "Please wait...", false, "", this.gameObject);
		}
	}

	IEnumerator ShowingAds()
	{
		yield return new WaitForSeconds (1);
		Debug.LogError ("showing=" + UnityEngine.Advertisements.Advertisement.isShowing);
		while (UnityEngine.Advertisements.Advertisement.isShowing) {
			yield return null;
		}
		
		PlayAdsState playAdResult = GameDataManager.unityAdsResult;
		
		if (playAdResult == PlayAdsState.NOTENOUGHTIME) {
			//GemuDialogBoxController.GetInstance ().Show ("Info", "Please wait...", false, "", this.gameObject);
		} else if (playAdResult == PlayAdsState.SKIPPED) {
			//GemuDialogBoxController.GetInstance ().Show ("Info", "Skipping ads will not give you any coin.", false, "", this.gameObject);
		} else if (playAdResult == PlayAdsState.FINISHED) {
			/*
			int jmlCoinsNow = PlayerPrefs.GetInt(PlayerPrefHandler.keyCoin);
			jmlCoinsNow += 5;
			PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin, jmlCoinsNow);
			
			CoinTimerHandler.instance.countCoin = jmlCoinsNow;
			
			GameDataManager.instance.SendPlayResult(
				GameDataManager.GEMU_APP_ID,"0",
				jmlCoinsNow.ToString(),
				"0",
				"1");
			
			GemuDialogBoxController.GetInstance ().Show ("Info", "You got 5 coins.", false, "", this.gameObject);
			*/
		}else if (playAdResult == PlayAdsState.FAILED) {
			//GemuDialogBoxController.GetInstance ().Show ("Info", "Failed playing ads", false, "", this.gameObject);
		}else if (playAdResult == PlayAdsState.NOTREADY) {
			//GemuDialogBoxController.GetInstance ().Show ("Info", "Ad is not ready", false, "", this.gameObject);
		}
		bWatchedAds = true;
		continue_game ();
	}

	// Use this for initialization
	void Start () {
		textOpeningBonusStage.gameObject.SetActive (false);
		tscale = Time.timeScale;
		Debug.Log ("Time Scale 1 : " + Time.timeScale);

		//canvasGameOver = GameObject.Find ("Canvas_gameover").GetComponent<Canvas>();
		canvasGameOver.gameObject.SetActive( false );
		imgNewHighScore.gameObject.SetActive(false);
		//GameObject.Find ("imgGameOver").transform.localScale = new Vector3 (3f,3f,3f);

		//cvsPassLevel = GameObject.Find ("Canvas_pass_level").GetComponent<Canvas> ();
		cvsPassLevel.gameObject.SetActive( false );

		//GameObject.Find ("Canvas_gameover").renderer.enabled = false;
		textWaktu = GetComponent<Text> ();
		//textOpening = GameObject.Find("Text_opening").GetComponent<Text> ();

		canvasOpening.gameObject.SetActive( true );
		canvasOpeningScale = new Vector3( canvasOpening.transform.localScale.x,  
		                                  canvasOpening.transform.localScale.y,
		                                  canvasOpening.transform.localScale.z);

		//sc_stage script = GameObject.Find("txtStage").gameObject.GetComponent<sc_stage>();
		scriptStage = GameObject.Find("txtStage").GetComponent<sc_stage>();
		scriptScore = GameObject.Find("txtScore").GetComponent<sc_score>();

		if(scriptStage.levelAktif==1) {
			waktu_main=60;
			waktu3Point=-1;
			//waktu3Point=20;
			//if(PlayerPrefs.GetString(PlayerPrefHandler.keyLoginActive)=="guest") {
			//	PlayerPrefs.SetInt (PlayerPrefHandler.keyUserTiket, 0);
			//}
		} else if (scriptStage.levelAktif==2) {
			waktu_main=60;
			waktu3Point=20;
		} else if (scriptStage.levelAktif==3) {
			waktu_main=60;
			waktu3Point=40;
		} else if (scriptStage.levelAktif==4) {
			waktu_main=60;
			waktu3Point=20;
		}

		//waktu_main = 5;
		PlayerPrefs.Save();
		
		textWaktu.text = "" + waktu_main;


	}

	bool bDone = false;
	// Update is called once per frame
	void Update () {
		if (bDone)
			return;
		if(statOpeningDone==true) 
		{
			//Debug.Log("Time Scale : " + Time.timeScale);
			if(PlayerPrefs.GetInt(PlayerPrefHandler.keyPause)==0) 
			{
				waktu_main -= 1 * (Time.deltaTime / Time.timeScale);

				if(waktu_main<0) {
					waktu_main=0;

					int levelNext = scriptStage.levelAktif;
					levelNext += 1;
					if(levelNext>4) {
						if(bDone == false ) {
							bDone = true;
							//StartCoroutine("tampil_hscore");
							StartCoroutine(tampil_hscore());
						}	
					} else {
						if(canvasGameOver.gameObject.activeSelf == false) {
							//PlayerPrefs.SetInt(PlayerPrefHandler.keyLevelAktif,levelNext);	
							//PlayerPrefs.Save();
							//StartCoroutine("naikLevel");
							StartCoroutine(naikLevel());
						}
					}

					if(statBunyiTimesUp==false) {
						GameObject.Find("Main Camera").SendMessage("lagu_bg_stop");
						if(PlayerPrefs.GetInt (PlayerPrefHandler.keySound)==1) {
							GetComponent<AudioSource>().PlayOneShot(soTimesUp,20f);
						}
						statBunyiTimesUp=true;
					}
				}
				textWaktu.text = "" + Mathf.Floor(waktu_main);

				if(waktu_main < waktu3Point) {
					stat3Point = true;

					GameObject.Find("tiangbasket").transform.position = Vector3.MoveTowards(GameObject.Find("tiangbasket").transform.position,target_ring_pindah.position,2*Time.deltaTime);
					
					if(GameObject.Find("tiangbasket").transform.position == target_ring_pindah.position) {
						GameObject.Find("Main Camera").SendMessage("paskan_target_ring");
					} else {
						GameObject.Find("Main Camera").SendMessage("Pindahkan_target_ring");
					}
				} 
			}


		} else {

			//opening Time
			timerOpening -= 1 * (Time.deltaTime / Time.timeScale);
			if(timerOpening>=0) {
				if(Mathf.Floor(timerOpening)==5) {
					if ( scriptStage.levelAktif < 4 )
					{
						textOpening.text = "STAGE " + scriptStage.levelAktif;
						textOpeningBonusStage.gameObject.SetActive(false);
					}
					else
					{
						textOpening.text = "";
						textOpeningBonusStage.gameObject.SetActive(true);
					}
					if(hasTween == enumHasTween.NONE)
					{
						iTween.ScaleFrom(canvasOpening.gameObject, iTween.Hash( "scale", Vector3.zero, "time", 0.8f, "easeType", "easeInQuad"));
						hasTween = enumHasTween.STAGE;
					}
				} else if(Mathf.Floor(timerOpening)==2) {
					
					textOpeningBonusStage.gameObject.SetActive(false);
					textOpening.text = "READY"; //+ scriptStage.levelAktif;
					textOpening.color = Color.red;
					if(statSayReady) {
						if (PlayerPrefs.GetInt (PlayerPrefHandler.keySound) == 1) {
							GetComponent<AudioSource>().clip = soReady;
							GetComponent<AudioSource>().PlayOneShot(soReady);
						}
						statSayReady=false;
					}
					if(hasTween == enumHasTween.STAGE)
					{
						iTween.ShakeScale(canvasOpening.gameObject, iTween.Hash("amount",Vector3.one * 0.1f ,"time",0.5f,"loopType","pingPong"));
						StartCoroutine(StopTweenCoRtn(true, 0.5f));
						hasTween = enumHasTween.READY;
					}
				} else if(Mathf.Floor(timerOpening)==1) {
					textOpening.text = "SET";
					textOpening.color = Color.yellow;
					if(statSaySet) {
						if (PlayerPrefs.GetInt (PlayerPrefHandler.keySound) == 1) {
							GetComponent<AudioSource>().PlayOneShot(soSaySet,10f);
						}
						statSaySet=false;
					}
					if(hasTween == enumHasTween.READY)
					{
						//iTween.Resume();
						iTween.ShakeScale(canvasOpening.gameObject, iTween.Hash("amount",Vector3.one * 0.1f ,"time",0.5f,"loopType","pingPong"));
						StartCoroutine(StopTweenCoRtn(true, 0.5f));
						hasTween = enumHasTween.SET;
					}
				} else if (Mathf.Floor(timerOpening)==0) {
					textOpening.text = "GO";
					textOpening.color = Color.green;
					if(statSayGo) {
						if (PlayerPrefs.GetInt (PlayerPrefHandler.keySound) == 1) {
							GetComponent<AudioSource>().PlayOneShot(soSayGo,10f);
						}
						statSayGo=false;
					}
					if(hasTween == enumHasTween.SET)
					{
						//iTween.Resume();
						iTween.ShakeScale(canvasOpening.gameObject, iTween.Hash("amount",Vector3.one * 0.3f ,"time",0.8f,"loopType","pingPong"));
						StartCoroutine(StopTweenCoRtn(true, 0.8f));
						StartCoroutine(HideOpeningCoRtn());
						hasTween = enumHasTween.GO;
					}
				}
			} else {
				if (hasTween == enumHasTween.GO)
				{
					StartCoroutine(DoneOpeningCoRtn());
					hasTween = enumHasTween.REMOVE_OPENING;
				}
			}

		}



	}

	IEnumerator StopTweenCoRtn(bool isStop, float time)
	{
		yield return new WaitForSeconds(time);


		if (isStop)
		{
			iTween.Stop();
			canvasOpening.transform.localScale = canvasOpeningScale;
		}
		else
		{
			iTween.Pause();
			canvasOpening.transform.localScale = canvasOpeningScale;
		}
	}

	IEnumerator HideOpeningCoRtn()
	{
		yield return new WaitForSeconds(1.2f);
		iTween.MoveTo(canvasOpening.gameObject,iTween.Hash("x",2000.0f,"time",2.0f, "easeType", "easeInQuad"));
	}

	IEnumerator DoneOpeningCoRtn()
	{
		//yield return new WaitForSeconds(1.8f);

		GameObject.Find("Canvas_opening").SetActive(false);
		statOpeningDone=true;
		Debug.Log("Lets Play");
		yield break;
	}

	void OnApplicationPause() {
		//Time.timeScale=0f;
		//statPaused=true;
	}

	public void btnNextLevel_click() {
		SoundManager.instance.PlayButton();

		PlayerPrefs.SetInt(PlayerPrefHandler.keyCurrentScore,scriptScore.score);
		PlayerPrefs.Save();
		Application.LoadLevel ("3Dbasketball");
	}

	public void restart_game() {
		SoundManager.instance.PlayButton();
		int jmlCoinsNow =  CoinTimerHandler.instance.countCoin;

		if (jmlCoinsNow >= 1) {
			jmlCoinsNow-=1;

			PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin, jmlCoinsNow);
			CoinTimerHandler.instance.countCoin = jmlCoinsNow;
			GameDataManager.instance.SendPlayResult(GameDataManager.instance.gameID.ToString(),"0",jmlCoinsNow.ToString(),"0","1");


			GameDataManager.instance.SendPlayResult(GameDataManager.instance.gameID.ToString(),"0",jmlCoinsNow.ToString(),PlayerPrefs.GetInt(PlayerPrefHandler.keyCurrentScore).ToString(),PlayerPrefs.GetInt(PlayerPrefHandler.keyLevelAktif).ToString());
			sc_stage scStage = GameObject.Find ("txtStage").GetComponent<sc_stage> ();
			scStage.levelAktif = 1;
			PlayerPrefs.SetInt (PlayerPrefHandler.keyLevelAktif, 1);
			//PlayerPrefs.SetInt ("highscore", 0);
			PlayerPrefs.SetInt (PlayerPrefHandler.keyCurrentScore, 0);
			
			PlayerPrefs.SetInt (PlayerPrefHandler.keyPause, 0);

			PlayerPrefs.Save();
			Application.LoadLevel ("3Dbasketball");
		}
		else
		{
			cvsNoCoin.gameObject.SetActive(true);
		}
		bDone = false;
	}

	public void show_ad()
	{
		ShowOptions options = new ShowOptions();
		options.resultCallback = HandleShowAd;
		
		Advertisement.Show(null,options);
	}

	public void HandleShowAd(ShowResult result)
	{
		if (result == ShowResult.Finished)
		{
			CoinTimerHandler.instance.countCoin += 2;
			continue_game();
		}
	}

	bool bWatchedAds = false;
	public void continue_game() {
		SoundManager.instance.PlayButton();

		int jmlCoinsNow =  CoinTimerHandler.instance.countCoin;
		
		if (bWatchedAds)
			jmlCoinsNow += 2;
		if (jmlCoinsNow >= 2) {
			jmlCoinsNow -= 2;
			PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin, jmlCoinsNow);
			CoinTimerHandler.instance.countCoin = jmlCoinsNow;




			sc_stage scStage = GameObject.Find ("txtStage").GetComponent<sc_stage> ();
			scStage.levelAktif = scriptStage.levelAktif;
			PlayerPrefs.SetInt (PlayerPrefHandler.keyLevelAktif, scriptStage.levelAktif);
			//PlayerPrefs.SetInt ("highscore", 0);
			
			PlayerPrefs.SetInt (PlayerPrefHandler.keyPause, 0);
			PlayerPrefs.Save();
			GameDataManager.instance.SendPlayResult(GameDataManager.instance.gameID.ToString(),"0",jmlCoinsNow.ToString(),PlayerPrefs.GetInt(PlayerPrefHandler.keyCurrentScore).ToString(),PlayerPrefs.GetInt(PlayerPrefHandler.keyLevelAktif).ToString());

			Application.LoadLevel ("3Dbasketball");
		}
		else
		{
			cvsNoCoin.gameObject.SetActive(true);
		}
	}

	 
	private void update_tiket_based_skor() {
		int jml_tiket_user = PlayerPrefs.GetInt (PlayerPrefHandler.keyUserTiket);
		int tambahTiket = 0;

		hscore = PlayerPrefs.GetInt("highscore");
		//PlayerPrefs.SetInt(PlayerPrefHandler.keyCurrentScore,scriptScore.score);
		if (scriptScore.score > hscore)
		{
			hscore=scriptScore.score;
			PlayerPrefs.SetInt("highscore",hscore);
			PlayerPrefs.Save();

			imgNewHighScore.gameObject.SetActive(true);
		}
		Debug.Log("Total Score : " + scriptScore.score);

		//tambahTiket = (int)Mathf.Floor(hscore / 20);
		tambahTiket = GetTicketBasedOnLevel(scriptStage.levelAktif, scriptScore.score);

		jml_tiket_user += tambahTiket;
		Debug.Log ("Score:" + scriptScore.score + " | Dapat Tiket : " + tambahTiket + " ==> total Tiket Didapat : " + jml_tiket_user);
		PlayerPrefs.SetInt (PlayerPrefHandler.keyUserTiket, jml_tiket_user);
		//Text txtTiketGet = GameObject.Find ("txtTicketGet").GetComponent<Text> ();
		//txtTiketGet.text = jml_tiket_user.ToString ();
		txtTiketGet.text = tambahTiket.ToString ();
		/*
		GameDataManager.instance.SendPlayResult (
			GameDataManager.GEMU_APP_ID, 
			tambahTiket.ToString (), 
			PlayerPrefs.GetInt (PlayerPrefHandler.keyCoin).ToString (), 
			PlayerPrefs.GetInt (PlayerPrefHandler.keyCurrentScore).ToString(), 
			PlayerPrefs.GetInt(PlayerPrefHandler.keyLevelAktif).ToString()) ;
		PlayerPrefs.Save();
		*/
		txtHighestCombo.text = PlayerPrefs.GetInt(PlayerPrefHandler.keyCombo, 1).ToString();
		txtYourScore.text = scriptScore.score.ToString();
		txtHighestScore.text = hscore.ToString();


	}

	private int GetTicketBasedOnLevel(int level, int score)
	{
		if (level == 1)
			return 1;
		else if (level == 2)
			return 3;
		else if (level == 3)
			return 5;

		return (7 + ((score-250) / 15));
	}

	IEnumerator proses_insert_leaderboard()
	{
		/*
		string postScoreURL = PlayerPrefs.GetString("ipaddress") + postURLinsertLeaderboard;
		string jsonString = "{ \"gameId\":\""+ PlayerPrefs.GetInt(PlayerPrefHandler.keyGameId) +"\", " +
				"\"userId\":\""+ PlayerPrefs.GetString(PlayerPrefHandler.keyUserName) +"\", " +
				"\"rankingNo\":\"\"," +
				"\"score\":\""+ hscore +"\"," +
				"\"status\":\"1\" }";
		
		var encoding = new System.Text.UTF8Encoding();
		var postHeader = new Dictionary<string, string>();
		
		postHeader.Add("Content-Type", "application/json");
		postHeader.Add("Content-Length", jsonString.Length.ToString());
		
		print("JSON Insert Leaderboard: " + jsonString);
		WWW insertLeaderboard = new WWW(postScoreURL, encoding.GetBytes(jsonString), postHeader);
		
		yield return insertLeaderboard;
		
		// Print the error to the console
		if (insertLeaderboard.error != null) {
			Debug.Log("Insert Leaderboard Error: " + insertLeaderboard.error);
		} else {
			Debug.Log("Success Insert Leaderboard data" + insertLeaderboard.text);
			
			string reqData = insertLeaderboard.text;
			string[] lines = reqData.Split(new string[] {","}, System.StringSplitOptions.None);
			string strLines = lines[0].ToString();
			string[] resultCode = strLines.ToString().Split(new string[]{":"}, System.StringSplitOptions.None);
			string errCode = resultCode[1].ToString();
			
			Debug.Log("Insert Leaderboard ErrorCode : " + errCode);
			if(errCode=="\"0\"") {
				Debug.Log("Insert Leaderboard Sukses!!!!!!!!");
			} else {
				Debug.Log("Insert Leaderboard Failed!!!!!!!!");
			}
			
		}
		*/
		yield break;
	}

	IEnumerator naikLevel() {
		GameObject.Find("Main Camera").SendMessage("refreshBar");
		
		PlayerPrefs.SetInt (PlayerPrefHandler.keyPause, 0);
		int levelNext = scriptStage.levelAktif;
		levelNext += 1;
		
		PlayerPrefs.SetInt(PlayerPrefHandler.keyLevelAktif,levelNext);	
		PlayerPrefs.Save();
		
		//PlayerPrefs.SetInt(PlayerPrefHandler.keyCurrentScore,scriptScore.score);
		
		Debug.Log("Total Score : " + scriptScore.score);
		
		scriptTarget = GameObject.Find("txtTarget").GetComponent<sc_target>();
		Debug.Log ("Target Level : " + scriptTarget.targetLevel + " | Current Score : " + scriptScore.score + "High Score: " + hscore);
		if (scriptScore.score >= scriptTarget.targetLevel) {
			yield return new WaitForSeconds(4);
			
			//show canvas_pass_level
			cvsPassLevel.gameObject.SetActive( true );
			//Text txtScorePass = GameObject.Find("txtScorePass").GetComponent<Text>();
			txtScorePass.text = scriptScore.score.ToString();
			//Application.LoadLevel ("3Dbasketball");
		} else {
			//GameOver
			if(canvasGameOver.gameObject.activeSelf == false) {
				update_tiket_based_skor();
				canvasGameOver.gameObject.SetActive( true );
				
				if (scriptStage.levelAktif >= 4) {
					buttonContinue.gameObject.SetActive(false);
				}
				iTween.ScaleTo(GameObject.Find("imgGameOver"),iTween.Hash("x",1f,"y",1f,"time",0.5f));
				iTween.ShakeScale(GameObject.Find("imgGameOver"),iTween.Hash("x",1f,"y",1f,"time",0.5f,"delay",0.2f));
			}
		}
		PlayerPrefs.Save();
	}

	IEnumerator tampil_hscore() {
		GameObject.Find("Main Camera").SendMessage("refreshBar");

		yield return new WaitForSeconds(4);

		update_tiket_based_skor();
		PlayerPrefs.SetInt (PlayerPrefHandler.keyPause, 1);

		//int jml_tiket_user = PlayerPrefs.GetInt (PlayerPrefHandler.keyUserTiket);
		int tambahTiket = 0;
		
		tambahTiket = (int)Mathf.Floor(hscore / 20);
		tambahTiket = GetTicketBasedOnLevel(scriptStage.levelAktif, scriptScore.score);
		
		//jml_tiket_user += tambahTiket;
		Debug.Log ("Score:" + scriptScore.score + " | Dapat Tiket : " + tambahTiket + " ==> total Tiket Didapat : " + PlayerPrefs.GetInt (PlayerPrefHandler.keyUserTiket));
		GameDataManager.instance.SendPlayResult (
			GameDataManager.instance.gameID.ToString(), 
				tambahTiket.ToString (), 
				PlayerPrefs.GetInt (PlayerPrefHandler.keyCoin).ToString (), 
				PlayerPrefs.GetInt (PlayerPrefHandler.keyCurrentScore).ToString(), 
				PlayerPrefs.GetInt(PlayerPrefHandler.keyLevelAktif).ToString()) ;
		//PlayerPrefs.SetInt (PlayerPrefHandler.keyUserTiket, jml_tiket_user);
		PlayerPrefs.Save();

		txtHighestCombo.text = PlayerPrefs.GetInt(PlayerPrefHandler.keyCombo, 1).ToString();

		canvasGameOver.gameObject.SetActive( true );
		if (scriptStage.levelAktif >= 4) {
			buttonContinue.gameObject.SetActive(false);
		}
		//Application.LoadLevel("scene_hscore");
	}



	IEnumerator update_asset_API(string userid, string statLogin) 
	{
		/*
		int jmlCoinsNow = CoinTimerHandler.instance.countCoin;

		string jsonString = "{ \"userId\":\"" + userid + "\", " +
				"\"bonus\":\""+ PlayerPrefs.GetInt(PlayerPrefHandler.keyUserTiket) +"\"," +
				"\"cash\":\""+ PlayerPrefs.GetInt("cash_game_user") +"\"," +
				"\"coin\":\""+ jmlCoinsNow +"\" }";
		
		var encoding = new System.Text.UTF8Encoding ();
		var postHeader = new Dictionary<string, string> ();
		
		postHeader.Add ("Content-Type", "application/json");
		postHeader.Add ("Content-Length", jsonString.Length.ToString());
		
		print ("Update COINS & TIKET : " + jsonString);
		WWW requestUpdateAsset = new WWW (PlayerPrefs.GetString("ipaddress") + postURLupdateAsset, encoding.GetBytes (jsonString), postHeader);
		
		yield return requestUpdateAsset;
		
		if (requestUpdateAsset.error != null) {
			Debug.Log ("Update COINS & TIKET error: " + requestUpdateAsset.error);
		} else {
			Debug.Log ("Update COINS & TIKET success:" + requestUpdateAsset.text);
			
			string reqData = requestUpdateAsset.text;
			string[] lines = reqData.Split(new string[] {","}, System.StringSplitOptions.None);
			string strLines = lines[0].ToString();
			string[] resultCode = strLines.ToString().Split(new string[]{":"}, System.StringSplitOptions.None);
			string errCode = resultCode[1].ToString();
			
			Debug.Log("errCode : " + errCode);
			if(errCode=="\"0\"") {
				Debug.Log("Update COINS & TIKET API Sukses!!!!!");
			} else {
				Debug.Log("update asset API Failed!!!!!!!!");
			}
		}
		*/
		yield break;
	}



	//GET USER ASSET
	public void get_asset_API(string userid) 
	{
		/*
		string url = PlayerPrefs.GetString("ipaddress") + postURLgetAsset + userid;
		WWW www = new WWW(url);
		StartCoroutine(WaitForRequest(www));
		*/
	}
	IEnumerator WaitForRequest(WWW www)
	{
		/*
		yield return www;
		if (www.error == null) {
			Debug.Log("Load Asset Ok!: " + www.text);
			SimpleJSON.JSONNode rsl = SimpleJSON.JSON.Parse(www.text);
			string idUser = rsl[0].Value;
			int cashUser = int.Parse(rsl[1].Value);
			int coinsUser = int.Parse(rsl[2].Value);
			int tiketUser = int.Parse(rsl[3].Value);
			
			Debug.Log("Gameplay userid : " + idUser);
			Debug.Log("Gameplay cashUser : " + cashUser);
			Debug.Log("Gameplay coinsUser : " + coinsUser);
			Debug.Log("Gameplay tiketUser : " + tiketUser);

			PlayerPrefs.SetInt ("cash_game_user", cashUser);
			PlayerPrefs.SetInt ("coins_game_user", coinsUser);
			PlayerPrefs.SetInt (PlayerPrefHandler.keyUserTiket, tiketUser);

			PlayerPrefs.Save();

		} else {
			Debug.Log("WWW Error: "+ www.error);
		}   
		*/
		yield break;
	}


}
