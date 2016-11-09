using UnityEngine;
using System.Collections;

public static class SocialMediaManager {

	private const string TWITTER_ADDRESS = "http://twitter.com/intent/tweet";
	private const string TWEET_LANGUAGE = "en";

	private const string FACEBOOK_APP_ID = "865513516904725";
	private const string FACEBOOK_URL = "http://www.facebook.com/dialog/feed";


	static public void OpenTwitterPage () {
		Application.OpenURL ("https://twitter.com/flippyuncle");
	}

	static public void OpenFacebookPage() {
		Application.OpenURL ("https://www.facebook.com/flippyuncle");
	}

	static public void ShareToTwitter () {
		string tweetText = "I'm playing #FlippyUncle! Check it out: www.flippyuncle.com";
		Application.OpenURL (TWITTER_ADDRESS + "?text=" + WWW.EscapeURL (tweetText) + "&amp;lang=" + WWW.EscapeURL (TWEET_LANGUAGE) + "&amp;via=" + WWW.EscapeURL("FlippyUncle") + "&amp;hashtags=" + WWW.EscapeURL("gaming, mobilegaming"));
	}


	static public void ShareToFacebook () {
		string facebookshare = "https://www.facebook.com/sharer/sharer.php?u=" + WWW.EscapeURL("http://www.flippyuncle.com");
		Application.OpenURL(facebookshare);
	}

	static public void ShareToGooglePlus () {
		string googleShare = "https://plus.google.com/share?url=http://www.flippyuncle.com";
		Application.OpenURL(googleShare);
	}

	static public void ShareToReddit () {
		string redditShare = "http://www.reddit.com/submit/?url=http://www.flippyuncle.com";
		Application.OpenURL(redditShare);
	}

	static public void ShareToLinkedIn () {
		string linkedInShare = "https://www.linkedin.com/shareArticle?mini-true" + "&url=http://www.flippyuncle.com";
		Application.OpenURL(linkedInShare);
	}
}