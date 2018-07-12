using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;

public class GUI_Shop_New : MonoBehaviour {
	public static GUI_Shop_New instance;

	public Text txt_Coin;
	public Text txt_energy;
	public Text txt_level;

	public Image expProgressBar;

	public Button tabEnergy;
	public Button tabClaw;
	public Button tabJoystick;
	public Button tabPowerups;

	public GameObject contentEnergy;
	public GameObject contentClaw;
	public GameObject contentJoystick;
	public GameObject contentPowerups;

	public GameObject[] tabBG = new GameObject[4];
	public GameObject[] tabContents = new GameObject[4];

	//for joystick and claw indexing, all starts from the 2nd object
	public GameObject[] clawPriceLabel = new GameObject[2];
	public GameObject[] joystickPriceLabel = new GameObject[2]; 
	public GameObject[] clawBuyButton = new GameObject[2];
	public GameObject[] joystickBuyButton = new GameObject[2];

	private ShopContent currShopContent;

	private int powerupPrice = 100;
	private int currPowerupIdx = 0;

	private string buyString = "BUY";
	private string equipString = "EQUIP";
	private string equippedString = "EQUIPPED";

	private string[] powerupName = new string[]{"Bomb","Lucky Charm","Laser Pointer","Wizard Wand","Black Hole"};
	private string[] clawPrice = new string[]{"Rp 12.000","Rp 12.000"};
	private string[] joystickPrice = new string[]{"Rp 12.000","Rp 12.000"};

	void Awake(){
		instance=this;
		//PlayerPrefs.DeleteAll ();
//		if (Advertisement.isSupported) {
//			Advertisement.Initialize ("29466");
//		} else {
//			Debug.Log("Platform not supported");
//		}
	}

	public void setTabDisplay (int idx)
	{
		for (int i = 0; i < 4; i++) {
			if (i == idx) {
				tabBG [i].SetActive (true);
				tabContents[i].SetActive(true);
			} else {
				tabBG [i].SetActive (false);
				tabContents[i].SetActive(false);
			}
		}
	}

	public void RefreshInfo(){
		int iAmountGemuCoin = 0;
		if ( PlayerPrefs.HasKey(PlayerPrefHandler.keyCoin) )
			iAmountGemuCoin = PlayerPrefs.GetInt(PlayerPrefHandler.keyCoin);
		txt_Coin.text = iAmountGemuCoin.ToString ();

		int iAmountExp = 0;
		if (PlayerPrefs.HasKey ("exp"))
			iAmountExp = PlayerPrefs.GetInt ("exp");

		txt_energy.text = GameManager.FREECOINS + "/200";
		txt_level.text = GameManager.getLevelValue ().ToString ();
		//Debug.LogError ("currlv=" + GameManager.getLevelValue ());
		if ( GameManager.getLevelValue() < 99 )
		{
			float fProgress =  GameManager.getProgressValue();
			//Debug.LogError("progress="+fProgress);
			expProgressBar.fillAmount = fProgress;
		}
		else
		{
			expProgressBar.fillAmount = 1;
		}
	}

