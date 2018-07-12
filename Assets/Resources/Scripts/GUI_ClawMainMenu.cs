using UnityEngine;
using System.Collections;

public class GUI_ClawMainMenu : MonoBehaviour {

	public Transition transition;
	public GL1Connector gl1Connector;
	public UILabel labelLv;
	public UILabel energyLbl;
	public UILabel gpLbl;
	public UILabel ggLbl;
	// Use this for initialization
	void Start () {
	
	}

	public void RefreshLevelInfo()
	{
		GameManager.SetNGUILabel (labelLv.transform, GameManager.getLevelValue().ToString ());
		//GameManager.SetNGUILabel (energyLbl.transform, GameManager.FREECOINS.ToString ());
		//GameManager.SetNGUILabel (gpLbl.transform, GameManager.TICKET.ToString ());
		//GameManager.SetNGUILabel (ggLbl.transform, GameManager.GEMUCOINS.ToString ());
	}

	public void RefreshInfo()
	{
		if ( PlayerPrefs.HasKey("exp" ) )
			GameManager.EXP = PlayerPrefs.GetInt("exp");
		
		GameManager.SetupAudio ();
		//gl1Connector.RequestUserData (this.gameObject,gl1Connector.GetCurrUser(),gl1Connector.GetToken(),"",SystemInfo.deviceUniqueIdentifier,sFBID);

	}
	
	public void OnClickPlay()
	{
		transition.Show (Transition.TransitionMode.EaseIn);
		StartCoroutine (Playing ());
	}
	
	IEnumerator Playing()
	{
		while ( transition.IsDone == false )
			yield return null;

		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay> ();
		if ( gs )
		{
			gs.gsMode = Gamestate_Mode.Gameplay;
			gs.StartGame();
		}
		yield break;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
