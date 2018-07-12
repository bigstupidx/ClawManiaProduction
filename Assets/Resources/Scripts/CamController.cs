using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {
	Vector3 prevPos;
	UISprite sprite;
	// Use this for initialization
	void Start () {
		sprite = this.gameObject.GetComponent<UISprite> ();
	}
	
	float dirX = 0;
	float dirY = 0;
	public float GetDirX()
	{
		return dirX;
	}
	
	public float GetDirY()
	{
		return dirY;
	}
	
	public void Reset()
	{
		bTouch = false;
		dirX = 0;
		dirY = 0;
	}
	
	bool bTouch = false;
	// Update is called once per frame
	void Update () 
	{

		if ( Input.GetMouseButtonDown(0) )
		{
			Vector2 startPos = Input.mousePosition;
			RaycastHit hit;
			Camera cam2D = Camera.allCameras [0];
			Ray ray = cam2D.ScreenPointToRay( startPos );
			
			if (Physics.Raycast (ray, out hit, 1000.0f , 1<<GameManager.LayerCamController ))
			{
				bTouch = true;
				prevPos = cam2D.WorldToScreenPoint(hit.point);
			}
		}
		
		if ( bTouch && Input.GetMouseButtonUp(0) )
		{
			Vector2 startPos = Input.mousePosition;
			RaycastHit hit;
			Camera cam2D = Camera.allCameras [0];
			Ray ray = cam2D.ScreenPointToRay( startPos );
			
			if (Physics.Raycast (ray, out hit, 1000.0f , 1<<GameManager.LayerCamController ))
			{
				
				Vector3 currPos = cam2D.WorldToScreenPoint(hit.point);
				if (  Mathf.Abs(prevPos.x-currPos.x) > 40 )
				{
					int iDir = 0;
					if ( prevPos.x < currPos.x  )
						iDir = 1;
					else if ( prevPos.x > currPos.x )
						iDir = -1;
					Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag("Gamestate").GetComponent<Gamestate_Gameplay>();
					if ( gs )
					{
						gs.OnSwipe(iDir);
					}
				}
				else
					bTouch = false;
			}
			else
				bTouch = false;
		}
	}
}
