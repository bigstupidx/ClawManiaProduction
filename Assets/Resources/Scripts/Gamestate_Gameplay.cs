/* ---------------------------------------
 * Programmer : Aji Pamungkas
 * Desc : Handling Gamestate of Claw Mania gameplay
 */

using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public enum Gamestate_Mode
{
	Loading,
	MainMenu,
	Gameplay
}

public enum GrabState
{
	None,
	GettingDown,
	Grabbing,
	GettingUp,
	Return,
	Release,
	FailGrab,
	WaitAfterGrab,
	WaitAfterGettingUp
}

public class Gamestate_Gameplay : Gamestate
{
	public GameObject leftArrow, rightArrow;
	public UILabel labelLv;
	public GameObject[] prefabClaws;
	public GameObject[] prefabJoystick;
	public ContentCategory[] categories; // prefab
	public LineRenderer laser;
	public GameObject prefabBomb;
	public GameObject holeBorder;
	public GameObject machineClawControl;
	public GrabButton grabButton;
	public GameObject ClawControl;
	public JoyCamGrabController JoystickController;
	public float areaSize = 0;
	public GrabState grabState = GrabState.None;
	public Pengait hook;
	public GameObject cameraParent;
	
	public UIPanel guiLoading;
	public UIProgressBar pbLoading;
	public GUI_AchievementUnlocked guiAchievementUnlocked;
	public GUI_Result guiResult;
	public GUI_InGame guiIngame;
	public GUI_LevelUpDialog guiLevelUp;
	bool bRotatingCam = false;

    private int MAX_PRIZES = 40;

    ArrayList targetList = new ArrayList();
	int indexPrize = 0;
	bool bUseLaser = false;
	bool bUseLuckyCharm = false;
	bool bUseWizardWand = false;
	public GUI_PrizeCollection guiPrizeCollection;

	int firstTimer = 0;
	bool bCountSeriousGrabber = true;
	bool bResuming = false;
	public Gamestate_Mode gsMode = Gamestate_Mode.Loading;

	public UIPanel obLoading;
	public UITexture textureLogoGemugemu;
	public UITexture textureLogoGL1;

	public GameObject guiGacha;
	public GUI_ClawMainMenu obMainMenu;
	public UIPanel obOptionMainMenu;
	public UIPanel obOptionInGame;
	public UIPanel obInGame;
	public GUI_MissionCompleted obMissionCompleted;
	public GameObject[] machineBorders;
	public AudioClip musicInGame;
	public AudioClip musicMainMenu;
	public UILabel labelWizardWand;
	public Mission[] missions;
	public Mission currentMission;
	public GameObject ropeFBX;
	public GameObject rope;
	public GameObject ropeLastJoint;
	float ropeLong = 2;
	
	private int LastNotificationId = 0;
	private int freeCoinsCount = 0;

	private string bundleVersion;
	public UILabel versionText;

	private UIButton shareBtn,inviteBtn;
	private bool startingGame = false;

//	const string IntersAdID = "ca-app-pub-6136977680704470/7842342343";
//	InterstitialAd interstitial;
//	AdRequest request;

//	string bannerAdID = "ca-app-pub-6136977680704470/3666371144";
//	BannerView bannerView;
//	AdRequest bannerRequest;

	void Awake(){
//		PlayerPrefs.DeleteAll ();

//		if (Advertisement.isSupported) {
//			Advertisement.Initialize ("1199173");
//		} else {
//			Debug.Log("Platform not supported");
//		}

		//AppsFlyer.setAppsFlyerKey ("o67gJkTD8G2qTWdLXxKb77");
		
		#if UNITY_IOS 
		
		AppsFlyer.setAppID ("YOUR_APP_ID_HERE");
		
		// For detailed logging
		//AppsFlyer.setIsDebug (true); 
		
		// For getting the conversion data will be triggered on AppsFlyerTrackerCallbacks.cs file
		AppsFlyer.getConversionData (); 
		
		// For testing validate in app purchase (test against Apple's sandbox environment
		//AppsFlyer.setIsSandbox(true);         
		
		AppsFlyer.trackAppLaunch ();
		
		#elif UNITY_ANDROID
		
		// All Initialization occur in the override activity defined in the mainfest.xml, 
		// including the track app launch
		// For your convinence (if your manifest is occupied) you can define AppsFlyer library
		// here, use this commented out code.
		
//		AppsFlyer.setAppID ("com.GameLevelOne.ClawMania"); 
//		AppsFlyer.setIsDebug (true);
//		AppsFlyer.createValidateInAppListener ("AppsFlyerTrackerCallbacks", "onInAppBillingSuccess", "onInAppBillingFailure");
//		AppsFlyer.loadConversionData("AppsFlyerTrackerCallbacks","didReceiveConversionData", "didReceiveConversionDataWithError");
//		AppsFlyer.trackAppLaunch ();
		
		#endif

		//print ("AppsFlyerId = " + AppsFlyer.getAppsFlyerId());

	}

	void Start () 
	{
        //PlayerPrefs.DeleteAll();
		guiGacha.SetActive (false);

		Screen.orientation = ScreenOrientation.Portrait;
		Application.runInBackground = false;
		Application.targetFrameRate = 30;
		obLoading.alpha = 1;
		transition.Setup ();

		//shareBtn.gameObject.SetActive(false);
		//inviteBtn.gameObject.SetActive(false);

		AndroidNotificationManager.instance.OnNotificationIdLoaded += OnNotificationIdLoaded;

		StartGame ();
	}

	public void OnClickGachaButton(){
		guiGacha.SetActive (true);
	}

	void OnApplicationPause(bool pauseStatus) 
	{
		//Debug.LogError ("paused");
		int diffEnergy = GameManager.MAX_FREECOIN_FROM_TIMER - GameManager.FREECOINS;
		if ( diffEnergy > 0 )
		{
			int timer = (((int)(diffEnergy/10))+1)*300;
			//Debug.LogError(timer);
			//LastNotificationId = AndroidNotificationManager.instance.ScheduleLocalNotification("Hello", "Your energy is full", timer);
		}
	}

	private void OnNotificationIdLoaded (int notificationid){
		
		//Debug.Log( "App was laucnhed with notification id: " + notificationid);
		
	}

	public void SetIgnoreCollider(Collider collider)
	{
		if (collider == null)
			return;
		for (int i=0; i<machineBorders.Length; i++) {
			Physics.IgnoreCollision (collider,machineBorders[i].GetComponent<Collider>());
		}

		if (hook) {
			for ( int i=0; i<hook.colliders.Length; i++ )
			{
				Physics.IgnoreCollision (collider,hook.colliders[i].GetComponent<Collider>());
			}

			for ( int i=0; i<hook.hooks.Length; i++ )
			{
				Physics.IgnoreCollision (collider,hook.hooks[i].sensor.GetComponent<Collider>());

			}
		}
	}

