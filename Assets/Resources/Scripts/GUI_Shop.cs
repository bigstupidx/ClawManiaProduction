using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using UnityEngine.Advertisements;
using System;

public class GUI_Shop : GUI_Dialog 
{
	public GameObject containerShop;
	public GameObject containerRedeem;

	public UILabel labelGemuCoin;
	//public UILabel labelGemuTicketicket;
	public UILabel labelLevel;
	public UILabel labelEnergy;
	public UIProgressBar progressExp;
	//public UIButton tabShop;
	//public UIButton tabRedeem;
	
	public UIButton buttonShopCoins;
	public UIButton buttonShopClaw;
	public UIButton buttonShopJoystick;
	public UIButton buttonShopMachine;
	public UIButton buttonShopFreeCoins;
	public UIButton buttonShopPowerups;
	public GameObject prefabContentShop;
	//public GameObject prefabContentRedeem;

	public UIScrollView containerShopCoins;
	public UIScrollView containerShopClaw;
	public UIScrollView containerShopJoystick;
	public UIScrollView containerShopMachine;
	public UIScrollView containerShopFreeCoins; //energy
	public UIScrollView containerShopPowerups;

	public GUI_InGame guiIngame;
	public UIDragScrollView dragScroll;
	public GUI_GL1_Reward gl1Reward;
	static GUI_Shop instance;
	public GemuFreeCoins gemuFreeCoins;

	int watchCount = 0;
	//int watchAchCount = 0;

	bool powerUpAch = false;

	void Awake(){
		//PlayerPrefs.DeleteAll ();
//		if (Advertisement.isSupported) {
//			Advertisement.Initialize ("29466");
//		} else {
//			Debug.Log("Platform not supported");
//		}
	}

	public void OnClickBack()
	{
		if (powerUpAch) {
			powerUpAch = false;

			Gamestate_Gameplay gs = GameObject.Find ("Gamestate").GetComponent<Gamestate_Gameplay> ();
			if (gs == null)
				return;

			//gs.achievementManager.OnAchievementEvent (AchievementType.PowerPlay, 1);
		} else {
			GUI_Dialog.ReleaseTopCanvas ();
		}
	}
	public void OnClickRedeem()
	{
		StopCoroutine ("WaitingToShowReward");
		StartCoroutine (WaitingToShowReward ());
	}
	
	IEnumerator WaitingToShowReward()
	{
		yield return new WaitForSeconds (1);
		//gl1Reward.Show ();
		
		GUI_Dialog.InsertStack(gl1Reward.gameObject);
		yield break;
	}

	public void RefreshInfo()
	{
		int iAmountGemuCoin = 0;
		if ( PlayerPrefs.HasKey(PlayerPrefHandler.keyCoin) )
			iAmountGemuCoin = PlayerPrefs.GetInt(PlayerPrefHandler.keyCoin);
		labelGemuCoin.text = iAmountGemuCoin.ToString ();

//		int iAmountTicket = 0;
//		if (PlayerPrefs.HasKey (PlayerPrefHandler.keyUserTiket))
//			iAmountTicket = PlayerPrefs.GetInt (PlayerPrefHandler.keyUserTiket);
//		labelGemuTicketicket.text = iAmountTicket.ToString ();

		int iAmountExp = 0;
		if (PlayerPrefs.HasKey ("exp"))
			iAmountExp = PlayerPrefs.GetInt ("exp");

		labelEnergy.text = GameManager.FREECOINS + "/200";
		labelLevel.text = GameManager.getLevelValue ().ToString ();
		//Debug.LogError ("currlv=" + GameManager.getLevelValue ());
		if ( GameManager.getLevelValue() < 99 )
		{
			float fProgres =  GameManager.getProgressValue();
			//Debug.LogError("progress="+fProgres);
			progressExp.value = fProgres;
		}
		else
		{
			progressExp.value = 1;
		}

	}

	public override void OnShow()
	{

		UIButton.current = buttonShopFreeCoins;
		OnClickShopButton ();
		RefreshInfo ();
		containerShopCoins.ResetPosition ();
		containerShopClaw.ResetPosition ();
		containerShopJoystick.ResetPosition ();
		containerShopMachine.ResetPosition ();
		containerShopFreeCoins.ResetPosition ();
		containerShopPowerups.ResetPosition ();
	}

