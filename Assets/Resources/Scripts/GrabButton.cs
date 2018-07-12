using UnityEngine; 
using System.Collections;

public class GrabButton : MonoBehaviour {

	float initPosY;
	// Use this for initialization
	void Start () {
		initPosY = transform.position.y;
	}

	public void Reset()
	{
		Vector3 pos = this.transform.position;
		pos.y = initPosY;
		this.transform.position = pos;
	}

	bool bMouseOnObject = false;
	// Update is called once per frame
	void Update () {

		if ( Input.GetMouseButtonDown(0) )
		{
			Vector2 startPos = Input.mousePosition;
			RaycastHit hit;
			Camera cam3D = Camera.allCameras [0];
			Ray ray = cam3D.ScreenPointToRay( startPos );
			
			if (Physics.Raycast (ray, out hit, 1000.0f , 1<<GameManager.LayerGrabButton ))
			{
				bMouseOnObject = true;
			}
		}
		if ( bMouseOnObject && Input.GetMouseButtonUp(0) )
		{
			Vector2 startPos = Input.mousePosition;
			RaycastHit hit;
			Camera cam3D = Camera.allCameras [0];
			Camera cam2D = Camera.allCameras [1];
			Ray ray = cam3D.ScreenPointToRay( startPos );
			
			if (Physics.Raycast (ray, out hit, 1000.0f , 1<<GameManager.LayerGrabButton ))
			{
				
				ray = cam2D.ScreenPointToRay( startPos );
				if (Physics.Raycast (ray, out hit, 1000.0f , 1<<GameManager.LayerGUI ))
				{

				}
				else
				{
					Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag("Gamestate").GetComponent<Gamestate_Gameplay>();
					if ( gs && gs.grabState == GrabState.None )
					{
						gs.GrabObject();
						Vector3 pos = this.transform.position;
						pos.y = initPosY-0.2f;
						this.transform.position = pos;
					}
				}
				bMouseOnObject = false;
			}
			else
				bMouseOnObject = false;
		}
	}

}
