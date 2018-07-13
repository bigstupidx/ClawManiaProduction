using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class sc_fbholder : MonoBehaviour {
	#region Facebook
	// public UITexture UIFBAvatar;
	// public UILabel UIFBUsername;
	// public Texture2D UIFBGuest;

	// private Dictionary<string, string> profile = null;
	// public string fb_fullname = "";

	// // Use this for initialization
	// void Awake () {
	// 	if ( FB.IsInitialized == false )
	// 		FB.Init (SetInit, OnHideUnity);
	// }

	// void Start() {
	// 	loadGambarGuest ();
	// }

	// public void loadGambarGuest() {
	// 	//Image userAvatar = UIFBAvatar.GetComponent<Image>();
	// 	//userAvatar.sprite = Sprite.Create(UIFBGuest, new Rect (0, 0, 250, 250), new Vector2 (0, 0));
	// }
	
	// private void SetInit() {
	// 	Debug.Log("FB Init Done");
	// 	if (FB.IsLoggedIn) {
	// 		Debug.Log ("FB Logged in");
	// 		//FBLogin();
	// 	} else {

	// 	}
	// }

	// private void OnHideUnity(bool isGameShown) {
	// 	if (!isGameShown) {
	// 		Time.timeScale = 0;
	// 	} else {
	// 		Time.timeScale = 1;
	// 	}
	// }

	// public void FBLogin() {
	// 	int statFB = PlayerPrefs.GetInt ("facebook_stat");
	// 	Debug.Log("FB button Clicked " + statFB);

	// 	if (statFB == 0) {
	// 		FB.Login ("user_about_me, user_birthday", AuthCallBack);
	// 	} else {
	// 		Debug.Log("Facebook Logout Clicked");
	// 		ubahNamaUser("Guest");
	// 		ubahProfileUser("guest");
	// 		loadGambarGuest();

	// 		UIFBUsername.text = "Guest";

	// 		FB.Logout();
	// 	}
	// }

	// void AuthCallBack(FBResult result)
	// {
	// 	Debug.Log ("FB Login Result : " + result.Text + " | Error : " + result.Error);
	// 	//Text txtResultfb = GameObject.Find ("txtData").GetComponent<Text> ();
	// 	//txtResultfb.text = "Error : " + result.Error + " | Result : " + result.Text;

	// 	if (FB.IsLoggedIn) {
	// 		Debug.Log ("FB Login Worked");
	// 		Debug.Log("========>>> accessToken : " + FB.AccessToken);

	// 		FB.API(Util.GetPictureURL("me",128,128),Facebook.HttpMethod.GET, pasangFotoProfile);
	// 		FB.API ("/me?fields=id,first_name,last_name", Facebook.HttpMethod.GET, pasangNamaProfile);
	// 	} else {
	// 		Debug.Log ("FB Login Failed"); 
	// 	}
	// }

	// public void GetFotoAndName()
	// {
	// 	if (FB.IsInitialized == false)
	// 		return;
	// 	if (FB.IsLoggedIn == false)
	// 		return;
	// 	FB.API(Util.GetPictureURL("me",128,128),Facebook.HttpMethod.GET, pasangFotoProfile);
	// 	FB.API ("/me?fields=id,first_name,last_name", Facebook.HttpMethod.GET, pasangNamaProfile);
	// }

	// void pasangFotoProfile(FBResult hasil) {
	// 	Debug.Log ("Fb Resut : "+ hasil.Text);
	// 	if (hasil.Error != null) {
	// 		Debug.Log("ada masalah saat load picture profile");
	// 		FB.API(Util.GetPictureURL("me",128,128),Facebook.HttpMethod.GET, pasangFotoProfile);
	// 		return;
	// 	}

	// 	if (UIFBAvatar) {
	// 		UIFBAvatar.mainTexture = hasil.Texture;
	// 	}
	// }

	// void pasangNamaProfile(FBResult hasil) {
	// 	if (hasil.Error != null) {
	// 		Debug.Log("ada masalah saat load picture profile");
	// 		FB.API ("/me?fields=id,first_name", Facebook.HttpMethod.GET, pasangNamaProfile);
	// 		return;
	// 	}
	// 	Debug.Log ("Fb Resut >:> "+ hasil.Text);

	// 	profile = Util.DeserializeJSONProfile (hasil.Text);

	// 	SimpleJSON.JSONNode rsl = SimpleJSON.JSON.Parse(hasil.Text);
	// 	Debug.Log ("Facebook ID : " + rsl[0].Value);
	// 	PlayerPrefs.SetString (PlayerPrefHandler.keyUserName, rsl[0].Value);

	// 	fb_fullname = profile ["first_name"] + " " + rsl [2].Value;
	// 	Debug.Log ("1.Namaaaaaaaaaaaa userrrrrrr :::" + fb_fullname);
	// 	//GameObject.Find ("cont_signup").SendMessage ("registerByFacebook_proses",rsl[0].Value);
	// 	ubahNamaUser(profile["first_name"]);
	// 	ubahProfileUser("facebook");

	// 	PlayerPrefs.Save();
	// }


	// public void ubahNamaUser(string namaUser) {
	// 	/*
	// 	if (namaUser == "guest") {
	// 		loadGambarGuest ();
	// 		PlayerPrefs.SetString (PlayerPrefHandler.keyLoginActive, "guest");
	// 		PlayerPrefs.Save();
	// 	}
	// 	Debug.Log ("2.Namaaaaaaaaaaaa userrrrrrr :::" + fb_fullname);
	// 	*/
	// 	//ubah Nama
	// 	UIFBUsername.text = namaUser;
	// }

	// public void ubahProfileUser(string statLogin) {
	// 	//PlayerPrefs.SetString (PlayerPrefHandler.keyLoginActive, statLogin);
	// 	//PlayerPrefs.Save();
	// }

	// public void reLogin_fb() {
	// 	if(FB.IsInitialized ) {
	// 		FB.Login ("user_about_me, user_birthday", AuthCallBack);
	// 	}
	// }
	#endregion
}
