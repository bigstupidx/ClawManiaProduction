using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GemuRegisterGemuController : GUI_Dialog {
	
	public GemuDialogBoxController dialogBox;
	public UIInput username;
	public UIInput mail;
	public UIInput yourname;
	public UIInput password;
	public UIInput retypepassword;
	public UIInput referal;
	public UIScrollView scrollView;
	public GameObject progress;
	public GameObject buttonBack;
	// Use this for initialization
	public override void OnStart () {

		GemuAPI.OnRegisterResponse += OnRegisterResponse;
	}


	void OnDestroy()
	{
		GemuAPI.OnRegisterResponse -= OnRegisterResponse;
	}

	// Update is called once per frame
	void Update () {
	
	}


	public override void OnShow()
	{
		username.value = "";
		mail.value = "";
		yourname.value = "";
		password.value = "";
		retypepassword.value = "";
		referal.value = "";
		scrollView.transform.gameObject.SetActive (false);
		scrollView.transform.gameObject.SetActive (true);
		scrollView.ResetPosition ();
		ActivateColliders (this.Window, true);
		progress.gameObject.SetActive (false);
		/*
		for ( int i=0; i<scrollView.transform.childCount; i++ )
		{
			scrollView.transform.GetChild(i).gameObject.SetActive(false);
			scrollView.transform.GetChild(i).gameObject.SetActive(true);
		}
		*/
	}

	public void OnClickRegister()
	{
		SoundManager.instance.PlayButton();
		string sError = "";
		if ( 	username.value == "" 
		    || 	mail.value == "" 
		    || yourname.value == "" 
		    || 	password.value == "" 
		    || 	retypepassword.value == "" )
		    
		{
			sError = "Fill in all the required data.";
		}
		else if ( username.value.Contains(" ") )
		{
			sError = "Username may not contain spaces.";
		}
		else if ( mail.value.Contains("@") == false 
		         || mail.value.Contains(".") == false 
		         )
		{
			sError = "Email Address is not in the correct format.";
		}
		else if ( password.value.Equals(retypepassword.text) == false
		         )
		{
			sError = "Retype your password.";
		}

		if ( !string.IsNullOrEmpty(sError) )
		{
			dialogBox.Show("Error",sError,false,"",this.gameObject);
		}
		else
		{
			ActivateColliders(this.Window,false);
			progress.gameObject.SetActive (true);
			buttonBack.gameObject.GetComponent<Collider>().enabled = true;
			Hashtable data = new Hashtable();
			data.Add("full_name", yourname.value);
			data.Add("email", mail.value);
			data.Add("username", username.value);
			data.Add("password", retypepassword.value);
			data.Add("tiket", "0");
			data.Add("ref_code", referal.value);
			
			try
			{
				GemuAPI.Register(data);
			}
			catch(GemuAPI_Exception exc)
			{
				Debug.LogError(exc.Message);
			}

		}
	}

	void OnRegisterResponse(Restifizer.RestifizerResponse response)
	{
		Hashtable data = response.Resource;
		
		if ( data["errcode"].ToString() == "0")
		{
			dialogBox.Show("Info","You have been registered. Please Verify your email before login.",false,"",this.gameObject);
			GUI_Dialog.ReleaseTopCanvas();
		}
		else
		{
			dialogBox.Show("Error",data["errdetail"].ToString(),false,"",this.gameObject);
		}
	}

	public void OnClickBack()
	{
		SoundManager.instance.PlayButton();
		GUI_Dialog.ReleaseTopCanvas ();
	}
}
