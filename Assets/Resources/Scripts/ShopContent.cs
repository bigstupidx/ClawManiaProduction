using UnityEngine;
using System.Collections;

public enum ShopContentType
{
	IAP,
	Joystick,
	Claw,
	Machine,
	Cosmetic,
	FreeCoin,
	PowerUp
}

public class ShopContent : MonoBehaviour {
	public ShopContentType type;
	public int Amount;
	public float Price;
	public string Name;
	public string uniqueID; // must be the same with prefab name
	public bool bFree;
	public string Desc;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
