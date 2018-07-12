		using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class sc_timer_coins : MonoBehaviour {
	public UILabel waktuTambahCoins;
	//Text jumlahCoinsUser;

	public sc_coin_user coinUserHandler;

	void Awake() {
		//jumlahCoinsUser = GameObject.Find ("txtCoinsUser").GetComponent<Text> ();
	}

	// Use this for initialization
	void Start () {
		//waktuTambahCoins = this.GetComponent<Text>();
		updateCoins();
	}

	/*
	private void max_five_coins_guest() {
		if (PlayerPrefs.GetInt ("facebook_stat") == 0 &&
		    PlayerPrefs.GetInt ("gemu_stat") == 0 &&
		    PlayerPrefs.GetInt ("gplus_stat") == 0) {
			
			if(countCoin>5) {
				countCoin=5;
				//waktuCount = 0;
				//isTimerOn = false;
				//jumlahCoinsUser.text = "X " + countCoin.ToString();
				PlayerPrefs.SetInt("coins_user", countCoin);
				PlayerPrefs.Save();
			}
		}
	}
	*/

	
	// Update is called once per frame
	void Update () {
		updateCoins();

		if (waktuTambahCoins != null)
			waktuTambahCoins.text = CoinTimerHandler.FormatJam (Mathf.Floor (CoinTimerHandler.instance.GetTimeDiff()));
	}
	
	public void updateCoins() {
		if (CoinTimerHandler.instance == null)
			return;
		int coinUser = CoinTimerHandler.instance.countCoin;
		//Debug.Log ("SHOW COINS GUEST ::::::::::: " + PlayerPrefs.GetInt (PlayerPrefHandler.keyCoin));
			//jumlahCoinsUser.text = "X " + coinUser.ToString ();
		coinUserHandler.UpdateCoins(coinUser);

	}

}
