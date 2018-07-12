using UnityEngine;
using System.Collections;

public class BlinkingSprite : MonoBehaviour {

	UITexture uitexture;
	// Use this for initialization
	void Start () {
		uitexture = this.GetComponent<UITexture> ();

		uitexture.SetDimensions (10, 10);
	}

	public IEnumerator Blinking(float waitTime)
	{
		yield return new WaitForSeconds (waitTime);
		bool bRunning = true;
		while ( bRunning )
		{
			if ( uitexture.alpha > 0 )
				uitexture.alpha = 0;
			else
				uitexture.alpha = 1;
			yield return new WaitForSeconds(1);
		}
	}
}
