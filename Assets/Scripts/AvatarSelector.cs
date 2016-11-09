using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AvatarSelector : MonoBehaviour {

	private Ray ray;
	private RaycastHit hit;

	public GameObject cupSelector;
	public List<Sprite> avatarSprites;
	public List<SpriteRenderer> avatarRenderers;
	public SpriteRenderer avatarButtonRenderer;
	public GameObject buttonHighlight;
	public SpriteRenderer flippyPrefabRenderer;
	public TextMesh unlockText;
	public TextMesh starsText;
	public AudioSource tapSound;
	public Sprite lockSprite;
	public List<GameObject> lockInfos;

	// Use this for initialization
	void Start () {
		/*
		// Enable sprites for avatars that have been unlocked
		for (int i = 0; i < avatarRenderers.Count; i++) {
			if (PlayerPrefs.GetInt ("AvatarUnlocked") >= i) {
				avatarRenderers[i].sprite = avatarSprites[i];
			}
		}
		*/

		SetLocks ();

		buttonHighlight.transform.position = avatarRenderers [PlayerPrefs.GetInt ("Avatar")].transform.position;
		avatarButtonRenderer.sprite = avatarSprites[PlayerPrefs.GetInt ("Avatar")];
		//flippyPrefabRenderer.sprite = avatarSprites[PlayerPrefs.GetInt ("Avatar")];	
	}

	void SetLocks() {

		string prefName = "";
		for (int i = 1; i < avatarRenderers.Count; i++) {
			prefName = "Avatar" + i.ToString ();
			if (PlayerPrefs.GetInt (prefName) == 0) {
				avatarRenderers [i].sprite = lockSprite;
				lockInfos [i].SetActive (true);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
		#if UNITY_ANDROID || UNITY_IOS
		if(Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) {
			ray = Camera.main.ScreenPointToRay(Input.GetTouch (0).position);
			if (Physics.Raycast(ray, out hit)) {
				GetButtonInput(hit);
			}
		}
		#endif
		
		
		
		#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_WEBGL
		if (Input.GetMouseButtonUp(0)) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit)) {
				GetButtonInput(hit);
			}
		}
		
		#endif
		
		#if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_ANDROID || UNITY_WEBGL
		if (Input.GetKeyUp(KeyCode.Escape)) {
			cupSelector.SetActive(true);
			gameObject.SetActive(false);
		}
		#endif
	}

	void AvatarButton(int num) {
		string prefName = "Avatar" + num.ToString ();
		if (PlayerPrefs.GetInt(prefName) == 1) {
			SetAvatar (num);
		} 
		else {
			UnlockAvatar (num);
		}
	}

	void GetButtonInput(RaycastHit hit) {
		if (hit.transform.gameObject.name == "Button_Cup") {
			cupSelector.SetActive(true);
			gameObject.SetActive(false);
		}
		/*
		if (hit.transform.gameObject.name == "Button_Lock") {
			UnlockAvatar();
		}
		*/
		if (hit.transform.gameObject.name == "Scotty") {
			SetAvatar (0);
		}
		if (hit.transform.gameObject.name == "Anachron") {
			AvatarButton (1);
		}
		if (hit.transform.gameObject.name == "Kid") {;
			AvatarButton (2);
		}
		if (hit.transform.gameObject.name == "Cat") {;
			AvatarButton (3);
		}
		if (hit.transform.gameObject.name == "Duck") {;
			AvatarButton (4);
		}
		if (hit.transform.gameObject.name == "Elephant") {;
			AvatarButton (5);
		}
		if (hit.transform.gameObject.name == "Frog") {;
			AvatarButton (6);
		}
		if (hit.transform.gameObject.name == "Hippo") {;
			AvatarButton (7);
		}
		if (hit.transform.gameObject.name == "Penguin") {;
			AvatarButton (8);
		}
		if (hit.transform.gameObject.name == "Owl") {;
			AvatarButton (9);
		}
		if (hit.transform.gameObject.name == "Whale") {;
			AvatarButton (10);
		}
		if (hit.transform.gameObject.name == "Orange") {;
			AvatarButton (11);
		}
		if (hit.transform.gameObject.name == "Slime") {;
			AvatarButton (12);
		}
		if (hit.transform.gameObject.name == "Pewdiepie") {;
			AvatarButton (13);
		}
		if (hit.transform.gameObject.name == "Brofist") {;
			AvatarButton (14);
		}
		if (hit.transform.gameObject.name == "JackSepticEye") {;
			AvatarButton (15);
		}
		if (hit.transform.gameObject.name == "SepticEye") {;
			AvatarButton (16);
		}
		if (hit.transform.gameObject.name == "Unicorn") {;
			AvatarButton (17);
		}
	}


	void UnlockAvatar(int num) {
		int cost = 50;
		int starsOnHand = PlayerPrefs.GetInt ("Stars");
		if (starsOnHand >= cost) {
			if (PlayerPrefs.GetInt("Sound") == 0) { tapSound.Play (); }
			int starsToSave = starsOnHand - cost;
			PlayerPrefs.SetInt ("Stars", starsToSave);
			starsText.text = starsToSave.ToString();
			avatarRenderers [num].sprite = avatarSprites [num];
			lockInfos [num].SetActive (false);
			SetAvatar (num);

			// Set player pref
			string prefName = "Avatar" + num.ToString();
			PlayerPrefs.SetInt (prefName, 1);

		}
	}

	/*
	void UnlockAvatar() {
		int unlocked = PlayerPrefs.GetInt ("AvatarUnlocked");
		if (unlocked < avatarRenderers.Count - 1) {
			int cost = 50;
			//Debug.Log (cost.ToString());
			int starsOnHand = PlayerPrefs.GetInt ("Stars");
			if (starsOnHand >= cost) {
				if (PlayerPrefs.GetInt("Sound") == 0) { tapSound.Play (); }
				unlocked++;
				PlayerPrefs.SetInt ("AvatarUnlocked", unlocked);
				avatarRenderers [unlocked].sprite = avatarSprites [unlocked];
				int starsToSave = starsOnHand - cost;
				PlayerPrefs.SetInt ("Stars", starsToSave);
				starsText.text = starsToSave.ToString();
			}
		}
	}
*/

	void SetAvatar(int arrayPos) {
		buttonHighlight.transform.position = avatarRenderers[arrayPos].gameObject.transform.position;
		avatarButtonRenderer.sprite = avatarSprites [arrayPos];
		flippyPrefabRenderer.sprite = avatarSprites [arrayPos];
		PlayerPrefs.SetInt ("Avatar", arrayPos);
	}
}
