using UnityEngine;
using System.Collections;

public class sc_firering : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<ParticleSystem>().emissionRate = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void nyalakan_api() {
		GetComponent<ParticleSystem>().emissionRate = 208;
	}
	void matikan_api() {
		GetComponent<ParticleSystem>().emissionRate = 0;
	}
}
