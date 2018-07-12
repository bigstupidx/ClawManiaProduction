using UnityEngine;
using System.Collections;

public class GUI_Achievement : GUI_Dialog {
	public GameObject prefabContentAchievement;
	public UIPanel panelContainer;
	public UILabel labelInfo;
	public UIScrollView scrollView;
	// Use this for initialization

	public override void OnStart ()  
	{
		Gamestate gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate> ();
		if ( gs && gs.achievementManager )
		{
			int unlockedAmount = 0;
			for ( int i=0; i<gs.achievementManager.achievements.Length ; i ++ )
			{
				Achievement achievement = gs.achievementManager.achievements[i];
				GameObject obInst = (GameObject) Instantiate (prefabContentAchievement);
				obInst.name = achievement.id;
				obInst.transform.parent = panelContainer.transform;
				obInst.transform.localScale = new Vector3(1,1,1);
				obInst.transform.localPosition = new Vector3(0,200-(i*150),0);
				
				UIDragScrollView scrollView = obInst.GetComponent<UIDragScrollView>();
				scrollView.scrollView = panelContainer.GetComponent<UIScrollView>();

				GameManager.SetNGUILabel(obInst.transform.Find("Label Name"),achievement.id);
				GameManager.SetNGUILabel(obInst.transform.Find("Label Description"),achievement.desc);
				string sAmount =achievement.rewardAmount.ToString()+" ";
				if ( achievement.rewardType == AchievementReward.Coin )
					sAmount += "Gemu Gold";
				else if ( achievement.rewardType == AchievementReward.Ticket )
					sAmount += "Gemu Points";
				else if ( achievement.rewardType == AchievementReward.Exp )
					sAmount += "Exp";
				
				GameManager.SetNGUILabel(obInst.transform.Find("Label Reward"),sAmount);
				string sKey = "AC_"+achievement.id+"_"+achievement.amount;
				if ( PlayerPrefs.HasKey(sKey) )
				{
					unlockedAmount ++;
					//PlayerPrefs.SetInt(sKey,1);
					//PlayerPrefs.DeleteKey("AC_"+achievement.id+"_"+achievement.amount); //for testing
					Transform trIconLock = obInst.transform.Find("Icon");
					if ( trIconLock )
						trIconLock.gameObject.SetActive(false);
				}
			}
			labelInfo.text = unlockedAmount.ToString()+"/"+gs.achievementManager.achievements.Length;
		}

	}

	
	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public override void OnShow()
	{
		Gamestate gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate> ();
		if ( gs && gs.achievementManager )
		{
			int unlockedAmount = 0;
			for ( int i=0; i<gs.achievementManager.achievements.Length ; i ++ )
			{
				Achievement achievement = gs.achievementManager.achievements[i];
				Transform trContent = panelContainer.transform.Find(achievement.id);
				if ( trContent )
				{

					if ( PlayerPrefs.HasKey("AC_"+achievement.id+"_"+achievement.amount) )
					{
						unlockedAmount ++;
						Transform trIconLock = trContent.gameObject.transform.Find("Icon");
						if ( trIconLock )
							trIconLock.gameObject.SetActive(false);
					}
				}
			}
			labelInfo.text = unlockedAmount.ToString()+"/"+gs.achievementManager.achievements.Length;
		}
		scrollView.ResetPosition ();
	}
}
