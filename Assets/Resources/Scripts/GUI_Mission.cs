using UnityEngine;
using System.Collections;

public class GUI_Mission : GUI_Dialog {
	
	public GameObject prefabContentAchievement;
	public UIScrollView panelContainer;
	
	int iIndexMissionTutorial = 0;
	int iIndexMissionProcedural = 0;

	// Update is called once per frame
	public override void OnStart ()  
	{
		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay> ();
		if ( gs )
		{
			int unlockedAmount = 0;
			for ( int i=0; i<gs.missions.Length ; i ++ )
			{
				Mission mission = gs.missions[i];
				GameObject obInst = (GameObject) Instantiate (prefabContentAchievement);
				obInst.name = mission.name;
				obInst.transform.parent = panelContainer.transform;
				obInst.transform.localScale = new Vector3(1,1,1);

				if ( mission.missionType == Mission_Type.Tutorial )
				{
					obInst.transform.localPosition = new Vector3(0,260-(iIndexMissionTutorial*200),0);
					iIndexMissionTutorial ++;
				}
				else
				{
					obInst.transform.localPosition = new Vector3(0,260-(iIndexMissionProcedural*200),0);
					iIndexMissionProcedural ++;

				}
				UIDragScrollView scrollView = obInst.GetComponent<UIDragScrollView>();
				scrollView.scrollView = panelContainer.GetComponent<UIScrollView>();
				
				GameManager.SetNGUILabel(obInst.transform.Find("Label Name"),mission.MissionName);
				GameManager.SetNGUILabel(obInst.transform.Find("Label Desc"),mission.MissionDesc);
				GameManager.SetNGUILabel(obInst.transform.Find("Reward Coin Value"),mission.CoinReward.ToString());
				GameManager.SetNGUILabel(obInst.transform.Find("Reward Exp Value"),mission.ExpReward.ToString());

				Transform trButtonPlay = obInst.transform.Find("ButtonPlay");
				if ( trButtonPlay )
				{
					UIButton button = trButtonPlay.gameObject.GetComponent<UIButton>();
					if ( button )
					{
						button.onClick.Add(new EventDelegate(this,"OnClickButton"));
					}
				}
			}
		}
		
	}

	public void OnClickButton()
	{
		UIButton button = UIButton.current;
		if ( button == null )
			return;

		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay> ();
		if (gs == null)
			return;

		for ( int i=0; i<gs.missions.Length ; i ++ )
		{
			Mission mission = gs.missions[i];

			if ( button.transform.parent.name.Equals(mission.name) )
			{
				gs.PlayMission(mission);
				break;
			}
		}

		OnClickBack ();
	}

	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}

	public override void OnShow()
	{
		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay> ();
		if ( gs )
		{
			bool bAllTutorialDone = true;
			for ( int i=0; i<gs.missions.Length ; i ++ )
			{
				Mission mission = gs.missions[i];
				if ( mission.missionType == Mission_Type.Tutorial && PlayerPrefs.HasKey("mission."+mission.MissionName.Replace(" ","") ) == false )
				{
					bAllTutorialDone = false;
					break;
				}
			}

			Debug.LogError("bAllTutorialDone = "+bAllTutorialDone);
			for ( int i=0; i<gs.missions.Length ; i ++ )
			{
				Mission mission = gs.missions[i];

				Transform trMissionContent = panelContainer.transform.Find(mission.name);
				if ( trMissionContent )
				{
					Transform trButtonPlay = trMissionContent.Find("ButtonPlay");
					if ( trButtonPlay )
					{
						trButtonPlay.gameObject.SetActive(true);
						if ( gs.currentMission && gs.currentMission.name.Equals( mission.name ) )
						{
							trButtonPlay.gameObject.SetActive(false);
						}
					}

					if ( mission.missionType == Mission_Type.Tutorial )
					{
						if ( bAllTutorialDone == false )
						{
							
							trMissionContent.gameObject.SetActive(true);
							if ( PlayerPrefs.HasKey("mission."+mission.MissionName.Replace(" ","") ) )
							{
								Transform trDone = trMissionContent.Find("InfoDone");
								if ( trDone )
									trDone.gameObject.SetActive(true);

								if ( trButtonPlay )
									trButtonPlay.gameObject.SetActive(false);

							}
						}
						else
						{
							trMissionContent.gameObject.SetActive(false);
						}
					}
					else if ( mission.missionType == Mission_Type.Procedural )
					{
						//Debug.LogError("bMissionTutorialDone="+bMissionTutorialDone);
						if ( bAllTutorialDone == true )
						{
							trMissionContent.gameObject.SetActive(true);
							if ( trButtonPlay  )
							{
								if ( mission.missionEvent == Mission_Event.Prize )
								{
									if ( trButtonPlay.gameObject.activeInHierarchy )
									{
										int rnd = Random.Range(mission.Procedural_Min, mission.Procedural_Max);
										if ( rnd == 0 )
											rnd = 1;
										PlayerPrefs.SetInt(GameManager.PREF_PROCEDURAL_MISSION_PRIZE_AMOUNT,rnd);
										//PlayerPrefs.SetInt(GameManager.PREF_PROCEDURAL_MISSION_AMOUNT,1);

										GameManager.SetNGUILabel(trMissionContent.transform.Find("Label Desc"),"Collect "+rnd+" Prize");
									}
									else
									{
										
										GameManager.SetNGUILabel(trMissionContent.transform.Find("Label Desc"),"Collect "+PlayerPrefs.GetInt(GameManager.PREF_CURR_USER_PROCEDURAL_MISSION_PRIZE_AMOUNT)+"/"+PlayerPrefs.GetInt(GameManager.PREF_PROCEDURAL_MISSION_PRIZE_AMOUNT)+" Prize");
									}
								}
								else if ( mission.missionEvent == Mission_Event.Coin )
								{
									if ( trButtonPlay.gameObject.activeInHierarchy )
									{
										int rnd = Random.Range(mission.Procedural_Min, mission.Procedural_Max);
										if ( rnd == 0 )
											rnd = 1;
										PlayerPrefs.SetInt(GameManager.PREF_PROCEDURAL_MISSION_COIN_AMOUNT,rnd);
										//PlayerPrefs.SetInt(GameManager.PREF_PROCEDURAL_MISSION_AMOUNT,1);
										GameManager.SetNGUILabel(trMissionContent.transform.Find("Label Desc"),"Collect "+rnd+" GemuGold");
									}
									else
									{
										
										GameManager.SetNGUILabel(trMissionContent.transform.Find("Label Desc"),"Collect "+PlayerPrefs.GetInt(GameManager.PREF_CURR_USER_PROCEDURAL_MISSION_COIN_AMOUNT)+"/"+PlayerPrefs.GetInt(GameManager.PREF_PROCEDURAL_MISSION_COIN_AMOUNT)+" GemuGold");
									}
								}
							}
						}
						else
						{
							trMissionContent.gameObject.SetActive(false);
						}
					}
				}
			}
			//labelInfo.text = unlockedAmount.ToString()+"/"+gs.achievementManager.achievements.Length;
		}
		panelContainer.ResetPosition ();
	}
}
