using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomeText : MonoBehaviour {

    public TextMesh welcomeText;

	// Use this for initialization
	void Start () {
        #if UNITY_ANDROID || UNITY_IOS
        welcomeText.text = "Tap anywhere...";
        #endif

        #if UNITY_EDITOR || UNITY_WEBPLAYER || UNITY_STANDALONE || UNITY_WEBGL
        welcomeText.text = "Click anywhere...";
        #endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
