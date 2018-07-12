using UnityEngine;
using System.Collections;

public class ButtonOptions : MonoBehaviour {
	public TweenScale tweenScale;
	int bJumpUp = 0;
	bool bFirst = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnDoneTweening()
	{
		if (bJumpUp == 1) 
		{
			bJumpUp = 2;
			this.transform.parent.gameObject.SendMessage("OnClickMenuButton");
			//tweenScale.method = UITweener.Method.BounceOut;
			//tweenScale.PlayReverse ();
		} 
		else if (bJumpUp == 2)
		{
			this.transform.parent.gameObject.SendMessage("OnClickMenuButton");
			bJumpUp = 0;
		}
	}

	void OnClick()
	{
		/*
		//if (bJumpUp != 0)
		//	return;
		if ( tweenScale )
		{
			bJumpUp = 1;
			tweenScale.method = UITweener.Method.BounceIn;
			if ( bFirst )
			{
				bFirst = false;
				tweenScale.ResetToBeginning();
			}
			tweenScale.PlayForward();
		}
		*/
	}
}
