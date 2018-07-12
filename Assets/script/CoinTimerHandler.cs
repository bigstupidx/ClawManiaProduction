using UnityEngine;
using System.Collections;
using System;

public class CoinTimerHandler : MonoBehaviour 
{

	public static CoinTimerHandler instance = null;
	public float waktuResetAwal=600; //1 coin / 10 mins
	
	//public float waktuCount=60;
	public int countCoin=3;
	public int bonusCoinAtLogin = 0;
	int jmlCoinPerhari=3;

	float realTimeSinceLastFrame;


	void Awake()
	{
		realTimeSinceLastFrame = Time.realtimeSinceStartup;
		DontDestroyOnLoad(this.gameObject);
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			DestroyImmediate (this.gameObject);
		} 
	}

	public void Reset()
	{
		
		//waktuCount = waktuResetAwal;
		
		PlayerPrefs.SetInt(PlayerPrefHandler.keyFreeCoinTimer,GameManager.getEpochTime());
	}

	public bool isCounting()
	{
		int timer = GetTimeDiff ();
		if (timer > 0)
			return true;
		/*
		float realTimeNow = Time.realtimeSinceStartup;
		float realDeltaTime = realTimeNow - realTimeSinceLastFrame;
		realTimeSinceLastFrame = realTimeNow;
		waktuCount -=  realDeltaTime;
		if (waktuCount <= 0)
			return false;
			*/
		return false;
	}

	void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}


	void OnEnable()
	{
		/*
		countCoin = PlayerPrefs.GetInt(PlayerPrefHandler.keyCoin);
		//waktuCount = waktuResetAwal;
		
		//jumlahCoinsUser = GameObject.Find ("txtCoinsUser").GetComponent<Text> ();
		//jumlahCoinsUser.text = "X " + countCoin.ToString();
		
		if (countCoin + bonusCoinAtLogin <= 5)
		{
			countCoin += bonusCoinAtLogin;
		}
		else if (countCoin + bonusCoinAtLogin > 5)
		{
			countCoin = 5;
		}
		
		Debug.Log("Coins User " + countCoin);
		bonusCoinAtLogin = 0;
		
		
		//PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin, countCoin);
		
		//PlayerPrefs.Save();
		*/
	}

	// Use this for initialization
	void Start () {

	}

	public int GetTimeDiff()
	{
		int waktuCount = 0;
		int fTimer = GameManager.getEpochTime();
		if ( countCoin < 5 && PlayerPrefs.HasKey( PlayerPrefHandler.keyFreeCoinTimer ) )
		{
			fTimer = PlayerPrefs.GetInt( PlayerPrefHandler.keyFreeCoinTimer );
			
			int iDiff = GameManager.getEpochTime() - fTimer;
			waktuCount = (int)waktuResetAwal - iDiff;
			if ( waktuCount < 0 )
				waktuCount = 0;
		}
		return waktuCount;
	}

	// Update is called once per frame
	void Update () {
		//Debug.LogError ("update");
		if (countCoin < 5) 
		{

			int fTimer = GameManager.getEpochTime();
			if ( PlayerPrefs.HasKey( PlayerPrefHandler.keyFreeCoinTimer ) )
			{
				fTimer = PlayerPrefs.GetInt( PlayerPrefHandler.keyFreeCoinTimer );
			}
			else
				PlayerPrefs.SetInt(PlayerPrefHandler.keyFreeCoinTimer,fTimer);
			
			int iDiff = GameManager.getEpochTime() - fTimer;
			//waktuCount = waktuResetAwal - iDiff;
			//if ( waktuCount < 0 )
			//	waktuCount = 0;

			if ( iDiff > waktuResetAwal )
			{
				countCoin += 1;
				
				PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin, countCoin);
				PlayerPrefs.Save();
				
				PlayerPrefs.SetInt(PlayerPrefHandler.keyFreeCoinTimer,GameManager.getEpochTime());
				GameDataManager.instance.SendPlayResult(GameDataManager.instance.gameID.ToString(),"0",countCoin.ToString(),"0","0");
			}
			/*
			float realTimeNow = Time.realtimeSinceStartup;
			float realDeltaTime = realTimeNow - realTimeSinceLastFrame;
			realTimeSinceLastFrame = realTimeNow;
			waktuCount -=  realDeltaTime;
			//Debug.LogError("cointimer counting amount="+countCoin);
			if (waktuCount <= 0) {
				
				Debug.LogError("waktuCount="+waktuCount);
				waktuCount = waktuResetAwal + waktuCount;
				countCoin += 1;

				PlayerPrefs.SetInt(PlayerPrefHandler.keyCoin, countCoin);
				PlayerPrefs.Save();

				GameDataManager.instance.SendPlayResult(GameDataManager.GEMU_APP_ID,"0",countCoin.ToString(),"0","0");
				//max_five_coins_guest ();
			}
			*/
		} else {
			//waktuCount = 0;
		}

	}

	public static string FormatJam(float elapsTime) {
		string waktuGab;
		
		float menit;
		float detik;
		string bulatMenit;
		string bulatDetik;
		
		menit = Mathf.Floor(elapsTime / 60);
		detik = Mathf.Floor(elapsTime % 60);
		
		bulatMenit = menit.ToString ();
		bulatDetik = detik.ToString ();
		
		if (menit.ToString ().Length == 1) {
			bulatMenit = "0" + menit.ToString();
		}
		if (detik.ToString ().Length == 1) {
			bulatDetik = "0" + detik.ToString();
		}
		waktuGab = bulatMenit.ToString() + ":" + bulatDetik.ToString();
		
		return waktuGab;
	}
}
