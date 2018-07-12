using UnityEngine;
using System.Collections;

public class Game_Content : MonoBehaviour 
{
	public string sGameID;
	public UIProgressBar progressBar;
	// Use this for initialization
	void Start () {
		progressBar.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
