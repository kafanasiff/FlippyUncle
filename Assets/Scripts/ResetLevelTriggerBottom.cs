using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ResetLevelTriggerBottom : MonoBehaviour {

	public GameObject level;
	private float stepIncrement = 0.5f;
	public GameObject[] stars;
	public TextMesh roundText;
	private int round = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider hit) {
		if (hit.transform.gameObject.name == "ResetLevelTriggerTop") {
			Debug.Log ("Reset level");
			level.transform.position = new Vector3(0,0,0);
			float step = level.GetComponent<MoveDown>().GetStep();
			step += stepIncrement;
			level.GetComponent<MoveDown>().SetStep(step);
			foreach (GameObject star in stars) {
				star.SetActive(true);
			}

			if (PlayerPrefs.GetInt ("Practice") == 0) {

				int currentCupsUnlocked = PlayerPrefs.GetInt ("CupsUnlocked");
				string levelName = SceneManager.GetActiveScene().name;
				switch (levelName) {
				case "Flippy0":
					if (currentCupsUnlocked == 0) { PlayerPrefs.SetInt ("CupsUnlocked", 1); }
					break;
				case "Flippy1":
					if (currentCupsUnlocked == 1) { PlayerPrefs.SetInt ("CupsUnlocked", 2); }
					break;
				default:
					break;
				}
			}

			round++;
			roundText.text = "Round " + round.ToString ();
			roundText.gameObject.SetActive (true);
			Invoke ("DisableRoundText", 2f);
		}
	}

	void DisableRoundText() {
		roundText.gameObject.SetActive (false);
	}
}
