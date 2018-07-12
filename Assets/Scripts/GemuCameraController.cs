using UnityEngine;
using System.Collections;

public class GemuCameraController : MonoBehaviour {

	Camera cam;

	void Awake()
	{
		cam = GetComponent<Camera>();
		float camSize = cam.orthographicSize;

		Debug.Log("Width: " + (Screen.currentResolution.width ).ToString());
		Debug.Log("Height: " + (Screen.currentResolution.height).ToString());
		Debug.Log("Aspect Ratio: " + (((float)Screen.currentResolution.width) / ((float)Screen.currentResolution.height)).ToString());

		//camSize = camSize * 1.6f / cam.aspect;

		//cam.orthographicSize = camSize;


	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
