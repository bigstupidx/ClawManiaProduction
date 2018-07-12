using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ControllerType
{
	Joystick,
	Camera,
	GrabButton,
	None
}

public class JoyCamGrabController : MonoBehaviour 
{
	UISprite sprite;
	public GameObject obJoystick;
	public GameObject obCamContainer;
	// Use this for initialization
	void Start () {
		sprite = this.gameObject.GetComponent<UISprite> ();
	}

	public Vector3 vDir;

	public void Reset()
	{
		bTouch = false;
		vDir = Vector3.zero;
	}

	public Vector3 Get2DPos(Vector3 Pos3D)
	{
		
		Camera cam2D = Camera.allCameras [0];
		Vector3 pos2D =	 cam2D.WorldToScreenPoint (Pos3D);
		pos2D.x -= Screen.width * 0.5f;
		pos2D.y -= Screen.height *0.5f;
		pos2D.z = 0;
		return pos2D;
	}

	bool bTouch = false;
	GameObject currTouchedObj = null;
	ControllerType currType;
	Vector3 startPos;
	// Update is called once per frame
	void Update () 
	{
		if ( !bTouch && Input.GetMouseButtonDown(0) )
		{
			currType = ControllerType.None;
			bTouch = true;
			currTouchedObj = null;
			startPos = Input.mousePosition;
			RaycastHit hit;
			
			Camera cam2D = Camera.allCameras [1];
			Ray ray = cam2D.ScreenPointToRay( startPos );
			
			if (Physics.Raycast (ray, out hit, 1000.0f , 1<<GameManager.LayerGUI ))
			{
				//Debug.LogError("hit gui "+hit.collider.transform.parent.name+"."+hit.collider.name);
			}
			else
			{
				Camera cam3D = Camera.allCameras [0];
				ray = cam3D.ScreenPointToRay( startPos );
				if (Physics.Raycast (ray, out hit, 1000.0f , 1<<GameManager.LayerJoystickController ))
				{
					currType = ControllerType.Joystick;
					currTouchedObj = hit.collider.gameObject;
				}
				else if (Physics.Raycast (ray, out hit, 1000.0f , 1<<GameManager.LayerGrabButton ))
				{
					currType = ControllerType.GrabButton;
					currTouchedObj = hit.collider.gameObject;
				}
				else if (Physics.Raycast (ray, out hit, 1000.0f , 1<<GameManager.LayerCamController ))
				{
					currType = ControllerType.Camera;
					currTouchedObj = hit.collider.gameObject;
				}

			}
		}

		if ( Input.GetMouseButtonUp(0) )
		{
			if ( currTouchedObj )
			{
				if ( currType == ControllerType.Camera )
				{
					Vector3 currPos = Input.mousePosition;

					if (  Mathf.Abs(startPos.x-currPos.x) > 40 )
					{
						int iDir = 0;
						if ( startPos.x < currPos.x  )
							iDir = 1; //left
						else if ( startPos.x > currPos.x )
							iDir = -1; //right
						Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag("Gamestate").GetComponent<Gamestate_Gameplay>();
						if ( gs )
						{
							gs.OnSwipe(iDir);
						}
					}
				}
				else if ( currType == ControllerType.GrabButton )
				{
					
					RaycastHit hit;
					
					Camera cam3D = Camera.allCameras [0];
					Ray ray = cam3D.ScreenPointToRay( Input.mousePosition );
					if (Physics.Raycast (ray, out hit, 1000.0f , 1<<GameManager.LayerGrabButton ))
					{
						Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag("Gamestate").GetComponent<Gamestate_Gameplay>();
						if ( gs )
						{
							gs.GrabObject();
						}
					}
					else
					{

					}
				}
			}
			currType = ControllerType.None;
			fPutJoyBackTimer = 0;
			vDir = Vector3.zero;
			bTouch = false;
			currTouchedObj = null;
		}

		//Debug.LogError("touching "+Input.mousePosition);
		if ( bTouch && currTouchedObj != null )
		{
			if ( currType == ControllerType.Joystick )
			{
				Vector3 initPos = Input.mousePosition+new Vector3(-Screen.width*0.5f,-Screen.height*0.5f,0);
				//debugSprite2.transform.localPosition = initPos ;
				Vector3 targetPos = Get2DPos(currTouchedObj.transform.position);
				Vector3 vDiff = (targetPos-initPos);
				vDiff *= 0.01f;

				if ( Mathf.Abs(vDiff.x) > 1 )
					if ( vDiff.x > 0 )
						vDiff.x = 1;
					else if ( vDiff.x < 0 )
						vDiff.x = -1;

				if ( Mathf.Abs(vDiff.y) > 1 )
					if ( vDiff.y > 0 )
						vDiff.y = 1;
				else if ( vDiff.y < 0 )
					vDiff.y = -1;

				if ( Mathf.Abs(vDiff.z) > 1 )
					if ( vDiff.z > 0 )
						vDiff.z = 1;
				else if ( vDiff.z < 0 )
					vDiff.z = -1;

				Vector3 euler = obJoystick.transform.eulerAngles;
				euler.x = -vDiff.y*30;
				euler.z = vDiff.x*30;	
				obJoystick.transform.eulerAngles = euler;
				//lastEuler = obJoystick.transform.eulerAngles;
				vDir = vDiff;
				//Debug.LogError("vDir="+vDir);
			}
		}
		if ( obJoystick && bTouch == false  )
		{
			obJoystick.transform.localEulerAngles = targetEuler;
			//Quaternion euler = obJoystick.transform.rotation;
			//obJoystick.transform.localEulerAngles = Vector3.Lerp(lastEuler,targetEuler,fPutJoyBackTimer);
			//fPutJoyBackTimer += Time.deltaTime;
		}
	}

	public void SetEuler()
	{
		targetEuler = obJoystick.transform.localEulerAngles;
	}

	Vector3 lastEuler;
	float fPutJoyBackTimer = 0;
	Vector3 targetEuler;
	void LateUpdate()
	{

	}

}
