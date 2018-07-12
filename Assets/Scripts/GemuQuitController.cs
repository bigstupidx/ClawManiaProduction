using UnityEngine;
using System.Collections;

public class GemuQuitController : GUI_Dialog {

	void Awake()
	{

	}

	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnYesButton()
	{
		SoundManager.instance.PlayButton();
		GameDataManager.instance.SaveData();
		Application.Quit();
	}

	public void OnNoButton()
	{
		SoundManager.instance.PlayButton();
		GUI_Dialog.ReleaseTopCanvas ();
	}
}
