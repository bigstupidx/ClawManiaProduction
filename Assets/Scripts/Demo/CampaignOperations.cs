using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Pokkt;

public class CampaignOperations : MonoBehaviour
{
	public Text AutoCacheStatus;
	public Button DemoOfferwallButton;
	
	void Start()
	{
		AutoCacheStatus.text = "Auto Cache: " + (Storage.getAutoCaching() ? "ENABLED" : "DISABLED");

		DemoOfferwallButton.gameObject.SetActive(Application.platform == RuntimePlatform.Android);
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
}
