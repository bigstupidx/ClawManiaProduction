using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TicketsHandler : MonoBehaviour {

	public Text txtTickets;

	// Use this for initialization
	void Start () {
		txtTickets.text = PlayerPrefs.GetInt(PlayerPrefHandler.keyUserTiket, 0).ToString();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
