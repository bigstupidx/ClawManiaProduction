using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MascotManager : MonoBehaviour {

	public static MascotManager instance = null;

	public List<MascotController> mascots = new List<MascotController>();
	public List<Transform> randomPositions = new List<Transform>();

	private int currentMascotIdx;
	private int currentPositionIdx;

	void Awake()
	{
		
		//Disable macots
		for (int i = 0; i < mascots.Count; ++i)
		{
			mascots[i].gameObject.SetActive(false);
		}
		for (int i = 0; i < randomPositions.Count; ++i)
		{
			randomPositions[i].gameObject.SetActive(false);
		}
		if (instance == null)
		{
			instance = this;
		}
		else 
		{
			DestroyImmediate(this.gameObject);
		}

	}

	void OnDestroy()
	{
		if (instance == this)
			instance = null;
	}


	// Use this for initialization
	void Start () {
		//Random mascot
		currentMascotIdx = Random.Range(0, mascots.Count);
		currentPositionIdx = Random.Range(0, randomPositions.Count);

		//Activate curent mascot & pos
		mascots[currentMascotIdx].transform.localPosition = randomPositions[currentPositionIdx].localPosition;
		mascots[currentMascotIdx].gameObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
