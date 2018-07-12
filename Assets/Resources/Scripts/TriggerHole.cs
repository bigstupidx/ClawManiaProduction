using UnityEngine;
using System.Collections;

public class TriggerHole : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider collider)
	{
		//Debug.LogError ("[TriggerHole] collider=" + collider.name);
		if ( collider.gameObject.layer == GameManager.LayerContent )
		{
			Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").gameObject.GetComponent<Gamestate_Gameplay>();
			if ( gs )
			{
				gs.OnDropSuccess(collider.gameObject);
			}
		}
	}
}
