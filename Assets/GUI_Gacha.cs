using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GUI_Gacha : GUI_Dialog {
	public static GUI_Gacha instance;
	public UIButton buttonGachaActive;
	public UIButton buttonGachaInactive;
	public GUI_InGame guiIngame;

	List<string> gachaList = new List<string>();

	private int currCoin;

	void OnEnable(){
		instance = this;
		currCoin = GameManager.GEMUCOINS; // ingamecurrency
		
		Debug.Log (currCoin);
		
		if (currCoin >= 100) {
			buttonGachaActive.gameObject.SetActive(true);
			buttonGachaInactive.gameObject.SetActive(false);
		}
		else{
			buttonGachaActive.gameObject.SetActive(false);
			buttonGachaInactive.gameObject.SetActive(true);
		}
	}

	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
		this.gameObject.SetActive (false);
	}

	public void OnClickSpin(){
		currCoin = GameManager.GEMUCOINS;
		currCoin -= 100;
		GameManager.GEMUCOINS = currCoin;
		PlayerPrefs.SetInt (PlayerPrefHandler.keyCoin, currCoin);

		GUI_InGame.instance.RefreshGemuCoinInfo ();
		Debug.Log("spin gacha");

		int itemWon = 0,contentChild=0;
		Transform trChild;

		Gamestate_Gameplay gs = GameObject.Find("Gamestate").GetComponent<Gamestate_Gameplay>();
		if ( gs == null )
			return;

		//play animation

		if(Random.value>=0 && Random.value<=0.1)
			itemWon = Random.Range (0, 4);
		else
			itemWon = Random.Range (4, gachaList.Count);

		gs.ShowDialogBox ("Info", "You got " + gachaList [itemWon], false, "", this.gameObject);

		switch (itemWon) {
		case 0:
			contentChild=1;
			trChild = GUI_Shop.GetInstance().containerShopJoystick.transform.GetChild(contentChild);
			setShopItem(trChild,"joystick");
			break;
		case 1:
			contentChild=2;
			trChild = GUI_Shop.GetInstance().containerShopJoystick.transform.GetChild(contentChild);
			setShopItem(trChild,"joystick");
			break;
		case 2:
			contentChild=1;
			trChild = GUI_Shop.GetInstance().containerShopClaw.transform.GetChild(contentChild);
			setShopItem(trChild,"claw");
			break;
		case 3:
			contentChild=2;
			trChild = GUI_Shop.GetInstance().containerShopClaw.transform.GetChild(contentChild);
			setShopItem(trChild,"claw");
			break;
		case 4:
			contentChild=0;
			trChild = GUI_Shop.GetInstance().containerShopPowerups.transform.GetChild(contentChild);
			setShopItem(trChild,"powerup");
			break;
		case 5:
			contentChild=1;
			trChild = GUI_Shop.GetInstance().containerShopPowerups.transform.GetChild(contentChild);
			setShopItem(trChild,"powerup");
			break;
		case 6:
			contentChild=2;
			trChild = GUI_Shop.GetInstance().containerShopPowerups.transform.GetChild(contentChild);
			setShopItem(trChild,"powerup");
			break;
		case 7:
			contentChild=3;
			trChild = GUI_Shop.GetInstance().containerShopPowerups.transform.GetChild(contentChild);
			setShopItem(trChild,"powerup");
			break;
		case 8:
			contentChild=4;
			trChild = GUI_Shop.GetInstance().containerShopPowerups.transform.GetChild(contentChild);
			setShopItem(trChild,"powerup");
			break;
		}

		if (currCoin < 100) {
			buttonGachaActive.gameObject.SetActive (false);
			buttonGachaInactive.gameObject.SetActive (true);
		} else {
			buttonGachaActive.gameObject.SetActive(true);
			buttonGachaInactive.gameObject.SetActive(false);
		}
	}

	private void setShopItem(Transform trChild,string a){
		if (trChild) {
			ShopContent content = trChild.gameObject.GetComponent<ShopContent>();

			if(a == "powerup"){
				int iAmount = 0;
				string sKey = content.name+"_Amount";
				if ( PlayerPrefs.HasKey(sKey) )
				{
					iAmount = PlayerPrefs.GetInt(sKey);
				}
				iAmount += content.Amount;
				PlayerPrefs.SetInt(sKey,iAmount);
				guiIngame.RefreshPowerUpsInfo();
			}
			else{
				Transform trButtonBuy = content.transform.Find("ButtonBuy");
				GameManager.SetNGUILabel(trButtonBuy.transform.Find("Label"),"Equip");
				PlayerPrefs.SetInt(a+"."+content.uniqueID,1);
			}
		}
		PlayerPrefs.Save ();
	}

	// Use this for initialization
	void Start () {
		//joystick
		gachaList.Add ("The Mecha");
		gachaList.Add ("The Round Table");

		//claw
		gachaList.Add ("The Red Claw");
		gachaList.Add ("The Metallic Claw");

		//powerups
		gachaList.Add ("Bomb");
		gachaList.Add ("Lucky Charm");
		gachaList.Add ("Laser Pointer");
		gachaList.Add ("Wizard Wand");
		gachaList.Add ("Black Hole");
	}

}
