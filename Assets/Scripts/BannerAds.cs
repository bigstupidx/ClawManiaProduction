using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class BannerAds : MonoBehaviour {
	public static BannerAds instance;
	string bannerAdID = "ca-app-pub-6136977680704470/3666371144";
	BannerView bannerView;
	AdRequest bannerRequest;

	// Use this for initialization
	void Start () {
		instance = this;
	}

	public void RequestBannerAd(){
		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(
			bannerAdID, AdSize.Banner, AdPosition.Bottom);
		// Create an empty ad request.
		bannerRequest = new AdRequest.Builder().Build();
		// Load the banner with the request.
		bannerView.LoadAd(bannerRequest);
	}

	public void ShowBannerAd(){
		bannerView.Show ();
	}

	public void HideBannerAd(){
		bannerView.Hide ();
	}
}
