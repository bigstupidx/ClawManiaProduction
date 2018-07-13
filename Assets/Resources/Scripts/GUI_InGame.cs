using UnityEngine;
using System.Collections;

public class GUI_InGame : MonoBehaviour 
{
	public static GUI_InGame instance;

	public GUI_Shop guiShop;
	public UILabel labelAmountBomb;
	public UILabel labelAmountCharm;
	public UILabel labelAmountLaser;
	public UILabel labelCoins;
	public UILabel labelExp;
	public UIProgressBar progressExp;
	public UILabel labelTicket;
	public UILabel labelTimerCoin;
	public UILabel labelGemuCoin;
	public UILabel labelLevel;
	public GUI_GL1_Reward gl1Reward;

	
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

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		//labelAmountBomb.text = "0";
		//labelAmountCharm.text = "0";
		//labelAmountLaser.text = "0";
		//labelAmountLaser.text = "0";
		//labelAmountLaser.text = "0";
	}

	public void OnClickMission()
	{
		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay>();
		if ( gs )
		{
			gs.ShowNotImplemented();
		}
	}

	
	public void RefreshLabelTimer(string sText)
	{
		GameManager.SetNGUILabel (labelTimerCoin.transform, sText );
	}
	
	public void RefreshGemuCoinInfo()
	{
		//Debug.LogError ("RefreshGemuCoinInfo GemuCoin="+GameManager.GEMUCOINS.ToString());
		GameManager.SetNGUILabel (labelGemuCoin.transform, GameManager.GEMUCOINS.ToString ());
		Debug.Log("gemucoin = " + GameManager.GEMUCOINS.ToString ());
	}

	public void RefreshExpInfo()
	{
		GameManager.SetNGUILabel (labelExp.transform, GameManager.EXP.ToString ());
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
	
	public void RefreshTicketInfo()
	{
		GameManager.SetNGUILabel (labelTicket.transform, GameManager.TICKET.ToString ());
	}
	
	public void RefreshLevelInfo()
	{
		GameManager.SetNGUILabel (labelLevel.transform, GameManager.getLevelValue().ToString ());
	}

	public void RefreshFreeEnergyInfo()
	{
		//Debug.LogError ("energy=" + GameManager.FREECOINS.ToString ());
		GameManager.SetNGUILabel (labelCoins.transform, GameManager.FREECOINS.ToString ());
		Debug.Log("energy = " + GameManager.FREECOINS.ToString ());
	}

	public void RefreshPowerUpsInfo()
	{
		Transform trPowerUpsContainer = transform.Find ("PowerUps");
		for ( int i=0; i<trPowerUpsContainer.childCount; i++ )
		{
			Transform trPowerUp = trPowerUpsContainer.GetChild(i);
			ButtonPowerUp buttonPowerUp = trPowerUp.GetComponent<ButtonPowerUp>();
			if ( buttonPowerUp )
			{
				string sKey = buttonPowerUp.type.ToString()+"_Amount";
				//PlayerPrefs.DeleteKey(sKey);
				//if ( PlayerPrefs.HasKey(sKey) == false )
				//	PlayerPrefs.SetInt(sKey,5);
				int iAmount = 0;
				if ( PlayerPrefs.HasKey(sKey) )
				{
					iAmount = PlayerPrefs.GetInt(sKey);
					//Debug.LogError("[GUI_InGame] sKey="+sKey+" amount="+iAmount);
					Transform trAmount = buttonPowerUp.transform.Find("Amount");
					if ( trAmount )
						GameManager.SetNGUILabel(trAmount,iAmount.ToString());
				}
				else
				{
					GameManager.SetNGUILabel(buttonPowerUp.transform.Find("Amount"),"0");
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickShop()
	{
		GUI_Dialog.InsertStack(guiShop.gameObject);
		GUI_Shop.GetInstance ().RefreshInfo ();
	}
	
	public void OnClickGrab()
	{
		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay>();
		if ( gs )
		{
			gs.GrabObject();
		}
	}
}