	public void StartGame()
	{
		GUI_Dialog.ClearStack ();
		if (gsMode == Gamestate_Mode.Gameplay) 
		{
			guiGacha.SetActive(false);

			if ( !bUseWizardWand )
				labelWizardWand.gameObject.SetActive(false);
			if ( gameObject.GetComponent<AudioSource>() )
			{
				gameObject.GetComponent<AudioSource>().clip = musicInGame;
			}
			indexPrize = 0;
			obLoading.gameObject.SetActive(false);
			guiLoading.gameObject.SetActive(true);
			obOptionInGame.gameObject.SetActive(true);
			obOptionInGame.alpha = 1;
			
			obInGame.gameObject.SetActive(true);
			obInGame.alpha = 1;
			grabButton.Reset();
			if (PlayerPrefs.HasKey (GameManager.PREF_SETTING_AUDIO)) {
				int iValue = PlayerPrefs.GetInt (GameManager.PREF_SETTING_AUDIO);
				if (iValue == 0)
					GameManager.bSoundOn = false;
				else if (iValue == 1)
					GameManager.bSoundOn = true;
			} else {
				GameManager.bSoundOn = true;
			}
			GameManager.SetupAudio ();
			luckyCharmEffect.gameObject.SetActive (false);
			holeBorder.gameObject.SetActive (true);
			
			//PlayerPrefs.SetInt ("freecoins", 100); // buat testing
			if (PlayerPrefs.HasKey ("exp"))
				GameManager.EXP = PlayerPrefs.GetInt ("exp");
			
			
			GameManager.FREECOINS = GameManager.MAX_FREECOIN_FROM_TIMER;
			if (PlayerPrefs.HasKey ("freecoins")) {
				GameManager.FREECOINS = PlayerPrefs.GetInt ("freecoins");
				if (GameManager.FREECOINS > GameManager.MAX_FREECOIN_FROM_TIMER) {
					GameManager.FREECOINS = GameManager.MAX_FREECOIN_FROM_TIMER;
					PlayerPrefs.SetInt ("freecoins", GameManager.MAX_FREECOIN_FROM_TIMER);
				}
			} else
				PlayerPrefs.SetInt ("freecoins", GameManager.MAX_FREECOIN_FROM_TIMER);
			//GameManager.FREECOINS = 200;
			GameManager.TICKET = 0;
			if (PlayerPrefs.HasKey (PlayerPrefHandler.keyUserTiket))
				GameManager.TICKET = PlayerPrefs.GetInt (PlayerPrefHandler.keyUserTiket);
			else
				PlayerPrefs.SetInt (PlayerPrefHandler.keyUserTiket, 0);

            GameManager.GEMUCOINS = PlayerPrefs.GetInt(PlayerPrefHandler.keyCoin, GameManager.startingFunds);
			
			guiIngame.RefreshLevelInfo ();
			guiIngame.RefreshExpInfo ();
			guiIngame.RefreshFreeEnergyInfo ();
			guiIngame.RefreshTicketInfo ();
			guiIngame.RefreshGemuCoinInfo ();
			guiIngame.RefreshPowerUpsInfo();
			
			guiLoading.alpha = 1;
			firstTimer = GameManager.getEpochTime ();
			
			if (PlayerPrefs.HasKey (PlayerPrefHandler.keyClawFreeEnergyTimer) == false)
				PlayerPrefs.SetInt (PlayerPrefHandler.keyClawFreeEnergyTimer, GameManager.getEpochTime ());
			
			
			string sJoystick = "";
			if (PlayerPrefs.HasKey ("playerjoy") == false)
				PlayerPrefs.SetString ("playerjoy", "ClawControl1");
			if (PlayerPrefs.HasKey ("playerjoy"))
				sJoystick = PlayerPrefs.GetString ("playerjoy");
			SetJoystick (sJoystick);
			
			if (hook) {
				hook.value = 0;
			}
			StartCoroutine (RandomBoxes ());
			obMainMenu.gameObject.SetActive(false);
			obOptionMainMenu.gameObject.SetActive(false);
			transition.Show(Transition.TransitionMode.EaseOut);
			//GL1Connector.GetInstance().PlayResult(this.gameObject,GameManager.getLevelValue().ToString(),"0",GameManager.EXP.ToString(),"200");
			//BannerAds.instance.RequestBannerAd();
			//BannerAds.instance.ShowBannerAd();
		} 
		else if (gsMode == Gamestate_Mode.Loading) 
		{
			StartCoroutine(ShowingLogos());
			//BannerAds.instance.RequestBannerAd();
		}
		else if (gsMode == Gamestate_Mode.MainMenu) 
		{
//			if(startingGame){
//				GameObject.Find ("CheckVersion-GPGS").GetComponent<CheckVersion>().RunCheckVersion();
//				startingGame=false;
//			}

			float tempE = cameraParent.transform.localEulerAngles.y;

			if(tempE != 180f){
				Vector3 newE = new Vector3(0,180f,0);
				cameraParent.transform.localEulerAngles = newE;
			}

			Transform trTitle = obMainMenu.transform.Find("Title");
			if ( trTitle )
			{
				int index = 0;
				foreach ( BlinkingSprite sprites in trTitle.gameObject.GetComponentsInChildren<BlinkingSprite>() )
				{
					//Debug.LogError(sprites.name);
					//StartCoroutine(sprites.Blinking(index%2));
					index ++;
				}
			}

			if ( gameObject.GetComponent<AudioSource>() )
			{
				gameObject.GetComponent<AudioSource>().clip = musicMainMenu;
			}
			obMainMenu.RefreshLevelInfo();
			obMainMenu.gameObject.SetActive(true);
			obOptionInGame.gameObject.SetActive(false);
			obMainMenu.GetComponent<UIPanel>().alpha = 1;
			obMainMenu.RefreshInfo();

			obOptionMainMenu.gameObject.SetActive(true);
			obInGame.gameObject.SetActive(false);
			//obOptionMainMenu.alpha = 1;

			transition.Show (Transition.TransitionMode.EaseOut);
			
			#region FACEBOOK
			// string sFBID = "";
			// if (SPFacebook.instance.IsInited && SPFacebook.instance.IsLoggedIn)
			// 	sFBID = SPFacebook.instance.UserId;
			#endregion

			//gl1Connector.RequestUserData (this.gameObject,gl1Connector.GetCurrUser(),gl1Connector.GetToken(),SystemInfo.deviceUniqueIdentifier,sFBID);
			
			labelLv.text = GameManager.getLevelValue ().ToString();

			//StartCoroutine(GooglePlayLogin());

//			BannerAds.instance.HideBannerAd();
//			bannerView = new BannerView(bannerAdID, AdSize.Banner, AdPosition.Top);
//			bannerRequest = new AdRequest.Builder().Build();
//			bannerView.LoadAd(bannerRequest);
		}
	}

	IEnumerator GooglePlayLogin(){
		yield return new WaitForSeconds(2f);
		GPGServices.instance.ActivateGooglePlay();
	}


	IEnumerator ShowingLogos()
	{
		obLoading.gameObject.SetActive (true);
		//textureLogoGemugemu.gameObject.SetActive (true);
		//textureLogoGL1.gameObject.SetActive (false);
		//transition.Show (Transition.TransitionMode.EaseOut);
		//yield return new WaitForSeconds (3);

		//transition.Show (Transition.TransitionMode.EaseIn);
		//while ( transition.IsDone == false )
			//yield return null;
		
		textureLogoGemugemu.gameObject.SetActive (false);
		textureLogoGL1.gameObject.SetActive (true);
		
		transition.Show (Transition.TransitionMode.EaseOut);
		yield return new WaitForSeconds (3);

		
		transition.Show (Transition.TransitionMode.EaseIn);
		while ( transition.IsDone == false )
			yield return null;

		obLoading.gameObject.SetActive (false);
		gsMode = Gamestate_Mode.MainMenu;
		startingGame = true;
		//gsMode = Gamestate_Mode.Gameplay;
		StartGame ();

		yield break;
	}

	public void SetClaw(string sClaw)
	{
		if ( hook )
		{
			GameObject.Destroy(hook.transform.parent.gameObject);
			hook = null;
		}

		if ( rope )
		{
			GameObject.Destroy(rope.gameObject);
			ropeLastJoint = null;
		}

		for ( int i=0; i<prefabClaws.Length; i++ )
		{
			string sPrefabClaw = prefabClaws[i].name;
			if ( string.Equals( sClaw, sPrefabClaw ) )
			{
				GameObject ropeIns = (GameObject)Instantiate(ropeFBX);
				rope = ropeIns;
				ropeLastJoint = ropeIns.transform.Find("Armature").transform.Find("Bone_005").gameObject;
				GameObject ob = (GameObject)Instantiate(prefabClaws[i]);
				//ob.transform.position = new Vector3(areaSize-0.4f,14,areaSize-0.4f);
				ob.transform.position = new Vector3(areaSize-0.2f,14,areaSize-0.2f);


				rope.transform.position = ob.transform.position+new Vector3(0,ropeLong,0);
				hook = ob.transform.Find("Pengait").gameObject.GetComponent<Pengait>();

				for ( int j=0; j<hook.colliders.Length; j++ )
				{
					Collider collider = hook.colliders[j];
					for ( int k=0; k<machineBorders.Length; k++ )
					{
						Physics.IgnoreCollision(collider,machineBorders[k].GetComponent<BoxCollider>());
					}
				}

				Transform trJoint = ob.transform.Find("capitan");
				if ( trJoint )
				{
					Rigidbody rigidBody = trJoint.gameObject.GetComponent<Rigidbody>();
					SpringJoint springJoint = ropeLastJoint.GetComponent<SpringJoint>();
					springJoint.connectedBody = rigidBody;
					
					FixedJoint fixedJoints = ropeLastJoint.GetComponent<FixedJoint>();
					fixedJoints.connectedBody = rigidBody;
				}
				break;
			}
		}
	}

