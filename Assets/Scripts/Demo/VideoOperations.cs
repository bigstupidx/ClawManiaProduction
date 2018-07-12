using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pokkt;

public class VideoOperations : MonoBehaviour
{
	// exposed properties
	public GameObject ScreenManagerGO;
	public Button StartCacheButton;
	public Button PlayVideoIncentButton;
	public Button PlayVideoNonIncentButton;
	public Text PointsEarned;
	public Text StatusMessage;
	private PokktConfig pokktConfig;

	// internal states
	private float _totalPointsEarned = 0;

	void Awake()
	{
		// You will need PokktConfig at different method calls
		// it is single place fro all sdk related info
		pokktConfig = new PokktConfig ();
		
		// set pokkt params
		pokktConfig.setSecurityKey("iJ02lJss0M");
		pokktConfig.setApplicationId("a2717a45b835b5e9f50284a38d62a74e");
		pokktConfig.setIntegrationType(PokktIntegrationType.INTEGRATION_TYPE_ALL);
		pokktConfig.setAutoCacheVideo(Storage.getAutoCaching());

		// this will be a unique id per user from developer side
		// developer is free to choose any String data point.
		pokktConfig.setThirdPartyUserId("123456");

		// enable 'Start Caching' button in case of manual caching
		StartCacheButton.gameObject.SetActive(!Storage.getAutoCaching());

		// set default skip time
		pokktConfig.setDefaultSkipTime(10);

		pokktConfig.setScreenName("sample_screen");

		// handle pokkt video ad events
		PokktManager.Dispathcer.VideoClosedEvent += (string message) =>
		{
			StatusMessage.text = message;
		};

		PokktManager.Dispathcer.VideoSkippedEvent += (string message) =>
		{
			StatusMessage.text = message;
		};

		PokktManager.Dispathcer.VideoCompletedEvent += (string message) =>
		{
			StatusMessage.text = message;
		};
		
		PokktManager.Dispathcer.VideoGratifiedEvent += (string message) =>
		{
			_totalPointsEarned += float.Parse(message);
			
			PointsEarned.text = "Points Earned: " + _totalPointsEarned.ToString();
			StatusMessage.text = "Points earned from last video: " + message;
		};
		
		PokktManager.Dispathcer.VideoDisplayedEvent += (string message) =>
		{
			StatusMessage.text = message;
		};
		
		PokktManager.Dispathcer.DownloadCompletedEvent += OnDownloadCompleted;
		
		PokktManager.Dispathcer.DownloadFailedEvent += (string message) =>
		{
			StatusMessage.text = message;
		};

		// it is mandatory to call init.
		PokktManager.initPokkt(pokktConfig);

		// if video is available then enable the buttons
		if (PokktManager.IsVideoAvailable())
		{
			float vc = PokktManager.GetVideoVc();
			OnDownloadCompleted(vc.ToString());
		}
	}

	private void OnDownloadCompleted(string message)
	{
		StatusMessage.text = "Download Completed! VC is: " + message;
		
		// enable playback buttons
		PlayVideoIncentButton.gameObject.SetActive(true);
		PlayVideoNonIncentButton.gameObject.SetActive(true);
		
		// get the video vc
		PlayVideoIncentButton.GetComponentInChildren<Text>().text = "Play Video To Earn: " + message;
	}

	void Start()
	{
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ScreenManagerGO.GetComponent<ScreenManager>().OpenScreen("CampaignSelectorScreen");

			// disable buttons
			StartCacheButton.gameObject.SetActive(!Storage.getAutoCaching());
			PlayVideoIncentButton.gameObject.SetActive(false);
			PlayVideoNonIncentButton.gameObject.SetActive(false);
		}
	}

	public void StartCaching()
	{
		PokktManager.CacheVideoCampaign();
	}

	public void PlayVideo(bool incent)
	{
		if (incent)
			PokktManager.GetVideo(pokktConfig);
		else
			PokktManager.GetVideoNonIncent(pokktConfig);
	}	
}
