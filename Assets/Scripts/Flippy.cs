using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

public class Flippy : MonoBehaviour {
	

	bool getInput = true;
	private bool starting = true;
	private bool paused = false;
	private bool gameOver = false;
	public Camera flippyCamera;
	public GameObject buttonPause;

	private Ray ray;
	private RaycastHit hit;

	private float yForce = 300;
	private float xForceModifier = 100;
	private float yRotationModifier = 1000;

	private int score = 0;
	private int finalScore = 0;
	public TextMesh scoreText;
	public TextMesh endScoreText;
	public TextMesh endStarsText;
	public TextMesh finalScoreText;

	public GameObject scoreIndicator;
	private int stars = 0;
	public TextMesh starsText;

	public SpriteRenderer avatarRenderer;
	public List<Sprite> avatarSprites;

	public GameObject startDialog;
	public GameObject pauseDialog;
	public GameObject gameOverDialog;
	public List<GameObject> medals;
	public TextMesh noMedalText;

	public GameObject topCollider;
	public GameObject bottomCollider;

	public GameObject level;

	public ParticleSystem fireParticles;
	public ParticleSystem waterParticles;

	public AudioSource starSound;
	public AudioSource pipeSound;
	public AudioSource dropSound;
	public AudioSource fireSound;
	public AudioSource gameOverSound;
	public AudioSource music;

	private int practice;
	private int soundMode = 0;

	public GameObject giftButton;

	public GameObject practiceModeText;
	public GameObject deathImageTop;
	public GameObject deathImageBotton;

	public GameObject shareDialog;
	private bool sharedToTwitter = false;
	private bool sharedToFacebook = false;
	private bool sharedToGooglePlus = false;
	private bool sharedToReddit = false;
	private bool sharedToLinkedIn = false;

	public TextMesh roundText;

	// Use this for initialization
	void Start () {
		giftButton.SetActive (false);
		Time.timeScale = 0;
		startDialog.SetActive (true);
		pauseDialog.SetActive (false);
		gameOverDialog.SetActive (false);
		soundMode = PlayerPrefs.GetInt ("Sound");
		SetStars ();
		SetAvatar ();
		practice = PlayerPrefs.GetInt ("Practice");

		if (practice == 0) {
			topCollider.SetActive (false);
			bottomCollider.SetActive (false);
			InvokeRepeating ("AddScorePoint", 1f, 1f);
			buttonPause.SetActive(false);
			practiceModeText.SetActive (false);
			deathImageTop.SetActive (true);
			deathImageBotton.SetActive (true);
		} 
		else {
			scoreIndicator.SetActive (false);
			topCollider.SetActive (true);
			bottomCollider.SetActive (true);
			buttonPause.SetActive(true);
			practiceModeText.SetActive (true);
			deathImageTop.SetActive (false);
			deathImageBotton.SetActive (false);
		}



	}

