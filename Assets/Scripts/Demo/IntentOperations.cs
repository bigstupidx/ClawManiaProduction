using UnityEngine;
using System.Collections;

public class IntentOperations : MonoBehaviour
{
	// exposed properties
	public GameObject ScreenManagerGO = null;

	void Start()
	{
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			ScreenManagerGO.GetComponent<ScreenManager>().OpenScreen("CampaignSelectorScreen");
	}
}
