using UnityEngine;
using UnityEngine.Advertisements;

namespace GameEnvironment.Ads
{
    public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
    {
        public const string AndroidId = "";
        public const string IOSId = "";
        public bool TestMode { get; private set; } = true;

        private string _gameId;

        private void Awake()
        {
            //InitializeAds();
        }

        private void InitializeAds()
        {
            _gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? IOSId : AndroidId;

            Advertisement.Initialize(_gameId, TestMode, this);
        }

        public void OnInitializationComplete()
        {
            Debug.LogAssertion("Initialization is successfully completed!");
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message)
        {
            Debug.LogError($"Initialization is failed: {error} - {message}");
        }
    }
}