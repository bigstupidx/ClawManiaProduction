using UnityEngine;
using System.Collections;
using Facebook.MiniJSON;
using System.Collections.Generic;

public class FacebookWrapper : MonoBehaviour {

	public static string sFBEmail;
	public static string sFBName;

	// Use this for initialization
	void Start () 
	{

	}
	
	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("Is game showing? " + isGameShown);
	}

	private void OnInitComplete()
	{
		Debug.Log("FB.Init completed: Is user logged in? " + FB.IsLoggedIn);
	}

	public void InitFB()
	{
		StartCoroutine(InitiatingFB());
	}

	IEnumerator InitiatingFB()
	{
		sFBEmail = "";
		sFBName = "";

		Debug.LogError("[FacebookWrapper] InitiatingFB");
		if ( FB.IsInitialized == false )
			FB.Init(OnInitComplete, OnHideUnity);

		while ( FB.IsInitialized == false )
		{
			//Debug.LogError("Connecting to FB");
			yield return null;
		}

		yield break;
	}

	FBResult lastFBResult;
	void LoginCallback(FBResult result)
	{
		lastFBResult = result;
		if (result.Error != null)
		{
			//lastResponse = "Error Response:\n" + result.Error;
		}
		else if (!FB.IsLoggedIn)
		{
			//lastResponse = "Login cancelled by Player";
		}
		else
		{
			//lastResponse = "Login was successful!";
		}
		bLoginCallbackDone = true;
	}

	public void LoginFB()
	{
		lastFBResult = null;
		StartCoroutine (LoggingInFB ());
	}

	bool bLoginCallbackDone = false;
	IEnumerator WaitingLoginCallback()
	{
		while ( bLoginCallbackDone == false )
		{
			yield return null;
		}
		yield break;
	}

	IEnumerator LoggingInFB()
	{
		yield return StartCoroutine(InitiatingFB ());

		if ( FB.IsLoggedIn == false )
		{
			Debug.LogError("[FacebookWrapper] User Not Logged In. Logging In...");
			bLoginCallbackDone = false;
			FB.Login("email,publish_actions,user_friends", LoginCallback);
			yield return StartCoroutine(WaitingLoginCallback());
			this.gameObject.SendMessage ("OnFBLoggedIn",lastFBResult);
		}
		else
			Debug.LogError("[FacebookWrapper] User has logged in.");
		yield break;
	}

	public void GetFBAPI(string sAPI)
	{
		StartCoroutine (GettingFBAPI (sAPI));
	}

	bool bGettingFBAPI = false;
	IEnumerator GettingFBAPI(string sAPI)
	{
		yield return StartCoroutine (InitiatingFB ());
		yield return StartCoroutine (LoggingInFB ());
		bGettingFBAPI = true;
		Debug.LogError("[Facebook Wrapper] GettingFBAPI");
		//FB.API("me?fields=first_name,email,friends.limit(10).fields(first_name,id)", Facebook.HttpMethod.GET, UserCallBack);
		FB.API(sAPI, Facebook.HttpMethod.GET, FBAPICallBack);
		yield return StartCoroutine (WaitingFBAPICallBack ());

		this.gameObject.SendMessage ("OnFBResult",tAPIResult);
		
		yield break;
	}

	IEnumerator WaitingFBAPICallBack()
	{
		while ( bGettingFBAPI )
		{
			yield return null;
		}
		yield break;
	}

	bool bPosting = false;
	IEnumerator WaitingToPost(string sLink,string sLinkName,string sLinkCaption,string sLinkDesc,string sPicURL)
	{
		yield return StartCoroutine (InitiatingFB ());
		yield return StartCoroutine (LoggingInFB ());
		bPosting = true;
		FB.Feed(
			//toId: FeedToId,
			link: sLink,
			linkName: sLinkName,
			linkCaption: sLinkCaption,
			linkDescription: sLinkDesc,
			picture: sPicURL,
			//mediaSource: FeedMediaSource,
			//actionName: FeedActionName,
			//actionLink: FeedActionLink,
			//reference: FeedReference,
			//properties: feedProperties,
			callback: CallbackPosting
			);
		yield return StartCoroutine (WaitingFBAPICallBack ());
		
		this.gameObject.SendMessage ("OnFBPostDone");

		yield break;
	}

	void CallbackPosting(FBResult result)
	{
		bPosting = false;

	}

	public void Post(string sLink,string sLinkName,string sLinkCaption,string sLinkDesc,string sPicURL)
	{
		StartCoroutine(WaitingToPost(sLink,sLinkName,sLinkCaption,sLinkDesc,sPicURL));
	}

	Hashtable tAPIResult;
	void FBAPICallBack(FBResult result) 
	{
		Debug.LogError ("[FacebookWrapper] Facebook API Callback");

		tAPIResult = null;

		if (result.Text.ToString().Length > 0) 
		{
			tAPIResult = new Hashtable ();

			var dict = Json.Deserialize (result.Text) as Dictionary<string,object>;
			if (dict.ContainsKey ("first_name")) {
					tAPIResult ["first_name"] = dict ["first_name"].ToString ();
					sFBName = dict ["first_name"].ToString ();
			}

			if (dict.ContainsKey ("name")) {
					tAPIResult ["name"] = dict ["name"].ToString ();
			}
			if (dict.ContainsKey ("email")) {
					tAPIResult ["email"] = dict ["email"].ToString ();
			}
			tAPIResult ["friends_amount"] = 0;

			object friendsH;
			var friends = new List<object> ();
			if (dict.TryGetValue ("friends", out friendsH)) {
					friends = (List<object>)(((Dictionary<string, object>)friendsH) ["data"]);
					if (friends.Count > 0) 
					{
							tAPIResult ["friends_amount"] = friends.Count;
							for (int i=0; i<friends.Count; i ++) 
							{
								var friendDict = ((Dictionary<string,object>)(friends [i]));
								Debug.LogError("[FacebookWrapper] friend id="+(string)friendDict ["id"]);
						        Debug.LogError("[FacebookWrapper] friend first_name="+(string)friendDict ["first_name"]);

								if (friendDict.ContainsKey ("id"))
									tAPIResult ["friend." + i + ".id"] = (string)friendDict ["id"];

								if (friendDict.ContainsKey ("first_name"))
									tAPIResult ["friend." + i + ".first_name"] = (string)friendDict ["first_name"];
							}
					}
			}
		}
		bGettingFBAPI = false;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
