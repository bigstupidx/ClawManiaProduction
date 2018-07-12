using UnityEngine;
using System.Collections;

public class REST_API_Handler : MonoBehaviour {
	public static REST_API_Handler instance;

	public const string baseURL = "http://api.gemugemu.com/";

	public const string loginSubURL = "login/";
	public const string registerSubURL = "register/";

	void Awake () 
	{
		if (!instance)
		{
			DontDestroyOnLoad(this.gameObject);
			instance = this;
		}
		else if (instance != this)
		{
			DestroyImmediate(this.gameObject);
		}
	}

	void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//public 
}
