using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pokkt;

public class OfferwallOperations : MonoBehaviour
{
	// exposed properties
	public GameObject ScreenManagerGO = null;
	public Text PointsEarned;
	public Text StatusMessage;
	private PokktConfig pokktConfig;

	// internal states
	private int _totalPointsEarned = 0;

	void Awake()
	{
		// You will need PokktConfig at different method calls
		// it is single place fro all sdk related info
		pokktConfig = new PokktConfig ();

		// set pokkt params
		pokktConfig.setSecurityKey("iJ02lJss0M");
		pokktConfig.setApplicationId("a2717a45b835b5e9f50284a38d62a74e");
		pokktConfig.setIntegrationType(PokktIntegrationType.INTEGRATION_TYPE_ALL);
		pokktConfig.setAutoCacheVideo(false);

		PokktManager.initPokkt(pokktConfig);

		// this will be a unique id per user from developer side
		// developer is free to choose any String data point.
		pokktConfig.setThirdPartyUserId("123456");

		// handle pokkt offerwall events
		PokktManager.Dispathcer.CoinResponseEvent += (string message) =>
		{
			_totalPointsEarned += int.Parse(message);
			
			PointsEarned.text = "Points Earned: " + _totalPointsEarned.ToString();
			StatusMessage.text = "Points earned from last operation: " + message;
		};

		PokktManager.Dispathcer.CoinResponseWithTransIdEvent += (string message) =>
		{
			string[] items = message.Split(new char[] { ',', ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
			string points = items[0];
			string transId = items[1];

			_totalPointsEarned += int.Parse(points);

			PointsEarned.text = "Points Earned: " + _totalPointsEarned.ToString();
			StatusMessage.text = "Points earned from transaction id: " + transId + ", earned points: " + points;
		};

		PokktManager.Dispathcer.CoinResponseFailedEvent += (string message) =>
		{
			StatusMessage.text = message;
		};

		PokktManager.Dispathcer.CampaignAvailabilityEvent += (string message) =>
		{
			StatusMessage.text = message;
		};

		PokktManager.Dispathcer.OfferwallClosedEvent += (string message) =>
		{
			StatusMessage.text = message;
		};
	}

	void Start()
	{
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			ScreenManagerGO.GetComponent<ScreenManager>().OpenScreen("CampaignSelectorScreen");
	}
	
	void OnApplicationPause(bool paused)
	{
		if (!paused)
		{
			PokktManager.GetPendingCoins();
			PokktManager.CheckOfferWallCampaign();
		}
	}

	public void GetOfferwallAny()
	{
		pokktConfig.setOfferWallAssetValue("");
		PokktManager.GetCoins(pokktConfig);
	}

	public void GetOfferwallFixed()
	{
		PokktManager.GetCoins(pokktConfig);
	}

	public void GetPendingCoins()
	{
		PokktManager.GetPendingCoins();
	}

	public void SetAssetValue(InputField inputField)
	{
		pokktConfig.setOfferWallAssetValue(inputField.text);
	}
}
