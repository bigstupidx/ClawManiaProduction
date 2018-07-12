using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class GemuRewardVerifyController : GUI_Dialog {
	
	public GemuDialogBoxController dialogBox;
	//private RewardData data = null;
	public string sKodeReward;
	public UILabel textTickets = null;
	public GameObject progress;
	public UIButton buttonOk;
	void Awake()
	{
		gameObject.SetActive(false);
	}

	public override void OnShow()
	{
		buttonOk.gameObject.GetComponent<Collider>().enabled = true;
		progress.gameObject.SetActive (false);
	}

	// Use this for initialization
	public override void OnStart () {
		
		GemuAPI.OnRedeemResponse += OnRedeemResponse;
	}
	
	void OnDestroy()
	{
		GemuAPI.OnRedeemResponse -= OnRedeemResponse;
	}

	void OnRedeemResponse(Restifizer.RestifizerResponse response)
	{
		
		Debug.LogError ("OnRedeemResponse");
		Hashtable data = response.Resource;
		
		if ( data["errcode"].ToString() == "0")
		{
			OnButtonNo();
			//GameDataManager.instance.LoadData ();
			dialogBox.Show("Info","Redeem success. Please check your email.",false,"",this.gameObject);
		}
		else
		{
			dialogBox.Show("Error",data["errdetail"].ToString(),false,"",this.gameObject);
		}
		buttonOk.gameObject.GetComponent<Collider>().enabled = true;
		progress.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		
	}
	
	void OnBecameVisible()
	{
		//if (data != null)
		//	textTickets.text = data.ticketCost.ToString();
		
	}
	
	public void SetData(RewardData data)
	{
		//this.data = data;
		
		//if (this.data != null)
		//	textTickets.text = this.data.ticketCost.ToString();
	}
	
	public void OnButtonYes()
	{
		
		buttonOk.gameObject.GetComponent<Collider>().enabled = false;
		progress.gameObject.SetActive (true);

		SoundManager.instance.PlayButton();

		Hashtable data = new Hashtable();
		data.Add("username", PlayerPrefs.GetString(PlayerPrefHandler.keyUserName));
		data.Add("token", PlayerPrefs.GetString(PlayerPrefHandler.keyToken));
		data.Add("rewardid", sKodeReward);
		
		try
		{
			GemuAPI.Redeem(data);
		}
		catch(GemuAPI_Exception exc)
		{
			Debug.LogError("error "+exc);
			OnButtonNo();
		}
	}
	
	public void OnButtonNo()
	{
		SoundManager.instance.PlayButton();
		GUI_Dialog.ReleaseTopCanvas();
	}
}
