	using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

using UnityEngine.SocialPlatforms;

public class CupSelector : MonoBehaviour {

	private Ray ray;
	private RaycastHit hit;

	public GameObject avatarSelector;

	public List<SpriteRenderer> cups;
	public List<Sprite> cupSprites;

	public TextMesh starText;

	public List<Sprite> avatarSprites;
	public SpriteRenderer avatarButtonRenderer;

	private int cupsUnlocked;

	private int practiceMode = 0;
	public TextMesh practiceModeText;
	public GameObject crossedOutSprite;
	public GameObject practiceSprite;

	private int soundMode = 0;
	public GameObject soundCrossedOut;

	private int musicMode = 0;
	public GameObject musicCrossedOut;
	public AudioSource music;

	private int cupCost = 100;
	public GameObject lockedCup3;
	public GameObject unlockedCup3;
	public GameObject lockedCup4;
	public GameObject unlockedCup4;
	public GameObject lockedCup5;
	public GameObject unlockedCup5;

	public AudioSource tapSound;

	// Use this for initialization
	void Start () {
		//Debug.Log (Social.localUser.authenticated);

		// For debugging
		//ResetPlayerPrefs ();

		avatarSelector.SetActive(false);
		// Android Initialization
	
		// cache here instead of on title to make sure chartboost is initialized
		#if UNITY_ANDROID || UNITY_IOS
		CBManager.CacheInterstitial();
		CBManager.CacheRewardedVideo();
		CBManager.CacheMoreApps();
		#endif

		// Set practice mode button
		practiceMode = PlayerPrefs.GetInt ("Practice");
		if (practiceMode == 0) {
			practiceModeText.text = "Practice mode off";
			crossedOutSprite.SetActive (false);
			practiceSprite.GetComponent<Animator>().enabled = true;
		} 
		else {
			practiceModeText.text = "Practice mode on";
			crossedOutSprite.SetActive (true);
			practiceSprite.GetComponent<Animator>().enabled = false;
		}

		// Set sound button
		soundMode = PlayerPrefs.GetInt ("Sound");
		if (soundMode == 0) {
			soundCrossedOut.SetActive(false);
		} 
		else {
			soundCrossedOut.SetActive(true);
		}

		// Set music button
		musicMode = PlayerPrefs.GetInt ("Music");
		if (musicMode == 0) {
			musicCrossedOut.SetActive(false);
		} 
		else {
			musicCrossedOut.SetActive(true);
		}
		//Set Avatar Button Sprite
		avatarButtonRenderer.sprite = avatarSprites [PlayerPrefs.GetInt ("Avatar")];

		// Set # of stars
		starText.text = PlayerPrefs.GetInt ("Stars").ToString ();

		// Set Cup Sprites
		if (PlayerPrefs.GetInt ("Cup3") == 1) {
			lockedCup3.SetActive (false);
			unlockedCup3.SetActive (true);
		}
		if (PlayerPrefs.GetInt ("Cup4") == 1) {
			lockedCup4.SetActive (false);
			unlockedCup4.SetActive (true);
		}
		if (PlayerPrefs.GetInt ("Cup5") == 1) {
			lockedCup5.SetActive (false);
			unlockedCup5.SetActive (true);
		}

	}

	void ResetPlayerPrefs() {
		PlayerPrefs.SetInt ("Avatar", 0);

		PlayerPrefs.SetInt ("Avatar1", 0);
		PlayerPrefs.SetInt ("Avatar2", 0);
		PlayerPrefs.SetInt ("Avatar3", 0);
		PlayerPrefs.SetInt ("Avatar4", 0);
		PlayerPrefs.SetInt ("Avatar5", 0);
		PlayerPrefs.SetInt ("Avatar6", 0);
		PlayerPrefs.SetInt ("Avatar7", 0);
		PlayerPrefs.SetInt ("Avatar8", 0);
		PlayerPrefs.SetInt ("Avatar9", 0);
		PlayerPrefs.SetInt ("Avatar10", 0);
		PlayerPrefs.SetInt ("Avatar11", 0);
		PlayerPrefs.SetInt ("Avatar12", 0);
		PlayerPrefs.SetInt ("Avatar13", 0);
		PlayerPrefs.SetInt ("Avatar14", 0);
		PlayerPrefs.SetInt ("Avatar15", 0);
		PlayerPrefs.SetInt ("Avatar16", 0);
		PlayerPrefs.SetInt ("Avatar17", 0);

		PlayerPrefs.SetInt ("AvatarUnlocked", 0);
		PlayerPrefs.SetInt ("Stars", 0);
		PlayerPrefs.SetInt ("Practice", 0);
		PlayerPrefs.SetInt ("Sound", 0);

		//Unlockable cups
		PlayerPrefs.SetInt ("Cup3", 0);
		PlayerPrefs.SetInt ("Cup4", 0);
		PlayerPrefs.SetInt ("Cup5", 0);

	}

	// Update is called once per frame
	void Update () {

		#if UNITY_ANDROID || UNITY_IOS
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
			ray = Camera.main.ScreenPointToRay(Input.GetTouch (0).position);
			if (Physics.Raycast(ray, out hit)) {
				GetButtonInput (hit);
			}
		}
		#endif
		
		
		
