using UnityEngine;
using System.Collections;

public class sc_mainmenu_canvas_handler : MonoBehaviour {

	private static sc_mainmenu_canvas_handler instance = null;

	public GUI_BasketQuit cvQuit;
	public GUI_BasketSetting cvSetting;
	public GUI_BasketCredits cvCredits;
	public GUI_BasketHTP cvHtp;
	public GemuRegisterGemuController cvSignup;
	public GemuTicketsController cvTiket;
	public GemuUserDataController cvGemuData;
	public GemuLoginGemuController cvLogin;
	public GemuPromoCode cvGemuPromo;
	public GemuCoinShopController cvShop;
	public Canvas cvPayment;
	public Canvas cvMoreGames;
	public Canvas cvHighscore;
	public Canvas cvDialogBox;

	void Awake()
	{
		if (instance == null)
		{
			instance = this;

			//cvQuit.gameObject.SetActive(false);
			//cvSetting.gameObject.SetActive(false);
			//cvLogin.gameObject.SetActive(false);
			//cvShop.gameObject.SetActive(false);
			//cvPayment.gameObject.SetActive(false);
			//cvSignup.gameObject.SetActive(false);
			//cvMoreGames.gameObject.SetActive(false);
			//cvTiket.gameObject.SetActive(false);
			//cvCredits.gameObject.SetActive(false);
			//cvHtp.gameObject.SetActive(false);
			//cvHighscore.gameObject.SetActive(false);
			//cvDialogBox.gameObject.SetActive(false);
			//cvGemuData.gameObject.SetActive(false);
			//cvGemuPromo.gameObject.SetActive(false);
		}
	}

	// Use this for initialization
	void Start () {
	
	}

	public void ShowDialogBox(string text, bool showCancel)
	{
		if ( cvDialogBox && cvDialogBox.GetComponent<sc_dialogbox>() )
		{
			cvDialogBox.gameObject.SetActive(true);
			cvDialogBox.GetComponent<sc_dialogbox>().Show(text,showCancel);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}

	public static sc_mainmenu_canvas_handler Instance()
	{
		return instance;
	}
}
