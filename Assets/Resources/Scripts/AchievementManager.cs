using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class AchievementManager : MonoBehaviour 
{
	public Achievement[] achievements;

	private int totalAchCount=19;
	// Use this for initialization
	void Start () {
		//PlayerPrefs.DeleteAll ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void AddToFinalAchievementEvent(){
		int achCount = 0;

		if (PlayerPrefs.HasKey (PlayerPrefHandler.keyTotalAchCount)) {
			achCount = PlayerPrefs.GetInt(PlayerPrefHandler.keyTotalAchCount);
		}

		if (achCount >= 0 && achCount < totalAchCount) {
			achCount++;
			PlayGamesPlatform.Instance.IncrementAchievement (
				GPGSIds.achievement_claw_mania_professor, 1, (bool success) => {
				// handle success or failure
			});
			PlayerPrefs.SetInt (PlayerPrefHandler.keyTotalAchCount, achCount);
			PlayerPrefs.Save ();
		} else {
			//do nothing
		}
	}

	public void OnAchievementEvent(AchievementType type,int amount)
	{
		//Debug.LogError ("[AchievementManager] event=" + type.ToString () + " amount=" + amount);
		GameObject gamestate = GameObject.FindGameObjectWithTag ("Gamestate");
		Gamestate_Gameplay gameplay = null;
		if ( gamestate )
			gameplay = gamestate.GetComponent<Gamestate_Gameplay> ();

		for ( int i=0; i<achievements.Length; i++ )
		{
			Achievement achievement = (Achievement)achievements[i];
			if ( gameplay != null  && type == achievement.type && achievement.amount == amount && !achievement.isUnlocked() )
			{
//				Debug.LogError ("[AchievementManager] process event=" + type.ToString () + " amount=" + amount);
//
//				PlayerPrefs.SetInt("AC_"+achievement.id+"_"+achievement.amount,1);
//				gameplay.guiIngame.RefreshTicketInfo();
//
//				if(type == AchievementType.PowerPlay){
//					gameplay.guiAchievementUnlocked.ShowPowerPlay();
//				}
//				else
//					gameplay.guiAchievementUnlocked.Show(achievement.id,achievement.rewardAmount,achievement.rewardType);

//				if ( achievement.rewardType == AchievementReward.Ticket )
//				{
//					GameManager.TICKET += achievement.rewardAmount;
//					PlayerPrefs.SetInt("ticket",GameManager.TICKET);
//					GL1Connector.GetInstance().AddGameBalance(null,achievement.rewardAmount.ToString(),"","CMACHIEVE"+achievement.id);
//				}

//				switch(type){
//				case AchievementType.ShopTime:
//
//					break;
//
//				}
			}
			else
			{
				if ( type == achievement.type )
				{
					if ( gameplay == null )
						Debug.LogError("gameplay is null");
					if ( achievement.amount != amount )
						Debug.LogError("amount is different");
					if ( achievement.isUnlocked() == false )
						Debug.LogError("achievement has been unlocked");
				}
			}
		}
		
	}
}