		#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_WEBGL
		if (Input.GetMouseButtonUp(0)) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				GetButtonInput (hit);
			}
		}
		
		#endif
		
		#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_ANDROID || UNITY_WEBGL
		if (Input.GetKeyUp(KeyCode.Escape)) {
			SceneManager.LoadScene ("Title");
		}
		#endif
	}

	void GetButtonInput(RaycastHit hit) {
		if (hit.transform.gameObject.name == "Button_Avatar") {
			avatarSelector.SetActive (true);
			gameObject.SetActive(false);
		}
		if (hit.transform.gameObject.name == "Button_Leaderboards") {
			ShowLeaderboards();
		}
		if (hit.transform.gameObject.name == "Button_Achievements") {
			ShowAchievements();
		}
		if (hit.transform.gameObject.name == "Button_PracticeMode") {
			CyclePracticeMode();
		}
		if (hit.transform.gameObject.name == "Button_Sound") {
			CycleSound();
		}
		if (hit.transform.gameObject.name == "Button_Music") {
			CycleMusic();
		}
		if (hit.transform.gameObject.name == "Button_Back") {
			SceneManager.LoadScene ("Title");
		}
		if (hit.transform.gameObject.name == "Cup0") {
			SceneManager.LoadScene ("Flippy0");
		}
		if (hit.transform.gameObject.name == "Cup1") {
			SceneManager.LoadScene ("Flippy1");
		}
		if (hit.transform.gameObject.name == "Cup2") {
			SceneManager.LoadScene ("Flippy2");
		}
		if (hit.transform.gameObject.name == "Cup3") {
			if (PlayerPrefs.GetInt ("Cup3") == 0) {
				int stars = PlayerPrefs.GetInt ("Stars");
				if (stars >= cupCost) {
					stars = stars - cupCost;
					PlayerPrefs.SetInt ("Stars", stars);
					starText.text = stars.ToString ();
					lockedCup3.SetActive (false);
					unlockedCup3.SetActive (true);
					PlayerPrefs.SetInt ("Cup3", 1);
					if (PlayerPrefs.GetInt ("Sound") == 0) {
						tapSound.Play ();
					}
				}
			}
			else {
				SceneManager.LoadScene ("Flippy3");
			}
		}
		if (hit.transform.gameObject.name == "Cup4") {
			if (PlayerPrefs.GetInt ("Cup4") == 0) {
				int stars = PlayerPrefs.GetInt ("Stars");
				if (stars >= cupCost) {
					stars = stars - cupCost;
					PlayerPrefs.SetInt ("Stars", stars);
					starText.text = stars.ToString ();
					lockedCup4.SetActive (false);
					unlockedCup4.SetActive (true);
					PlayerPrefs.SetInt ("Cup4", 1);
					if (PlayerPrefs.GetInt ("Sound") == 0) {
						tapSound.Play ();
					}
				}
			}
			else {
				SceneManager.LoadScene ("Flippy4");
			}
		}
		if (hit.transform.gameObject.name == "Cup5") {
			if (PlayerPrefs.GetInt ("Cup5") == 0) {
				int stars = PlayerPrefs.GetInt ("Stars");
				if (stars >= cupCost) {
					stars = stars - cupCost;
					PlayerPrefs.SetInt ("Stars", stars);
					starText.text = stars.ToString ();
					lockedCup5.SetActive (false);
					unlockedCup5.SetActive (true);
					PlayerPrefs.SetInt ("Cup5", 1);
					if (PlayerPrefs.GetInt ("Sound") == 0) {
						tapSound.Play ();
					}
				}
			}
			else {
				SceneManager.LoadScene ("Flippy5");
			}
		}
	}

	void CycleSound() {
		if (soundMode == 0) {
			PlayerPrefs.SetInt ("Sound", 1);
			soundMode = 1;
			soundCrossedOut.SetActive (true);
		} 
		else {
			PlayerPrefs.SetInt ("Sound", 0);
			soundMode = 0;
			soundCrossedOut.SetActive (false);
		}
	}

	void CycleMusic() {
		if (musicMode == 0) {
			PlayerPrefs.SetInt ("Music", 1);
			musicMode = 1;
			musicCrossedOut.SetActive (true);
			music.Stop ();
		} 
		else {
			PlayerPrefs.SetInt ("Music", 0);
			musicMode = 0;
			musicCrossedOut.SetActive (false);
			music.Play ();
		}
	}

	void CyclePracticeMode() {
		if (practiceMode == 0) {
			PlayerPrefs.SetInt ("Practice", 1);
			practiceMode = 1;
			practiceModeText.text = "Practice mode on";
			crossedOutSprite.SetActive(true);
			practiceSprite.GetComponent<Animator>().enabled = false;
		} 
		else {
			PlayerPrefs.SetInt ("Practice", 0);
			practiceMode = 0;
			practiceModeText.text = "Practice mode off";
			crossedOutSprite.SetActive(false);
			practiceSprite.GetComponent<Animator>().enabled = true;
		}
	}

	void ShowAchievements() {
		#if UNITY_ANDROID || UNITY_IOS
		Social.ShowAchievementsUI();
		#endif
	}

	void ShowLeaderboards() {
		#if UNITY_ANDROID || UNITY_IOS
		Social.ShowLeaderboardUI();
		#endif
	}

	void AddStars(int toAdd) {
		PlayerPrefs.SetInt ("Stars", PlayerPrefs.GetInt ("Stars") + toAdd);
		starText.text = PlayerPrefs.GetInt ("Stars").ToString ();
	}
}
