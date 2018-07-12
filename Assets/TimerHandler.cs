using UnityEngine;
using System.Collections;
using System;
using SimpleJSON;

public class TimerHandler : MonoBehaviour {
	public static TimerHandler instance;
	public TimeSpan DurationLeft_Ads, DurationLeft_FreeGift;
	DateTime timeNow;

	void Awake(){
		instance = this;
	}

	public bool AdsAvailability(){
		if(PlayerPrefs.HasKey(GamePreferences.Key_AdsTimer) == false){ //available
			return true;
		}else{ //not available, timer
			//get current time
			DateTime currTime = DateTime.Now;
			
			//get saved time
			long temp = Convert.ToInt64(PlayerPrefs.GetString(GamePreferences.Key_AdsTimer));
			DateTime SavedTime = DateTime.FromBinary(temp);
			
			//get difference
			DurationLeft_Ads = SavedTime.Subtract(currTime);
			
			if(DurationLeft_Ads <= TimeSpan.FromSeconds(0)){
				PlayerPrefs.DeleteKey(GamePreferences.Key_AdsTimer); 
				return true;
			}
			else return false;
		}
	}

	public bool FreeGiftAvailablilty(){
		if(PlayerPrefs.HasKey(GamePreferences.Key_FreeGiftTimer) == false){ //available
			return true;
		}else{ //not available, timer
			//get current time
			DateTime currTime = DateTime.Now;

			//get saved time
			long temp = Convert.ToInt64(PlayerPrefs.GetString(GamePreferences.Key_FreeGiftTimer));
			DateTime SavedTime = DateTime.FromBinary(temp);

			//get difference
			DurationLeft_FreeGift = SavedTime.Subtract(currTime);

			if(DurationLeft_FreeGift <= new TimeSpan(0,0,0)){ 
				PlayerPrefs.DeleteKey(GamePreferences.Key_FreeGiftTimer);
				return true;
			}
			else return false;
		}
	}

	public TimeSpan FreeGiftExponentialTimer(){
		int FreeGiftCode = 0;
		if(PlayerPrefs.HasKey(GamePreferences.Key_FreeGiftCode) == false) PlayerPrefs.SetInt(GamePreferences.Key_FreeGiftCode,1);
		FreeGiftCode = PlayerPrefs.GetInt(GamePreferences.Key_FreeGiftCode);
		
		if(FreeGiftCode > 7){
			return new TimeSpan(3,0,0);
		}else{
			int WaitDuration = (int)Mathf.Pow(GamePreferences.Value_BasePower,FreeGiftCode);
			TimeSpan temp = TimeSpan.FromMinutes(WaitDuration);
			
			FreeGiftCode++;
			PlayerPrefs.SetInt(GamePreferences.Key_FreeGiftCode,FreeGiftCode);
			
			return temp;
		}
	} 

	public void GetTime(){
		StartCoroutine(getServerTime());
	}

	private IEnumerator getServerTime(){
		//post request
		string postURL = "http://api.timezonedb.com/?zone=Europe/London&format=json&key=A9DTMGUDV74U";
		WWW www = new WWW(postURL);

		//catch result
		yield return www;

		if(www.error == null){
			//parse timestamp (unix timestamp)
			print (www.text);
			JSONNode data = JSONNode.Parse(www.text);
			double timeStamp = data["timestamp"].AsDouble;
			
			DateTime epochTime = new DateTime(1970,1,1,0,0,0,DateTimeKind.Utc);
			timeNow = epochTime.AddSeconds(timeStamp).ToLocalTime();
			
			int TimeDifference = timeNow.CompareTo(DateTime.Now);

			//comment if it work successfully
			print ("Server Time = "+timeNow);//the time
			print ("Difference = "+TimeDifference);//difference between server time and localtime (in seconds)
			
			if(TimeDifference < -60f || TimeDifference > 60f){
				//cheating, not using server time
				PlayerPrefs.SetInt(GamePreferences.Key_IsCheat,1);
				//hide prize
			}else{ 
				//no cheat
				PlayerPrefs.SetInt(GamePreferences.Key_IsCheat,0);
			}
		}else{
			//no connection / internet error
			PlayerPrefs.SetInt(GamePreferences.Key_IsNetAvailable,0);
			//hide prize
		}
	}
}
