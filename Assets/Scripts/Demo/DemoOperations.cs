using UnityEngine;
using System.Collections;

public class DemoOperations : MonoBehaviour
{
	void Start ()
	{
	}
	
	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
	}
}
