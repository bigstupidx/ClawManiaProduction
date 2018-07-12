using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GemuCoinShopController : GUI_Dialog {

	public UILabel coinText;
	public GemuDialogBoxController dialogBox;
	public GameObject[] buttonCoins;
	static GemuCoinShopController instance;
	public GemuFreeCoins uiFreeCoins;
	void Awake()
	{
		instance = this;
		for ( int i=0; i<buttonCoins.Length; i++ )
		{
			Transform trCurrency = buttonCoins[i].transform.Find("Currency");
			if ( trCurrency )
			{
				trCurrency.gameObject.SetActive(false);
			}
		}
	}

	void OnDestroy()
	{
		instance = null;
	}

	public static GemuCoinShopController GetInstance()
	{
		return instance;
	}

	// Use this for initialization
	public override void OnStart () {
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.25coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.50coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.100coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.180coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.300coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.500coins");
		AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;  
		AndroidInAppPurchaseManager.ActionProductConsumed  += OnProductConsumed;
		
		//listening for store initilaizing finish
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
		AndroidInAppPurchaseManager.instance.loadStore();
	}

	private static void OnProductPurchased(BillingResult result) 
	{
		if(result.isSuccess) 
		{
			AndroidInAppPurchaseManager.instance.consume (result.purchase.SKU);
			
		} 
		else 
		{
			// product has been purchased, but not consumed yet
			if ( result.response == 7 )
			{
				AndroidInAppPurchaseManager.instance.consume (result.purchase.SKU);
				
			}
		}
		
		Debug.Log ("Purchased Responce: " + result.response.ToString() + " " + result.message);
	}
	
	private static void OnProductConsumed(BillingResult result)
	{
		if(result.isSuccess) 
		{
			int iAddCoin = 0;
			if ( result.purchase.SKU == "gl1.gemugemu.25coins" )
				iAddCoin = 25;
			else if ( result.purchase.SKU == "gl1.gemugemu.50coins" )
				iAddCoin = 50;
			else if ( result.purchase.SKU == "gl1.gemugemu.100coins" )
				iAddCoin = 100;
			else if ( result.purchase.SKU == "gl1.gemugemu.180coins" )
				iAddCoin = 180;
			else if ( result.purchase.SKU == "gl1.gemugemu.300coins" )
				iAddCoin = 300;
			else if ( result.purchase.SKU == "gl1.gemugemu.500coins" )
				iAddCoin = 500;
			//GUI_Shop.GetInstance().OnSuccessfulPurchase(result.purchase.SKU);
			int iCurrCoin = PlayerPrefs.GetInt(PlayerPrefHandler.keyCoin);
			iCurrCoin += iAddCoin;
			GameDataManager.instance.SendPlayResult(GameDataManager.instance.gameID.ToString(),"0",iCurrCoin.ToString(),"0","0");
			// TODO : Fixthis dialogBox.Show("Info","You bought "+iAddCoin+" Coins",false,"",null);
			//GL1Connector.GetInstance().AddBalance(this.gameObject,"0",iAddCoin.ToString(),"");
		} 
		else 
		{
		}
		Debug.LogError ("OnProductConsumed "+result.response.ToString());
	}

	private static void OnBillingConnected(BillingResult result) 
	{
		AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnBillingConnected;
		
		
		if(result.isSuccess) {
			AndroidInAppPurchaseManager.instance.retrieveProducDetails();
			AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnRetrieveProductsFinised;
		} 
		
		Debug.Log ("[GemuCoinShopController] Connection Response: " + result.response.ToString() + " " + result.message);
	}
	
	private static void OnRetrieveProductsFinised(BillingResult result) {
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished -= OnRetrieveProductsFinised;
		
		
		if(result.isSuccess) {
			
			foreach(GoogleProductTemplate tpl in AndroidInAppPurchaseManager.instance.inventory.products) 
			{
				for ( int i=0; i<GemuCoinShopController.GetInstance().buttonCoins.Length ; i++ )
				{
					GameObject content = GemuCoinShopController.GetInstance().buttonCoins[i];
					if ( tpl.SKU.Equals(content.name) )
					{
						Transform trCurrency = content.transform.Find("Currency");
						if ( trCurrency )
						{
							UILabel text = trCurrency.GetComponent<UILabel>();
							text.text = tpl.price;	
							trCurrency.gameObject.SetActive(true);
						}
						Transform trTitle = content.transform.Find("Title");
						if ( trTitle )
						{
							Text text = trTitle.GetComponent<Text>();
							//text.text = tpl.title;
						}

						//content.gameObject.SetActive(true);
						break;
					}
				}
			}
		} else {
			//AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
		}
		
		Debug.Log ("[GemuCoinShopController] Connection Response: " + result.response.ToString() + " " + result.message);
		
	}
	// Update is called once per frame
	void Update () {
		if ( coinText && CoinTimerHandler.instance  )
		coinText.text = CoinTimerHandler.instance.countCoin.ToString();
	}

	void OnEnable()
	{
		if ( coinText && CoinTimerHandler.instance )
		coinText.text = CoinTimerHandler.instance.countCoin.ToString();
	}

	public void OnBackButton()
	{
		SoundManager.instance.PlayButton();

		GUI_Dialog.ReleaseTopCanvas();

	}

	public void OnBuyCoin() //Type = 0 - 5
	{
		UIButton button = UIButton.current;
		if (button == null)
			return;

		SoundManager.instance.PlayButton();
		AndroidInAppPurchaseManager.instance.purchase (button.name);
		//paymentController.currentType = type;
		//paymentController.gameObject.SetActive(true);
		//paymentController.gameObject.SetActive(true);

		//GemuMainMenuController.instance.activeCanvasStack.Push(paymentController.gameObject);
	}

	public void OnFreeCoins()
	{
		Debug.Log ("[GemuCoinShopController] PlayUnityVideAds: Start");
		SoundManager.instance.PlayButton();
		GUI_Dialog.InsertStack (uiFreeCoins.gameObject);


	}

}
