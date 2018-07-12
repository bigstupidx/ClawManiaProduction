using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUI_InGame_New : MonoBehaviour {

	public static GUI_InGame_New instance;

	public Text coinText;

	public Transform[] powerupObj = new Transform[5];

	// Use this for initialization
	void Start () {
		instance=this;
	}

	public void refreshInGamePowerUpInfo (ShopContent content)
	{
		string sKey = content.Name + "_Amount";
		int iAmount = PlayerPrefs.GetInt (sKey, 0);

		int idx = 0;

		if (content.Name == GameManager.powerUpName_Bomb) {
			idx=0;
		} 
		else if (content.Name == GameManager.powerUpName_LuckyCharm) {
			idx=1;
		} 
		else if (content.Name == GameManager.powerUpName_LaserPointer) {
			idx=2;
		} 
		else if (content.Name == GameManager.powerUpName_WizardWand) {
			idx=3;
		} 
		else if (content.Name == GameManager.powerUpName_BlackHole) {
			idx=4;
		} 

		powerupObj[idx].GetChild(0).GetComponent<Text>().text = iAmount.ToString();
	}

	public void refreshInGameCoinInfo(){
		coinText.text = PlayerPrefs.GetInt(PlayerPrefHandler.keyCoin,0).ToString();
	}
}
