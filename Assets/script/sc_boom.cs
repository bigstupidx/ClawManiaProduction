using UnityEngine;
using System.Collections;

public class sc_boom : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void nyalakan_boom() {
		gameObject.GetComponent<ParticleSystem>().Play ();
	}

}
