using UnityEngine;
using System.Collections;

public class TestRope : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 pos = this.transform.position;

		pos.x += Input.GetAxis ("Horizontal")*Time.deltaTime;

		this.transform.position = pos;
	}
}
