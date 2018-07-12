using UnityEngine;
using System.Collections;
using Pokkt;

public class GemuFreeCoins : GUI_Dialog {
	public UIButton buttonRateUs;
	public GUI_DialogBox dialogBox;
	private PokktConfig pokktConfig;

	bool bPokktInitialized = false;

	// Update is called once per frame
	void Update () {
	
	}

	public void OnBackButton()
	{
		if ( SoundManager.instance )
			SoundManager.instance.PlayButton();
		
		GUI_Dialog.ReleaseTopCanvas();
		
	}

	void GiveCoinToUserAfterWatchingAds()
	{

		int jmlCoinsNow = PlayerPrefs.GetInt(PlayerPrefHandler.keyCoin);
		jmlCoinsNow += 1;
		PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin, jmlCoinsNow);
		GameManager.GEMUCOINS = jmlCoinsNow;
		if ( CoinTimerHandler.instance )
			CoinTimerHandler.instance.countCoin = jmlCoinsNow;


		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay>();
		if ( gs )
		{
			gs.RefreshAllInfo();
			gs.guiIngame.guiShop.RefreshInfo();
		}
		dialogBox.Show ("Info", "You got 1 GemuGold.", false, "", this.gameObject);
		
		/*GameDataManager.instance.SendPlayResult(
			GameDataManager.instance.gameID.ToString(),"0",
			jmlCoinsNow.ToString(),
			GameManager.EXP.ToString(),
			GameManager.getLevelValue().ToString());*/
		GL1Connector.GetInstance().AddBalance(this.gameObject,"","1","","ADS");
	}

	IEnumerator ShowingAds()
	{
		yield return new WaitForSeconds (1);

		Debug.LogError ("showing=" + UnityEngine.Advertisements.Advertisement.isShowing);
		while (UnityEngine.Advertisements.Advertisement.isShowing) {
			yield return null;
		}
		
		PlayAdsState playAdResult = GameDataManager.unityAdsResult;
		
		if (playAdResult == PlayAdsState.NOTENOUGHTIME) {
			dialogBox.Show ("Info", "Please wait...", false, "", this.gameObject);
		} else if (playAdResult == PlayAdsState.SKIPPED) {
			dialogBox.Show ("Info", "Skipping ads will not give you any GemuGold.", false, "", this.gameObject);
		} else if (playAdResult == PlayAdsState.FINISHED) {
			GiveCoinToUserAfterWatchingAds();
		}else if (playAdResult == PlayAdsState.FAILED) {
			dialogBox.Show ("Info", "Failed playing ads", false, "", this.gameObject);
		}else if (playAdResult == PlayAdsState.NOTREADY) {
			dialogBox.Show ("Info", "Ad is not ready", false, "", this.gameObject);
		}
	}

	private void OnDownloadCompleted(string message)
	{
		Debug.LogError ("video download complete");
		
		PokktManager.GetVideo (pokktConfig);
	}

	public void OnClickWatchAds()
	{
		if (GameDataManager.PlayUnityVideoAd ()) {

			StartCoroutine(ShowingAds());
		} else {
			PokktManager.CacheVideoCampaign();
		}
	}

	public void OnClickLikeUs()
	{
		Debug.LogError ("Like Us");
		//SPFacebook.instance.LoadLikes (FB.UserId, "1435959710032078");
		SPFacebook.instance.LoadLikes (FB.UserId);
		/*
		bool isLike = SPFacebook.instance.IsUserLikesPage (FB.UserId, "646187692163223");
		if (isLike) {
			GemuDialogBoxController.GetInstance().Show("Error","Like",false,"",this.gameObject);
		} else {
			GemuDialogBoxController.GetInstance().Show("Error","Not Like",false,"",this.gameObject);
		}
		*/
	}

	public void Like(string likeID)
	{
		/*
		Facebook.instance.graphRequest(
			likeID+"/likes",
			HTTPVerb.POST,
			( error, obj ) =>
			{
			if (obj != null)
			{
				Prime31.Utils.logObject( obj );
			}
			if( error != null )
			{
				Debug.Log("Error liking:"+error);
				return;
			}
			Debug.Log( "like finished: " );
		});
		*/
	}

	public void OnClickRateUs()
	{

		AndroidRateUsPopUp rate = AndroidRateUsPopUp.Create("Rate Us", rateText, rateUrl);
		rate.OnComplete += OnRatePopUpClose;
	}
	
	public override void OnShow()
	{
		bool bRated = false;
		if (PlayerPrefs.HasKey (PlayerPrefHandler.keyRateUs) == false ) 
		{
			PlayerPrefs.SetInt (PlayerPrefHandler.keyRateUs, 0);
		} 
		else 
		{
			if ( PlayerPrefs.GetInt(PlayerPrefHandler.keyRateUs) == 1 )
			{
				bRated = true;
			}
		}

		if (bRated) {
			buttonRateUs.gameObject.SetActive(false);
		} else {
			buttonRateUs.gameObject.SetActive(true);
		}

		if (!bPokktInitialized) {
			bPokktInitialized = true;
			pokktConfig = new PokktConfig ();
			pokktConfig.setSecurityKey ("0c264c7465abbe0bf7327b59cd86bffd");
			pokktConfig.setApplicationId ("b9ae5052bc5209c0b9fc026d5598eba5");
			pokktConfig.setIntegrationType (PokktIntegrationType.INTEGRATION_TYPE_ALL);
			pokktConfig.setAutoCacheVideo (Storage.getAutoCaching ());
			
			PokktManager.initPokkt (pokktConfig);
			pokktConfig.setThirdPartyUserId ("123456");
			
			//StartCacheButton.gameObject.SetActive(!Storage.getAutoCaching());
			
			// set default skip time
			pokktConfig.setDefaultSkipTime (10);
			
			pokktConfig.setScreenName ("sample_screen");
			
			// handle pokkt video ad events
			PokktManager.Dispathcer.VideoClosedEvent += (string message) =>
			{
				//dialogBox.Show ("Info", message, false, "", this.gameObject);
				//GiveCoinToUserAfterWatchingAds();
			};
			
			PokktManager.Dispathcer.VideoSkippedEvent += (string message) =>
			{
				//dialogBox.Show ("Info", message, false, "", this.gameObject);
				dialogBox.Show ("Info", "Skipping ads will not give you any GemuGold.", false, "", this.gameObject);
			};
			
			PokktManager.Dispathcer.VideoCompletedEvent += (string message) =>
			{
				//dialogBox.Show ("Info", message, false, "", this.gameObject);
				//GiveCoinToUserAfterWatchingAds();
			};
			
			PokktManager.Dispathcer.VideoGratifiedEvent += (string message) =>
			{
				//dialogBox.Show ("Info", message, false, "", this.gameObject);
				//_totalPointsEarned += float.Parse(message);
				
				//PointsEarned.text = "Points Earned: " + _totalPointsEarned.ToString();
				//StatusMessage.text = "Points earned from last video: " + message;
				
				GiveCoinToUserAfterWatchingAds();
			};
			
			PokktManager.Dispathcer.VideoDisplayedEvent += (string message) =>
			{
				//dialogBox.Show ("Info", message, false, "", this.gameObject);
				//StatusMessage.text = message;
			};
			
			PokktManager.Dispathcer.DownloadCompletedEvent += OnDownloadCompleted;
			
			PokktManager.Dispathcer.DownloadFailedEvent += (string message) =>
			{
				dialogBox.Show ("Info", message, false, "", this.gameObject);
				//StatusMessage.text = message;
			};
			
			// if video is available then enable the buttons
			if (PokktManager.IsVideoAvailable ()) {
				float vc = PokktManager.GetVideoVc ();
				OnDownloadCompleted (vc.ToString ());
			}
		}
	}

	private string rateText = "If you enjoy playing with GemuGemu, please take a moment to rate it. Thanks for your support!";
	private string rateUrl = "https://play.google.com/store/apps/details?id=com.GameLevelOne.ClawMania";
	private void OnRatePopUpClose(AndroidDialogResult result) {
		
		switch(result) {
		case AndroidDialogResult.RATED:
			PlayerPrefs.SetInt (PlayerPrefHandler.keyRateUs, 1);
			buttonRateUs.gameObject.SetActive(false);
			int jmlCoinsNow = PlayerPrefs.GetInt(PlayerPrefHandler.keyCoin);
			jmlCoinsNow += 5;
			PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin, jmlCoinsNow);

			GL1Connector.GetInstance().AddBalance(this.gameObject,"0","5","","RATE");

			/*int jmlTiketNow = PlayerPrefs.GetInt(PlayerPrefHandler.keyUserTiket);
			jmlTiketNow += 100;
			PlayerPrefs.SetInt(PlayerPrefHandler.keyUserTiket, jmlTiketNow);*/

			if ( CoinTimerHandler.instance )
				CoinTimerHandler.instance.countCoin = jmlCoinsNow;
			
			/*GameDataManager.instance.SendPlayResult(
				GameDataManager.instance.gameID.ToString(),"100",
				jmlCoinsNow.ToString(),
				GameManager.EXP.ToString(),
				GameManager.getLevelValue().ToString());*/

			Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay>();
			if ( gs )
			{
				gs.RefreshAllInfo();
				gs.guiIngame.guiShop.RefreshInfo();
			}

			dialogBox.Show ("Info", "You got 5 GemuGold.", false, "", this.gameObject);

			break;
		case AndroidDialogResult.REMIND:
			break;
		case AndroidDialogResult.DECLINED:
			break;
			
		}
		
		//AN_PoupsProxy.showMessage("Result", result.ToString() + " button pressed");
	}
}
