using UnityEngine;
using System.Collections;

public enum AchievementReward
{
	Coin,
	Ticket,
	Exp
}

public enum AchievementType
{
	ShopTime,
	SeriousGrabber,
	SharingIsFun,
	RegisterGemuGemu,
	CompletePrizeCollection,
	PowerPlay,
	UsePowerUp,
	PrizeCurator,
	PrizeCollector,
	GoodDeal,
	FirstCollection
}

public abstract class Achievement : MonoBehaviour 
{
	public AchievementType type;
	public string id;
	public int amount;
	public bool MultipleTimes;
	public AchievementReward rewardType;
	public int rewardAmount;
	public string desc;
	// Use this for initialization


	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual bool isUnlocked()
	{
		if ( PlayerPrefs.HasKey("AC_"+id+"_"+amount) )
			return true;
		else
			return false;
	}
}
