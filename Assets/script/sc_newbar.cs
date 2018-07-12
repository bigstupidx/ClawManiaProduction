using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class sc_newbar : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Image image = GetComponent<Image>();
		image.fillAmount += 0.1f * Time.deltaTime;
	}
}