	void OnEnable()
	{
		instance = this;

		RefreshInfo ();
	}

	public static GUI_Shop GetInstance()
	{
		return instance;
	}

	// Use this for initialization
	public override void OnStart () 
	{
		for ( int i=0; i<containerShopPowerups.transform.childCount ; i++ )
		{
			ShopContent content = containerShopPowerups.transform.GetChild(i).gameObject.GetComponent<ShopContent>();
			//Debug.LogError(content.name);
			GameManager.SetNGUILabel(content.transform.Find("Label Price"),content.Price.ToString()+" coins");
			GameManager.SetNGUILabel(content.transform.Find("Label Title"),content.Name);
		}

		for ( int i=0; i<containerShopFreeCoins.transform.childCount ; i++ )
		{
			if(i>=1){
				ShopContent content = containerShopFreeCoins.transform.GetChild(i).gameObject.GetComponent<ShopContent>();
				//Debug.LogError(content.name);
				//GameManager.SetNGUILabel(content.transform.Find("Label Price"),content.Price.ToString()+" GemuGold");
				GameManager.SetNGUILabel(content.transform.Find("Label Title"),content.Name);
			}
		}

		for ( int i=0; i<containerShopJoystick.transform.childCount ; i++ )
		{
			ShopContent content = containerShopJoystick.transform.GetChild(i).gameObject.GetComponent<ShopContent>();

			//GameManager.SetNGUILabel(content.transform.Find("Label Price"),content.Price.ToString()+" GemuGold");
			GameManager.SetNGUILabel(content.transform.Find("Label Title"),content.Name);

			Transform trButtonBuy = content.transform.Find("ButtonBuy");
			if  (PlayerPrefs.HasKey("joystick."+content.uniqueID) == false && content.bFree == true )
			{
				PlayerPrefs.SetInt("joystick."+content.uniqueID,1);
			}
			
			if ( PlayerPrefs.HasKey("joystick."+content.uniqueID ) )
			{
				GameManager.SetNGUILabel(content.transform.Find("Label Price"),"");
				GameManager.SetNGUILabel(trButtonBuy.transform.Find("Label"),"Equip");
			}
		}

		for ( int i=0; i<containerShopClaw.transform.childCount ; i++ )
		{
			ShopContent content = containerShopClaw.transform.GetChild(i).gameObject.GetComponent<ShopContent>();
			
			//GameManager.SetNGUILabel(content.transform.Find("Label Price"),content.Price.ToString()+"Gold");
			GameManager.SetNGUILabel(content.transform.Find("Label Title"),content.Name);

			//PlayerPrefs.DeleteKey("claw."+content.uniqueID);
			Transform trButtonBuy = content.transform.Find("ButtonBuy");
			if  (PlayerPrefs.HasKey("claw."+content.uniqueID) == false && content.bFree == true )
			{
				PlayerPrefs.SetInt("claw."+content.uniqueID,1);
			}

			if ( PlayerPrefs.HasKey("claw."+content.uniqueID ) )
			{
				GameManager.SetNGUILabel(content.transform.Find("Label Price"),"");
			    GameManager.SetNGUILabel(trButtonBuy.transform.Find("Label"),"Equip");
			}
		}

//		for ( int i=0; i<containerShopCoins.transform.childCount ; i++ )
//		{
//			ShopContent content = containerShopCoins.transform.GetChild(i).gameObject.GetComponent<ShopContent>();
//			GameManager.SetNGUILabel(content.transform.Find("Label Price"),"");
//			Transform trButtonBuy = content.transform.Find("ButtonBuy");
//			if ( trButtonBuy )
//				trButtonBuy.gameObject.SetActive(false);
//
//		}
		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;  
		AndroidInAppPurchaseManager.ActionProductConsumed  += OnProductConsumed;

		//listening for store initilaizing finish
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
		AndroidInAppPurchaseManager.instance.loadStore();

		RefreshInfo ();



		PlayerPrefs.Save ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void OnSuccessfulPurchase(string SKU)
	{
		Gamestate gs = GameObject.Find("Gamestate").GetComponent<Gamestate>();
		if (gs) {
			//gs.achievementManager.OnAchievementEvent(AchievementType.ShopTime,1);
		}

		Transform trChild;
//		for ( int i=0; i<GUI_Shop.GetInstance().containerShopCoins.transform.childCount; i++ )
//		{
//			Transform trChild = GUI_Shop.GetInstance().containerShopCoins.transform.GetChild(i);
//			if ( trChild )
//			{
//				ShopContent content = trChild.gameObject.GetComponent<ShopContent>();
//				if ( content && content.type == ShopContentType.IAP )
//				{
//					if ( content.uniqueID == SKU )
//					{
//						gs.ShowDialogBox("Info","You bought "+content.Amount+" GemuGold",false,"",GUI_Shop.GetInstance().gameObject);
//						GameManager.GEMUCOINS += content.Amount;
//						PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,GameManager.GEMUCOINS);
//						RefreshInfo();
//						/*GameDataManager.instance.SendPlayResult(
//							GameDataManager.instance.gameID.ToString(),"0",
//							GameManager.GEMUCOINS.ToString(),
//							GameManager.EXP.ToString(),
//							GameManager.getLevelValue().ToString());*/
//						//GL1Connector.GetInstance().PlayResult(this.gameObject,GameManager.getLevelValue().ToString(),"0",GameManager.EXP.ToString(),GameManager.GEMUCOINS.ToString());
//						GL1Connector.GetInstance().AddBalance(this.gameObject,"",content.Amount.ToString(),"","BUYGOLD"+content.Amount.ToString());
//						if ( gs.GetComponent<Gamestate_Gameplay>() )
//						{
//							Gamestate_Gameplay gsPlay = gs.GetComponent<Gamestate_Gameplay>();
//							gsPlay.RefreshAllInfo();
//						}
//						break;
//					}
//				}
//			}
//		}

		for(int k=1;k<GUI_Shop.GetInstance().containerShopFreeCoins.transform.childCount-2;k++){
			trChild = GUI_Shop.GetInstance().containerShopFreeCoins.transform.GetChild(k);
			if ( trChild )
			{
				ShopContent content = trChild.gameObject.GetComponent<ShopContent>();
				if ( content && content.type == ShopContentType.IAP )
				{
					if ( content.uniqueID == SKU )
					{
						GameManager.FREECOINS += content.Amount;
						gs.ShowDialogBox("Info","You just bought "+content.Name,false,"confirm",this.gameObject);
						RefreshInfo();

						if(!PlayerPrefs.HasKey(PlayerPrefHandler.keyEnergyAch)){
							Social.ReportProgress(GPGSIds.achievement_the_spender, 100.0f, (bool success) => {
								// handle success or failure
								//gs.achievementManager.AddToFinalAchievementEvent();
							});
							PlayerPrefs.SetInt(PlayerPrefHandler.keyEnergyAch,1);
							PlayerPrefs.Save();
						}

						break;
					}
				}
			}
		}

		for(int i=1;i<GUI_Shop.GetInstance().containerShopJoystick.transform.childCount;i++){
			trChild = GUI_Shop.GetInstance().containerShopJoystick.transform.GetChild(i);
			if ( trChild )
			{
				ShopContent content = trChild.gameObject.GetComponent<ShopContent>();
				if ( content && content.type == ShopContentType.IAP )
				{
					if ( content.uniqueID == SKU )
					{
						gs.ShowDialogBox("Info","You just bought "+content.Name,false,"confirm",this.gameObject);
						Transform trButtonBuy = content.transform.Find("ButtonBuy");
						GameManager.SetNGUILabel(trButtonBuy.transform.Find("Label"),"Equip");

						PlayerPrefs.SetInt("joystick."+content.uniqueID,1);

						break;
					}
				}
			}
		}

		for(int j=1;j<GUI_Shop.GetInstance().containerShopClaw.transform.childCount;j++){
			trChild = GUI_Shop.GetInstance().containerShopClaw.transform.GetChild(j);
			if ( trChild )
			{
				ShopContent content = trChild.gameObject.GetComponent<ShopContent>();
				if ( content && content.type == ShopContentType.IAP )
				{
					if ( content.uniqueID == SKU )
					{
						gs.ShowDialogBox("Info","You just bought "+content.Name,false,"confirm",this.gameObject);
						Transform trButtonBuy = content.transform.Find("ButtonBuy");
						GameManager.SetNGUILabel(trButtonBuy.transform.Find("Label"),"Equip");

						
						PlayerPrefs.SetInt("claw."+content.uniqueID,1);
						
						break;
					}
				}
			}
		}

		PlayerPrefs.Save ();

		int playerClaw = 0, playerJoystick = 0;

		if (PlayerPrefs.HasKey ("claw.clawmania.claw2")) {
			if(PlayerPrefs.GetInt("claw.clawmania.claw2") == 1)
				playerClaw++;
		}
		if (PlayerPrefs.HasKey ("claw.clawmania.claw3")) {
			if(PlayerPrefs.GetInt("claw.clawmania.claw3") == 1)
				playerClaw++;
		}

		if (playerClaw >= 2) {
			Social.ReportProgress(GPGSIds.achievement_serious_grabber, 100.0f, (bool success) => {
				// handle success or failure
				//gs.achievementManager.AddToFinalAchievementEvent();
			});
		}

		if (PlayerPrefs.HasKey ("claw.clawmania.joystick2")) {
			if(PlayerPrefs.GetInt("claw.clawmania.joystick2") == 1)
				playerJoystick++;
		}
		if (PlayerPrefs.HasKey ("claw.clawmania.joystick3")) {
			if(PlayerPrefs.GetInt("claw.clawmania.joystick3") == 1)
				playerJoystick++;
		}
		if (playerJoystick >= 2) {
			Social.ReportProgress(GPGSIds.achievement_fancy_grabber, 100.0f, (bool success) => {
				// handle success or failure
				//gs.achievementManager.AddToFinalAchievementEvent();
			});
		}
	}

	void RefreshClawStatus()
	{
		for ( int i=0; i<containerShopClaw.transform.childCount; i++ )
		{
			Transform trChild = containerShopClaw.transform.GetChild(i);
			if ( trChild )
			{
				Transform trLabelEquipped = trChild.transform.Find("Label Equipped");
				if ( trLabelEquipped )
					trLabelEquipped.gameObject.SetActive(false);
				
				Transform trEquipButton = trChild.transform.Find("ButtonBuy");
				if ( trEquipButton )
					trEquipButton.gameObject.SetActive(true);
				
				Transform trLabelPrice = trChild.transform.Find("Label Price");
				if ( trLabelPrice )
					trLabelPrice.gameObject.SetActive(true);

				if(trEquipButton.transform.Find("Label").GetComponent<UILabel>().text == "Equip")
					trLabelPrice.gameObject.SetActive(false);

				if ( PlayerPrefs.HasKey("playerclaw") )
				{
					if ( PlayerPrefs.GetString("playerclaw").Equals(trChild.name) )
					{
						trLabelEquipped.gameObject.SetActive(true);
						trEquipButton.gameObject.SetActive(false);
						trLabelPrice.gameObject.SetActive(false);
					}
				}
				
			}
		}
	}

	void RefreshJoystickStatus()
	{
		for ( int i=0; i<containerShopJoystick.transform.childCount; i++ )
		{
			Transform trChild = containerShopJoystick.transform.GetChild(i);
			if ( trChild )
			{
				Transform trLabelEquipped = trChild.transform.Find("Label Equipped");
				if ( trLabelEquipped )
					trLabelEquipped.gameObject.SetActive(false);
				
				Transform trEquipButton = trChild.transform.Find("ButtonBuy");
				if ( trEquipButton )
					trEquipButton.gameObject.SetActive(true);
				
				Transform trLabelPrice = trChild.transform.Find("Label Price");
				if ( trLabelPrice )
					trLabelPrice.gameObject.SetActive(true);

				if(trEquipButton.transform.Find("Label").GetComponent<UILabel>().text == "Equip")
					trLabelPrice.gameObject.SetActive(false);

				string sJoystick = "";
				if (PlayerPrefs.HasKey ("playerjoy") == false)
					PlayerPrefs.SetString ("playerjoy", "ClawControl1");
				if (PlayerPrefs.HasKey ("playerjoy"))
					sJoystick = PlayerPrefs.GetString ("playerjoy");

				if ( sJoystick.Equals(trChild.name) )
				{
					trLabelEquipped.gameObject.SetActive(true);
					trEquipButton.gameObject.SetActive(false);
					trLabelPrice.gameObject.SetActive(false);
				} 
			}
		}
	}

	void ShowIconBackround(UIButton button, bool val)
	{
		Transform trBack = button.transform.Find ("Icon_Background");
		if ( trBack )
		{
			trBack.gameObject.SetActive(val);
		}
	}

	public void OnClickShopButton()
	{
		UIButton button = UIButton.current;
		if ( button == null )
			return;

		containerShopCoins.gameObject.SetActive (false);
		containerShopClaw.gameObject.SetActive (false);
		containerShopFreeCoins.gameObject.SetActive (false);
		containerShopMachine.gameObject.SetActive (false);
		containerShopJoystick.gameObject.SetActive (false);
		containerShopPowerups.gameObject.SetActive (false);
		ShowIconBackround (buttonShopCoins,false);
		ShowIconBackround (buttonShopClaw,false);
		ShowIconBackround (buttonShopJoystick,false);
		ShowIconBackround (buttonShopFreeCoins,false);
		ShowIconBackround (buttonShopPowerups,false);

		UIScrollView scrollView = null;
		if ( button == buttonShopCoins )
		{
			scrollView = containerShopCoins;
			ShowIconBackround (buttonShopCoins,true);
		}
		else if ( button == buttonShopClaw )
		{
			scrollView = containerShopClaw;
			ShowIconBackround (buttonShopClaw,true);
			RefreshClawStatus();
		}
		else if ( button == buttonShopJoystick )
		{
			scrollView = containerShopJoystick;
			ShowIconBackround (buttonShopJoystick,true);
			RefreshJoystickStatus();
		}
		else if ( button == buttonShopMachine )
		{
			scrollView = containerShopMachine;
		}
		else if ( button == buttonShopFreeCoins )
		{
			scrollView = containerShopFreeCoins;
			ShowIconBackround (buttonShopFreeCoins,true);
		}
		else if ( button == buttonShopPowerups )
		{
			scrollView = containerShopPowerups;
			ShowIconBackround (buttonShopPowerups,true);
		}

		if ( scrollView != null )
		{
			dragScroll.scrollView = scrollView;
			scrollView.gameObject.SetActive(true);
		}	
	}

	public void OnClickTab2()
	{
		UIButton button = UIButton.current;
		if ( button == null )
			return;

	}


	ShopContent currentShopContent;
	public void ProcessBuyPowerup()
	{
		powerUpAch = true;
		Gamestate_Gameplay gs = GameObject.Find("Gamestate").GetComponent<Gamestate_Gameplay>();
		if ( gs == null )
			return;

		int gemucoins = GameManager.GEMUCOINS;
		gemucoins -= (int)currentShopContent.Price;
		PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,gemucoins);
		GameManager.GEMUCOINS = gemucoins;

		int iAmount = 0;
		string sKey = currentShopContent.name+"_Amount";
		if ( PlayerPrefs.HasKey(sKey) )
		{
			iAmount = PlayerPrefs.GetInt(sKey);
		}
		
		Debug.LogError("[GUI_Shop] sKey="+sKey+" amount="+iAmount);
		iAmount += currentShopContent.Amount;
		PlayerPrefs.SetInt(sKey,iAmount);
		Debug.LogError("[GUI_Shop] sKey="+sKey+" amount="+iAmount);
		guiIngame.RefreshPowerUpsInfo();
		guiIngame.RefreshGemuCoinInfo();
		RefreshInfo();
		//gs.achievementManager.OnAchievementEvent(AchievementType.PowerPlay,1);

		gs.ShowDialogBox("Info","You just bought 1 "+currentShopContent.Name+" Power-Up",false,"confirmAch",null);
//		if ( PlayerPrefs.HasKey(sKey) )
//		{
//			iAmount = PlayerPrefs.GetInt(sKey);
//			Debug.LogError("[GUI_Shop] sKey="+sKey+" amount="+iAmount);
//		}
		
		GL1Connector.GetInstance().DecBalance(null,currentShopContent.Price.ToString (),"","CMPOWERUP"+currentShopContent.Price.ToString ());
	}

