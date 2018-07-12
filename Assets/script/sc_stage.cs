using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sc_stage : MonoBehaviour {
	public int levelAktif=1;
	Text textLevel;

	void Start()
	{

	}

	// Use this for initialization
	void Awake () {
		levelAktif = PlayerPrefs.GetInt(PlayerPrefHandler.keyLevelAktif);
		//this.guiText.text = "Level : " + levelAktif;
		textLevel = GetComponent<Text> ();
		textLevel.text = "STAGE " + levelAktif;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
