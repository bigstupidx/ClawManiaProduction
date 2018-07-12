using UnityEngine;
using System.Collections;

public class RestifizerErrorHandler : MonoBehaviour, Restifizer.IErrorHandler {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	bool Restifizer.IErrorHandler.onRestifizerError(Restifizer.RestifizerError restifizerError)
	{
		Debug.Log("Restifizer: ERROR = " + restifizerError);
		return true;
	}
}