	public void SetJoystick(string sJoystick)
	{
		if ( ClawControl )
		{
			GameObject.Destroy(ClawControl.gameObject);
			ClawControl = null;
			//grabButton = null;
			JoystickController.obJoystick = null;
		}
		for ( int i=0; i<prefabJoystick.Length; i++ )
		{
			string sPrefabJoys = prefabJoystick[i].name;
			if ( string.Equals( sJoystick, sPrefabJoys ) )
			{
				GameObject ob = (GameObject)Instantiate(prefabJoystick[i]);
				ob.transform.parent = cameraParent.transform;
				ob.transform.localScale = new Vector3(0.9f,0.9f,0.9f);
				Joystick joy = ob.GetComponent<Joystick>();
				JoystickController.obJoystick = joy.theJoystick;
				JoystickController.SetEuler();
				ClawControl = ob;
			}
		}
	}

	public void PlayMission(Mission mission)
	{
		if ( currentMission )
			GameObject.Destroy(currentMission.gameObject);

		bMissionDone = false;
		currentMission = (Mission)Instantiate(mission);
		currentMission.name = mission.name;
		//if ( mission.missionType == Mission_Type.Procedural )
		//{

			//currentMission.ProceduralAmount = mission.ProceduralAmount;
		//}
		
		PlayerPrefs.SetInt(GameManager.PREF_CURR_USER_PROCEDURAL_MISSION_PRIZE_AMOUNT,0);
		PlayerPrefs.SetInt(GameManager.PREF_CURR_USER_PROCEDURAL_MISSION_COIN_AMOUNT,0);
		//Debug.LogError ("PlayMission mission=" + mission.name + " currentmission=" + currentMission.name+" proceduralamount="+PlayerPrefs.GetInt(""));
	}

