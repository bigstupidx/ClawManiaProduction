using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour {
	public enum TransitionMode
	{
		EaseIn,
		EaseOut
	};
	public TweenAlpha tween;
	private bool bDone;
	private TransitionMode eMode;
	UIPanel panel = null;
	bool bShowing = false;
	bool bFirstTime = true;
	// Use this for initialization
	void Start () 
	{
		//gameObject.AddComponent<TweenAlpha>();
		
		//Show (TransitionMode.EaseOut);
	}

	public void Setup()
	{
		panel = this.GetComponent<UIPanel> ();
		if (panel)
			panel.alpha = 1;
	}

	public TransitionMode mode
	{
		get { return eMode;}
		set { eMode = value;}
	}
	
	public void Show ( TransitionMode mode )
	{
		Setup ();
		if (panel == null) {
			Debug.LogError("[Transition] UIPanel not found");
			return;
		}
		
		this.gameObject.SetActive(true);
		if ( mode == TransitionMode.EaseIn )
		{
			bShowing = true;
			bDone = false;
			if ( bFirstTime )
			{
				bFirstTime = false;
				tween.ResetToBeginning();
			}
			tween.Play(false);
		}
		else if ( mode == TransitionMode.EaseOut )
		{
			panel.alpha = 1;
			bShowing = false;
			bDone = false;
			if ( bFirstTime )
			{
				bFirstTime = false;
				tween.ResetToBeginning();
			}
			tween.PlayForward();
		}
		tween.enabled = true;
	}
	
	public bool IsDone
    {
        get { return bDone; }
    }

	public void OnDoneTweening () 
	{
		bDone = true;
		if ( !bShowing )
		{
			if (panel)
				panel.alpha = 0;
			this.gameObject.SetActive(false);
		}
	}

}
