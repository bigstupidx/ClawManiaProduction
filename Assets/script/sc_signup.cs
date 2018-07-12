using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class sc_signup : MonoBehaviour
{

	public AudioClip suaraClick;
	GemuRegisterGemuController cvsSignup;
	GUI_BasketSetting cvsSetting;
	GemuLoginGemuController cvsLogin;

	int jumlahCoinAwalDaftar=5;

	void Start() {
		cvsSignup = sc_mainmenu_canvas_handler.Instance().cvSignup;
		cvsSetting = sc_mainmenu_canvas_handler.Instance().cvSetting;
		cvsLogin = sc_mainmenu_canvas_handler.Instance().cvLogin;

	}

	//Register Gemu --------------------------------------------------------------------------
	public void registerGemu_click() {
		Debug.Log("ini dari sc_signup...................!!!!!!!!");
		GetComponent<AudioSource>().PlayOneShot (suaraClick);

		InputField uname = GameObject.Find("signup_username").GetComponent<InputField>();
		InputField pass1 = GameObject.Find("signup_pass").GetComponent<InputField>();
		InputField pass2 = GameObject.Find("signup_pass2").GetComponent<InputField>();
		InputField email = GameObject.Find("signup_email").GetComponent<InputField>();
		Text result = GameObject.Find("txtProgressSignup").GetComponent<Text>();
		
		if (uname.text == "" || pass1.text == "" || pass2.text == "" || email.text == "") {
			result.text = "All Field Must Be Filled";
		} else if (pass1.text != pass2.text) {
			result.text = "Password is not match";
		} else {
			result.text = "Registering, please wait...";
		}
	}

	IEnumerator close_popup_signup(float waitTime) {
		yield return new WaitForSeconds(waitTime);
		reset_register();
		cvsSignup.enabled = false;
	}

	void reset_register() {
		InputField uname = GameObject.Find("signup_username").GetComponent<InputField>();
		InputField pass1 = GameObject.Find("signup_pass").GetComponent<InputField>();
		InputField pass2 = GameObject.Find("signup_pass2").GetComponent<InputField>();
		InputField email = GameObject.Find("signup_email").GetComponent<InputField>();
		Text result = GameObject.Find("txtProgressSignup").GetComponent<Text>();
		uname.text = "";
		pass1.text = "";
		pass2.text = "";
		email.text = "";
		result.text = "";
	}



	//Register By Facebook --------------------------------------------------------------------------
	public void registerByFacebook_proses(string idFB) {
		Debug.Log ("Facebook ID Get : " + idFB);
	}

	//public void update_asset_coins(string userid) {
	//	string statLogin = "guest";
	//	statLogin = PlayerPrefs.GetString (PlayerPrefHandler.keyLoginActive);
	//}

}
