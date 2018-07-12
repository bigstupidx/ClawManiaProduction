using UnityEngine;
using System.Collections;

public enum Mission_Event
{
	MoveJoystick,
	Grab,
	ChangeCameraAngle,
	UsePowerup,
	ShopBuyNewClaw,
	Prize,
	Coin
}

public enum Mission_Type
{
	Tutorial,
	Procedural
}
public class Mission : MonoBehaviour {

	public Mission_Type missionType;
	public Mission_Event missionEvent;
	public int CoinReward;
	public int ExpReward;
	public string MissionName;
	public string MissionDesc;
	public int Procedural_Min;
	public int Procedural_Max;


}
