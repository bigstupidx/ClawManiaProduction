using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RewardData
{
	public int code = 0;
	public string name = "";
	public string description = "";
	public string thumbnail = "";
	public Image thumbnailImg = null;
	public int ticketCost = 0;

	public RewardData()
	{

	}

	public RewardData(int code, string name, string description, string thumbnail, Image thumbnailImg, int ticketCost)
	{
		this.code = code;
		this.name = name;
		this.description = description;
		this.thumbnail = thumbnail;
		this.thumbnailImg = thumbnailImg;
		this.ticketCost = ticketCost;
	}
}

public class RewardHandler : MonoBehaviour {

	public List<RewardData> rewardList = new List<RewardData>();

	public RewardVerifyHandler rewardVerify;

	// Use this for initialization
	void Start () {
		GemuAPI.OnRewardResponse += OnRewardResponse;
	}

	void OnDestroy () {
		
		GemuAPI.OnRewardResponse += OnRewardResponse;
	}

	void Awake()
	{
		Debug.LogError ("awake");
		//GemuAPI.Reward ();
	}
	void OnRewardResponse(Restifizer.RestifizerResponse response)
	{
	
		/*
		Hashtable data = response.Resource;
		if (data == null)
		{
			Debug.LogError("No data");
			return;
		}
		*/
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void OnRewardChosen(int idx)
	{
		Debug.LogError ("idx=" + idx);
		for ( int i=0; i<rewardList.Count; i++ )
		{
			RewardData rewardData = rewardList[i];
			Debug.LogError ("code=" + rewardData.code);
			if ( rewardData.code == idx )
			{
				
				rewardVerify.SetData(rewardList[i]);
				rewardVerify.gameObject.SetActive(true);
				break;
			}
		}
	}
}