	public void BuyCoins()
	{
		AndroidInAppPurchaseManager.instance.purchase (currentShopContent.uniqueID);
	}

	public void BuyClaw()
	{
//		Gamestate_Gameplay gs = GameObject.Find("Gamestate").GetComponent<Gamestate_Gameplay>();
//		if ( gs == null )
//			return;
//		
//		int gemucoins = GameManager.GEMUCOINS;
//		gemucoins -= (int)currentShopContent.Price;
//		PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,gemucoins);
//		GameManager.GEMUCOINS = gemucoins;
//		
//		Transform trButtonBuy = currentShopContent.transform.Find("ButtonBuy");
//		GameManager.SetNGUILabel(trButtonBuy.transform.Find("Label"),"Equip");
//		
//		PlayerPrefs.SetInt("claw."+currentShopContent.uniqueID,1);
//		guiIngame.RefreshGemuCoinInfo();
//		
//		RefreshInfo();
//		
//		gs.ShowDialogBox("Info","You just bought "+currentShopContent.name,false,"confirm",this.gameObject);
//		
//		GL1Connector.GetInstance().DecBalance(this.gameObject,currentShopContent.Price.ToString (),"","CMCLAW"+currentShopContent.Price.ToString ());
	}

	public void BuyJoystick()
	{
		
//		Gamestate_Gameplay gs = GameObject.Find("Gamestate").GetComponent<Gamestate_Gameplay>();
//		if ( gs == null )
//			return;
		
//		int gemucoins = GameManager.GEMUCOINS;
//
//		gemucoins -= (int)currentShopContent.Price;
//		PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,gemucoins);
//		GameManager.GEMUCOINS = gemucoins;
		
//		Transform trButtonBuy = currentShopContent.transform.Find("ButtonBuy");
//		GameManager.SetNGUILabel(trButtonBuy.transform.Find("Label"),"Equip");
		
//		PlayerPrefs.SetInt("joystick."+currentShopContent.uniqueID,1);
//		guiIngame.RefreshGemuCoinInfo();
//		
//		RefreshInfo();
//		
//		gs.ShowDialogBox("Info","You just bought "+currentShopContent.name,false,"confirm",this.gameObject);
//		GL1Connector.GetInstance().DecBalance(this.gameObject,currentShopContent.Price.ToString (),"","CMJOY"+currentShopContent.Price.ToString ());
	}

