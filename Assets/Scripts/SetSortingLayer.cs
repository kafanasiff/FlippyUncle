using UnityEngine;
using System.Collections;

public class SetSortingLayer : MonoBehaviour {

	public string sortingLayerName = "";

	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer> ().sortingLayerName = sortingLayerName;
		GetComponent<MeshRenderer> ().sortingOrder = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
