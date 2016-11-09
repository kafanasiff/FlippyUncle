using UnityEngine;
using System.Collections;

public class Music : MonoBehaviour {

	private int musicMode = 0;

	// Use this for initialization
	void Start () {
		musicMode = PlayerPrefs.GetInt ("Music");
		if (musicMode == 0) {
			gameObject.GetComponent<AudioSource>().Play ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
