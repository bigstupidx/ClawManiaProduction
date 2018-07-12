/* -----------------------------------------------
 * Programmer : Aji Pamungkas
 * 
 */

using UnityEngine;
using System.Collections;

public enum IAPPurchaseResult
{
	BILLING_CONNECTED,
	BILLING_NOT_CONNECTED,
	RETRIEVE_PRODUCTS_SUCCESS,
	RETRIEVE_PRODUCTS_FAIL
}

public class IAPPurchaseController : MonoBehaviour {
	static IAPPurchaseController instance;
	public GameObject receiver;
	public string[] productIDs;
	// Use this for initialization
	void Start () {
		instance = this;
#if UNITY_ANDROID
		for ( int i=0; i<productIDs.Length; i++ )
			AndroidInAppPurchaseManager.instance.addProduct((string)productIDs[i]);
		/*
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.25coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.50coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.100coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.180coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.300coins");
		AndroidInAppPurchaseManager.instance.addProduct("gl1.gemugemu.500coins");
		*/
		//AndroidInAppPurchaseManager.ActionProductPurchased += OnProductPurchased;  
		//AndroidInAppPurchaseManager.ActionProductConsumed  += OnProductConsumed;
		
		//listening for store initilaizing finish
#endif

	}

	void OnDestroy()
	{
		instance = null;
	}

	void Awake () 
	{
		instance = this;
	}

	public static IAPPurchaseController GetInstance()
	{
		if ( instance == null )
		{
			GameObject ob = new GameObject();
			ob.name = "IAPPurchaseController";
			instance = ob.AddComponent<IAPPurchaseController>();
		}
		return instance;
	}

	public void LoadStore()
	{
#if UNITY_ANDROID
		AndroidInAppPurchaseManager.ActionBillingSetupFinished += OnGooglePlayStoreBillingConnected;
		AndroidInAppPurchaseManager.instance.loadStore();
#endif
	}

	public void retrieveProducDetails()
	{
#if UNITY_ANDROID
		AndroidInAppPurchaseManager.ActionRetrieveProducsFinished += OnGooglePlayStoreRetrieveProductsFinised;
		AndroidInAppPurchaseManager.instance.retrieveProducDetails();
#endif
	}
	
#if UNITY_ANDROID
	private static void OnGooglePlayStoreBillingConnected(BillingResult result) 
	{

		AndroidInAppPurchaseManager.ActionBillingSetupFinished -= OnGooglePlayStoreBillingConnected;
		
		
		if(result.isSuccess) 
		{

		} 
		
		Debug.Log ("[IAPPurchaseController] Connection Response: " + result.response.ToString() + " " + result.message);
	}
#endif
	
#if UNITY_ANDROID
	private static void OnGooglePlayStoreRetrieveProductsFinised(BillingResult result) 
	{
		if(result.isSuccess) 
		{

		} 
		else 
		{
			//AndroidMessage.Create("Connection Responce", result.response.ToString() + " " + result.message);
		}
		
		Debug.Log ("[IAPPurchaseController] Connection Response: " + result.response.ToString() + " " + result.message);
		
	}
#endif


	// Update is called once per frame
	void Update () {
	
	}
}
