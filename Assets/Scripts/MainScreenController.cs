using UnityEngine;
using System.Collections;

public class MainScreenController : MonoBehaviour 
{
	public Transition transition;
	void Awake()
	{
		//Application.runInBackground = true;
	}


	// Use this for initialization
	void Start () {
		GUI_Dialog.ClearStack ();
		//GameDataManager.GEMU_APP_ID = "1431032373";
		transition.Show (Transition.TransitionMode.EaseOut);
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		if (PlayerPrefs.HasKey (PlayerPrefHandler.keyCoin) == false) {
			PlayerPrefs.SetInt (PlayerPrefHandler.keyCoin, 5);
			CoinTimerHandler.instance.countCoin = 5;
		} else {
			CoinTimerHandler.instance.countCoin = PlayerPrefs.GetInt (PlayerPrefHandler.keyCoin);
			//CoinTimerHandler.instance.countCoin = 0;
		}
		if (PlayerPrefs.HasKey (PlayerPrefHandler.keyUserTiket) == false) {
			PlayerPrefs.SetInt(PlayerPrefHandler.keyUserTiket,0);
		}
		//GameDataManager.instance.SendPlayResult(GameDataManager.GEMU_APP_ID,"0","50","0","0");
		//GameDataManager.instance.LoadData ();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
