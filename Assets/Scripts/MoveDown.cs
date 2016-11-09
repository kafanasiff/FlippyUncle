using UnityEngine;
using System.Collections;

public class MoveDown : MonoBehaviour {

	public float step = 1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate(Vector3.down * (Time.deltaTime * step));
	}

	public float GetStep() {
		return step;
	}

	public void SetStep(float toSet) {
		step = toSet;
	}

}