	public void OnClickBonusButton(int index)
	{
		if ( index >= categories.Length )
			return;

		ContentCategory cat = categories [index];
		if ( cat )
		{
			switch(index){
			case 0:
				Social.ReportProgress(GPGSIds.achievement_the_sporty, 100.0f, (bool success) => {
					// handle success or failure
					//achievementManager.AddToFinalAchievementEvent();
				});
				break;
			case 1:
				Social.ReportProgress(GPGSIds.achievement_sweet_tooth, 100.0f, (bool success) => {
					// handle success or failure
					//achievementManager.AddToFinalAchievementEvent();
				});
				break;
			case 2:
				Social.ReportProgress(GPGSIds.achievement_car_collector, 100.0f, (bool success) => {
					// handle success or failure
					//achievementManager.AddToFinalAchievementEvent();
				});
				break;
			case 3:
				//no achievement
				break;
			case 4:
				Social.ReportProgress(GPGSIds.achievement_papa_bear, 100.0f, (bool success) => {
					// handle success or failure
					//achievementManager.AddToFinalAchievementEvent();
				});
				break;
			case 5:
				Social.ReportProgress(GPGSIds.achievement_our_big_fan, 100.0f, (bool success) => {
					// handle success or failure
					//achievementManager.AddToFinalAchievementEvent();
				});
				break;
			case 6:
				Social.ReportProgress(GPGSIds.achievement_the_animal_lover, 100.0f, (bool success) => {
					// handle success or failure
					//achievementManager.AddToFinalAchievementEvent();
				});
				break;

			}
			//Debug.LogError("freecoins="+GameManager.FREECOINS);
			GameManager.FREECOINS += cat.bonus;
			if ( GameManager.FREECOINS > GameManager.MAX_FREECOIN_FROM_TIMER )
				GameManager.FREECOINS = GameManager.MAX_FREECOIN_FROM_TIMER;
			//Debug.LogError("freecoins="+GameManager.FREECOINS);
			PlayerPrefs.SetInt("freecoins",GameManager.FREECOINS);
			guiIngame.RefreshFreeEnergyInfo();

			GameManager.EXP+=cat.bonusEXP;

			PlayerPrefs.SetInt("exp",GameManager.EXP);
			
			int checkNextLevel = GameManager.getLevelValue();
			int currLevel = 1;
			//Debug.LogError("currLv="+currLevel+" checkNextLv="+checkNextLevel);
			if ( PlayerPrefs.HasKey(GameManager.keyClawMania_Level) )
			{
				currLevel = PlayerPrefs.GetInt(GameManager.keyClawMania_Level);
			}
			else{
				PlayerPrefs.SetInt(GameManager.keyClawMania_Level,currLevel);
				PlayerPrefs.Save();
			}

			if ( checkNextLevel > currLevel && PlayerPrefs.HasKey(GameManager.keyClawMania_Level) )
			{
				guiDialogBox.Show("Level Up !!", "You are now level "+checkNextLevel.ToString()+". You got 100 coins",false,"levelup",this.gameObject);
				
				GameObject.Find ("ButtonShareLvUp").SetActive (true);
				GameObject.Find ("ButtonInviteLvUp").SetActive (true);
				GameManager.GEMUCOINS+=100;
				PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,GameManager.GEMUCOINS);
				
				PlayerPrefs.SetInt(GameManager.keyClawMania_Level,checkNextLevel);
			}

			guiIngame.RefreshLevelInfo ();
			guiIngame.RefreshExpInfo ();
			RefreshAllInfo();

			for ( int j=0; j<cat.contents.Length; j++ )
			{
				int iAmount = 0;
				if ( PlayerPrefs.HasKey("cc."+index+"."+j) )
				{
					iAmount = PlayerPrefs.GetInt("cc."+index+"."+j);
				}
				iAmount --;
				PlayerPrefs.SetInt("cc."+index+"."+j,iAmount);
			}
			guiPrizeCollection.OnShow();
			ShowDialogBox("Info","You got "+cat.bonus+" Energy",false,"",this.gameObject);
		}
	}

	public void GoToMainMenu()
	{
		transition.Show (Transition.TransitionMode.EaseIn);
		StartCoroutine (GoingToMainMenu ());
	}

	IEnumerator GoingToMainMenu()
	{
		while ( transition.IsDone == false )
			yield return null;

		ArrayList list = new ArrayList ();
		for ( int i=0; i<transform.childCount ;i++ )
		{
			list.Add(transform.GetChild(i));
		}

		for (int i=0; i<list.Count; i++) {
				GameObject.Destroy(((Transform)list[i]).gameObject);
		}

		guiDialogBox.ClearQueue ();
		gsMode = Gamestate_Mode.MainMenu;
		StartGame ();
		yield break;
	}


	IEnumerator RandomBoxes()
	{
		//Debug.LogError ("RandomBoxes");
		pbLoading.value = 0;

		for ( int i=0; i<categories.Length; i++ )
		{
			ContentCategory cat = categories[i];
			for ( int j=0; j<cat.contents.Length ; j++ )
			{
				Content content = cat.contents[j];
				targetList.Add(content);
			}
		}

//		print ("start print target list");
//		for (int x=0; x<targetList.Count; x++) {
//			print (targetList[x]);
//		}
//		print ("end print target list");

		while ( indexPrize<MAX_PRIZES )
		{
			RandomPrize();

			float fVal = ((100.0f/MAX_PRIZES)/100.0f)*(indexPrize+1);
			//Debug.LogError(pbLoading.value);
			pbLoading.value = fVal;
			yield return new WaitForSeconds(0.1f);
		}

		yield return new WaitForSeconds (2);
		guiLoading.gameObject.SetActive (false);
		holeBorder.gameObject.SetActive (false);
		
		string sClaw = "";
		if ( PlayerPrefs.HasKey("playerclaw") == false )
			PlayerPrefs.SetString("playerclaw","Claw1");
		if ( PlayerPrefs.HasKey("playerclaw") )
			sClaw = PlayerPrefs.GetString("playerclaw");
		SetClaw (sClaw);
		
		#region FACEBOOK
		// string sFBID = "";
		// if (SPFacebook.instance.IsInited && SPFacebook.instance.IsLoggedIn)
		// 	sFBID = SPFacebook.instance.UserId;
		#endregion
		//gl1Connector.RequestUserData (this.gameObject,gl1Connector.GetCurrUser(),gl1Connector.GetToken(),SystemInfo.deviceUniqueIdentifier,sFBID);
		StartCoroutine (CheckingPrize ());
		yield break;
	}

	void SetPowerUpAmount(string sID,int iAmount)
	{
		PlayerPrefs.SetInt (sID, iAmount);
	}

	int GetPowerUpAmount(string sID)
	{
		//PlayerPrefs.SetInt (sID, 5);
		int iAmount = 0;
		if ( PlayerPrefs.HasKey(sID) )
		{
			iAmount = PlayerPrefs.GetInt(sID);
		}
		return iAmount;
	}


	public void OnClickPowerUpButton()
	{
		int powerUpCount = 0;
		bool countAch = true;
		int bombFlag = 0, laserFlag = 0, wandFlag = 0, charmFlag = 0, holeFlag = 0;

		if (PlayerPrefs.HasKey (PlayerPrefHandler.keyPowerupCount))
			powerUpCount = PlayerPrefs.GetInt (PlayerPrefHandler.keyPowerupCount);

		UIButton button = UIButton.current;
		if ( button == null )
			return;

		ButtonPowerUp buttonPowerUp = button.GetComponent<ButtonPowerUp> ();
		if ( buttonPowerUp == null )
			return;

		int iLastAmount = GetPowerUpAmount (buttonPowerUp.type.ToString () + "_Amount");
		if ( iLastAmount != 0 )
		{
			
			SendMissionEvent(Mission_Event.UsePowerup);
			bool bProcess = false;
			if ( buttonPowerUp.type == PowerUp.Bomb )
			{
				GameObject obInst = (GameObject)Instantiate(prefabBomb);
				if ( obInst )
				{
					Vector3 pos = Vector3.zero;
					pos.x = ((Random.value*100) % areaSize)*Random.Range(-1.5f,1.5f);
					pos.y = 14;
					pos.z = ((Random.value*100) % areaSize)*Random.Range(-1.5f,1.5f);
					obInst.transform.localPosition = pos;
					bProcess = true;
				}
				if(PlayerPrefs.HasKey(PlayerPrefHandler.keyBomb)){
					bombFlag=PlayerPrefs.GetInt(PlayerPrefHandler.keyBomb);
				}
				if(bombFlag==0){
					PlayGamesPlatform.Instance.IncrementAchievement(
						GPGSIds.achievement_aim_for_power, 1, (bool success) => {
						// handle success or failure
					});
					bombFlag=1;
					PlayerPrefs.SetInt(PlayerPrefHandler.keyBomb,bombFlag);
				}
			}
			else if ( buttonPowerUp.type == PowerUp.LaserPointer )
			{
				//Debug.LogError("Laser");
				if ( !bUseLaser )
				{
					bUseLaser = true;
					bProcess = true;
					laser.gameObject.SetActive(true);
					laser.gameObject.GetComponent<Laser>().SetActive(true);

					if(PlayerPrefs.HasKey(PlayerPrefHandler.keyLaser)){
						laserFlag=PlayerPrefs.GetInt(PlayerPrefHandler.keyLaser);
					}

					if(laserFlag==0){
						PlayGamesPlatform.Instance.IncrementAchievement(
							GPGSIds.achievement_aim_for_power, 1, (bool success) => {
							// handle success or failure
						});
						laserFlag=1;
						PlayerPrefs.SetInt(PlayerPrefHandler.keyLaser,laserFlag);
					}
				}
				else
				{
					ShowDialogBox("Info", "There is an active laser",false,"",this.gameObject);
				}
			}
			else if ( buttonPowerUp.type == PowerUp.LuckyCharm )
			{
				if ( !bUseLuckyCharm )
				{
					bUseLuckyCharm = true;
					bProcess = true;
					
					luckyCharmEffect.gameObject.SetActive (true);
					luckyCharmEffect.gameObject.GetComponent<LuckyCharmEffect>().SetActive(true);

					if(PlayerPrefs.HasKey(PlayerPrefHandler.keyCharm)){
						charmFlag=PlayerPrefs.GetInt(PlayerPrefHandler.keyCharm);
					}
					if(charmFlag==0){
						PlayGamesPlatform.Instance.IncrementAchievement(
							GPGSIds.achievement_aim_for_power, 1, (bool success) => {
							// handle success or failure
						});
						charmFlag=1;
						PlayerPrefs.SetInt(PlayerPrefHandler.keyCharm,charmFlag);
					}
				}
				else
				{
					ShowDialogBox("Info", "There is an active lucky charm",false,"",this.gameObject);
				}
			}
			else if ( buttonPowerUp.type == PowerUp.WizardWand )
			{
				if ( !bUseWizardWand )
				{
					bUseWizardWand = true;
					bProcess = true;
					labelWizardWand.gameObject.SetActive(true);

					if(PlayerPrefs.HasKey(PlayerPrefHandler.keyWand)){
						wandFlag=PlayerPrefs.GetInt(PlayerPrefHandler.keyWand);
					}
					if(wandFlag==0){
						PlayGamesPlatform.Instance.IncrementAchievement(
							GPGSIds.achievement_aim_for_power, 1, (bool success) => {
							// handle success or failure
						});
						wandFlag=1;
						PlayerPrefs.SetInt(PlayerPrefHandler.keyWand,wandFlag);
					}
				}
				else
				{
					ShowDialogBox("Info", "There is an active wizard wand",false,"",this.gameObject);
				}
			}
			else if ( buttonPowerUp.type == PowerUp.BlackHole )
			{
				ArrayList list = new ArrayList ();
				for ( int i=0; i<transform.childCount ;i++ )
				{
					list.Add(transform.GetChild(i));
				}
				for (int i=0; i<list.Count; i++) {
					GameObject.Destroy(((Transform)list[i]).gameObject);
				}
				indexPrize = 0;
				StartCoroutine (RandomBoxes ());
				bProcess = true;

				if(PlayerPrefs.HasKey(PlayerPrefHandler.keyBlackHole)){
					holeFlag=PlayerPrefs.GetInt(PlayerPrefHandler.keyBlackHole);
				}
				if(holeFlag==0){
					PlayGamesPlatform.Instance.IncrementAchievement(
						GPGSIds.achievement_aim_for_power, 1, (bool success) => {
						// handle success or failure
					});
					holeFlag=1;
					PlayerPrefs.SetInt(PlayerPrefHandler.keyBlackHole,holeFlag);
				}
			}
			else
			{
				ShowNotImplemented();
			}

			if ( bProcess )
			{
				//achievementManager.OnAchievementEvent(AchievementType.UsePowerUp,1);
				iLastAmount --;
				SetPowerUpAmount(buttonPowerUp.type.ToString()+"_Amount",iLastAmount);
				GameManager.SetNGUILabel(buttonPowerUp.transform.Find("Amount"),iLastAmount.ToString());
			}

			powerUpCount++;
		}

		if (powerUpCount < 50 ) {
			PlayGamesPlatform.Instance.IncrementAchievement(
				GPGSIds.achievement_the_badass, 1, (bool success) => {
				// handle success or failure
			});
		}
		PlayerPrefs.Save ();
	}

	IEnumerator CheckingPrize()
	{
		bool bRunning = true;
		while ( bRunning )
		{
			if ( grabState == GrabState.Release )
			{
				if ( transform.childCount < MAX_PRIZES )
				{
					RandomPrize();
				}
			}
			yield return null;
		}
		yield break;
	}

    int getFixedIndex(int currIdx) { //0 = normal, 1 = high value
//        if (currIdx < 20)
//            return 0;
//        else if (currIdx >= 20 && currIdx < 32)
//            return 1;
//        else if (currIdx >= 32 && currIdx < 38)
//            return 2;
//        else
//            return 3;

        if (currIdx < 30)
            return 0;
        else
            return 1;
    }

	void RandomPrize()
	{
        //int iRandom = (int)(Random.value * 100) % targetList.Count;
        int iRandom = getFixedIndex(indexPrize);

		Content obTarget = (Content)targetList [iRandom];
		//Debug.LogError (obTarget.name);
		Content obInst = (Content)Instantiate(obTarget);
		Collider coll = obInst.gameObject.GetComponentInChildren<Collider> ();
		if ( coll )
		{
			coll.gameObject.AddComponent<ContentCollider>();
		}
		obInst.transform.parent = this.transform;
		obInst.name = indexPrize.ToString();
		//obInst.transform.localScale = new Vector3(((Random.value*100)%1)+0.4f,0.4f,0.4f);
		Vector3 pos = Vector3.zero;
		pos.x = ((Random.value*100) % areaSize)*Random.Range(-1,1);
		pos.y = 10f;
		pos.z = ((Random.value*100) % areaSize)*Random.Range(-1,1);
		obInst.transform.localPosition = pos;
		indexPrize++;
	}

	bool bGrab = false;
	public void GrabObject()
	{
		bGrab = true;
		
		SendMissionEvent(Mission_Event.Grab);
	}

	bool bMissionDone = false;
	public void SendMissionEvent(Mission_Event missionEvent)
	{
		if ( currentMission == null )
			return;

		if (bMissionDone)
			return;
		
		//Debug.LogError("current="+currentMission.missionEvent.ToString()+" mission="+missionEvent.ToString());
		if (currentMission.missionEvent == missionEvent )
		{
			if ( currentMission.missionType == Mission_Type.Tutorial )
			{

				StartCoroutine(DestroyingMission());
			}
			else if ( currentMission.missionType == Mission_Type.Procedural )
			{

				int iCurrUser_ProceduralAmount = 0;
				
				int iMaxProceduralAmount = 0;
				if ( currentMission.missionEvent == Mission_Event.Prize )
				{
					if ( PlayerPrefs.HasKey(GameManager.PREF_CURR_USER_PROCEDURAL_MISSION_PRIZE_AMOUNT) )
						iCurrUser_ProceduralAmount = PlayerPrefs.GetInt(GameManager.PREF_CURR_USER_PROCEDURAL_MISSION_PRIZE_AMOUNT);
					iCurrUser_ProceduralAmount ++;
					PlayerPrefs.SetInt(GameManager.PREF_CURR_USER_PROCEDURAL_MISSION_PRIZE_AMOUNT,iCurrUser_ProceduralAmount);

					if ( PlayerPrefs.HasKey(GameManager.PREF_PROCEDURAL_MISSION_PRIZE_AMOUNT) )
						iMaxProceduralAmount = PlayerPrefs.GetInt(GameManager.PREF_PROCEDURAL_MISSION_PRIZE_AMOUNT);
				}
				else if ( currentMission.missionEvent == Mission_Event.Coin )
				{
					if ( PlayerPrefs.HasKey(GameManager.PREF_CURR_USER_PROCEDURAL_MISSION_COIN_AMOUNT) )
						iCurrUser_ProceduralAmount = PlayerPrefs.GetInt(GameManager.PREF_CURR_USER_PROCEDURAL_MISSION_COIN_AMOUNT);
					iCurrUser_ProceduralAmount ++;
					PlayerPrefs.SetInt(GameManager.PREF_CURR_USER_PROCEDURAL_MISSION_PRIZE_AMOUNT,iCurrUser_ProceduralAmount);

					if ( PlayerPrefs.HasKey(GameManager.PREF_PROCEDURAL_MISSION_COIN_AMOUNT) )
						iMaxProceduralAmount = PlayerPrefs.GetInt(GameManager.PREF_PROCEDURAL_MISSION_COIN_AMOUNT);

				}
				if ( iCurrUser_ProceduralAmount >= iMaxProceduralAmount )
				{
					StartCoroutine(DestroyingMission());
				}
				//Debug.LogError("target amount="+PlayerPrefs.GetInt(GameManager.PREF_PROCEDURAL_MISSION_AMOUNT));
			}
		}
	}

	IEnumerator DestroyingMission()
	{
		yield return new WaitForSeconds (1);

		if (currentMission == null)
			yield break;

		bMissionDone = true;
		obMissionCompleted.coinRewardValue.text = currentMission.CoinReward.ToString ()+" GemuGold";
		obMissionCompleted.expRewardValue.text = currentMission.ExpReward.ToString ()+" Exp";
		obMissionCompleted.missionName.text = currentMission.MissionName;

		GameManager.GEMUCOINS += currentMission.CoinReward;
		GameManager.EXP += currentMission.ExpReward;
		PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,GameManager.GEMUCOINS);
		guiIngame.RefreshGemuCoinInfo ();
		guiIngame.RefreshExpInfo ();
		guiIngame.RefreshLevelInfo ();
		//GL1Connector.GetInstance().AddBalance(this.gameObject,"0",currentMission.CoinReward.ToString(),"");

		GameObject.Destroy (currentMission.gameObject);
		if ( currentMission.missionType == Mission_Type.Tutorial )
		{
			int bTutorialAmount = 0;
			for ( int i=0; i<missions.Length ; i ++ )
			{
				Mission mission = missions[i];
				if ( mission.missionType == Mission_Type.Tutorial && PlayerPrefs.HasKey("mission."+mission.MissionName.Replace(" ","") ) )
				{
					bTutorialAmount ++;
				}
			}
			
			Debug.LogError("bTutorialAmount="+bTutorialAmount);
			if ( bTutorialAmount == 4 )
			{
				ShowDialogBox("Congratulations","You have finished all tutorial missions",false,"",this.gameObject);
			}
			PlayerPrefs.SetInt ( "mission."+currentMission.MissionName.Replace(" ", "") , 1 );
		}
	
		GUI_Dialog.InsertStack (obMissionCompleted.gameObject);
		currentMission = null;

	}

	//bool bGrabbing = false;
	float xDir = 0;
	float zDir = 0;
	float timerWaiting = 0;
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyUp (KeyCode.Escape)) 
		{
			//isKeyDown = true;

			
			if (GUI_Dialog.GetActiveStackAmount () > 0) {
				GUI_Dialog.ReleaseTopCanvas ();
			}
			else
			{
				if ( gsMode == Gamestate_Mode.Gameplay )
					ShowDialogBox("Confirmation","Are you sure you want to go to main menu ?",true,"gotomainmenu",this.gameObject);
				else if ( gsMode == Gamestate_Mode.MainMenu )
					ShowDialogBox("Confirmation","Are you sure you want to quit ?",true,"exitgame",this.gameObject);
			}
		}

		if (gsMode != Gamestate_Mode.Gameplay)
			return;

		if ( bUseWizardWand )
		{
			if ( Input.GetMouseButtonDown(0) )
			{
				Vector2 startPos = Input.mousePosition;
				RaycastHit hit;
				Camera cam2D = Camera.allCameras [0];
				Ray ray = cam2D.ScreenPointToRay( startPos );
				
				if (Physics.Raycast (ray, out hit, 1000.0f , 1<<GameManager.LayerContent ))
				{
					bUseWizardWand = false;
					
					labelWizardWand.gameObject.SetActive(false);
					OnDropSuccess(hit.collider.gameObject);
				}
			}
		}
		xDir = 0;
		zDir = 0;
		if ( hook  )
		{
			Vector3 hookPos = hook.transform.position;
			if ( grabState == GrabState.None )
			{
				//Debug.LogError(JoystickController.vDir);
				if ( fTargetRotCam == 180 )
				{
					xDir = JoystickController.vDir.x;
					zDir = JoystickController.vDir.y;
				}
				else
				{
					if ( fTargetRotCam == 270 )
					{
						xDir = JoystickController.vDir.y;
						zDir = -JoystickController.vDir.x;
					}
					else if ( fTargetRotCam == 90 )
					{
						xDir = -JoystickController.vDir.y;
						zDir = JoystickController.vDir.x;

					}
				}

				if ( xDir != 0 )
				{
					hook.PlayAudio(hook.audioMoveClaw);
					hookPos.x += xDir*Time.deltaTime*0.5f;
					SendMissionEvent(Mission_Event.MoveJoystick);
					Debug.Log("movejoystick1");
				}
				
				if ( zDir != 0 )
				{
					hook.PlayAudio(hook.audioMoveClaw);
					hookPos.z += zDir*Time.deltaTime*0.5f;
					SendMissionEvent(Mission_Event.MoveJoystick);
					Debug.Log("movejoystick2");
				}


				if ( hookPos.x < -areaSize )
					hookPos.x = -areaSize;
				else if ( hookPos.x > areaSize )
					hookPos.x = areaSize;

				if ( hookPos.z < -areaSize )
					hookPos.z = -areaSize;
				else if ( hookPos.z > areaSize )
					hookPos.z = areaSize;


				if ( Input.GetKeyUp(KeyCode.Space) || bGrab )
				{	

					if ( grabState == GrabState.None )
					{
						if ( GameManager.GEMUCOINS - GameManager.GRAB_COIN_REDUCED >= 0 )
						{
                            //GameManager.FREECOINS -= GameManager.GRAB_COIN_REDUCED;
                            GameManager.GEMUCOINS -= GameManager.GRAB_COIN_REDUCED; //energy changed with funds
							PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,GameManager.GEMUCOINS);
                            PlayerPrefs.Save();
                            //guiIngame.RefreshFreeEnergyInfo();
                            guiIngame.RefreshGemuCoinInfo();
							//hook.TurnOnOffSensors (true);
							grabState = GrabState.GettingDown;
							hook.ClearListTrigger();
							hook.PlayAudio(hook.audioClawDrop);
							Physics.IgnoreLayerCollision (GameManager.LayerContent, GameManager.LayerClaw, false);
						}
						else
						{
							grabButton.Reset();
							ShowDialogBox("Not Enough Fund", "",false,"AddFund",this.gameObject);
							//disable free gift button
//							if (TimerHandler.instance.FreeGiftAvailablilty ()) {
//								//TextAndButton_FreeGift.SetActive (true);
//								//Text_FreeGiftTimer.gameObject.SetActive (false);
//
//							} else {
//								GUI_Shop.GetInstance().showTimerBox();		
//							}

							bGrab = false;
						}
					}

				}
			}
			else if ( grabState == GrabState.GettingDown )
			{
				hookPos.y -= Time.deltaTime *2;
				if ( hookPos.y < 9 )
				{
					hookPos.y = 9;
					hook.PlayAudio(hook.audioClawGrab);
					grabState = GrabState.Grabbing;

				}
			}
			else if ( grabState == GrabState.Grabbing )
			{
				hook.value += Time.deltaTime;
				bool bDone = false;

				if ( hook.DoneGrabbing )
				{
					bDone = true;
					Physics.IgnoreLayerCollision (GameManager.LayerContent, GameManager.LayerClaw, true);
				}
				else if ( hook.value >= 1 )
				{
					bDone = true;
					hook.value = 1;
				}

				if ( bDone )
				{
					grabState = GrabState.WaitAfterGrab;
					timerWaiting = 0;
					hook.EnableCollider(false);
					if ( hook.GetTarget() == null && bUseLuckyCharm  )
					{
						//Debug.LogError("no target");
						// find the closest target
						Transform trTarget = null;
						float iDist = 9999;

						for ( int i=0; i<this.transform.childCount; i++ )
						{
							Transform trChild = this.transform.GetChild(i);
							//Debug.LogError(trChild.name);
							Rigidbody rigidboy = trChild.GetComponentsInChildren<Rigidbody>()[0];
							float diffX = rigidboy.transform.position.x - hook.transform.position.x;
							float diffZ = rigidboy.transform.position.z - hook.transform.position.z;
							float iCurrDist = Mathf.Sqrt((diffX*diffX)+(diffZ*diffZ));
							if ( iCurrDist < iDist )
							{
								trTarget = trChild;
								iDist = iCurrDist;
							}
						}

						if ( trTarget != null )
						{
							Rigidbody rigidboy = trTarget.transform.GetComponentsInChildren<Rigidbody>()[0];
							hook.SetTarget(rigidboy.gameObject);
							trTarget.localPosition = Vector3.zero;
							rigidboy.transform.localPosition = new Vector3(0,-1,0);

						}
					}
				}
			}
			else if ( grabState == GrabState.WaitAfterGrab )
			{
				timerWaiting += Time.deltaTime;
				if ( timerWaiting > 1 )
				{
					grabState = GrabState.GettingUp;
				}
			}
			else if ( grabState == GrabState.GettingUp )
			{
				hook.PlayAudio(hook.audioMoveClaw);
				grabButton.Reset();
				hookPos.y += Time.deltaTime * 2;
				if ( hookPos.y > 14 )
				{
					hookPos.y = 14;

					timerWaiting = 0;
					grabState =GrabState.WaitAfterGettingUp;

					laser.gameObject.GetComponent<Laser>().SetActive(false);
					laser.gameObject.SetActive(false);
					bUseLaser = false;
					luckyCharmEffect.gameObject.SetActive(false);
					laser.gameObject.SetActive(false);
				}
			}
			else if ( grabState == GrabState.WaitAfterGettingUp )
			{
				
				timerWaiting += Time.deltaTime;
				if ( timerWaiting > 1 )
				{
					if ( hook.GetTarget() != null )
					{
						if ( bUseLuckyCharm == false )
						{
							int iRandom = (int)(Random.value*100)%3;
							if ( iRandom == 0 )
							{
								hook.PlayAudio(hook.audioClawGrab);
								hook.ReleaseTarget();
								grabState = GrabState.FailGrab;
							}
							else
							{
								hook.PlayAudio(hook.audioMoveClaw);
								grabState = GrabState.Return;
							}
						}
						else
						{
							hook.PlayAudio(hook.audioMoveClaw);
							grabState = GrabState.Return;

						}
						
						bUseLuckyCharm = false;
					}
					else
						grabState = GrabState.FailGrab;

				}
			}
			else if ( grabState == GrabState.Return )
			{
				hook.PlayAudio(hook.audioMoveClaw);
				Vector3 targetPos = Vector3.Lerp(hook.transform.position,new Vector3(areaSize,hook.transform.position.y,areaSize),Time.deltaTime*0.5f);
				hookPos.x = targetPos.x;
				hookPos.z = targetPos.z;
				float Dis = Vector3.Distance(targetPos,new Vector3(areaSize,hook.transform.position.y,areaSize));
				if ( Dis < 0.2f )
				{
					hook.EnableCollider(true);
					hook.PlayAudio(hook.audioClawGrab);
					grabState = GrabState.Release;
					GameObject target = hook.GetTarget();
					hook.ReleaseTarget();
					
					if ( target )
					{
						SetIgnoreCollider(target.GetComponentInChildren<Collider>());
					}
				}
			}
			else if ( grabState == GrabState.Release )
			{
				hook.value -= Time.deltaTime;
				if ( hook.value <= 0 )
				{
					grabState = GrabState.None;
					hook.ClearListTrigger();
				}
			}
			else if ( grabState == GrabState.FailGrab )
			{
				grabState = GrabState.Return;
				//hook.value -= Time.deltaTime;
				//if ( hook.value <= 0 )
				//{
				//	grabState = GrabState.Return;
				//}
			}

			hook.transform.position = hookPos;
			rope.transform.position = hookPos+new Vector3(0,ropeLong,0);
		}
		bGrab = false;

		if ( bUseLaser )
		{
			Laser theLaser = laser.GetComponent<Laser>();
			laser.SetPosition(0,hook.laserAttachTo.transform.position);
			laser.SetPosition(1,hook.laserTargetTo.transform.position);
		}

		int fTimer = GameManager.getEpochTime();
		if ( PlayerPrefs.HasKey(PlayerPrefHandler.keyClawFreeEnergyTimer ) )
		{
			fTimer = PlayerPrefs.GetInt(PlayerPrefHandler.keyClawFreeEnergyTimer);
		}
		else
			PlayerPrefs.SetInt(PlayerPrefHandler.keyClawFreeEnergyTimer,fTimer);

		if ( GameManager.FREECOINS<GameManager.MAX_FREECOIN_FROM_TIMER)
		{
			int iDiff = GameManager.getEpochTime() - fTimer;
			int iMultiply = 1;
			bool bAddCoin = false;
			if ( iDiff > GameManager.MAX_TIMER_COIN )
			{
				iMultiply = (int)(iDiff/GameManager.MAX_TIMER_COIN);
				bAddCoin = true;
			}

			if ( bAddCoin == false )
			{
				int fCurrTimer = GameManager.MAX_TIMER_COIN - (GameManager.getEpochTime() - fTimer);
				//Debug.LogError(fCurrTimer);
				if ( fCurrTimer < 0 )
					fCurrTimer = 0;
				/*
				int iMinute = (int)(fCurrTimer / 60);
				int iSecond = (int)(fCurrTimer - (iMinute * 60));
				string sMinutes = iMinute.ToString ();
				if ( sMinutes.Length < 2 )
					sMinutes = "0"+sMinutes;
				string sSeconds = iSecond.ToString ();
				if ( sSeconds.Length < 2 )
					sSeconds = "0"+sSeconds;
					*/
				guiIngame.RefreshLabelTimer (GameManager.convertIntToStringTime((int)fCurrTimer,false));
				if ( fCurrTimer == 0 )
				{
					bAddCoin = true;
				}
			}
			else
			{
				guiIngame.RefreshLabelTimer ("00:00");
			}

			if ( bAddCoin )
			{
				GameManager.FREECOINS += iMultiply*GameManager.TIMER_ADD_COIN;
				if ( GameManager.FREECOINS > GameManager.MAX_FREECOIN_FROM_TIMER )
				{
					GameManager.FREECOINS = GameManager.MAX_FREECOIN_FROM_TIMER;
				}
				guiIngame.RefreshFreeEnergyInfo();
				PlayerPrefs.SetInt(PlayerPrefHandler.keyClawFreeEnergyTimer,GameManager.getEpochTime());
				PlayerPrefs.SetInt("freecoins",GameManager.FREECOINS);
			}
		}
		else
		{

		}

		if ( bCountSeriousGrabber && ( GameManager.getEpochTime() - firstTimer ) > GameManager.MAX_TIMER_SERIOUS_GRABBER )
		{
			bCountSeriousGrabber = false;
			//achievementManager.OnAchievementEvent(AchievementType.SeriousGrabber,1);
		}
		//Debug.LogError ("play timer=" + (getEpochTime() - firstTimer));
	}

	public void BackToGemu()
	{
		StartCoroutine (GoingBackToGemu ());
	}

	IEnumerator GoingBackToGemu()
	{
		transition.Show (Transition.TransitionMode.EaseIn);
		while (transition.IsDone == false)
			yield return null;
		Application.LoadLevel ("main");
	}

	public void OnDropSuccess(GameObject droppedob)
	{
		if ( droppedob == null )
			return;
		
		Debug.Log("dropped "+droppedob.name);
		Debug.Log ("content=" + droppedob.name+" parent="+droppedob.transform.parent.name);
		//Content con = droppedob.transform.parent.gameObject.GetComponent<Content> ();

		int freecoin = 0;

		Content con = droppedob.transform.parent.GetComponent<Content> ();
		if ( con )
		{
			Debug.LogError("success "+droppedob.name);

			if ( droppedob.name.Contains("ticket") )
			{
				Debug.LogError("ticket");
				int iTicket = 0;
				if ( PlayerPrefs.HasKey(PlayerPrefHandler.keyUserTiket) )
					iTicket = PlayerPrefs.GetInt(PlayerPrefHandler.keyUserTiket);
				iTicket += con.exp;

				PlayerPrefs.SetInt(PlayerPrefHandler.keyUserTiket,iTicket);
				
				guiResult.Show(con.icon,con.exp+" "+con.theName, con.fundReward);
				//GL1Connector.GetInstance().PlayResult(this.gameObject,GameManager.getLevelValue().ToString(),GameManager.EXP.ToString(),"");
				//GL1Connector.GetInstance().AddGameBalance(this.gameObject,con.exp.ToString(),"","CMPRIZEGP"+con.exp.ToString());

				GameManager.TICKET += con.exp;
				guiIngame.RefreshTicketInfo();
			}
			else if ( droppedob.name.Contains("Coin") )
			{
				int iCoin = 0;
				if ( PlayerPrefs.HasKey(PlayerPrefHandler.keyCoin) )
					iCoin = PlayerPrefs.GetInt(PlayerPrefHandler.keyCoin);
				iCoin += con.exp;
				
				PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,iCoin);
				
				guiResult.Show(con.icon,con.exp+" "+con.theName, con.fundReward);
				//GL1Connector.GetInstance().AddBalance(this.gameObject,"0",con.exp.ToString(),"");
				
				GameManager.GEMUCOINS += con.exp;
				RefreshAllInfo();
			}
			else
			{
				int iTotalPrize = 0;
				if( PlayerPrefs.HasKey("totalprize") )
					iTotalPrize = PlayerPrefs.GetInt("totalprize");
				iTotalPrize ++;
				PlayerPrefs.SetInt("totalprize",iTotalPrize);
				
				SendMissionEvent(Mission_Event.Prize);
				//Debug.LogError("totalPrize="+iTotalPrize);
				if ( iTotalPrize == GameManager.AMOUNT_PRIZE_COLLECTOR )
				{
					//achievementManager.OnAchievementEvent(AchievementType.PrizeCollector,GameManager.AMOUNT_PRIZE_COLLECTOR);
				}

				GameManager.EXP += con.exp;
				PlayerPrefs.SetInt("exp",GameManager.EXP);

				int checkNextLevel = GameManager.getLevelValue();
				Debug.Log("checkNextLevel:"+checkNextLevel);
				int currLevel = 1;
				//Debug.LogError("currLv="+currLevel+" checkNextLv="+checkNextLevel);
				if ( PlayerPrefs.HasKey(GameManager.keyClawMania_Level) )
				{
					currLevel = PlayerPrefs.GetInt(GameManager.keyClawMania_Level);
					Debug.Log ("setCurrLevel:"+currLevel);
				}
				else{
					PlayerPrefs.SetInt(GameManager.keyClawMania_Level,currLevel);
					PlayerPrefs.Save();
					Debug.Log ("setDefaultLevel:"+currLevel);
				}

                //				if ( checkNextLevel > currLevel && PlayerPrefs.HasKey(GameManager.keyClawMania_Level) )
                //				{
                //					Debug.LogError("LevelUp");
                //					guiDialogBox.Show("Level Up !!", "You are now level "+checkNextLevel.ToString()+". You got 100 coins",false,"levelup",this.gameObject);
                //					
                //					GameObject.Find ("ButtonShareLvUp").SetActive (true);
                //					GameObject.Find ("ButtonInviteLvUp").SetActive (true);
                //					GameManager.GEMUCOINS+=100;
                //					PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,GameManager.GEMUCOINS);
                //					
                //					PlayerPrefs.SetInt(GameManager.keyClawMania_Level,checkNextLevel);
                //				}
                //				
                //				RefreshAllInfo();
                //
                //				guiIngame.RefreshLevelInfo();
                //				guiIngame.RefreshExpInfo();
                //guiResult.icon.mainTexture = con.icon;
                //guiResult.labelName.text = con.theName;
                int reward = con.fundReward;

                GameManager.GEMUCOINS += reward;
                PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin, GameManager.GEMUCOINS);
				guiResult.Show(con.icon,con.theName,reward);
                PlayerPrefs.Save();

#region free coins and banner ads -> deactivated

//				if(Random.value>=0.7){
//					if (interstitial.IsLoaded()) {
//						interstitial.Show();
//						interstitial.LoadAd(request);
//					}
//				}
//				else{
//					//do nothing
//				}

				/*if (Random.value >= 0.5 && checkNextLevel==currLevel) {
					if (Random.value >= 0.1)
						freecoin = Random.Range (1, 80);
					else
						freecoin = Random.Range (80, 101);
					
					guiDialogBox.Show("Info","You got "+freecoin+" coins",false,"",this.gameObject);

					GameManager.GEMUCOINS += freecoin;
					RefreshAllInfo();

					if(PlayerPrefs.HasKey(PlayerPrefHandler.keyFreeCoinsCount))
						freeCoinsCount = PlayerPrefs.GetInt(PlayerPrefHandler.keyFreeCoinsCount);
					else
						freeCoinsCount=0;
					
					
					freeCoinsCount++;
					
					if(freeCoinsCount==1){
						Social.ReportProgress(GPGSIds.achievement_first_free_coins, 100.0f, (bool success) => {
							// handle success or failure
							//achievementManager.AddToFinalAchievementEvent();
						});
						Social.ReportProgress(GPGSIds.achievement_watch_ads, 100.0f, (bool success) => {
							// handle success or failure
							//achievementManager.AddToFinalAchievementEvent();
						});
					}
					
					if(freeCoinsCount<50){
						PlayGamesPlatform.Instance.IncrementAchievement(
							GPGSIds.achievement_the_more_the_better, 1, (bool success) => {
							// handle success or failure
							//achievementManager.AddToFinalAchievementEvent();
						});
					}
					
					PlayerPrefs.Save();
					
				} */

#endregion

				if ( checkNextLevel > currLevel && PlayerPrefs.HasKey(GameManager.keyClawMania_Level) )
				{
					Debug.Log("Level Up");
					guiDialogBox.Show("Level Up !!", "You are now level "+checkNextLevel.ToString()+". You got 100 coins",false,"levelup",this.gameObject);
					//guiLevelUp.Show("Level Up !!", "You are now level "+checkNextLevel.ToString()+". You got 100 coins");

					GameObject.Find ("ButtonShareLvUp").SetActive (true);
					GameObject.Find ("ButtonInviteLvUp").SetActive (true);
					GameManager.GEMUCOINS+=100;
					PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,GameManager.GEMUCOINS);

					PlayerPrefs.SetInt(GameManager.keyClawMania_Level,checkNextLevel);
				}

				RefreshAllInfo();

				//GUI_Dialog.InsertStack(guiResult.gameObject);
				Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag("Gamestate").GetComponent<Gamestate_Gameplay>();
				if ( gs )
				{
					bool bPrizeCurrator = true;
					for ( int i=0; i<gs.categories.Length; i++ )
					{
						ContentCategory cat = (ContentCategory)gs.categories[i];
						if ( cat )
						{
							for ( int j=0; j<cat.contents.Length; j++ )
							{
								Content content = (Content)cat.contents[j];
								if ( con.theName.Equals(content.theName) )
								{
									int iAmount = 0;
									if ( PlayerPrefs.HasKey("cc."+i+"."+j) )
										iAmount = PlayerPrefs.GetInt("cc."+i+"."+j);
									iAmount ++;
									PlayerPrefs.SetInt("cc."+i+"."+j,iAmount);
									break;
								}
							}
							
							bool bFirstCollection = true;
							for ( int j=0; j<cat.contents.Length; j++ )
							{
								int iAmount = 0;
								if ( PlayerPrefs.HasKey("cc."+i+"."+j) )
									iAmount = PlayerPrefs.GetInt("cc."+i+"."+j);


								if ( iAmount == 0 )
								{
									bFirstCollection = false;
									bPrizeCurrator = false;
									break;
								}
							}

							if ( bFirstCollection ){
								//achievementManager.OnAchievementEvent(AchievementType.FirstCollection,1);
								Social.ReportProgress(GPGSIds.achievement_my_first_collection, 100.0f, (bool success) => {
									// handle success or failure
									//achievementManager.AddToFinalAchievementEvent();
								});
							}
						}
					}

					if ( bPrizeCurrator ){
						//achievementManager.OnAchievementEvent(AchievementType.PrizeCurator,1);
						Social.ReportProgress(GPGSIds.achievement_prize_curator, 100.0f, (bool success) => {
							// handle success or failure
							//achievementManager.AddToFinalAchievementEvent();
						});
					}
					
				}
				//Debug.LogError(con.theName);
				GameObject.Destroy (droppedob.transform.parent.gameObject);

			}
		}
		else
			Debug.LogError("null");

	}

	float fTargetRotCam = 180;
	IEnumerator RotatingCam()
	{
		ClawControl.gameObject.SetActive (false);
		machineClawControl.gameObject.SetActive (false);
		grabButton.gameObject.SetActive (false);
		Vector3 euler = cameraParent.transform.localEulerAngles;
		float fInitY = euler.y;
		float fTimer = 0;
		while ( bRotatingCam )
		{
			JoystickController.Reset ();
			euler.y = Mathf.Lerp(fInitY,fTargetRotCam,fTimer);

			if ( euler.y == fTargetRotCam )
			{
				bRotatingCam = false;
			}
			fTimer += Time.deltaTime;
			cameraParent.transform.localEulerAngles = euler;

			yield return null;
		}
		
		ClawControl.gameObject.SetActive (true);
		machineClawControl.gameObject.SetActive (true);
		grabButton.gameObject.SetActive (true);
		JoystickController.Reset ();
		yield break;
	}

	public void OnSwipe(int iDirection)
	{
		if ( bRotatingCam == false )
		{		
			
			SendMissionEvent(Mission_Event.ChangeCameraAngle);
			bRotatingCam = true;
			fTargetRotCam += (iDirection*90);
			if (fTargetRotCam > 270)
			{
				bRotatingCam = false;
				fTargetRotCam = 270;
			}
			else if (fTargetRotCam < 90)
			{
				bRotatingCam = false;
				fTargetRotCam = 90;
			}
			else if(fTargetRotCam > 180 && fTargetRotCam <= 270){
				leftArrow.SetActive(false);
			}
			else if(fTargetRotCam >= 90 && fTargetRotCam < 180){
				rightArrow.SetActive(false);
			}
			else{
				leftArrow.SetActive (true);
				rightArrow.SetActive (true);
			}
			StartCoroutine (RotatingCam ());
		}
	}

	/*
	public override void WhenGL1Done()
	{
		Debug.LogError ("WhenGL1Done");
		guiIngame.RefreshGemuCoinInfo ();
		guiIngame.RefreshTicketInfo ();
	}
	*/

	public void RefreshAllInfo()
	{
		//Debug.LogError ("RefreshAllInfo");
		
		obMainMenu.RefreshLevelInfo ();
		guiIngame.RefreshLevelInfo ();
		guiIngame.RefreshTicketInfo ();
		guiIngame.RefreshGemuCoinInfo ();
		guiIngame.RefreshFreeEnergyInfo ();
		guiIngame.RefreshExpInfo ();
		labelLv.text = GameManager.getLevelValue ().ToString();

		PlayerPrefs.SetInt (PlayerPrefHandler.keyCoin, GameManager.GEMUCOINS);
		PlayerPrefs.Save ();
	}

	public void OnGL1Done(JSONNode N)
	{
		//Debug.LogError ("[Gamestate_Gameplay] OnGL1Done");
		if (gl1Connector.GetLastURL().Contains ("getuser") ) 
		{
			if ( N ["errcode"].ToString () == "\"0\"" )
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

					PlayerPrefs.SetInt("exp",GameManager.EXP);
					PlayerPrefs.SetInt(PlayerPrefHandler.keyUserTiket,GameManager.TICKET);
					PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,GameManager.GEMUCOINS);
				}
			}
			else
			{
				LogOut();
				//ShowDialogBox("Info",N["errdetail"].ToString(),false,"",this.gameObject);
			}
		}
		RefreshAllInfo ();
	}

	public void LogOut()
	{
		Debug.LogError ("Logout");
		PlayerPrefs.SetString (PlayerPrefHandler.keyToken, "");
		PlayerPrefs.SetString (PlayerPrefHandler.keyUserName, "");
		PlayerPrefs.SetInt (PlayerPrefHandler.keyCoin, 0);
		PlayerPrefs.SetInt (PlayerPrefHandler.keyUserTiket, 0);
		//PlayerPrefs.SetInt (GameManager.keyClawMania_Energy, 200);
		PlayerPrefs.SetInt (GameManager.keyClawMania_Level, 1);
		PlayerPrefs.SetInt ("exp", 0);

		GameManager.EXP = 0;
		RefreshAllInfo ();
	}

