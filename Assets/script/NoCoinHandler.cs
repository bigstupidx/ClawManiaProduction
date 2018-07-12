using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NoCoinHandler : MonoBehaviour {

	public Text textTimer;
	public GemuCoinShopController gemuCoin;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		textTimer.text = CoinTimerHandler.FormatJam(Mathf.Floor (CoinTimerHandler.instance.GetTimeDiff()));
	}

	public void OnBuyButton()
	{
		Debug.LogError ("test");
		SoundManager.instance.PlayButton();
		gemuCoin.gameObject.SetActive (true);
		GUI_Dialog.InsertStack(gemuCoin.gameObject);
		this.gameObject.SetActive (false);
	}

	public void OnAskFriendButton()
	{
		SoundManager.instance.PlayButton();
	}

	public void OnClose()
	{
		SoundManager.instance.PlayButton();
		this.gameObject.SetActive(false);
	}
}
