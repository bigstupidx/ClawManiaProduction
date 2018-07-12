using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GemuAPI_Exception : Exception
{
	public enum ExceptionType
	{
		EMPTY_PARAMETER,
		MISSING_REQUIRED_FIELD
	}

	private static string[] exceptionStrings = new string[2] 
	{
		"Parameter list is empty! ",
		"The required field(s) is(are) missing! "
	};

	public GemuAPI_Exception(ExceptionType type, string message = "" ) : base(BaseMessage(type, message))
	{

	}

	private static string BaseMessage(ExceptionType type, string message = "") {
		return exceptionStrings[(int)type] + message;
	}
	
}

public class GemuAPI : MonoBehaviour {
	private static GemuAPI instance = null;

	private const string baseUrl = "http://api.gemugemu.com";

	private const string registerSub = "register/";
	private const string loginSub = "login/";
	private const string getUserSub = "getuser/";
	private const string playSub = "play/";
	private const string playResultSub = "playresult/";
	private const string rewardSub = "reward/";
	private const string leaderboardSub = "leaderboard/";
	private const string redeemSub = "redeem/";
	private const string promoSub = "promo/";

	private Restifizer.RestifizerManager restifizerManager;

	//Delegates & events
	public delegate void ResponseDelegate(Restifizer.RestifizerResponse response);
	public static event ResponseDelegate OnRegisterResponse; 
	public static event ResponseDelegate OnLoginResponse; 
	public static event ResponseDelegate OnGetUserResponse; 
	public static event ResponseDelegate OnPlayResponse; 
	public static event ResponseDelegate OnPlayResultResponse; 
	public static event ResponseDelegate OnRewardResponse; 
	public static event ResponseDelegate OnLeaderboardResponse; 
	public static event ResponseDelegate OnRedeemResponse; 
	public static event ResponseDelegate OnPromoResponse; 

	void Awake()
	{
		if (instance == null)
		{
			DontDestroyOnLoad(this.gameObject);

			instance = this;

			restifizerManager = gameObject.GetComponent<Restifizer.RestifizerManager>();

			if (restifizerManager == null)
				restifizerManager = gameObject.AddComponent<Restifizer.RestifizerManager>();

			restifizerManager.baseUrl = baseUrl;

		}
		else
			DestroyImmediate(this.gameObject);
	}

	void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	static void Instantiate()
	{
		if (instance == null)
		{
			GameObject go = new GameObject("GemuAPI");

			instance = go.AddComponent<GemuAPI>();
			instance.restifizerManager = go.AddComponent<Restifizer.RestifizerManager>();
			instance.restifizerManager.baseUrl = baseUrl;
		}
	}

	public static void Register(Hashtable formData)
	{
		if (instance == null)
		{
			Instantiate();
		}


		if (CheckRegisterData(formData))
		{
			instance.restifizerManager.ResourceAt(registerSub).Post(formData, (response) => {
				// response.Resource - at this point you can use the result 
				OnRegisterResponse(response);
			});
		}
	}

	private static bool CheckRegisterData(Hashtable formData)
	{
		//-------------------------------------------------------------------
		if (formData.Count == 0)
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.EMPTY_PARAMETER, "Register!");
			
