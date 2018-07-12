using UnityEngine;
using System.Collections;

public static class GameManager 
{
	public static bool separateAPKs = true;
	public static int LayerGUI = 5;
	public static int LayerContent = 9;
	public static int LayerClaw = 10;
	public static int LayerJoystickController = 11;
	public static int LayerGrabButton = 12;
	public static int LayerCamController = 13;
	public static int LayerMachineBorder = 14;
	public static int LayerSuccessfullPrize = 15;

	public static int GRAB_COIN_REDUCED = 25; //5 or 25 for now
	public static int AMOUNT_PRIZE_COLLECTOR = 500;
	public static int MAX_TIMER_SERIOUS_GRABBER = 60 * 60; // in seconds
	public static int MAX_TIMER_COIN = (int)(60 * 5); // in seconds
	public static int MAX_FREECOIN_FROM_TIMER = 200;
	public static int TIMER_ADD_COIN = 10;
	public static bool bSoundOn = true;

	public static int EXP = 0;
	public static int GEMUCOINS = 0; // not original GG, but changed to ingame currency
	public static int FREECOINS = 1;
	public static int TICKET = 0;
    public static int startingFunds = 200;

	public static string PREF_SETTING_AUDIO = "setting_audio";
	public static string PREF_PROCEDURAL_MISSION_PRIZE_AMOUNT = "proceduralMissionPrizeAmount";
	public static string PREF_CURR_USER_PROCEDURAL_MISSION_PRIZE_AMOUNT = "currProceduralMissionPrizeAmount";
	public static string PREF_PROCEDURAL_MISSION_COIN_AMOUNT = "proceduralMissionCoinAmount";
	public static string PREF_CURR_USER_PROCEDURAL_MISSION_COIN_AMOUNT = "currProceduralMissionCoinAmount";
	public static ArrayList listExpToLevel;

	public static string keyClawMania_Energy = "freecoins";
	public static string keyClawMania_Level = "level";

	public static string skuEnergy200 = "clawmania.200energy";
	public static string skuEnergy500 = "clawmania.500energy";
	public static string skuEnergy1000 = "clawmania.1000energy";

	public static string skuClawRed = "clawmania.claw2";
	public static string skuClawMetallic = "clawmania.claw3";

	public static string skuJoystickMecha = "clawmania.joystick2";
	public static string skuJoystickRound = "clawmania.joystick3";

	public static string clawName_Default = "The Claw";
	public static string clawName_Red = "The Red Claw";
	public static string clawName_Metallic = "The Metallic Claw";
	public static string joystickName_Default = "The Stick";
	public static string joystickName_Mecha = "The Mecha";
	public static string joystickName_RoundTable = "The Round Table";
	public static string powerUpName_Bomb = "Bomb";
	public static string powerUpName_LuckyCharm = "Lucky Charm";
	public static string powerUpName_LaserPointer = "Laser Pointer";
	public static string powerUpName_WizardWand = "Wizard Wand";
	public static string powerUpName_BlackHole = "Black Hole";

	public static void SetNGUILabel(Transform transform, string sText)
	{
		if ( transform == null )
			return;

		UILabel label = transform.gameObject.GetComponent<UILabel> ();
		if ( label == null )
			return;

		label.text = sText;
	}

	public static void SetEnergy(int val)
	{

	}

	public static void SetupAudio()
	{
		if ( bSoundOn )
		{
			NGUITools.soundVolume = 1;
			PlayerPrefs.SetInt (PREF_SETTING_AUDIO, 1);
		}
		else
		{
			NGUITools.soundVolume = 0;
			PlayerPrefs.SetInt (PREF_SETTING_AUDIO, 0);
		}
		Gamestate gs = GameObject.FindGameObjectWithTag ("Gamestate").gameObject.GetComponent<Gamestate>();
		if ( gs )
		{
			gs.GetComponent<AudioSource>().enabled = bSoundOn;
			if ( gs.GetComponent<AudioSource>().enabled && gs.GetComponent<AudioSource>().isPlaying == false )
				gs.GetComponent<AudioSource>().Play();
		}
		else
			Debug.LogError ("SetupAudio fail");
	}

	public static float getProgressValue ()
	{
		if ( listExpToLevel == null )
			return 0;

		int minExp = (int)listExpToLevel [getLevelValue () - 1];
		int maxExp = (int)listExpToLevel [getLevelValue () ];
		int currExp = GameManager.EXP - minExp;
		float fProgress = (float)currExp / ((float)maxExp - (float)minExp);
		//Debug.LogError ("minExp=" + minExp + " curr=" +currExp+ " maxExp=" + maxExp + " progress="+fProgress);
		return fProgress;
	}

	public static string convertIntToStringTime(int value, bool getHour)
	{
		int iHour = (int)(value / 3600);
		int iMinute = (int)((value - (iHour*3600)) / 60);
		int iSecond = (int)(value - (iMinute * 60) - (iHour*3600));
		
		string sHour = iHour.ToString ();
		if ( sHour.Length < 2 )
			sHour = "0"+sHour;

		string sMinutes = iMinute.ToString ();
		if ( sMinutes.Length < 2 )
			sMinutes = "0"+sMinutes;

		string sSeconds = iSecond.ToString ();
		if ( sSeconds.Length < 2 )
			sSeconds = "0"+sSeconds;

		if (getHour)
			return sHour + ":" + sMinutes + ":" + sSeconds;
		else
			return sMinutes + ":" + sSeconds;
	}

	public static int getEpochTime()
	{
		System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
		return (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
	}

	public static int getLevelValue ()
	{
		if ( listExpToLevel == null )
		{
			listExpToLevel = new ArrayList();
			for ( int i=0; i<99; i++ )
			{
				int expToLvUp = 100+(i*20);
				int prevExp = 0;
				int exp = 0;
				if ( i != 0 )
				{
					prevExp = (int) listExpToLevel[i-1];
					exp = prevExp + expToLvUp;
				}
				//Debug.LogError("lv="+(i+1)+" expNeeded="+exp);
				listExpToLevel.Add(exp);
			}
		}
		int Level = 1;
		for ( int i=0; i<listExpToLevel.Count ; i++ )
		{
			int limitExp = (int)listExpToLevel[i];
			//Debug.LogError("i="+i+" limitExp="+limitExp);
			
			Level = i;
			if (  limitExp > GameManager.EXP )
			{
				break;
			}
		}
		//Debug.LogError("my exp = "+GameManager.EXP+" lv="+Level);
		return Level;
	}

}
