using UnityEngine;
using System.Collections;
using SimpleJSON;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GUI_Share : GUI_Dialog 
{
	#region FACEBOOK
	// public WWW_Texture textureFacebookUser;
	// public UILabel labelUsername;
	// public Texture textureNoUser;
	// //private static string TWITTER_CONSUMER_KEY = "wEvDyAUr2QabVAsWPDiGwg";
	// //private static string TWITTER_CONSUMER_SECRET = "igRxZbOrkLQPNLSvibNC3mdNJ5tOlVOPH3HNNKDY0";

	// public GUI_Gl1_Login guiGL1_Login;
	// public GUI_GL1_UserData guiGL1_UserData;
	// public GUI_GL1_Reward guiGL1_Reward;
	// public GUI_GL1_Promo guiGL1_Promo;
	// bool bIsTwitterAuntifivated = false;
	// public UIButton buttonFacebook;
	// public UIButton buttonFBShare;
	// public UIButton buttonFBInvite;
	// public UIButton buttonEmail;

	// public UIButton buttonGemuReward;
	// public UIButton buttonGemuPromo;
	// //public UIButton buttonTwitter;
	// // Use this for initialization

	// public void OpenRedeemURL(){
	// 	Application.OpenURL ("https://www.gemugemu.com/"+"redeem.htm?token=" + PlayerPrefs.GetString(PlayerPrefHandler.keyToken) + "&email=" + PlayerPrefs.GetString(PlayerPrefHandler.keyUserName));
	// }

	// public void OnClickBack()
	// {
	// 	GUI_Dialog.ReleaseTopCanvas ();
	// }

	// public void OnClickRedeemInsideUserData(){

	// 	GUI_Dialog.ReleaseTopCanvas ();
	// 	GUI_Dialog.InsertStack(guiGL1_Reward.gameObject);
	// }

	// public override void OnStart () 
	// {
	// 	SPFacebook.instance.addEventListener(FacebookEvents.AUTHENTICATION_SUCCEEDED,  	 OnFBLoggedIn);
	// 	//SPTwitter.instance.addEventListener(TwitterEvents.AUTHENTICATION_SUCCEEDED,  OnTwitterLoggedIn);
		
	// 	if ( FB.IsInitialized == false )
	// 		SPFacebook.instance.Init();
	// 	//SPTwitter.instance.Init(TWITTER_CONSUMER_KEY, TWITTER_CONSUMER_SECRET);
	// }
	
	// // Update is called once per frame
	// void Update () {
	
	// }

	// public void OnClickGemuButtonPromo()
	// {
	// 	//guiGL1_Promo.Show ();
		
	// 	GUI_Dialog.InsertStack(guiGL1_Promo.gameObject);
	// }

	// void SetIcon(UIButton button,string sIcon)
	// {
		
	// 	Transform trIcon = button.transform.Find ("Icon");
	// 	if ( trIcon )
	// 	{
	// 		UISprite sprite = trIcon.gameObject.GetComponent<UISprite>();
	// 		sprite.spriteName = sIcon;
	// 	}
	// }

	// public void OnFBLoggedIn()
	// {
	// 	Debug.Log ("OnFBLoggedIn");
		
	// 	buttonFBShare.gameObject.SetActive (true);
	// 	buttonFBInvite.gameObject.SetActive (true);

	// 	SetIcon (buttonFacebook, "check_icon");
	// 	buttonFacebook.GetComponent<Collider>().enabled = true;
	// 	buttonFacebook.gameObject.SetActive (false);
	// 	buttonFacebook.gameObject.SetActive (true);
	// 	//SPFacebook.instance.OnUserDataRequestCompleteAction += OnUserDataLoaded;
	// 	//SPFacebook.instance.LoadUserData ();
	// 	//textureFacebookUser.StartDownloadImage("https" + "://graph.facebook.com/" + FB.UserId + "/picture?type=large");
	// }
	
	// private void OnUserDataLoaded(FBResult result) 
	// {
	// 	SPFacebook.instance.OnUserDataRequestCompleteAction -= OnUserDataLoaded;

	// 	if (result.Error == null)  
	// 	{ 
	// 		labelUsername.text = SPFacebook.instance.userInfo.first_name;
	// 		/*
	// 		SA_StatusBar.text = "User data loaded";
	// 		IsUserInfoLoaded = true;
			
	// 		//user data available, we can get info using
	// 		//SPFacebook.instance.userInfo getter
	// 		//and we can also use userInfo methods, for exmple download user avatar image
	// 		SPFacebook.instance.userInfo.LoadProfileImage(FacebookProfileImageSize.square);
	// 		*/
			
	// 	} 
	// 	else 
	// 	{
	// 		/*
	// 		SA_StatusBar.text ="Opps, user data load failed, something was wrong";
	// 		Debug.Log("Opps, user data load failed, something was wrong");
	// 		*/
	// 	}

	// }

	// public void OnClickGemuReward()
	// {
	// 	StartCoroutine (WaitingToShowReward ());
	// }

	// IEnumerator WaitingToShowReward()
	// {
	// 	yield return new WaitForSeconds (1);
	// 	//guiGL1_Reward.Show ();
		
	// 	GUI_Dialog.InsertStack(guiGL1_Reward.gameObject);
	// }
	// public void OnClickGemuPromo()
	// {
	// 	//guiGL1_Promo.Show ();
		
	// 	GUI_Dialog.InsertStack(guiGL1_Promo.gameObject);
	// }

	// public void OnClickShareFacebook()
	// {
	// 	/*SPFacebook.instance.PostWithAuthCheck (
	// 		link: "http://gemugemu.com",
	// 		linkName: "GemuGemu",
	// 		linkCaption: "Play for free and be rewarded. It's magic.\t",
	// 		linkDescription: "",
	// 		picture: "http://ajipamungkas.com/pub/Gemu_Icon_256.png"
	// 		);*/
	// 	FBShare ();
	// }

	// public void OnClickInviteFacebook()
	// {
	// 	//string title = "Hello";
	// 	//string message = "Play with me";
		
	// 	//SPFacebook.instance.SendInvite(title, message);
	// 	//SPFacebook.instance.OnAppRequestCompleteAction += OnAppRequestCompleteAction;
	// 	FBInvite ();
	// }

	// void OnAppRequestCompleteAction (FBAppRequestResult result) {
		
	// 	if(result.IsSucceeded) {
	// 		foreach(string UserId in result.Recipients) {
	// 			Debug.Log(UserId);
	// 		}
			
	// 		Debug.Log("Original Facebook Responce: " + result.Result.Text);
	// 	} else {
	// 		Debug.Log("App request has failed");
	// 	}
		
		
	// 	SPFacebook.instance.OnAppRequestCompleteAction -= OnAppRequestCompleteAction;
		
		
	// }

	// public void OnTwitterLoggedIn()
	// {
	// 	bIsTwitterAuntifivated = true;
	// 	Debug.Log ("OnTwitterLoggedIn");

	// 	/*
	// 	SetIcon (buttonTwitter, "check_icon");
	// 	buttonTwitter.collider.enabled = true;
	// 	buttonTwitter.gameObject.SetActive (false);
	// 	buttonTwitter.gameObject.SetActive (true);
	// 	*/
	// }

	// public void OnGemuGemuLoggedIn()
	// {

	// 	buttonGemuReward.gameObject.SetActive (true);
	// 	//buttonGemuPromo.gameObject.SetActive (true);
	// 	SetIcon (buttonEmail, "check_icon");
	// 	buttonEmail.GetComponent<Collider>().enabled = true;
	// 	buttonEmail.gameObject.SetActive (false);
	// 	buttonEmail.gameObject.SetActive (true);
	// 	Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay>();
	// 	if ( gs )
	// 	{
	// 		gs.RefreshAllInfo();
	// 	}
	// 	if (FB.IsLoggedIn) {
	// 		print ("asd");
	// 		GameObject.Find ("GUI_Share").GetComponent<GUI_Share> ().OnFBLoggedIn ();
	// 	}
	// }

	// public void OnClickFB()
	// {
	// 	if (string.IsNullOrEmpty (PlayerPrefs.GetString (PlayerPrefHandler.keyToken)) || string.IsNullOrEmpty (PlayerPrefs.GetString (PlayerPrefHandler.keyUserName))) {
	// 		if (!FB.IsLoggedIn) {
	// 			//SPFacebook.instance.Login ("email,publish_actions");
	// 			FB.Login("email,publish_actions",LoginCallback);
	// 			//FacebookWrapper.GetInstance().LoginFB();
	// 		} else {
	// 			Gamestate gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate> ();
	// 			if (gs) {
	// 				gs.ShowDialogBox ("Confirmation", "Are you sure you want to logout from Facebook ?", true, "logoutfb", this.gameObject);
	// 			}
	// 		}
	// 	} else {
	// 		GUI_Dialog.InsertStack(guiGL1_UserData.gameObject);
	// 	}
	// }

	// public void FBShare(){
	// 	/*FB.Feed (
	// 		linkCaption: "I'm playing TerraWorms!",
	// 		linkName: "Check out this game",
	// 		link: "https://www.gemugemu.com/",
	// 		callback:ShareCallback
	// 	);*/
		
	// 	FB.Feed ("", "https://www.gemugemu.com/", "Play for free and be rewarded. It's magic.", "from ClawMania", "Check out this game!", "", "", "", "", "", null, ShareCallback);
	// 	//ShareToFacebook ("http://www.gemugemu.com/","I'm playing TerraWorms!","New High Score!","lorem ipsum","",
	// 	//"http://www.facebook.com/");
		
	// }
	
	// private void ShareCallback(FBResult result){
	// 	if (result.Error != null) {
	// 		//logTxt.text="Error response: " + result.Error;
			
	// 	} 
	// 	if (result == null || result.Error != null) {
	// 		//cancelled
	// 	}
	// 	else {
	// 		//logTxt.text="share success";
	// 		//GL1Connector.GetInstance().AddBalance(this.gameObject,"5","0","","SHARE");
	// 		//GL1Connector.GetInstance().AddGameBalance(this.gameObject,"5","","SHARE");
	// 		Gamestate_Gameplay gs = GameObject.Find ("Gamestate").GetComponent<Gamestate_Gameplay> ();
	// 		if (gs == null)
	// 			return;

	// 		Social.ReportProgress(GPGSIds.achievement_share_our_game, 100.0f, (bool success) => {
	// 			// handle success or failure
	// 			//gs.achievementManager.AddToFinalAchievementEvent();
	// 		});
	// 	}
	// }
	
	// public void FBInvite(){
	// 	/*FB.AppRequest (
	// 		message:"Join me",
	// 		title:"Invite your friends to join",
	// 		callback:InviteCallback
	// 	);*/
		
	// 	FB.AppRequest ("Join me", null, null, null, null, "", "Invite your friends to join!", InviteCallback);
	// }
	
	// private void InviteCallback(FBResult result){
	// 	if (result.Error != null) {
	// 		//logTxt.text="Error response: " + result.Error;
			
	// 	} 
	// 	if (result == null || result.Error != null) {
	// 		//cancelled
	// 	}
	// 	else {
	// 		//logTxt.text="invite success";
	// 		//GL1Connector.GetInstance().AddBalance(this.gameObject,"5","0","","INVITE");
	// 		//GL1Connector.GetInstance().AddGameBalance(this.gameObject,"5","","INVITE");
	// 	}
	// 	//Debug.Log("asd");
	// }

	// private void LoginCallback(FBResult result){
	// 	if (FB.IsLoggedIn) {
	// 		Debug.Log ("FB Login worked");
	// 		FB.API("/me?fields=email", Facebook.HttpMethod.GET, LoginCallback2);
	// 	}
	// 	else{
	// 		Debug.Log ("FB Login failed");
	// 	}
	// }

	// private void LoginCallback2(FBResult result){
	// 	if (result.Error != null) {
	// 		Debug.Log ("Error response: " + result.Error);
	// 	} else if (!FB.IsLoggedIn) {
	// 		Debug.Log ("Login cancelled");
	// 	} else {
	// 		IDictionary dict = Facebook.MiniJSON.Json.Deserialize(result.Text) as IDictionary;
	// 		//Debug.Log (dict["email"]);
	// 		string userEmail=dict["email"].ToString();
	// 		//GL1Connector.GetInstance().LoginViaFB(userEmail);
	// 	}
	// }

	// public void OnClickTwitter()
	// {
	// 	/*
	// 	Debug.LogError ("OnClickTwitter bIsTwitterAuntifivated="+bIsTwitterAuntifivated);
	// 	if ( bIsTwitterAuntifivated == false )
	// 		SPTwitter.instance.AuthenticateUser();
	// 	else
	// 	{
	// 		bIsTwitterAuntifivated = false;
	// 		OnTwitterLogout();
	// 	}
	// 	*/
	// 	//Gamestate_Menu gs = GameObject.FindGameObjectWithTag("Gamestate").GetComponent<Gamestate_Menu>();
	// 	//if ( gs )
	// 	//{
	// 	//	gs.ShowNotImplemented();
	// 	//}
	// }

	// public void OnGL1Done(JSONNode N)
	// {
	// 	Debug.LogError ("OnGL1Done");
	// }

	// public void OnClickGameLevelOne()
	// {
	// 	Debug.Log("onclickGL1");
	// 	if (string.IsNullOrEmpty (GL1Connector.GetInstance ().GetToken ())) {
	// 		//guiGL1_Login.Show ();
	// 		Debug.Log("token empty");
	// 		GUI_Dialog.InsertStack(guiGL1_Login.gameObject);
	// 	} else {
	// 		//guiGL1_UserData.Show ();
	// 		Debug.Log ("has token");
	// 		GUI_Dialog.InsertStack(guiGL1_UserData.gameObject);
	// 	}
	// }

	// public void OnFBLogout()
	// {
	// 	SetIcon (buttonFacebook, "x_icon");
	// 	buttonFacebook.GetComponent<Collider>().enabled = true;
	// 	buttonFacebook.gameObject.SetActive (false);
	// 	buttonFacebook.gameObject.SetActive (true);
	// 	buttonFBShare.gameObject.SetActive(false);
	// 	buttonFBInvite.gameObject.SetActive (false);
	// 	SPFacebook.instance.Logout ();
	// 	textureFacebookUser.GetComponent<UITexture> ().mainTexture = textureNoUser;
	// }

	// public void OnTwitterLogout()
	// {
	// 	/*
	// 	SetIcon (buttonTwitter, "x_icon");
	// 	buttonTwitter.collider.enabled = true;
	// 	buttonTwitter.gameObject.SetActive (false);
	// 	buttonTwitter.gameObject.SetActive (true);
	// 	*/
	// }

	// public void OnGemuGemuLogout()
	// {
	// 	buttonGemuReward.gameObject.SetActive (false);
	// 	buttonGemuPromo.gameObject.SetActive (false);
	// 	SetIcon (buttonEmail, "x_icon");
	// 	buttonEmail.GetComponent<Collider>().enabled = true;
	// 	buttonEmail.gameObject.SetActive (false);
	// 	buttonEmail.gameObject.SetActive (true);
	// }

	// public override void OnShow()
	// {
	// 	buttonGemuPromo.gameObject.SetActive (false);
	// 	buttonGemuReward.gameObject.SetActive (false);
	// 	buttonFBShare.gameObject.SetActive (false);
	// 	buttonFBInvite.gameObject.SetActive (false);

	// 	SetIcon (buttonFacebook, "x_icon");
	// 	buttonFacebook.GetComponent<Collider>().enabled = true;
	// 	buttonFacebook.gameObject.SetActive (false);
	// 	buttonFacebook.gameObject.SetActive (true);

	// 	/*
	// 	SetIcon (buttonTwitter, "x_icon");
	// 	buttonTwitter.collider.enabled = true;
	// 	buttonTwitter.gameObject.SetActive (false);
	// 	buttonTwitter.gameObject.SetActive (true);
	// 	*/
	// 	SetIcon (buttonEmail, "x_icon");
	// 	buttonEmail.GetComponent<Collider>().enabled = true;
	// 	buttonEmail.gameObject.SetActive (false);
	// 	buttonEmail.gameObject.SetActive (true);

	// 	if ( FB.IsLoggedIn )
	// 	{
	// 		OnFBLoggedIn();
	// 	}

	// 	/*
	// 	if ( SPTwitter.instance.IsAuthed )
	// 	{
	// 		OnTwitterLoggedIn();
	// 	}
	// 	*/
	// 	if ( !string.IsNullOrEmpty(GL1Connector.GetInstance().GetToken() ) )
	// 	{
	// 		OnGemuGemuLoggedIn();
	// 	}
	// }
	#endregion
}
