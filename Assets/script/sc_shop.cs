using UnityEngine;
using UnityEngine.UI;

using System.Collections;

public class sc_shop : MonoBehaviour {
	static sc_shop instance;
	public GameObject[] buttonCoins;
	// Use this for initialization
	void Start () {
		instance = this;
		for (int i=0; i<buttonCoins.Length; i++) 
		{
			GameObject content = buttonCoins[i];

			Transform trTitle = content.transform.Find("Currency");
			trTitle.gameObject.SetActive(false);
		}
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.25coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.50coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.100coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.180coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.300coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.500coins");
		
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnBillingConnected;
		AndroidInAppPurchaseManager.instance.loadStore();
	}

	void OnDestroy()
	{
		instance = null;
	}
	
	public void OnClickIAP(string id)
	{

		AndroidInAppPurchaseManager.instance.purchase (id);
	}

	public static sc_shop GetInstance()
	{
		return instance;
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
				for ( int i=0; i<sc_shop.GetInstance().buttonCoins.Length ; i++ )
				{
					GameObject content = sc_shop.GetInstance().buttonCoins[i];
					if ( tpl.SKU.Equals(content.name) )
					{
						Transform trCurrency = content.transform.Find("Currency");
						if ( trCurrency )
						{
							Text text = trCurrency.GetComponent<Text>();
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
	
	}
}
