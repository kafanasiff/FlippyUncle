#if UNITY_ANDROID || UNITY_IOS
using UnityEngine;
using System.Collections;
using ChartboostSDK;


public static class CBManager {


	// REWARDED VIDEO
	static public void CacheRewardedVideo () {
		if (Chartboost.isInitialized() && !Chartboost.hasRewardedVideo (CBLocation.GameOver)) {
			Chartboost.cacheRewardedVideo (CBLocation.GameOver);
		}
	}

	static public void ShowRewardedVideo() {
		if (Chartboost.hasRewardedVideo (CBLocation.GameOver)) {
			Chartboost.showRewardedVideo (CBLocation.GameOver);
		} else {
			Chartboost.cacheRewardedVideo (CBLocation.GameOver);
		}
	}
		
	// INTERSTITIAL
	static public void CacheInterstitial () {
		if (Chartboost.isInitialized() && !Chartboost.hasInterstitial (CBLocation.Default)) {
			Chartboost.cacheInterstitial (CBLocation.Default);
		}
	}

	static public void ShowInterstitial() {
		if (Chartboost.hasInterstitial (CBLocation.Default)) {
			Chartboost.showInterstitial (CBLocation.Default);
		} else {
			Chartboost.cacheInterstitial (CBLocation.Default);
		}
	}


	// MORE APPS
	static public void CacheMoreApps () {
		if (Chartboost.isInitialized() && !Chartboost.hasMoreApps (CBLocation.Startup)) {
			Chartboost.cacheMoreApps (CBLocation.Startup);
		}
	}

	static public void ShowMoreApps() {
		if (Chartboost.hasMoreApps (CBLocation.Startup)) {
			Chartboost.showMoreApps (CBLocation.Startup);
		} else {
			Chartboost.cacheMoreApps (CBLocation.Startup);
		}
	}

}
#endif