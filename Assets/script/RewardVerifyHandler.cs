using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RewardVerifyHandler : MonoBehaviour {

	private RewardData data = null;
	public Text textTickets = null;

	// Use this for initialization
	void Start () {
		GemuAPI.OnRedeemResponse += OnRedeemResponse;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDestroy () {
		GemuAPI.OnRedeemResponse -= OnRedeemResponse;
	}

	void OnBecameVisible()
	{
		if (data != null)
			textTickets.text = data.ticketCost.ToString();

	}

	void OnRedeemResponse(Restifizer.RestifizerResponse response)
	{
		Hashtable data = response.Resource;

		Debug.LogError ("Redeem response");
		if ( data["errcode"].ToString() == "0" )
		{
			sc_mainmenu_canvas_handler.Instance().ShowDialogBox("Redeem Success",false);
			//GameDataManager.instance.LoadData();
		}
		else
			sc_mainmenu_canvas_handler.Instance().ShowDialogBox(data["errdetail"].ToString(),false);

	}


	public void SetData(RewardData data)
	{
		this.data = data;

		if (this.data != null)
			textTickets.text = this.data.ticketCost.ToString();
	}

	public void OnButtonYes()
	{
		GUI_Dialog.ReleaseTopCanvas();


		Hashtable data = new Hashtable();
		data.Add("username", PlayerPrefs.GetString(PlayerPrefHandler.keyUserName));
		data.Add("token", PlayerPrefs.GetString(PlayerPrefHandler.keyToken));
		data.Add("rewardid", this.data.code.ToString());
		
		GemuAPI.OnRedeemResponse += OnRedeemResponse;
		try
		{
			Debug.LogError("Redeem reward");
			GemuAPI.Redeem(data);
		}
		catch(GemuAPI_Exception exc)
		{
			OnButtonNo();
		}
		this.gameObject.SetActive(false);
	}

	public void OnButtonNo()
	{
		if ( GemuMainMenuController.instance )
			GUI_Dialog.ReleaseTopCanvas();
		this.gameObject.SetActive(false);
	}

}
