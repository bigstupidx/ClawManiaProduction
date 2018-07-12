using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class GemuLoginGemuController : GUI_Dialog {

	public UIInput username;
	public UIInput password;
	public GemuRegisterGemuController RegisterGemu;
	public GemuDialogBoxController dialogBox;
	public GameObject obProgress;
	public UIButton buttonBack;
	void Awake()
	{
		GemuAPI.OnLoginResponse += OnLoginResponse;
	}

	void OnDestroy()
	{
		GemuAPI.OnLoginResponse -= OnLoginResponse;
	}

	public override void OnShow()
	{
		username.value = "";
		password.value = "";
		obProgress.gameObject.SetActive (false);
		ActivateColliders (this.Window, true);
	}
	// Update is called once per frame
	void Update () {
	
	}

	public void OnRegisterButton()
	{
		SoundManager.instance.PlayButton();
		GUI_Dialog.InsertStack(RegisterGemu.gameObject);
	}

	public void OnLoginButton() 
	{
		SoundManager.instance.PlayButton();

		if (string.IsNullOrEmpty (username.value)) {
			dialogBox.Show ("Info", "Username is empty", false, "", this.gameObject);
		} else if (string.IsNullOrEmpty (password.value)) {
			dialogBox.Show ("Info", "Password is empty", false, "", this.gameObject);
		} else {
			obProgress.gameObject.SetActive (true);
			ActivateColliders (this.Window, false);
			buttonBack.gameObject.GetComponent<Collider> ().enabled = true;
			Hashtable data = new Hashtable ();
			data.Add ("username", username.value);
			data.Add ("password", password.value);
			data.Add ("gameid", GameDataManager.instance.gameID.ToString ());
			data.Add ("ip", GameDataManager.instance.deviceIP);

			try {
				GemuAPI.Login (data);
			} catch (GemuAPI_Exception exc) {
				Debug.LogError (exc.Message);
			}
		}
	}

	public void OnBackButton()
	{
		SoundManager.instance.PlayButton();

		GUI_Dialog.ReleaseTopCanvas();
	}

	void OnLoginResponse(Restifizer.RestifizerResponse response)
	{
		Hashtable data = response.Resource;
		if ( data["errcode"].ToString() == "0")
		{
			Hashtable logindata = (Hashtable)data["logindata"];
			string sUsername = logindata["username"].ToString();
			string sToken = logindata["token"].ToString();

			PlayerPrefs.SetString(PlayerPrefHandler.keyUserName,sUsername);
			PlayerPrefs.SetString(PlayerPrefHandler.keyToken,sToken);
			OnBackButton ();
			//if ( GemuDialogBoxController.GetInstance() )
				dialogBox.Show("Info","Welcome back, "+sUsername+".",false,"",this.gameObject);
			//GameDataManager.instance.LoadData();
		}
		else
		{
			dialogBox.Show("Error",data["errdetail"].ToString(),false,"",this.gameObject);
		}
	}
}
