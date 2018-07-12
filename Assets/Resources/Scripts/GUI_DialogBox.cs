using UnityEngine;
using System.Collections;
using UnityEngine.Advertisements;

public class DialogBoxQueue
{
	public string sTitle;
	public string sText;
	public bool bShowCancel;
	public string sExec;
	public GameObject sender;
}

[System.Serializable]
public class GUI_DialogBox : GUI_Dialog 
{
	public AudioClip audioError;
	public AudioClip audioNormal;
	public UILabel labelTitle;
	public UILabel labelText;
	public UIButton buttonOK;
	public UIButton buttonCancel;
	public UIButton buttonAds;
	public UIButton buttonShare;
	public UIButton buttonInvite;
	public UIButton buttonAddFund;
	string sExec;
	ArrayList queues;
	// Use this for initialization
	
	public override void OnStart () {
		queues = new ArrayList ();
	}

	public void OnClickCancel() {
		if (sExec == "update") {
			Application.Quit();
			Debug.Log ("quit app");
		}
		else
			GUI_DialogBox.ReleaseTopCanvas ();
	}

	public void OnClickAdsButton(){
		GUI_Shop.GetInstance().OnClickFreeEnergy();
		GUI_Dialog.ReleaseTopCanvas();
	}

	public void OnClickAddFund(){
		GameManager.GEMUCOINS += 200;
		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag("Gamestate").GetComponent<Gamestate_Gameplay>();
		gs.RefreshAllInfo();
		GUI_Dialog.ReleaseTopCanvas();
	}

	public void PlayAudio(AudioClip clip)
	{
		if ( this.GetComponent<AudioSource>() == null )
		{
			this.gameObject.AddComponent<AudioSource>();
		}
		
		if ( GameManager.bSoundOn == false )
			return;
		this.GetComponent<AudioSource>().clip = clip;
		this.GetComponent<AudioSource>().Play ();
	}

	public void OnClickOK()
	{
		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag("Gamestate").GetComponent<Gamestate_Gameplay>();
		if (sExec == "gotomainmenu") {
			if (gs)
				gs.GoToMainMenu ();
		} else if (sExec == "logoutfb") {
			Debug.LogError ("fb logout");
			//SPFacebook.instance.Logout();

			if (lastSender)
				lastSender.SendMessage ("OnFBLogout");

		} else if (sExec == "redeemreward") {
			if (lastSender)
				lastSender.SendMessage ("OnConfirmRedeem");
		} else if (sExec == "buyclaw") {
			if (gs)
				gs.SendMissionEvent (Mission_Event.ShopBuyNewClaw);
			if (lastSender)
				lastSender.SendMessage ("BuyClaw");
		} else if (sExec == "buypowerup") {
			//ReleaseTopCanvas();
			lastSender.SendMessage ("ProcessBuyPowerup");

		} else if (sExec == "buycoins") {
			lastSender.SendMessage ("BuyCoins");
		} else if (sExec == "buyjoystick") {
			lastSender.SendMessage ("BuyJoystick");
		} else if (sExec == "buyclaw") {
			lastSender.SendMessage ("BuyClaw");
		} else if (sExec == "buyenergy") {
			lastSender.SendMessage ("BuyEnergy");
		} else if (sExec == "confirmAch") { 
			//ReleaseTopCanvas();
			if (gs == null)
				return;
			//gs.achievementManager.OnAchievementEvent(AchievementType.PowerPlay,1);
		} else if (sExec == "checkver0" || sExec == "checkver3") {
			Application.Quit ();
		} else if (sExec == "checkver2") {
			Debug.Log ("update");
			Application.OpenURL ("https://play.google.com/store/apps/details?id=com.GameLevelOne.ClawMania");
			Application.Quit ();
		} else if (sExec == "exitgame") {
			if (GameManager.separateAPKs) {
				Application.Quit ();
			} else {
				if (gs)
					gs.BackToGemu ();
			}
		} else if (sExec == "levelup") {
			buttonShare.gameObject.SetActive (false);
			buttonInvite.gameObject.SetActive (false);
		} else if (sExec == "update") {
			Application.OpenURL ("https://play.google.com/store/apps/details?id=com.GameLevelOne.ClawMania");
			Application.Quit ();
		} 
//		else if (sExec == "FreeEnergy") {
//			GUI_Shop.GetInstance().OnClickFreeEnergy();
//		}

		sExec = "";
		lastSender = null;
		GUI_Dialog.ReleaseTopCanvas();
	}
	
	public void ClearQueue()
	{
		queues = new ArrayList ();
	}



	void ProcessQueue()
	{
		Debug.LogError("ProcessQueue count="+queues.Count);
		if ( queues != null && queues.Count > 0 )
		{
			DialogBoxQueue queue = (DialogBoxQueue)queues[0];
			queues.RemoveAt(0);

			labelText.text = queue.sText;
			labelText.gameObject.SetActive (false);
			labelText.gameObject.SetActive (true);
			labelTitle.text = queue.sTitle;
			lastSender = queue.sender;
			this.sExec = queue.sExec;
			//Debug.LogError("showcancel="+queue.bShowCancel);
			
			GUI_Dialog.InsertStack(this.gameObject);
			buttonCancel.gameObject.SetActive(queue.bShowCancel);
			//Show ();
		}
	}

	GameObject lastSender;
	public void Show (string sTitle, string sText, bool bShowCancel, string sExec, GameObject sender)
	{
		DialogBoxQueue queue = new DialogBoxQueue ();
		queue.sTitle = sTitle;
		queue.sText = sText;
		queue.bShowCancel = bShowCancel;
		queue.sExec = sExec;
		queue.sender = sender;

		queues.Add (queue);
		if (!isVisible ())
			ProcessQueue ();

		if (sExec == "energyFull") {
			buttonAds.gameObject.SetActive (false);
			buttonOK.gameObject.SetActive (true);
			buttonShare.gameObject.SetActive (false);
			buttonInvite.gameObject.SetActive (false);
			buttonAddFund.gameObject.SetActive (false);
		}

		else if (sExec == "confirmAch") {
			buttonAds.gameObject.SetActive (false);
			buttonOK.gameObject.SetActive (true);
			buttonShare.gameObject.SetActive (false);
			buttonInvite.gameObject.SetActive (false);
			buttonAddFund.gameObject.SetActive (false);
		}

		else if (sExec != "levelup" && sExec != "confirm" && sExec != "confirmAch" && sExec != "FreeEnergy" && sExec != "AddFund") {
			buttonShare.gameObject.SetActive (false);
			buttonInvite.gameObject.SetActive (false);
			buttonAds.gameObject.SetActive (false);
			buttonAddFund.gameObject.SetActive (false);
			Debug.Log("?");
		}

		else if (sExec == "AddFund") {
			buttonAddFund.gameObject.SetActive(true);
			buttonAds.gameObject.SetActive (false);
			buttonOK.gameObject.SetActive (false);
			buttonShare.gameObject.SetActive (false);
			buttonInvite.gameObject.SetActive (false);
			Debug.Log("addfund");
		}

		else if (sExec == "FreeEnergy") {
			buttonAds.gameObject.SetActive (true);
			buttonOK.gameObject.SetActive (true);
			buttonShare.gameObject.SetActive (false);
			buttonInvite.gameObject.SetActive (false);
			buttonAddFund.gameObject.SetActive(false);
		} 

		else {
			buttonAds.gameObject.SetActive (false);
			buttonOK.gameObject.SetActive (true);
			buttonAddFund.gameObject.SetActive(false);
		}
		
	}
	
	public override void OnTweenDone()
	{
		ProcessQueue ();
	}

}
