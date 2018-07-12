using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GemuRewardController : MonoBehaviour {
	
	public GemuDialogBoxController dialogBox;
	public List<RewardData> rewardList = new List<RewardData>();
	
	public GemuRewardVerifyController rewardVerify;
	
	// Use this for initialization
	void Start () {
		
		GemuAPI.OnRewardResponse += OnRewardResponse;
	}
	
	void OnDestroy()
	{
		GemuAPI.OnRewardResponse -= OnRewardResponse;
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnRewardResponse(Restifizer.RestifizerResponse response)
	{
		Hashtable data = response.Resource;
		
		if ( data["errcode"].ToString() == "0")
		{
			dialogBox.Show("Info","Your ticket has been redeemed.",false,"",this.gameObject);
		}
		else
		{
			dialogBox.Show("Error",data["errdetail"].ToString(),false,"",this.gameObject);
		}
	}

	public void OnClickBack()
	{
		SoundManager.instance.PlayButton();
		GUI_Dialog.ReleaseTopCanvas();
		this.gameObject.SetActive (false);
	}

	public void OnRewardChosen(int idx)
	{
		SoundManager.instance.PlayButton();
		for ( int i=0; i<rewardList.Count; i++ )
		{
			if ( rewardList[i].code == idx )
			{
				rewardVerify.SetData(rewardList[i]);
				rewardVerify.gameObject.SetActive(true);
				rewardVerify.gameObject.SetActive(true);
				GUI_Dialog.InsertStack(rewardVerify.gameObject);
				break;
			}
		}
	}
}
