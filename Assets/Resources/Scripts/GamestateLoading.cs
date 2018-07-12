using UnityEngine;
using System.Collections;

[System.Serializable]
public class GamestateLoading : MonoBehaviour 
{
	public GameObject obLogoNiji;
	public GameObject obLogoGameLevelOne;
	public Transition transition;
	// Use this for initialization
	void Start () {
		Debug.LogError ("[Gamestate_Loading] Start");
		Screen.orientation = ScreenOrientation.Portrait;
		GameManager.SetupAudio ();
		obLogoNiji.gameObject.SetActive (false);
		obLogoGameLevelOne.gameObject.SetActive (false);
		StartCoroutine (Waiting ());
	}

	IEnumerator Waiting()
	{
		yield return new WaitForSeconds (1);
		//StartCoroutine (ShowingLogos ());
		yield break;
	}

	IEnumerator ShowingLogos()
	{
		transition.Setup ();
		Debug.LogError ("[ShowingLogos] 1");
		obLogoGameLevelOne.gameObject.SetActive (true);
		
		Debug.LogError ("[ShowingLogos] 2");
		transition.Show (Transition.TransitionMode.EaseOut);
		yield return new WaitForSeconds (3);
		
		Debug.LogError ("[ShowingLogos] 3");
		transition.Show (Transition.TransitionMode.EaseIn);
		while ( transition.IsDone == false )
			yield return null;
		
		Debug.LogError ("[ShowingLogos] 4");
		
		obLogoNiji.gameObject.SetActive (true);
		obLogoGameLevelOne.gameObject.SetActive (false);

		yield return new WaitForSeconds (1);
		transition.Show (Transition.TransitionMode.EaseOut);

		while ( transition.IsDone == false )
			yield return null;
		
		obLogoNiji.gameObject.SetActive (false);
		obLogoGameLevelOne.gameObject.SetActive (false);

		foreach (GameObject o in GameObject.FindObjectsOfType<GameObject>()) {
			GameObject.Destroy(o);
		}
		
		transition.Show (Transition.TransitionMode.EaseIn);
		//Application.LoadLevelAdditive ("Menu");
		Debug.LogError ("[Gamestate_Loading] Done");
		yield break;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
	