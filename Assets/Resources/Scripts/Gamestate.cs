using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Gamestate : MonoBehaviour 
{
	public GL1Connector gl1Connector;
	public AchievementManager achievementManager;
	public GameObject luckyCharmEffect;
	public GUI_DialogBox guiDialogBox;
	public Transition transition;
	// Use this for initialization
	public virtual void StartGamestate ()
	{
		Debug.LogError ("[Gamestate] StartGamestate");
		//StartCoroutine (HideTransition ());
	}

	IEnumerator HideTransition()
	{
		while ( transition == null )
		{
			yield return null;
		}
		
		while ( transition.IsDone == false )
		{
			yield return null;
		}

		SendMessage ("OnTransitionDone", this.gameObject);
	}

	public void GoToScene ( string sScene )
	{
		StartCoroutine (GoingToOtherScene (sScene));
	}

	IEnumerator GoingToOtherScene(string sScene)
	{
		if ( transition )
		{
			transition.Show (Transition.TransitionMode.EaseIn);
			while ( transition.IsDone == false )
				yield return null;
		}
	}
	
	public void ShowDialogBox(string sTitle,string sText,bool bCancel,string sExec,GameObject sender)
	{
		guiDialogBox.Show (sTitle, sText, bCancel, sExec,sender);
		guiDialogBox.PlayAudio (guiDialogBox.audioError);
	}
	
	public void ShowNotImplemented()
	{
		ShowDialogBox("Info","This feature has not been implemented yet.",false,"",this.gameObject);
		guiDialogBox.PlayAudio (guiDialogBox.audioError);
	}

	public virtual void WhenGL1Done()
	{

	}

	public void OnGL1Done(JSONNode N)
	{
		if ( gl1Connector.GetLastURL().Contains("getuser") )
		{
			if ( N["errcode"].ToString() == "\"0\"" )
			{
				var userdata = JSONNode.Parse(N["userdata"].ToString());
				if ( userdata != null )
				{
					int Tiket = (int)float.Parse(userdata["tiket"]);
					int GemuCoins = (int)float.Parse(userdata["coin"]);
					GameManager.GEMUCOINS = GemuCoins;
					GameManager.TICKET = Tiket;
					Debug.LogError ("[Gamestate] OnGL1Done Done coins="+GameManager.GEMUCOINS+" ticket="+GameManager.TICKET);
					PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin,GameManager.GEMUCOINS);
					PlayerPrefs.SetInt(PlayerPrefHandler.keyUserTiket,GameManager.TICKET);
				}
			}
			else
			{
				
				Debug.LogError ("[Gamestate] OnGL1Done Error");
			}
		}
		WhenGL1Done ();
	}
}
