using UnityEngine;
using System.Collections;

public class Rotate_Object : MonoBehaviour
{
	public Vector3 rotation;
	void Update ()
	{
	    transform.Rotate(rotation * Time.deltaTime);
	}
}
