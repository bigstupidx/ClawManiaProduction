using UnityEngine;
using System.Collections;

public class sc_scoreboard : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//this.renderer.material.Set
		Color color = this.GetComponent<Renderer>().material.color;
		color.a = 0.8f;
		this.GetComponent<Renderer>().material.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
