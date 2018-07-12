using UnityEngine;
using System.Collections;

[System.Serializable]
public class GamestateMainMenu : Gamestate 
{
	public UIPanel guiMainMenu;
	public UILabel labelLv;
	// Use this for initialization
	void Start () 
	{
		Debug.LogError ("[Gamestate_Menu] StartGamestate");
		if ( PlayerPrefs.HasKey("exp" ) )
			GameManager.EXP = PlayerPrefs.GetInt("exp");
		
		GameManager.SetupAudio ();
		labelLv.text = GameManager.getLevelValue ().ToString();
		string sFBID = "";
		if (SPFacebook.instance.IsInited && SPFacebook.instance.IsLoggedIn)
			sFBID = SPFacebook.instance.UserId;
		gl1Connector.RequestUserData (this.gameObject,gl1Connector.GetCurrUser(),gl1Connector.GetToken(), SystemInfo.deviceUniqueIdentifier,sFBID);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( Input.GetKey("escape")  )
		{
			Debug.LogError("test1");
			if (guiDialogBox.isVisible() == false )
			{
				guiDialogBox.Show("Confirmation","Are you sure you want to exit ?",true,"exitgame",this.gameObject);
			}
		}
	}

	IEnumerator Playing()
	{
		while ( transition.IsDone == false )
			yield return null;

		Application.LoadLevel ("SceneGameplay");
		yield break;
	}

	public void OnClickPlay()
	{
		transition.Show (Transition.TransitionMode.EaseIn);
		StartCoroutine (Playing ());
	}

	public void OnGL1Done(SimpleJSON.JSONNode N)
	{
		labelLv.text = GameManager.getLevelValue ().ToString();
	}
}
