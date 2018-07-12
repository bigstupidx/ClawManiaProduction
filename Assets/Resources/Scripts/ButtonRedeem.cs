using UnityEngine;
using System.Collections;

public class ButtonRedeem : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnClick()
	{
		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay> ();
		if ( gs )
		{
			gs.ShowNotImplemented();
		}
	}
}
