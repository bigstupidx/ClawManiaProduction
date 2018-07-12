using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Pengait : MonoBehaviour {
	
	public Transform laserAttachTo;
	public Transform laserTargetTo;
	float m_value;
	public Hook[] hooks;
	public Transform targetParent;
	ArrayList listTrigger;
	GameObject target;
	public bool DoneGrabbing = false;
	public float maxRotateHook;
	
	public AudioClip audioClawGrab;
	public AudioClip audioMoveClaw;
	public AudioClip audioClawDrop;
	public BoxCollider[] colliders;
	// Use this for initialization
	void Start () {
		value = 0;
		listTrigger = new ArrayList ();
		for ( int i=0; i<hooks.Length; i++ )
			listTrigger.Add (null);
	}

	public void ClearListTrigger()
	{
		for ( int i=0; i<listTrigger.Count; i++ )
		{
			listTrigger[i] = null;
		}
		for ( int i=0; i<hooks.Length; i++ )
		{
			if ( hooks[i] )
			{
				hooks[i].sensor.currentCollider = null;
			}
		}
		target = null;
		DoneGrabbing = false;
	}

	public GameObject GetTarget()
	{
		return target;
	}

	/*
	public void ActivateSensors()
	{
		for ( int i=0; i<hooks.Length; i++ )
		{
			
			Hook hook = hooks[i];
			EnableCollider(hook.gameObject,true);
		}
	}
*/
	public void EnableCollider(bool val)
	{
		/*
		Collider collider = ob.GetComponent<Collider> ();
		if  ( collider )
			collider.enabled = val;
		for ( int i=0; i<ob.transform.childCount; i++ )
		{
			Transform trChild = ob.transform.GetChild(i);
			EnableCollider(trChild.gameObject,val);
		}
		*/
		Debug.LogError ("EnableCollider = " + val);
		for ( int i=0; i<colliders.Length; i++ )
		{
			colliders[i].enabled = val;
		}
	}

	/*
	public void DeactivateSensors()
	{
		for ( int i=0; i<hooks.Length; i++ )
		{

			Hook hook = hooks[i];
			EnableCollider(hook.gameObject,false);
		}
	}
	*/

	public void SetListTrigger(ClawSensor sensor)
	{
		string sDebug = "[Pengait] SetListTrigger";
		if ( sensor )
		{
			sDebug += "sensor=" + sensor.name+" collider=";
			if ( sensor.currentCollider )
				sDebug += " " + sensor.currentCollider.name;
			else
				sDebug += " NONE";
		}
		//Debug.LogError (sDebug);
		Gamestate_Gameplay gs = GameObject.FindGameObjectWithTag ("Gamestate").GetComponent<Gamestate_Gameplay> ();
		if (gs.grabState != GrabState.Grabbing )
			return;

		if ( target != null )
			return;

		if ( sensor == null )
			return;
		
		//Debug.LogError ("[Pengait] SetListTrigger:process");
		for ( int i=0; i<hooks.Length ; i++ )
		{
			if ( hooks[i].gameObject == sensor.transform.parent.parent.parent.gameObject )
			{
				listTrigger[i] = sensor.currentCollider;
				break;
			}
		}

		bool bAllTheSame = true;
		GameObject lastCheck = null;
		for ( int i=0; i<listTrigger.Count; i++)
		{
			if ( i==0 )
			{
				lastCheck = (GameObject)listTrigger[i];
			}
			else
			{
				if ( listTrigger[i] == null )
				{
					bAllTheSame = false;
					break;
				}
				else if ( listTrigger[i] != null && listTrigger[i] != lastCheck )
				{
					bAllTheSame = false;
					break;
				}
				lastCheck = (GameObject)listTrigger[i];
			}
		}

		if (bAllTheSame)
		{
			//Debug.LogError("all the same");
			DoneGrabbing = true;
			GameObject temp = ((GameObject)listTrigger[0]);
			SetTarget(temp);
			//Debug.LogError("done");
		}
	}

	public void SetTarget(GameObject temp)
	{
		Debug.LogError ("SetTarget " + temp.name);
		target = temp.transform.parent.gameObject;
		temp.GetComponent<Rigidbody>().isKinematic = true;
		target.transform.parent = targetParent.transform;
		temp.GetComponent<Collider>().enabled = false;
	}

	float fTargetValue = 0;
	public float value
	{
		get { return m_value; }
		set {fTargetValue = value;}
	}

	public void ReleaseTarget()
	{
		//Debug.LogError ("release target");
		if ( target == null )
			return;
		//Debug.LogError ("target=" + target.name);
		target.transform.parent = GameObject.FindGameObjectWithTag ("Gamestate").transform;
		Rigidbody rigids = target.GetComponentInChildren<Rigidbody>();
		if ( rigids )
			rigids.isKinematic = false;
		Collider collider = target.GetComponentInChildren<Collider>();
		if ( collider )
			collider.enabled = true;

		target = null;
	}

	// Update is called once per frame
	void Update () 
	{
		if ( fTargetValue == m_value )
			return;

		if ( fTargetValue > m_value )
		{
			m_value += Time.deltaTime;
			if ( m_value > fTargetValue )
				m_value = fTargetValue;
		}
		else if ( fTargetValue < m_value )
		{
			m_value -= Time.deltaTime;
			if ( m_value < fTargetValue )
				m_value = fTargetValue;
		}

		if ( hooks != null )
		{
			for ( int i=0; i<hooks.Length; i++ )
			{
				Hook hook = hooks[i];
				if ( hook )
				{
					Vector3 euler = hook.transform.localEulerAngles;
					euler.z = ( m_value * maxRotateHook );
					//euler.y = i*120;
					euler.x = 0;
					hook.transform.localEulerAngles = euler;
				}
			}
		}

	}

	public void PlayAudio(AudioClip clip)
	{
		if ( GetComponent<AudioSource>() == null )
			return;
		if ( GetComponent<AudioSource>().isPlaying && GetComponent<AudioSource>().clip == clip )
			return;
		
		if ( GameManager.bSoundOn == false )
			return;
		//Debug.LogError ("play audio");
		GetComponent<AudioSource>().volume = 0.5f;
		GetComponent<AudioSource>().clip = clip;
		GetComponent<AudioSource>().Play ();
	}

	void OnGUI()
	{
		/*
		string debug = "";
		if ( listTrigger != null )
		for ( int i=0; i<listTrigger.Count; i++ )
		{
			GameObject trigger = (GameObject)listTrigger[i];
			debug += "trig["+i.ToString()+"]=";
			if ( trigger == null )
			{
				debug += " null";
			}
			else
			{
				debug += " "+trigger.name;
			}
			debug += "\n";
		}
		GUI.Label(new Rect(0,0,200,100),debug);
		*/
	}
}
