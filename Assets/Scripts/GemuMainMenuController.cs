using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GemuMainMenuController : MonoBehaviour {
	public bool isUsingAssetBundle = true;
	public static GemuMainMenuController instance = null;

	public GemuCoinShopController canvasCoinShop;
	public GemuTicketsController canvasTickets;
	public GemuLoginController canvasLogin;
	public GemuSettingsController canvasSettings;
	public GemuQuitController canvasQuit;
	public UIProgressBar scrollbarABDownload;
	public UILabel labelPercentage;
	public Transition transition;
	//Stack<GameObject> activeCanvasStack = new Stack<GameObject>();

	/*
	public void InsertStack(GameObject ob)
	{
		if ( ob.GetComponent<GUI_Dialog>() )
		{
			ob.gameObject.SetActive(true);
			activeCanvasStack.Push(ob);
		}
	}
	*/
	void Awake()
	{
		instance = this;
		
		//scrollbarABDownload.gameObject.SetActive(false);
		//Debug.LogError (PlayerPrefs.GetString (PlayerPrefHandler.keyUserName));
	}

	void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

	// Use this for initialization
	void Start () {
		scrollbarABDownload.gameObject.SetActive (false);
		//Caching.CleanCache ();
		//scrollbarABDownload.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyUp(KeyCode.Escape) )
		{
			//isKeyDown = true;

			SoundManager.instance.PlayButton();

			if (GUI_Dialog.GetActiveStackAmount() > 0)
			{
				GUI_Dialog.ReleaseTopCanvas();
			}
			else
			{
				/*
				if (canvasQuit.gameObject.activeSelf)
				{
					canvasQuit.gameObject.SetActive(false);
					canvasQuit.gameObject.SetActive(false);
				}
				else
				{
					canvasQuit.gameObject.SetActive(true);
					canvasQuit.gameObject.SetActive(true);
				}*/
				GUI_Dialog.InsertStack(canvasQuit.gameObject);
			}
		}
		//else if (Input.GetKeyUp(KeyCode.Escape))
		//{
		//	isKeyDown = false;
		//}

		if ( isUsingAssetBundle )
		{
			if ( AssetBundleManager.GetInstance().IsDownloading() && AssetBundleManager.GetInstance().getWWW() != null && AssetBundleManager.GetInstance().getWWW().isDone == false )
			{

				if ( scrollbarABDownload != null && AssetBundleManager.GetInstance().getWWW().progress > 0.01f)
				{
					scrollbarABDownload.gameObject.SetActive(true);
					//scrollbarABDownload.value = 0.5f;
					//Debug.LogError(AssetBundleManager.progress);
					labelPercentage.text = ((int)(AssetBundleManager.GetInstance().getWWW().progress*100)).ToString()+"%";
					scrollbarABDownload.value = AssetBundleManager.GetInstance().getWWW().progress;
				}
			}
		}
	}

	/*
	public void ReleaseTopCanvas()
	{
		if (activeCanvasStack.Count > 0) 
		{
			GameObject canvasGo = activeCanvasStack.Pop ();
			canvasGo.GetComponent<GUI_Dialog>().Hide();
		}
	}
*/
	public void OnCoinShopButton()
	{
		SoundManager.instance.PlayButton();
		GUI_Dialog.InsertStack(canvasCoinShop.gameObject);
	}

	public void OnTicketsButton()
	{
		SoundManager.instance.PlayButton();
		
		GUI_Dialog.InsertStack(canvasTickets.gameObject);
	}

	public void OnLoginButton()
	{
		SoundManager.instance.PlayButton();
		
		GUI_Dialog.InsertStack(canvasLogin.gameObject);
	}

	public void OnSettingsButton()
	{
		SoundManager.instance.PlayButton();
		GUI_Dialog.InsertStack(canvasSettings.gameObject);
	}

	public void OnBasketButton()
	{
		//SoundManager.instance.PlayButton();
		//if (isUsingAssetBundle) 
		//{
		//	AssetBundleManager.GetInstance().PlayAssetBundle("BasketHoop.unity3d");
		//}
		//else
		//{
		//	Debug.Log("Basket game started!");
			sTargetGame = "scene_utama";
			StartCoroutine(WaitingToPlay());
		//}
	}
	
	public void OnClawButton()
	{
		SoundManager.instance.PlayButton();
		if (isUsingAssetBundle) 
		{
			AssetBundleManager.GetInstance().PlayAssetBundle("ClawMania.unity3d");
		}
		else
		{
			Debug.Log("Basket game started!");
			sTargetGame = "Loading";
			StartCoroutine(WaitingToPlay());
		}
	}

	string sTargetGame = "";
	IEnumerator WaitingToPlay()
	{
		transition.Show (Transition.TransitionMode.EaseIn);
		while (!transition.IsDone)
			yield return null;
		Application.LoadLevel(sTargetGame);
		yield break;
	}
}
