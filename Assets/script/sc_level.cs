using UnityEngine;
using System.Collections;

public class sc_level : MonoBehaviour {
	int waktu=0;
	
	// Use this for initialization
	void Start () {
		this.GetComponent<GUIText>().text = "Level : " + PlayerPrefs.GetInt(PlayerPrefHandler.keyLevelAktif);
	}
	
	
	// Update is called once per frame
	void Update () {
		waktu++;
		if(waktu==40) {
			Application.LoadLevel("3Dbasketball");
		}
	}
}
