using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GemuUserDataController : GUI_Dialog {
	public GemuDialogBoxController dialogBox;
	public UILabel textFullname;
	public UILabel textMail;
	public UILabel textUsername;
	public UILabel textCoin;
	public UILabel textTicket;
	public UILabel textReferal;
	public GemuPromoCode cvPromo;
	public UIScrollView uiScrollView;
	public GameObject progress;
	public override void OnShow()
	{
		bLogout = false;
		progress.gameObject.SetActive (true);
		uiScrollView.ResetPosition ();
		textFullname.text = "";
		textMail.text = "";
		textUsername.text = "";
		textCoin.text = "";
		textTicket.text = "";
		textReferal.text = "";

		uiScrollView.gameObject.SetActive (false);
		uiScrollView.gameObject.SetActive (true);
		uiScrollView.ResetPosition ();
		Hashtable data = new Hashtable();

		if ( !string.IsNullOrEmpty(PlayerPrefs.GetString(PlayerPrefHandler.keyToken) ) )
		{
			data.Add("username", PlayerPrefs.GetString(PlayerPrefHandler.keyUserName));
			data.Add("token", PlayerPrefs.GetString(PlayerPrefHandler.keyToken));
			
			try
			{
				GemuAPI.GetUser(data);
			}
			catch(GemuAPI_Exception exc)
			{
				Debug.LogError(exc.Message);
			}
		}
	}

	// Use this for initialization
	public override void OnStart () 
	{
		GemuAPI.OnGetUserResponse += OnGetUserResponse;
	}
	
	void OnDestroy()
	{
		GemuAPI.OnLoginResponse -= OnGetUserResponse;
	}

	void OnGetUserResponse(Restifizer.RestifizerResponse response)
	{
		if (PlayerPrefs.GetString (PlayerPrefHandler.keyUserName) == "")
			return;

		if ( progress.gameObject )
			progress.gameObject.SetActive (false);
		Hashtable data = response.Resource;

		Debug.LogError ("Done");
		if ( data["errcode"].ToString() == "0")
		{
			Hashtable userdata = (Hashtable)data["userdata"];
			string sFullname = userdata["full_name"].ToString();
			string sEmail = userdata["email"].ToString();
			string sCoin = userdata["coin"].ToString();
			string sTiket = userdata["tiket"].ToString();
			string sReferal = userdata["refcode"].ToString();

			textFullname.text = sFullname;
			textMail.text = sEmail;
			textCoin.text = sCoin;
			textTicket.text = sTiket;
			textReferal.text = sReferal;
			textUsername.text = PlayerPrefs.GetString(PlayerPrefHandler.keyUserName);
			PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,(int)float.Parse(sCoin));
			PlayerPrefs.SetInt(PlayerPrefHandler.keyUserTiket,(int)float.Parse(sTiket));
		}
		else
		{	
			dialogBox.Show("Error",data["errdetail"].ToString(),false,"",this.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void OnPromoButton()
	{
		
		GUI_Dialog.InsertStack(cvPromo.gameObject);
	}

	public void OnBackButton()
	{
		SoundManager.instance.PlayButton();
		
		GUI_Dialog.ReleaseTopCanvas();
	}

	void SetValueToDefault()
	{
		Debug.LogError ("SetValueToDefault");
		PlayerPrefs.SetString (PlayerPrefHandler.keyUserName, "");
		PlayerPrefs.SetString (PlayerPrefHandler.keyToken, "");
		PlayerPrefs.SetInt (PlayerPrefHandler.keyUserTiket, 0);
		PlayerPrefs.SetInt (PlayerPrefHandler.keyCoin, 5);
		CoinTimerHandler.instance.countCoin = 5;
	}

	bool bLogout = false;
	public void OnLogout()
	{
		bLogout = true;
		SoundManager.instance.PlayButton ();
		SetValueToDefault ();
		OnBackButton ();
	}

}
