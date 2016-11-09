using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class Title : MonoBehaviour {

	private Ray ray;
	private RaycastHit hit;

	public AudioSource tapSound;

	// Use this for initialization
	void Start () {
		InitializeSocialPlatforms ();
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID || UNITY_IOS
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
			ray = Camera.main.ScreenPointToRay(Input.GetTouch (0).position);
			if (Physics.Raycast(ray, out hit)) {
				GetInput (hit);
			}
		}
		#endif



		#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_WEBGL
		if (Input.GetMouseButtonUp(0)) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				GetInput (hit);
			}
		}
		#endif

		#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_ANDROID || UNITY_WEBGL
		if (Input.GetKeyUp (KeyCode.Escape)) {
			SceneManager.LoadScene("Cups");
		}
		#endif
	}



	void GetInput(RaycastHit hit) {
		if (hit.transform.gameObject.name == "FullScreenCollider") {
			if (PlayerPrefs.GetInt ("Sound") == 0) {
				tapSound.Play ();
			}
			Invoke ("LoadGame", 0.5f);

		}
		if (hit.transform.gameObject.name == "Button_Twitter") {
			SocialMediaManager.OpenTwitterPage ();
		}
		if (hit.transform.gameObject.name == "Button_Facebook") {
			SocialMediaManager.OpenFacebookPage ();
		}
		if (hit.transform.gameObject.name == "Button_MoreApps") {
			Debug.Log ("More apps!");
			#if UNITY_ANDROID || UNITY_IOS
			CBManager.ShowMoreApps ();
			#endif
		}
	}

	void InitializeSocialPlatforms() {
		#if UNITY_ANDROID
		// recommended for debugging:
		PlayGamesPlatform.DebugLogEnabled = false;
		// Activate the Google Play Games platform
		PlayGamesPlatform.Activate();
		#endif

		#if UNITY_ANDROID || UNITY_IOS
		Social.localUser.Authenticate((bool success) => {

		});
		#endif	
	}

	void LoadGame() {
		SceneManager.LoadScene ("Cups");
	}
}
