using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SnowflakeTest : MonoBehaviour {
	public GameObject testObj;
	public GameObject spawner;

	float spanTimer = 1f;
	float spanTimer2 = 10f;
	float timer = 0;
	float timer2 = 0;

	bool moveRight=true;

	// Use this for initialization
	void Start () {
		//StartCoroutine(spawnerAlternatePosition());
	}
	
	// Update is called once per frame
	void Update ()
	{
		timer += Time.deltaTime;

		if (timer > spanTimer) {
			GameObject temp = GameObject.Instantiate (testObj, new Vector3 (130, 0, 0), this.transform.rotation) as GameObject;
			temp.transform.SetParent (spawner.transform, false);
			StartCoroutine (fallingSnow (temp));
			timer = 0;
		}

		if (spawner.transform.position.x > 950) {
			//moveRight=false;
		} else if (spawner.transform.position.x < 130) {
			//moveRight=true;
		}

		timer2 += Time.deltaTime;

		if (timer2 > spanTimer2) {
			moveRight = !moveRight;
			timer2 = 0;
		}

		if (moveRight) {
			spawner.transform.Translate (Vector3.right * Time.deltaTime*30);
		} else {
			spawner.transform.Translate (Vector3.left * Time.deltaTime*30);
		}
	}

	IEnumerator fallingSnow (GameObject obj)
	{
		float elapsedTime = 0;
		float time = 10;

		Vector3 goalPos = new Vector3 (0, -1980f, 0);
		Vector3 startPos = obj.transform.position;

		int randSpeed = Random.Range (1, 10);

		while (elapsedTime < time) {
			obj.transform.position = Vector3.Lerp (startPos, goalPos, (elapsedTime / time));
			elapsedTime += (Time.deltaTime * 0.1f * randSpeed);
			yield return null;
		}
	}

	IEnumerator spawnerAlternatePosition ()
	{
		float elapsedTime = 0;
		float time = 10;

		Vector3 leftPos = new Vector3 (130, 0, 0);
		Vector3 rightPos = new Vector3 (950, 0, 0);

		while (elapsedTime < time) {
			if(moveRight)
				spawner.transform.position = Vector3.Lerp(spawner.transform.position,rightPos,(elapsedTime/time));
			else
				spawner.transform.position = Vector3.Lerp(spawner.transform.position,leftPos,(elapsedTime/time));

			elapsedTime+=(Time.deltaTime*0.1f);
			yield return null;
		}
	}
}
