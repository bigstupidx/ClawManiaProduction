using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class sc_coin_user : MonoBehaviour {

	public GameObject[] coins = new GameObject[5];
	public UILabel extraCoinText;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateCoins(int amount)
	{
		for (int i = 0; i< coins.Length; ++i)
		{
			coins[i].SetActive(false);
		}

		if (amount > 5)
		{
			for (int i = 0; i< coins.Length; ++i)
			{
				coins[i].SetActive(true);
			}

			extraCoinText.text = (amount - 5).ToString();
		}
		else
		{
			for (int i = 0; i< amount; ++i)
			{
				coins[i].SetActive(true);
			}

			if ( extraCoinText )
				extraCoinText.text = "0";
		}
	}
}