	public void BuyEnergy()
	{
		Gamestate_Gameplay gs = GameObject.Find("Gamestate").GetComponent<Gamestate_Gameplay>();
		if ( gs == null )
			return;
		
		int gemucoins = GameManager.GEMUCOINS;
		GameManager.FREECOINS += currentShopContent.Amount;
		if ( GameManager.FREECOINS > GameManager.MAX_FREECOIN_FROM_TIMER )
			GameManager.FREECOINS = GameManager.MAX_FREECOIN_FROM_TIMER;
		PlayerPrefs.SetInt("freecoins",GameManager.FREECOINS);
		
		gemucoins -= (int)currentShopContent.Price;
		PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,gemucoins);
		GameManager.GEMUCOINS = gemucoins;
		
		guiIngame.RefreshFreeEnergyInfo();
		GL1Connector.GetInstance().DecBalance(this.gameObject,currentShopContent.Price.ToString (),"","CMENERGY"+currentShopContent.Amount.ToString ());
		
		RefreshInfo();
		
		gs.ShowDialogBox("Info","You just bought "+currentShopContent.Amount+" Energy",false,"confirm",this.gameObject);
	}

	public void OnClickFreeEnergy(){
		Gamestate_Gameplay gs = GameObject.Find ("Gamestate").GetComponent<Gamestate_Gameplay> ();
		if (gs == null)
			return;

		if (GameManager.FREECOINS >= 200) {
			gs.ShowDialogBox("Info","Your energy is full",false,"energyFull",this.gameObject);
		} else {

			if (TimerHandler.instance.FreeGiftAvailablilty ()) {


				Advertisement.Show (null, new ShowOptions{
			resultCallback = result =>{ 
				
				Debug.Log (result.ToString());
				
				switch(result){
				case(ShowResult.Failed):
					gs.ShowDialogBox("Info","Ads Failed for some reason",false,"confirm",this.gameObject);
					break;
					
				case(ShowResult.Skipped):
					gs.ShowDialogBox("Info","Ads Skipped. No energy added",false,"confirm",this.gameObject);
					break;
					
				case(ShowResult.Finished):
					

					watchCount++;
					
					if(watchCount>=5){
						watchCount=0;
						System.TimeSpan FreeGiftDuration = TimeSpan.FromMinutes(20);
						System.DateTime FreeGiftDurationEnd = System.DateTime.Now.Add(FreeGiftDuration);
						
						PlayerPrefs.SetString(GamePreferences.Key_FreeGiftTimer,FreeGiftDurationEnd.ToBinary().ToString());

						gs.ShowDialogBox("Info","You got 20 energy",false,"confirm",this.gameObject);
						
						PlayerPrefs.SetString(GamePreferences.Key_FreeGiftTimer,FreeGiftDurationEnd.ToBinary().ToString());
						PlayerPrefs.Save();
					}
					else{
						gs.ShowDialogBox("Info","You got 20 energy",false,"FreeEnergy",this.gameObject);

					}

					GameManager.FREECOINS += 20;
					guiIngame.RefreshFreeEnergyInfo();
					guiIngame.RefreshGemuCoinInfo();
					RefreshInfo();


					//start the timer
					//System.TimeSpan FreeGiftDuration = TimerHandler.instance.FreeGiftExponentialTimer();


					break;
				}
			}
			});
			} else {
				showTimerBox ();
			}
		}
	}

	public void showTimerBox(){
		System.TimeSpan Dur = TimerHandler.instance.DurationLeft_FreeGift;

		Debug.Log ("Duration:" + Dur);

		Gamestate_Gameplay gs = GameObject.Find("Gamestate").GetComponent<Gamestate_Gameplay>();
		if ( gs == null )
			return;

		gs.ShowDialogBox("No Energy",setTimerString(Dur),false,"",this.gameObject);
	}

	private string setTimerString(System.TimeSpan duration){
		string temp;
		int hour = 0,min=0;
		
		//configure duration in hour / min
		min = duration.Minutes;
		temp = "free energy in ";
		if(duration.Hours < 1){}//nothing
		else {
			hour = duration.Hours;
			temp += hour.ToString() +" h ";
		}
		temp += min.ToString()+" m";
		
		return temp;
	}

	public void OnClickShopContent()
	{
		UIButton button = UIButton.current;
		if ( button == null )
			return;
		
		Gamestate_Gameplay gs = GameObject.Find("Gamestate").GetComponent<Gamestate_Gameplay>();
		if ( gs == null )
			return;

		ShopContent content = button.transform.parent.gameObject.GetComponent<ShopContent> ();
		if ( content.type == ShopContentType.IAP )
		{
			if ( content.uniqueID == "freecoins" )
			{
				GUI_Dialog.InsertStack(gemuFreeCoins.gameObject);
				//if ( GameDataManager.GetTimerToWatchAds() > GameDataManager.TIMER_TO_WATCH_ADS )
				//{
					//if (GameDataManager.PlayUnityVideoAd ()) {
					//	StartCoroutine(ShowingAds());
					//} else {
					//	gs.ShowDialogBox ("Info", "Please wait...", false, "", this.gameObject);
					//}
				//}
				//else
				//	gs.ShowDialogBox("Info","Please wait to watch the next ads.",false,"",this.gameObject);

			}
			else if(PlayerPrefs.HasKey("joystick."+content.uniqueID) == true){
				PlayerPrefs.SetString("playerjoy",content.uniqueID);
				gs.SetJoystick(content.uniqueID);
				RefreshJoystickStatus();
			}
			else if(PlayerPrefs.HasKey("claw."+content.uniqueID) == true ){
				PlayerPrefs.SetString("playerclaw",content.uniqueID);
				gs.SetClaw(content.uniqueID);
				RefreshClawStatus();
			}
			else
			{
				currentShopContent = content;
				AndroidInAppPurchaseManager.instance.purchase (currentShopContent.uniqueID);
				//OnSuccessfulPurchase(currentShopContent.uniqueID);
			}
		}
		else if ( content.type == ShopContentType.PowerUp )
		{
			int gemucoins = GameManager.GEMUCOINS;
			if ( gemucoins - content.Price >= 0 )
			{
				currentShopContent = content;
				gs.ShowDialogBox(content.Name,content.Desc+"\n\nDo you want to buy ?",true,"buypowerup",this.gameObject);
				//gs.achievementManager.OnAchievementEvent(AchievementType.PowerPlay,1);
			}
			else
			{
				gs.ShowDialogBox("Info","Not enough coins",false,"confirm",this.gameObject);
			}
		}
		else if ( content.type == ShopContentType.Claw )
		{
			if ( PlayerPrefs.HasKey("claw."+content.uniqueID) == false )
			{
				int gemucoins = GameManager.GEMUCOINS;
				if ( gemucoins-content.Price >= 0 )
				{
					//currentShopContent = content;
					//gs.ShowDialogBox(content.Name,content.Desc+"Do you want to buy ?",true,"buyclaw",this.gameObject);
					PlayerPrefs.SetString("playerclaw",content.uniqueID);
					gs.SetClaw(content.uniqueID);
					RefreshClawStatus();
				}
				else
				{
					//gs.ShowDialogBox("Info","Not enough GemuGold",false,"confirm",this.gameObject);
				}
			}
			else
			{
				PlayerPrefs.SetString("playerclaw",content.uniqueID);
				gs.SetClaw(content.uniqueID);
				RefreshClawStatus();
			}
		}
		else if ( content.type == ShopContentType.Joystick )
		{
			if ( PlayerPrefs.HasKey("joystick."+content.uniqueID) == false )
			{
				int gemucoins = GameManager.GEMUCOINS;
				if ( gemucoins-content.Price >= 0 )
				{
					//currentShopContent = content;
					//gs.ShowDialogBox(content.Name,content.Desc+"Do you want to buy ?",true,"buyjoystick",this.gameObject);
					PlayerPrefs.SetString("playerjoy",content.uniqueID);
					gs.SetJoystick(content.uniqueID);
					RefreshJoystickStatus();
				}
				else
				{
					//gs.ShowDialogBox("Info","Not enough GemuGold",false,"confirm",this.gameObject);
				}
			}
			else
			{
				PlayerPrefs.SetString("playerjoy",content.uniqueID);
				gs.SetJoystick(content.uniqueID);
				RefreshJoystickStatus();
			}
		}
		else if ( content.type == ShopContentType.FreeCoin )
		{
			int gemucoins = GameManager.GEMUCOINS;
			if ( gemucoins-content.Price >= 0 )
			{
				currentShopContent = content;
				gs.ShowDialogBox(content.Name,"Do you want to buy ?",true,"buyenergy",this.gameObject);
			}
			else
			{
				gs.ShowDialogBox("Info","Not enough GemuGold",false,"confirm",this.gameObject);
			}
		}
		else
		{
			gs.ShowNotImplemented();
		}

		PlayerPrefs.Save ();
	}
	
	private static void OnProductPurchased(BillingResult result) 
	{
		if(result.isSuccess) 
		{
			AndroidInAppPurchaseManager.instance.consume (result.purchase.SKU);

		} 
		else 
		{
			// product has been purchased, but not consumed yet
			if ( result.response == 7 )
			{
				AndroidInAppPurchaseManager.instance.consume (result.purchase.SKU);

			}
		}
		
		Debug.Log ("Purchased Responce: " + result.response.ToString() + " " + result.message);
	}
	
	private static void OnProductConsumed(BillingResult result)
	{
		if(result.isSuccess) 
		{
			GUI_Shop.GetInstance().OnSuccessfulPurchase(result.purchase.SKU);

		} 
		else 
		{
		}
		Debug.LogError ("OnProductConsumed "+result.response.ToString());
	}



	private static void OnBillingConnected(BillingResult result) {
		AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;
		
		
		if(result.isSuccess) {
			AndroidInAppPurchaseManager.instance.retrieveProducDetails();
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;
		} 

		Debug.Log ("Connection Responce: " + result.response.ToString() + " " + result.message);
	}

	private static void OnRetrieveProductsFinised(BillingResult result) {
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;
		
		
		if(result.isSuccess) {

			foreach(GoogleProductTemplate tpl in AndroidInAppPurchaseManager.instance.inventory.products) 
			{
				for ( int i=0; i<GUI_Shop.GetInstance().containerShopCoins.transform.childCount ; i++ )
				{
					ShopContent content = GUI_Shop.GetInstance().containerShopCoins.transform.GetChild(i).gameObject.GetComponent<ShopContent>();
					if ( tpl.SKU.Equals(content.uniqueID) )
					{
						GameManager.SetNGUILabel(content.transform.Find("Label Price"),tpl.price);
						Transform trButtonBuy = content.transform.Find("ButtonBuy");
						if ( trButtonBuy )
							trButtonBuy.gameObject.SetActive(true);
						break;
					}
				}
			}
		} else {
			//AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
		}
		
		Debug.Log ("Connection Response: " + result.response.ToString() + " " + result.message);
		
	}
}
