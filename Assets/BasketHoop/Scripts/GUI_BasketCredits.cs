using UnityEngine;
using System.Collections;

public class GUI_BasketCredits : GUI_Dialog {
	public GameObject scrollView;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if ( isVisible () )
		{
			
			Vector3 pos = scrollView.transform.localPosition;
			pos.y += Time.deltaTime*20;
			if ( pos.y > 1060 )
			{
				pos.y = -600;
			}
			scrollView.transform.localPosition = pos;
		}
	}

	public void OnBackButton()
	{
		SoundManager.instance.PlayButton();
		GUI_Dialog.ReleaseTopCanvas();
	}
	
	public override void OnShow()
	{
		Vector3 pos = scrollView.transform.localPosition;
		pos.y = -500;
		scrollView.transform.localPosition = pos;
	}
}
