using UnityEngine;
using System.Collections;

public class AchievementQueue
{
	public string sAchievementName;
	public int iRewardAmount;
	public AchievementReward iRewardType;
}

public class GUI_AchievementUnlocked : GUI_Dialog {
	public UILabel labelAchievementName;
	public UISprite spriteRewardIcon;
	public UILabel labelRewardAmount;
	
	ArrayList queues;
	// Use this for initialization
	
	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}

	public override void OnStart ()  {
		//gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay> ();
		queues = new ArrayList ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void ProcessQueue()
	{
		if ( queues != null && queues.Count > 0 )
		{
			AchievementQueue queue = (AchievementQueue)queues[0];
			queues.RemoveAt(0);
			
			labelAchievementName.text = queue.sAchievementName;
			string sReward = queue.iRewardAmount.ToString () + " ";
			if ( queue.iRewardType == AchievementReward.Ticket )
				sReward += "GemuPoint";
			else if ( queue.iRewardType == AchievementReward.Coin )
				sReward += "GemuGold";
			else if ( queue.iRewardType == AchievementReward.Exp )
				sReward += "Exp";
			if ( queue.iRewardAmount > 1 )
				sReward += "s";
			labelRewardAmount.text = sReward;
			
			GUI_Dialog.InsertStack(this.gameObject);
			//Show ();

		}
	}
	
	public override void OnTweenDone()
	{
		ProcessQueue ();
	}

	public void Show(string sAchievementName,int iRewardAmount, AchievementReward rewardType)
	{


		Debug.LogError ("show achievement"+sAchievementName);
		AchievementQueue queue = new AchievementQueue ();
		queue.sAchievementName = sAchievementName;
		queue.iRewardAmount = iRewardAmount;
		queue.iRewardType = rewardType;
		
		queues.Add (queue);
		if ( !isVisible() )
			ProcessQueue();

	}

	public void ShowPowerPlay(){
		labelAchievementName.text = "Power Play";
		string sReward = "10 GemuPoint";

		labelRewardAmount.text = sReward;
		
		GUI_Dialog.InsertStack(this.gameObject);
	}

}