//	public void TempGetExp(){
//		Debug.Log ("GetExp");
//		GameManager.EXP += 100;
//		int checkNextLevel = GameManager.getLevelValue();
//		int currLevel = 1;
//		//Debug.LogError("currLv="+currLevel+" checkNextLv="+checkNextLevel);
//		if ( PlayerPrefs.HasKey(GameManager.keyClawMania_Level) )
//		{
//			currLevel = PlayerPrefs.GetInt(GameManager.keyClawMania_Level);
//			Debug.Log ("setCurrLevel");
//		}
//		else{
//			PlayerPrefs.SetInt(GameManager.keyClawMania_Level,currLevel);
//			PlayerPrefs.Save();
//			Debug.Log ("setDefaultLevel");
//		}
//		
//		if ( checkNextLevel > currLevel && PlayerPrefs.HasKey(GameManager.keyClawMania_Level) )
//		{
//			Debug.LogError("LevelUp");
//			guiDialogBox.Show("Level Up !!", "You are now level "+checkNextLevel.ToString()+". You got 100 coins",false,"levelup",this.gameObject);
//			
//			GameObject.Find ("ButtonShareLvUp").SetActive (true);
//			GameObject.Find ("ButtonInviteLvUp").SetActive (true);
//			GameManager.GEMUCOINS+=100;
//			PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,GameManager.GEMUCOINS);
//			
//			PlayerPrefs.SetInt(GameManager.keyClawMania_Level,checkNextLevel);
//		}
//		
//		RefreshAllInfo();
//	}
}
