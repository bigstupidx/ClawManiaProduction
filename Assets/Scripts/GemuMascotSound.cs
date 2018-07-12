using UnityEngine;
using System.Collections;

public class GemuMascotSound : MonoBehaviour {

	private AudioSource audioSrc;

	// Use this for initialization
	void Start () {
		audioSrc = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTapMascot()
	{
		audioSrc.Play();
	}
}
