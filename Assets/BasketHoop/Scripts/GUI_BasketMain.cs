using UnityEngine;
using System.Collections;

public class GUI_BasketMain : MonoBehaviour {
	public GemuCoinShopController basketShop;
	// Use this for initialization
	void Start () {
		//GemuAPI.
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClickShop()
	{
		if ( basketShop == null )
			return;
		basketShop.gameObject.SetActive (true);
		GUI_Dialog.InsertStack (basketShop.gameObject);
		//basketShop.Show ();
	}
}