	void AddScorePoint() {
		score++;
		scoreText.text = score.ToString ();
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log ("Paused = " + paused);
		//Debug.Log ("Game Over = " + gameOver);

		if (getInput) {
			if (starting && !paused) {
				#if UNITY_ANDROID || UNITY_IOS
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
					ray = flippyCamera.ScreenPointToRay (Input.GetTouch (0).position);
					if (Physics.Raycast (ray, out hit)) {
						if (hit.transform.gameObject.name == "FullScreenCollider") {
							StartPlaying();
						}

					}
					if (hit.transform.gameObject.name == "Button_Pause") {
						Pause ();
					}
				}
				#endif
				
				#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_WEBGL
				if (Input.GetMouseButtonUp (0)) {
					ray = flippyCamera.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit)) {
						if (hit.transform.gameObject.name == "FullScreenCollider") {
							StartPlaying();
						}

					}
					if (hit.transform.gameObject.name == "Button_Pause") {
						Pause ();
					}
				}
				#endif
			}

			if (!paused && starting == false) {
				#if UNITY_ANDROID || UNITY_IOS
				if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
					ray = flippyCamera.ScreenPointToRay (Input.GetTouch (0).position);
					if (Physics.Raycast (ray, out hit)) {
						if (hit.transform.gameObject.name == "FullScreenCollider") {
							float xMousePos = flippyCamera.ScreenToWorldPoint (Input.GetTouch (0).position).x;
							float xForce = (xMousePos - transform.position.x) * xForceModifier;
							float yRotation = (xMousePos - transform.position.x) * yRotationModifier;
							Flip (-xForce, yRotation);
						}
						if (hit.transform.gameObject.name == "Button_Pause") {
							Pause ();
						}
					}
				}
				#endif
				
				#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_WEBGL
				if (Input.GetMouseButtonUp (0)) {
					ray = flippyCamera.ScreenPointToRay (Input.mousePosition);
					if (Physics.Raycast (ray, out hit)) {
						if (hit.transform.gameObject.name == "FullScreenCollider") {
							float xMousePos = flippyCamera.ScreenToWorldPoint (Input.mousePosition).x;
							float xForce = (xMousePos - transform.position.x) * xForceModifier;
							float yRotation = (xMousePos - transform.position.x) * yRotationModifier;
							Flip (-xForce, yRotation);
						}
						if (hit.transform.gameObject.name == "Button_Pause") {
							Pause ();
						}
					}
				}
				#endif
			}
		}
			
		if (paused) {
			#if UNITY_ANDROID || UNITY_IOS
			if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) {
				ray = flippyCamera.ScreenPointToRay (Input.GetTouch (0).position);
				if (Physics.Raycast (ray, out hit)) {
					if (hit.transform.gameObject.name == "Button_Play") {
						if (gameOver) {
							SceneManager.LoadScene (SceneManager.GetActiveScene().name);
						} else {
							Resume ();
						}
					}
					if (hit.transform.gameObject.name == "Button_Leaderboards") {
						ShowLeaderboards();
					}
					if (hit.transform.gameObject.name == "Button_Quit") {
						/*
						if (!gameOver) {
							SaveStars ();
						}
						*/
						Time.timeScale = 1;
						Quit();
					}
					if (hit.transform.gameObject.name == "Button_Gift") {
						GetGift();
					}
					if (hit.transform.gameObject.name == "Button_Share") {
						shareDialog.SetActive (true);
					}
					if (hit.transform.gameObject.name == "Button_Back") {
						shareDialog.SetActive (false);
					}
					if (hit.transform.gameObject.name == "Button_Share_Twitter") {
						ShareToTwitter();
					}
					if (hit.transform.gameObject.name == "Button_Share_Facebook") {
						ShareToFacebook();
					}
					if (hit.transform.gameObject.name == "Button_Share_GooglePlus") {
						ShareToGooglePlus();
					}
					if (hit.transform.gameObject.name == "Button_Share_Reddit") {
						ShareToReddit();
					}
					if (hit.transform.gameObject.name == "Button_Share_LinkedIn") {
						ShareToLinkedIn();
					}
				}
			}
			#endif
			
			#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_WEBGL
			if (Input.GetMouseButtonUp (0)) {
				ray = flippyCamera.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out hit)) {
					if (hit.transform.gameObject.name == "Button_Play") {
						if (gameOver) {
							SceneManager.LoadScene (SceneManager.GetActiveScene().name);
						} else {
							Resume ();
						}
					}
					if (hit.transform.gameObject.name == "Button_Quit") {
						if (!gameOver) {
							Time.timeScale = 1;
							Quit();
						}
						Time.timeScale = 1;
						Quit();
					}
					if (hit.transform.gameObject.name == "Button_Gift") {
						GetGift();
					}
					if (hit.transform.gameObject.name == "Button_Share") {
						shareDialog.SetActive (true);
					}
					if (hit.transform.gameObject.name == "Button_Back") {
						shareDialog.SetActive (false);
					}
					if (hit.transform.gameObject.name == "Button_Share_Twitter") {
						ShareToTwitter();
					}
					if (hit.transform.gameObject.name == "Button_Share_Facebook") {
						ShareToFacebook();
					}
					if (hit.transform.gameObject.name == "Button_Share_GooglePlus") {
						ShareToGooglePlus();
					}
					if (hit.transform.gameObject.name == "Button_Share_Reddit") {
						ShareToReddit();
					}
					if (hit.transform.gameObject.name == "Button_Share_LinkedIn") {
						ShareToLinkedIn();
					}
				}
			}
			#endif
		}
		#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_ANDROID || UNITY_WEBGL
		if (Input.GetKeyUp (KeyCode.Escape)) {
			if (starting) {
				Quit();
			}
			else if (paused) {
				Resume ();
			} 
			else if (gameOver) {
				Quit();
			}
			else {
				Pause ();
			}
		}
		#endif
	}

	void AddStarsForSharing() {
		int starsAmount = 10;
		starsText.text = (int.Parse (starsText.text) + starsAmount).ToString ();
		PlayerPrefs.SetInt ("Stars", PlayerPrefs.GetInt ("Stars") + starsAmount);
	}

	void ShareToTwitter() {
		//Debug.Log ("Twitter");
		if (!sharedToTwitter) {
			AddStarsForSharing ();
			sharedToTwitter = true;
		}
		SocialMediaManager.ShareToTwitter ();
	}

	void ShareToFacebook() {
		//Debug.Log ("Facebook");
		if (!sharedToFacebook) {
			AddStarsForSharing ();
			sharedToFacebook = true;
		}
		SocialMediaManager.ShareToFacebook ();
	}

	void ShareToGooglePlus() {
		//Debug.Log ("Google Plus");
		if (!sharedToGooglePlus) {
			AddStarsForSharing ();
			sharedToGooglePlus = true;
		}
		SocialMediaManager.ShareToGooglePlus ();
	}

	void ShareToReddit() {
		//Debug.Log ("Google Plus");
		if (!sharedToReddit) {
			AddStarsForSharing ();
			sharedToReddit = true;
		}
		SocialMediaManager.ShareToReddit ();
	}

	void ShareToLinkedIn() {
		//Debug.Log ("Google Plus");
		if (!sharedToLinkedIn) {
			AddStarsForSharing ();
			sharedToLinkedIn = true;
		}
		SocialMediaManager.ShareToLinkedIn ();
	}

	void StartPlaying() {
		startDialog.SetActive (false);
		Time.timeScale = 1;
		starting = false;
		roundText.gameObject.SetActive (true);
		Invoke ("DisableRoundText", 2f);
	}

	void DisableRoundText() {
		roundText.gameObject.SetActive (false);
	}

	void GetGift() {
		music.Stop ();
		starsText.text = (int.Parse(starsText.text) + 25).ToString();
		#if UNITY_ANDROID || UNITY_IOS
		CBManager.ShowRewardedVideo();
		#endif
		int giftAmount = 25;
		stars += giftAmount;
		SaveStars();
		giftButton.SetActive(false);
	}

	void Flip(float xForce, float yRotation) {
		//rigidbody.velocity = new Vector3 (rigidbody.velocity.x, 0, 0);
		GetComponent<Rigidbody>().AddForce (new Vector3 (xForce, yForce, 0));
		GetComponent<Rigidbody>().AddTorque(new Vector3 (0,0,yRotation));
	}

	void OnTriggerEnter(Collider hit) {
		if (hit.transform.gameObject.tag == "Pickup_Star") {
			if (soundMode == 0) { starSound.Play (); }
			stars++;
			starsText.text = stars.ToString ();
			//Destroy (hit.gameObject);
			hit.gameObject.SetActive (false);
		}
		if (hit.transform.gameObject.name == "Death") {
			if (!gameOver) { GameOver (); }
		}
	}
	
	void OnCollisionEnter (Collision hit) {
		if (hit.transform.gameObject.tag == "Pipe") {
			if (soundMode == 0) { pipeSound.Play (); }
			
		}
		if (hit.transform.gameObject.tag == "Drop") {
			waterParticles.transform.position = hit.contacts[0].point;
			waterParticles.Play();
			if (soundMode == 0) { dropSound.Play (); }
			
		}
		if (hit.transform.gameObject.tag == "Fire") {
			fireParticles.transform.position = hit.contacts[0].point;
			fireParticles.Play();
			if (soundMode == 0) { fireSound.Play (); }
			if (practice == 0) { getInput = false; }
		}
	}

	void GameOver() {
		if (soundMode == 0) { gameOverSound.Play (); }
		getInput = false;
		buttonPause.SetActive (false);
		paused = true;
		gameOver = true;
		CancelInvoke ("AddScorePoint");


		endScoreText.text = score.ToString ();
		endStarsText.text = stars.ToString ();
		finalScore = score * (stars + 1);
		finalScoreText.text = finalScore.ToString ();



		level.SetActive (false);
		topCollider.SetActive (true);
		bottomCollider.SetActive (true);
		if (practice == 0) { SaveStars (); }

		if (practice == 0) {
			// 1st place (gold medal)
			if (finalScore >= 1500) {
				medals[0].SetActive(true);
				noMedalText.gameObject.SetActive (false);
			} 
			// 2nd place (silver medal)
			else if (finalScore >= 1000) {
				medals[1].SetActive(true);
				noMedalText.gameObject.SetActive (false);
			} 
			// 3rd place (bronze medal)
			else if (finalScore >= 500) {
				medals[2].SetActive(true);
				noMedalText.gameObject.SetActive (false);
			}
			else if (finalScore >= 400){
				noMedalText.text = "Almost there!";
				noMedalText.gameObject.SetActive (true);
			}
			else if (finalScore >= 300){
				noMedalText.text = "Doing great!";
				noMedalText.gameObject.SetActive (true);
			}
			else if (finalScore >= 200){
				noMedalText.text = "Getting Good!";
				noMedalText.gameObject.SetActive (true);
			}
			else if (finalScore >= 100){
				noMedalText.text = "Keep it up!";
				noMedalText.gameObject.SetActive (true);
			}
			else if (finalScore >= 50){
				noMedalText.text = "Nice try!";
				noMedalText.gameObject.SetActive (true);
			}
			else {
				noMedalText.text = "Don't give up!";
				noMedalText.gameObject.SetActive (true);
			}

			// Report Leaderboard Scores
			#if UNITY_ANDROID || UNITY_IOS
			if (Social.localUser.authenticated == true) {
				switch (SceneManager.GetActiveScene().name) {
				case "Flippy0":
					// Green Cup Leaderboard
					Social.ReportScore(finalScore, "CgkIxfGr_rsHEAIQBw", (bool success) => {
						// handle success or failure
					});
					break;
				case "Flippy1":
					// Blue Cup Leaderboard
					Social.ReportScore(finalScore, "CgkIxfGr_rsHEAIQCA", (bool success) => {
						// handle success or failure
					});
					break;
				case "Flippy2":
					// Red Cup Leaderboard
					Social.ReportScore(finalScore, "CgkIxfGr_rsHEAIQCQ", (bool success) => {
						// handle success or failure
					});
					break;
				case "Flippy3":
					// Green Blue Cup Leaderboard
					Social.ReportScore(finalScore, "CgkIxfGr_rsHEAIQDQ", (bool success) => {
						// handle success or failure
					});
					break;
				case "Flippy4":
					// Blue Red Cup Leaderboard
					Social.ReportScore(finalScore, "CgkIxfGr_rsHEAIQDg", (bool success) => {
						// handle success or failure
					});
					break;
				case "Flippy5":
					// Blue Red Cup Leaderboard
					Social.ReportScore(finalScore, "CgkIxfGr_rsHEAIQDw", (bool success) => {
						// handle success or failure
					});
					break;
				default:
					break;
				}
				// Gems leaderboard
				Social.ReportScore(PlayerPrefs.GetInt ("Stars"), "CgkIxfGr_rsHEAIQDA", (bool success) => {
					// handle success or failure
				});
			}

			// 1 in 3 chance to show gift button
			int chance = Random.Range(0,3);
			if (chance == 0) {
				giftButton.SetActive (true);
			}
		
			#endif

		}
		gameOverDialog.SetActive (true);
	}

	void Pause() {
		buttonPause.SetActive (false);
		paused = true;
		Time.timeScale = 0;
		pauseDialog.SetActive (true);
	}

	void Resume() {
		if (practice == 1) {
			buttonPause.SetActive (true);
		}
		Time.timeScale = 1;
		paused = false;
		pauseDialog.SetActive (false);
	}

	void SetStars() {
		stars = 0;
		starsText.text = stars.ToString ();
	}

	void SetAvatar() {
		int avatar = PlayerPrefs.GetInt ("Avatar");
		avatarRenderer.sprite = avatarSprites[avatar];
		switch (avatar) {
		case 0: // Scotty
			break;
		case 1: // Anachron
			GetComponent<Rigidbody>().mass = 1.1f;
			break;
		case 2: // Kid
			GetComponent<Rigidbody>().mass = 0.9f;
			break;
		case 3: // Cat
			GetComponent<Rigidbody>().mass = 0.7f;
			break;
		case 4: // Duck
			GetComponent<Rigidbody>().mass = 0.6f;
			break;
		case 5: // Elephant
			GetComponent<Rigidbody>().mass = 1.3f;
			break;
		case 6: // Frog
			GetComponent<Rigidbody>().mass = 0.5f;
			break;
		case 7: // Hippo
			GetComponent<Rigidbody>().mass = 1.2f;
			break;
		case 8: // Penguin
			GetComponent<Rigidbody>().mass = 0.8f;
			break;
		case 9: // Owl
			GetComponent<Rigidbody>().mass = 0.75f;
			break;
		case 10: // Whale
			GetComponent<Rigidbody>().mass = 1.35f;
			break;
		case 11: // Orange
			GetComponent<Rigidbody>().mass = 0.55f;
			break;
		case 12: // Slime
			GetComponent<Rigidbody>().mass = 1.15f;
			break;
		case 13: // Pewdiepie
			GetComponent<Rigidbody>().mass = 1.05f;
			break;
		case 14: // Brofist
			GetComponent<Rigidbody>().mass = 1.25f;
			break;
		case 15: // JackSepticEye
			GetComponent<Rigidbody>().mass = 0.65f;
			break;
		case 16: // SepticEye
			GetComponent<Rigidbody>().mass = 0.85f;
			break;
		case 17: // Unicorn
			GetComponent<Rigidbody>().mass = 0.95f;
			break;
		default:
			break;
		}	
	}

	public void SaveStars() {
		if (stars > 0) {
			PlayerPrefs.SetInt ("Stars", PlayerPrefs.GetInt ("Stars") + stars);
			//Debug.Log (PlayerPrefs.GetInt ("Stars").ToString());
		}

		#if UNITY_ANDROID || UNITY_IOS
		// Gem Collector
		//Debug.Log (Social.localUser.authenticated);
		if (Social.localUser.authenticated == true) {
			if (PlayerPrefs.GetInt ("Stars") >= 25) {
				Social.ReportProgress("CgkIxfGr_rsHEAIQAQ", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// Super Gem Collector
			if (PlayerPrefs.GetInt ("Stars") >= 75) {
				Social.ReportProgress("CgkIxfGr_rsHEAIQAw", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// Super Duper Gem Collector
			if (PlayerPrefs.GetInt ("Stars") >= 150) {
				Social.ReportProgress("CgkIxfGr_rsHEAIQBA", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// Ultra Gem Collector
			if (PlayerPrefs.GetInt ("Stars") >= 300) {
				Social.ReportProgress("CgkIxfGr_rsHEAIQBQ", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// Omega Gem Collector
			if (PlayerPrefs.GetInt ("Stars") >= 600) {
				Social.ReportProgress("CgkIxfGr_rsHEAIQBg", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
		}
		#endif
	}

	void Quit() {
		Time.timeScale = 1;
		#if UNITY_ANDROID || UNITY_IOS
		CBManager.ShowInterstitial();
		#endif
		SceneManager.LoadScene("Cups");
	}

	void ShowLeaderboards() {
		#if UNITY_ANDROID || UNITY_IOS
		Social.ShowLeaderboardUI();
		#endif
	}
}
