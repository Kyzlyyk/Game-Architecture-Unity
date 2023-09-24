using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace GameEnvironment.Ads
{
    public class InterstitialAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        public const string androidAdID = "Intersitial_Android";
        public const string IOSAdID = "Intersitial_iOS";

        private string _adId;
      
        private void Awake()
        {
            _adId = (Application.platform == RuntimePlatform.IPhonePlayer) ? IOSAdID : androidAdID;

            //LoadAd();
        }

        private void LoadAd()
        {
            Debug.Log("Loading Ad: " + _adId);
            Advertisement.Load(_adId, this);
        }

        public void ShowAd()
        {
            Debug.Log("Showing ad: " + _adId);
            Advertisement.Show(_adId, this);
        }

        void IUnityAdsLoadListener.OnUnityAdsAdLoaded(string placementId)
        {
        }

        void IUnityAdsLoadListener.OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
        }

        void IUnityAdsShowListener.OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
        }

        void IUnityAdsShowListener.OnUnityAdsShowStart(string placementId)
        {
        }

        void IUnityAdsShowListener.OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            LoadAd();
        }
    }
}