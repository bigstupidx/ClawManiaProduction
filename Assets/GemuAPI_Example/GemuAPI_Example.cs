using UnityEngine;
using System.Collections;

public class GemuAPI_Example : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//test register
/*
		Hashtable parameters = new Hashtable();
		parameters.Add("full_name", "Bawenang Test");
		parameters.Add("username", "bawetest");
		parameters.Add("password", "newPassword");
		parameters.Add("email", "bawenang@hotmail.com");
		parameters.Add("tiket", "0");

		GemuAPI.Register(parameters);
*/

		//test login

		Hashtable parameters = new Hashtable();
		parameters.Add("username", "bawetest");
		parameters.Add("password", "newPassword");
		parameters.Add("gameid", "2000");
		parameters.Add("ip", "127.0.0.1");
		
		GemuAPI.Login(parameters);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