	public void OnClickFreeEnergy(){
		int watchCount=0;

		Gamestate_Gameplay gs = GameObject.Find ("Gamestate").GetComponent<Gamestate_Gameplay> ();
		if (gs == null)
			return;

		if (GameManager.FREECOINS >= 200) {
			//gs.ShowDialogBox("Info","Your energy is full",false,"energyFull",this.gameObject);
			Debug.Log("energy full");
		} else {

			if (TimerHandler.instance.FreeGiftAvailablilty ()) {

				Advertisement.Show (null, new ShowOptions{
			resultCallback = result =>{ 
				
				Debug.Log (result.ToString());
				
				switch(result){
				case(ShowResult.Failed):
					//gs.ShowDialogBox("Info","Ads Failed for some reason",false,"confirm",this.gameObject);
					Debug.Log("ads failed");
					break;
					
				case(ShowResult.Skipped):
					//gs.ShowDialogBox("Info","Ads Skipped. No energy added",false,"confirm",this.gameObject);
					Debug.Log("ads skipped");
					break;
					
				case(ShowResult.Finished):
					

					watchCount++;
					
					if(watchCount>=5){
						watchCount=0;
						System.TimeSpan FreeGiftDuration = TimeSpan.FromMinutes(20);
						System.DateTime FreeGiftDurationEnd = System.DateTime.Now.Add(FreeGiftDuration);
						
						PlayerPrefs.SetString(GamePreferences.Key_FreeGiftTimer,FreeGiftDurationEnd.ToBinary().ToString());

						//gs.ShowDialogBox("Info","You got 20 energy",false,"confirm",this.gameObject);
						Debug.Log("watched last ads");
						
						PlayerPrefs.SetString(GamePreferences.Key_FreeGiftTimer,FreeGiftDurationEnd.ToBinary().ToString());
						PlayerPrefs.Save();
					}
					else{
						//gs.ShowDialogBox("Info","You got 20 energy",false,"FreeEnergy",this.gameObject);
						Debug.Log("got 20 energy");
					}

					GameManager.FREECOINS += 20;
//					guiIngame.RefreshFreeEnergyInfo();
//					guiIngame.RefreshGemuCoinInfo();
					RefreshInfo();

					//start the timer
//					System.TimeSpan FreeGiftDuration = TimerHandler.instance.FreeGiftExponentialTimer();

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

		//gs.ShowDialogBox("No Energy",setTimerString(Dur),false,"",this.gameObject);
		Debug.Log("no energy");
	}

	public void buyPowerup (int idx){ //put on powerup buy button
		currPowerupIdx = idx;
	}

	public void ProcessBuyPowerup (){
		int currCoins = GameManager.GEMUCOINS;
		currCoins -= powerupPrice;
		PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,currCoins);
		GameManager.GEMUCOINS = currCoins;

		int powerupAmount = 0;
		string sKey = powerupName[currPowerupIdx]+"_Amount";
		powerupAmount = PlayerPrefs.GetInt(sKey,0);
		powerupAmount++;
		PlayerPrefs.SetInt(sKey,powerupAmount);

		GUI_InGame_New.instance.refreshInGameCoinInfo();
		GUI_InGame_New.instance.refreshInGamePowerUpInfo(currShopContent);
		RefreshInfo();

		//dialog box
	}

	public void refreshClawStatus (ShopContent content){

		int idx = 0;

		switch (content.Name) {
			case "The Red Claw":
				idx=0;
			break;
			case "The Metallic Claw":
				idx=1;
			break;
		}

		clawPriceLabel[idx].SetActive(true);
		string btnText = clawBuyButton [idx].transform.GetChild (0).GetComponent<Text> ().text;

		if (btnText == buyString) {
			btnText = equipString;
		} else if(btnText == equipString){
			btnText=equippedString;
			clawPriceLabel[idx].SetActive(false);
		}
	}

	public void refreshJoystickStatus(ShopContent content){

		int idx = 0;

		switch (content.Name) {
			case "The Mecha":
				idx=0;
			break;
			case "The Round Table":
				idx=1;
			break;
		}

		joystickPriceLabel[idx].SetActive(true);
		string btnText = joystickBuyButton [idx].transform.GetChild (0).GetComponent<Text> ().text;

		if (btnText == buyString) {
			btnText = equipString;
		} else if(btnText == equipString){
			btnText=equippedString;
			joystickPriceLabel[idx].SetActive(false);
		}
	}

	public void onClickBuyButton (GameObject button)
	{
		Gamestate_Gameplay gs = GameObject.Find ("Gamestate").GetComponent<Gamestate_Gameplay> ();
		if (gs == null)
			return;

		ShopContent content = button.GetComponent<ShopContent> ();

		if (content.type == ShopContentType.IAP) {
			if (PlayerPrefs.HasKey ("joystick." + content.uniqueID) == true) {
				PlayerPrefs.SetString ("playerjoy", content.uniqueID);
				gs.SetJoystick (content.uniqueID);
				refreshJoystickStatus (content);
			} else if (PlayerPrefs.HasKey ("claw." + content.uniqueID) == true) {
				PlayerPrefs.SetString ("playerclaw", content.uniqueID);
				gs.SetClaw (content.uniqueID);
				refreshClawStatus (content);
			} else {
				currShopContent = content;
				AndroidInAppPurchaseManager.instance.purchase (currShopContent.uniqueID);
				#if UNITY_EDITOR
				Debug.Log("purchased "+currShopContent.Name);
				#endif
			}
		} else if (content.type == ShopContentType.PowerUp) {
			int currCoins = GameManager.GEMUCOINS;

			if (currCoins - content.Price >= 0) {
				currShopContent = content;
				Debug.Log("purchased "+currShopContent.Name);
				//show dialog box buy confirmation

			} else {
				//show dialog box not enough coins
				Debug.Log("not enough coins");
			}
		}

		PlayerPrefs.Save();
	}
}