			return false;
		}

		//-------------------------------------------------------------------
		string missingFields = "";

		if (!formData.ContainsKey("full_name") || string.IsNullOrEmpty((string)formData["full_name"]))
			missingFields += "Full Name, ";
		if (!formData.ContainsKey("email") || string.IsNullOrEmpty((string)formData["email"]))
			missingFields += "Email, ";
		if (!formData.ContainsKey("username") || string.IsNullOrEmpty((string)formData["username"]))
			missingFields += "User Name, ";
		if (!formData.ContainsKey("password") || string.IsNullOrEmpty((string)formData["password"]))
			missingFields += "Password, ";
		if (!formData.ContainsKey("tiket") || string.IsNullOrEmpty((string)formData["tiket"]))
		{
			if (!formData.ContainsKey("tiket"))
			{
				formData.Add("tiket", "0");
			}
			else
			{
				formData["tiket"] = 0;
			}
		}

		if ( !string.IsNullOrEmpty(missingFields))
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.MISSING_REQUIRED_FIELD, 
			                            "Missing field(s): " + missingFields);

			return false;
		}

		//-------------------------------------------------------------------
		return true;
	}

	public static void Login(Hashtable formData)
	{
		if (instance == null)
		{
			Instantiate();
		}

		if (CheckLoginData(formData))
		{
			instance.restifizerManager.ResourceAt(loginSub).Post(formData, (response) => {
				// response.Resource - at this point you can use the result 
				OnLoginResponse(response);
			});
		}
	}

	private static bool CheckLoginData(Hashtable formData)
	{
		//-------------------------------------------------------------------
		if (formData.Count == 0)
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.EMPTY_PARAMETER, "Login!");
			
			return false;
		}
		
		//-------------------------------------------------------------------
		string missingFields = "";
		

		if (!formData.ContainsKey("username") || string.IsNullOrEmpty((string)formData["username"]))
			missingFields += "User Name, ";
		if (!formData.ContainsKey("password") || string.IsNullOrEmpty((string)formData["password"]))
			missingFields += "Password, ";
		if (!formData.ContainsKey("gameid") || string.IsNullOrEmpty((string)formData["gameid"]))
			missingFields += "Game ID, ";
		if (!formData.ContainsKey("ip") || string.IsNullOrEmpty((string)formData["ip"]))
			missingFields += "IP Address, ";
		
		if ( !string.IsNullOrEmpty(missingFields))
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.MISSING_REQUIRED_FIELD, 
			                            "Missing field(s): " + missingFields);
			
			return false;
		}
		
		//-------------------------------------------------------------------
		return true;
	}

	public static void GetUser(Hashtable formData)
	{
		if (instance == null)
		{
			Instantiate();
		}
		
		if (CheckGetUserData(formData))
		{
			instance.restifizerManager.ResourceAt(getUserSub).Post(formData, (response) => {
				// response.Resource - at this point you can use the result 
				OnGetUserResponse(response);
			});
		}
	}
	
	private static bool CheckGetUserData(Hashtable formData)
	{
		//-------------------------------------------------------------------
		if (formData.Count == 0)
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.EMPTY_PARAMETER, "Get user!");
			
			return false;
		}

		//-------------------------------------------------------------------
		string missingFields = "";
		
		
		if (!formData.ContainsKey("username") || string.IsNullOrEmpty((string)formData["username"]))
			missingFields += "User Name, ";
		if (!formData.ContainsKey("token") || string.IsNullOrEmpty((string)formData["token"]))
			missingFields += "Token, ";
		
		if ( !string.IsNullOrEmpty(missingFields))
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.MISSING_REQUIRED_FIELD, 
			                            "Missing field(s): " + missingFields);
			
			return false;
		}
		
		//-------------------------------------------------------------------
		return true;
	}

	public static void Play(Hashtable formData)
	{
		if (instance == null)
		{
			Instantiate();
		}
		
		if (CheckPlayData(formData))
		{
			instance.restifizerManager.ResourceAt(playSub).Post(formData, (response) => {
				// response.Resource - at this point you can use the result 
				OnPlayResponse(response);
			});
		}
	}
	
	private static bool CheckPlayData(Hashtable formData)
	{
		//-------------------------------------------------------------------
		if (formData.Count == 0)
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.EMPTY_PARAMETER, "Login!");
			
			return false;
		}
		
		//-------------------------------------------------------------------
		string missingFields = "";
		
		
		if (!formData.ContainsKey("username") || string.IsNullOrEmpty((string)formData["username"]))
			missingFields += "User Name, ";
		if (!formData.ContainsKey("token") || string.IsNullOrEmpty((string)formData["token"]))
			missingFields += "Token, ";
		
		if ( !string.IsNullOrEmpty(missingFields))
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.MISSING_REQUIRED_FIELD, 
			                            "Missing field(s): " + missingFields);
			
			return false;
		}
		
		//-------------------------------------------------------------------
		return true;
	}

	public static void PlayResult(Hashtable formData)
	{
		if (instance == null)
		{
			Instantiate();
		}
		
		if (CheckPlayResultData(formData))
		{
			instance.restifizerManager.ResourceAt(playResultSub).Post(formData, (response) => {
				// response.Resource - at this point you can use the result 
				OnPlayResultResponse(response);
			});
		}
	}
	
	private static bool CheckPlayResultData(Hashtable formData)
	{
		//-------------------------------------------------------------------
		if (formData.Count == 0)
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.EMPTY_PARAMETER, "Login!");
			
			return false;
		}
		
		//-------------------------------------------------------------------
		string missingFields = "";
		
		
		if (!formData.ContainsKey("username") || string.IsNullOrEmpty((string)formData["username"]))
			missingFields += "User Name, ";
		if (!formData.ContainsKey("token") || string.IsNullOrEmpty((string)formData["token"]))
			missingFields += "Token, ";
		if (!formData.ContainsKey("gameid") || string.IsNullOrEmpty((string)formData["gameid"]))
			missingFields += "Game ID, ";
		if (!formData.ContainsKey("gamelevel") || string.IsNullOrEmpty((string)formData["gamelevel"]))
			missingFields += "Game Level, ";
		if (!formData.ContainsKey("tiket") || string.IsNullOrEmpty((string)formData["tiket"]))
			missingFields += "Tiket, ";
		if (!formData.ContainsKey("score") || string.IsNullOrEmpty((string)formData["score"]))
			missingFields += "Score, ";
		if (!formData.ContainsKey("coin") || string.IsNullOrEmpty((string)formData["coin"]))
			missingFields += "Coin, ";
		
		if ( !string.IsNullOrEmpty(missingFields))
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.MISSING_REQUIRED_FIELD, 
			                            "Missing field(s): " + missingFields);
			
			return false;
		}
		
		//-------------------------------------------------------------------
		return true;
	}

	public static void Reward()
	{
		if (instance == null)
		{
			Instantiate();
		}

		instance.restifizerManager.ResourceAt(rewardSub).Get((response) => {
				// response.Resource - at this point you can use the result 
				OnRewardResponse(response);
		});
	}

	public static void Leaderboard(Hashtable formData)
	{
		if (instance == null)
		{
			Instantiate();
		}
		
		if (CheckLeaderboardData(formData))
		{
			instance.restifizerManager.ResourceAt(leaderboardSub).Post(formData, (response) => {
				// response.Resource - at this point you can use the result 
				OnLeaderboardResponse(response);
			});
		}
	}
	
	private static bool CheckLeaderboardData(Hashtable formData)
	{
		//-------------------------------------------------------------------
		if (formData.Count == 0)
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.EMPTY_PARAMETER, "Login!");
			
			return false;
		}
		
		//-------------------------------------------------------------------
		string missingFields = "";
		

		if (!formData.ContainsKey("gameid") || string.IsNullOrEmpty((string)formData["gameid"]))
			missingFields += "Game ID, ";
		if (!formData.ContainsKey("gamelvl") || string.IsNullOrEmpty((string)formData["gamelvl"]))
			missingFields += "Game Level, ";
		
		if ( !string.IsNullOrEmpty(missingFields))
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.MISSING_REQUIRED_FIELD, 
			                            "Missing field(s): " + missingFields);
			
			return false;
		}

		return true;
	}

	public static void Redeem(Hashtable formData)
	{
		if (instance == null)
		{
			Instantiate();
		}
		
		if (CheckRedeemData(formData))
		{
			instance.restifizerManager.ResourceAt(redeemSub).Post(formData, (response) => {
				// response.Resource - at this point you can use the result 
					OnRedeemResponse(response);
			});
		}
	}
	
	private static bool CheckRedeemData(Hashtable formData)
	{
		//-------------------------------------------------------------------
		if (formData.Count == 0)
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.EMPTY_PARAMETER, "Login!");
			
			return false;
		}
		
		//-------------------------------------------------------------------
		string missingFields = "";
		
		if (!formData.ContainsKey("username") || string.IsNullOrEmpty((string)formData["username"]))
			missingFields += "User Name, ";
		if (!formData.ContainsKey("token") || string.IsNullOrEmpty((string)formData["token"]))
			missingFields += "Token, ";
		if (!formData.ContainsKey("rewardid") || string.IsNullOrEmpty((string)formData["rewardid"]))
			missingFields += "Reward ID, ";
		
		if ( !string.IsNullOrEmpty(missingFields))
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.MISSING_REQUIRED_FIELD, 
			                            "Missing field(s): " + missingFields);
			
			return false;
		}
		
		return true;
	}

	public static void Promo(Hashtable formData)
	{
		if (instance == null)
		{
			Instantiate();
		}
		
		if (CheckPromoData(formData))
		{
			instance.restifizerManager.ResourceAt(promoSub).Post(formData, (response) => {
				// response.Resource - at this point you can use the result 
				OnPromoResponse(response);
			});
		}
	}
	
	private static bool CheckPromoData(Hashtable formData)
	{
		//-------------------------------------------------------------------
		if (formData.Count == 0)
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.EMPTY_PARAMETER, "Login!");
			
			return false;
		}
		
		//-------------------------------------------------------------------
		string missingFields = "";
		
		if (!formData.ContainsKey("username") || string.IsNullOrEmpty((string)formData["username"]))
			missingFields += "User Name, ";
		if (!formData.ContainsKey("token") || string.IsNullOrEmpty((string)formData["token"]))
			missingFields += "Token, ";
		if (!formData.ContainsKey("kdkupon") || string.IsNullOrEmpty((string)formData["kdkupon"]))
			missingFields += "Kode Kupon, ";
		
		if ( !string.IsNullOrEmpty(missingFields))
		{
			throw new GemuAPI_Exception(GemuAPI_Exception.ExceptionType.MISSING_REQUIRED_FIELD, 
			                            "Missing field(s): " + missingFields);
			
			return false;
		}
		
		return true;
	}

}
