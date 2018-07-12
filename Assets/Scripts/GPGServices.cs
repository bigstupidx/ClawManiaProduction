using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class GPGServices : MonoBehaviour {
	public static GPGServices instance;

	void Awake(){
		instance = this;
		DontDestroyOnLoad(this.gameObject);
		PlayGamesPlatform.Activate();
	}

	void Start(){

	}
	#region other google play functions
	public void ActivateGooglePlay(){

		GooglePlayLogin();
	}
	
	private void GooglePlayLogin(){
		Social.localUser.Authenticate((bool success) => {
			if(success){
				print ("login");
			}else{
				print ("not login");
			}
		});
	}
	
//	public void PostScoreToGooglePlay(int score){
//		Social.ReportScore(score,leaderboard_pixel_marathon_leaderboard, (bool success) => {
//			
//		});
//	}
	
	public void GooglePlay_ShowAchievements(){
		Social.ShowAchievementsUI();
	}
	
	public void GooglePlay_ShowLeaderboard(){
		Social.ShowLeaderboardUI();
	}
	
	public void LogOutGooglePlay(){
		PlayGamesPlatform.Instance.SignOut();
	}
	#endregion
}
