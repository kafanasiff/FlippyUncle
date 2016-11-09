using UnityEngine;
using System.Collections;

public class CentreShareButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		#if UNITY_WEBGL
		transform.position = new Vector3(0.25f,transform.position.y,transform.position.z);
		#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
