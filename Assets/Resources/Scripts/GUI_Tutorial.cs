using UnityEngine;
using System.Collections;

public class GUI_Tutorial : GUI_Dialog {
	public UIScrollView scrollView;
	public UISprite[] bullets;
	// Use this for initialization
	
	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}
	void RefreshInfo()
	{
		for ( int i=0; i<bullets.Length; i++ )
		{
			bullets[i].spriteName = "bullets_off";
			if ( i == currPage )
				bullets[i].spriteName = "bullets_on";
		}
	}

	// Update is called once per frame
	void Update () 
	{
		//Debug.LogError (scrollView.currentMomentum);
		if ( scrollView.isDragging == false )
		{
			//Debug.LogError(scrollView.transform.localPosition.x);
			int currIndex = Mathf.Abs((int)(Mathf.Ceil(scrollView.transform.localPosition.x)/300));
			//Debug.LogError("currIndex="+currIndex+" currPage="+currPage);
			if ( currIndex != currPage )
			{
				currPage = currIndex;
				RefreshInfo();
				//Debug.LogError(currPage);
			}
		}
	}

	public void Hide()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}

	int currPage = 0;
	public override void OnShow()
	{
		Vector3 pos = scrollView.transform.localPosition;
		pos.x = 0;
		scrollView.transform.localPosition = pos;
		currPage = 0;
		RefreshInfo ();
	}
}
