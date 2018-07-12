using UnityEngine;
using System.Collections;

public sealed class GamePreferences : MonoBehaviour {

	//pref for timer (TimeStamp)
	public const string Key_AdsTimer 		= "AdsTimer";
	public const string Key_FreeGiftTimer 	= "FreeGiftTimer";

	//pref using INT
	public const string Key_IsNetAvailable 	= "IsNetAvailable";
	public const string Key_IsCheat 		= "IsCheat";
	
	public const string Key_FreeGiftCode 	= "FreeGiftCode";

	#region GameData Key
	public const string Key_Coin  	 = "HaveCoin";
	public const string Key_Distance = "LongestDistance";
	#endregion

	#region scene name
	public const string Key_Scene_Splash 	 = "SplashScene";
	public const string Key_Scene_MainMenu 	 = "MenuScene";
	public const string Key_Scene_Collection = "CollectionScene";
	public const string Key_Scene_Credt 	 = "CreditScene";
	public const string Key_Scene_Gacha 	 = "GachaScene";
	public const string Key_Scene_Game 		 = "GameScene";
	#endregion
	
	//NOT a Pref! Constant Value Only
	public const int Value_BasePower = 2;

}
