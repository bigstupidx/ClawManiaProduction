using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;

public class TestAds : MonoBehaviour {

	// Use this for initialization
	void Start () {
		// Create a 320x50 banner at the top of the screen.
		BannerView bannerView = new BannerView(
			"ca-app-pub-6136977680704470/3666371144", AdSize.Banner, AdPosition.Top);
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the banner with the request.
		bannerView.LoadAd(request);
		bannerView.Show ();
	}
	

}
