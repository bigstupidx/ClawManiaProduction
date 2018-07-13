using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GemuLoginController : GUI_Dialog {
	#region GEMU API
	
	// public GemuLoginGemuController canvasLoginGemuController;
	// public GemuUserDataController canvasGemuData;
	// public GemuDialogBoxController dialogBox;
	// public UIButton buttonFBShare; 
	// public UILabel textFBName;
	// public UILabel textGoogle;
	// void Awake()
	// {
	// 	if (FB.IsLoggedIn) 
	// 	{
	// 		SPFacebook.instance.OnUserDataRequestCompleteAction += OnUserDataLoaded;
	// 		SPFacebook.instance.LoadUserData ();
	// 	}
	// }
	
	// // Use this for initialization
	// public override void OnStart () {
	// 	if ( buttonFBShare )
	// 		buttonFBShare.gameObject.SetActive (FB.IsLoggedIn);
	// 	SPFacebook.instance.addEventListener(FacebookEvents.AUTHENTICATION_SUCCEEDED,  	 OnFBLoggedIn);
	// 	//SPTwitter.instance.addEventListener(TwitterEvents.AUTHENTICATION_SUCCEEDED,  OnTwitterLoggedIn);
		
	// 	if ( FB.IsInitialized == false )
	// 		SPFacebook.instance.Init();
	// 	//AN_PlusButton b =  new AN_PlusButton("https://unionassets.com/", AN_PlusBtnSize.SIZE_TALL, AN_PlusBtnAnnotation.ANNOTATION_BUBBLE);
	// 	//b.SetGravity(TextAnchor.UpperLeft);

	// 	GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
	// 	GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
	// 	GooglePlayConnection.ActionConnectionResultReceived += ActionConnectionResultReceived;
	// 	GooglePlayManager.ActionOAuthTokenLoaded += ActionOAuthTokenLoaded;
	// 	GooglePlayManager.ActionAvailableDeviceAccountsLoaded += ActionAvailableDeviceAccountsLoaded;


	// }
	
	// // Update is called once per frame
	// void Update () {
	
	// }

	// public void OnFBLoggedIn()
	// {
		
	// 	buttonFBShare.gameObject.SetActive (true);
	// 	SPFacebook.instance.OnUserDataRequestCompleteAction += OnUserDataLoaded;
	// 	SPFacebook.instance.LoadUserData ();
	// }

	// private void OnUserDataLoaded(FBResult result) 
	// {
	// 	SPFacebook.instance.OnUserDataRequestCompleteAction -= OnUserDataLoaded;
		
	// 	if (result.Error == null)  
	// 	{ 
	// 		textFBName.text = SPFacebook.instance.userInfo.first_name;
			
	// 	} 
	// 	else 
	// 	{
	// 		textFBName.text = "";
	// 	}
		
	// }

	// public void OnFBShareButton()
	// {
	// 	SoundManager.instance.PlayButton();
	// 	if ( FB.IsLoggedIn )
	// 	{
	// 		SPFacebook.instance.PostWithAuthCheck (
	// 			link: "http://gemugemu.com",
	// 			linkName: "GemuGemu",
	// 			linkCaption: "Play for free and be rewarded. It's magic.\t",
	// 			linkDescription: "",
	// 			picture: "http://www.gemugemu.com/bundles/Gemu_Icon_256.png"
	// 			);
	// 	}

	// }

	// public void OnFacebookButton()
	// {
	// 	SoundManager.instance.PlayButton();
	// 	if ( !FB.IsLoggedIn )
	// 	{
	// 		SPFacebook.instance.Login("email,publish_actions");
	// 	}
	// 	else
	// 	{
	// 		dialogBox.Show("Confirmation","Are you sure you want to logout from Facebook ?",true,"logoutfb",this.gameObject);

	// 	}
	// }

	// public void ConfirmLogoutFB()
	// {
	// 	textFBName.text = "";
	// 	SPFacebook.instance.Logout ();
	// 	buttonFBShare.gameObject.SetActive (false);
	// }

	// public void OnGemuButton()
	// {
	// 	SoundManager.instance.PlayButton();
	// 	string sUsername = "";
	// 	if (PlayerPrefs.HasKey (PlayerPrefHandler.keyUserName))
	// 		sUsername = PlayerPrefs.GetString (PlayerPrefHandler.keyUserName);
		
	// 	string sToken = "";
	// 	if (PlayerPrefs.HasKey (PlayerPrefHandler.keyToken))
	// 		sToken = PlayerPrefs.GetString (PlayerPrefHandler.keyToken);

	// 	if (string.IsNullOrEmpty (sUsername) || string.IsNullOrEmpty(sToken)) 
	// 	{
			
	// 		GUI_Dialog.InsertStack(canvasLoginGemuController.gameObject);
	// 	}
	// 	else
	// 	{
	// 		GUI_Dialog.InsertStack(canvasGemuData.gameObject);
	// 	}
	// }

	// public override void OnShow()
	// {
	// 	if (FB.IsInitialized && FB.IsLoggedIn) {
	// 		OnFBLoggedIn();
	// 	}
	// 	textGoogle.text = "Disconnected";
	// 	if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
	// 		//checking if player already connected
	// 		OnPlayerConnected ();
	// 	} 
	// 	//else{
	// 	//	dialogBox.Show ("info","not connected",false,"",this.gameObject);
	// 	//}
	// }

	// public void OnBackButton()
	// {
	// 	SoundManager.instance.PlayButton();
	// 	GUI_Dialog.ReleaseTopCanvas();
	// }

	// private void OnPlayerDisconnected() {
	// 	//dialogBox.Show ("info","google dc",false,"",this.gameObject);
	// 	textGoogle.text = "Disconnected";
	// }
	
	// private void OnPlayerConnected() {
	// 	//dialogBox.Show ("info","google connected",false,"",this.gameObject);
	// 	textGoogle.text = "Connected";
	// }

	// private void ActionConnectionResultReceived(GooglePlayConnectionResult result) {
		
	// 	if(result.IsSuccess) {
	// 		Debug.Log("Connected!");
	// 	} else {
	// 		Debug.Log("Connection failed with code: " + result.code.ToString());
	// 	}
	// 	//dialogBox.Show ("info","ConnectionResul:  " + result.code.ToString(),false,"",this.gameObject);
	// }

	// public void OnGoogleButton()
	// {
	// 	if (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
	// 		GooglePlayConnection.instance.disconnect ();
	// 	}
	// 	else
	// 		GooglePlayConnection.instance.connect ();
	// }

	// private void ActionAvailableDeviceAccountsLoaded(List<string> accounts) {
	// 	string msg = "Device contains following google accounts:" + "\n";
	// 	foreach(string acc in GooglePlayManager.instance.deviceGoogleAccountList) {
	// 		msg += acc + "\n";
	// 	} 
		
	// 	AndroidDialog dialog = AndroidDialog.Create("Accounts Loaded", msg, "Sign With First one", "Do Nothing");
	// 	dialog.OnComplete += SighDialogComplete;
		
	// }
	
	// private void SighDialogComplete (AndroidDialogResult res) {
	// 	if(res == AndroidDialogResult.YES) {
	// 		GooglePlayConnection.instance.connect(GooglePlayManager.instance.deviceGoogleAccountList[0]);
	// 	}
		
	// }
	
	// private void ActionOAuthTokenLoaded(string token) {
		
	// 	AN_PoupsProxy.showMessage("Token Loaded", GooglePlayManager.instance.loadedAuthToken);
	// }
	#endregion
}
