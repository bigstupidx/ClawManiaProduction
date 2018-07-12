using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class GUI_Options : MonoBehaviour 
{
	public UIPanel guiCredits;
	public UIPanel guiAchievements;
	public UIPanel guiPrizes;
	public UIPanel guiShare;
	public UIPanel guiTutorial;
	public UIPanel guiMission;
	
	public UIButton buttonCredits;
	public UIButton buttonAchievement;
	public UIButton buttonSocial;
	public UIButton buttonSound;
	public UIButton buttonPrizes;
	public UIButton buttonHelp;
	public UIButton buttonMainMenu;
	public UIButton buttonOptions;
	public UIButton buttonMission;
	// Use this for initialization
	void Start () 
	{
		DeactivateButton (buttonCredits);
		DeactivateButton (buttonAchievement);
		//DeactivateButton (buttonSocial);
		DeactivateButton (buttonSound);
		DeactivateButton (buttonPrizes);
		DeactivateButton (buttonHelp);
		DeactivateButton (buttonMainMenu);
		DeactivateButton (buttonMission);
	}

	void DeactivateButton(UIButton button)
	{
		if ( button == null )
			return;
		button.transform.parent.gameObject.SetActive (false);
	}
	
	void ActivateTweenPosition ( UIButton button , bool val )
	{
		if ( button == null )
			return;
		button.transform.parent.gameObject.SetActive (val);
	}

	void PlayAnim(bool val)
	{
		TweenScale tweenPos = buttonOptions.gameObject.GetComponent<TweenScale> ();
		if ( tweenPos == null )
			return;

		if ( val )
		{
			tweenPos.method = UITweener.Method.BounceOut;
			tweenPos.PlayReverse ();
		}
		else
		{
			tweenPos.method = UITweener.Method.BounceIn;
			tweenPos.PlayForward ();
		}
	}

	public void ToogleOptionButtons()
	{
		ActivateTweenPosition(buttonHelp,!bOptionsShown);
		ActivateTweenPosition(buttonMainMenu,!bOptionsShown);
		ActivateTweenPosition(buttonPrizes,!bOptionsShown);
		ActivateTweenPosition(buttonCredits,!bOptionsShown);
		ActivateTweenPosition(buttonAchievement,!bOptionsShown);
		//ActivateTweenPosition(buttonSocial,!bOptionsShown);
		ActivateTweenPosition(buttonSound,!bOptionsShown);
		//ActivateTweenPosition(buttonMission,!bOptionsShown);
		
		CheckXIcon();
		PlayAnim(bOptionsShown);
		bOptionsShown = !bOptionsShown;

	}

	bool bOptionsShown = false;
	public void OnClickMenuButton()
	{
		CheckXIcon ();
		UIButton button = UIButton.current;
		if ( button == null )
			return;

		//Debug.LogError (button.name+" "+button.transform.parent.name);
		if ( buttonCredits && button.gameObject == buttonCredits.gameObject )
		{
			if ( guiCredits )
			{
				GUI_Credits gui_credits = guiCredits.GetComponent<GUI_Credits>();
				GUI_Dialog.InsertStack(gui_credits.gameObject);
				//gui_credits.Show();
			}
		}
		if  ( buttonSocial && button.gameObject == buttonSocial.gameObject )
		{
			if ( guiShare )
			{
				GUI_Share gui_share = guiShare.GetComponent<GUI_Share>();
				
				GUI_Dialog.InsertStack(gui_share.gameObject);
				//gui_share.Show();
			}
		}
		else if ( buttonMainMenu && button.gameObject == buttonMainMenu.gameObject )
		{
			Gamestate gs = GameObject.FindGameObjectWithTag("Gamestate").GetComponent<Gamestate>();
			Debug.Log (gs);
			if ( gs )
			{
				gs.ShowDialogBox("Confirmation","Are you sure you want to go to main menu ?",true,"gotomainmenu",this.gameObject);
			}
			ToogleOptionButtons();
		}
		else if ( buttonPrizes && button.gameObject == buttonPrizes.gameObject )
		{
			if ( guiPrizes )
			{
				GUI_PrizeCollection gui_Prize = guiPrizes.GetComponent<GUI_PrizeCollection>();
				GUI_Dialog.InsertStack(gui_Prize.gameObject);
			}
			ToogleOptionButtons();
		}
		else if ( buttonAchievement && button.gameObject == buttonAchievement.gameObject )
		{
			if ( guiAchievements )
			{
				Social.ShowAchievementsUI();
				//GUI_Achievement gui_achievement = guiAchievements.GetComponent<GUI_Achievement>();
				//GUI_Dialog.InsertStack(gui_achievement.gameObject);

				//gui_achievement.Show();
			}
			ToogleOptionButtons();
		}
		else if ( buttonSound && button.gameObject == buttonSound.gameObject )
		{
			GameManager.bSoundOn = !GameManager.bSoundOn;
			CheckXIcon();
		}
		else if ( buttonMission && button.gameObject == buttonMission.gameObject )
		{
			GUI_Dialog.InsertStack(guiMission.gameObject);
			ToogleOptionButtons();
		}
		else if ( buttonOptions && button.gameObject == buttonOptions.gameObject )
		{
			ToogleOptionButtons();
		}
		else if ( buttonHelp && button.gameObject == buttonHelp.gameObject )
		{
			GUI_Dialog.InsertStack(guiTutorial.gameObject);
			ToogleOptionButtons();
		}
	}

	void CheckXIcon()
	{
		Transform trXIcon = buttonSound.transform.parent.Find("Icon X");
		if ( trXIcon )
		{
			if ( GameManager.bSoundOn )
			{
				trXIcon.gameObject.SetActive(false);
			}
			else
			{
				trXIcon.gameObject.SetActive(true);
			}
		}
		GameManager.SetupAudio ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
