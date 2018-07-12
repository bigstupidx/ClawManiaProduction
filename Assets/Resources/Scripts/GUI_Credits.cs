using UnityEngine;
using System.Collections;

public class GUI_Credits : GUI_Dialog 
{
	public Transform container;
	float fMinY = -520;
	float fMaxY = 800;
	// Use this for initialization
	
	public void OnClickBack()
	{
		GUI_Dialog.ReleaseTopCanvas ();
	}
	public override void OnStart () 
	{
	}

	void SetPosY(float fY)
	{
		Vector3 pos = container.transform.localPosition;
		pos.y = fY;
		container.transform.localPosition = pos;
	}

	public override void OnShow()
	{
		SetPosY(fMinY);
	}

	// Update is called once per frame
	void Update () 
	{
		if (!isVisible ())
			return;
		float fY = container.localPosition.y;
		fY += Time.deltaTime * 50;
		SetPosY (fY);
		if ( fY > fMaxY )
			SetPosY(fMinY);
	}
}
