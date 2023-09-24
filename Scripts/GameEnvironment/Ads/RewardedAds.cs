using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using System;

namespace GameEnvironment.Ads
{
    public class RewardedAds : MonoBehaviour, IUnityAdsShowListener, IUnityAdsLoadListener
    {
        [SerializeField] private Button _showAddButton;

        public event Action AdShowCompleted;

        public const string AndroidAdID = "Rewarded_Android";
        public const string IOSAdID = "Rewarded_iOS";

        private string _adID;

        private void Awake()
        {
            _adID = (Application.platform == RuntimePlatform.IPhonePlayer) ? IOSAdID : AndroidAdID;

            //_showAddButton.interactable = false;
        }

        private void Start()
        {
            LoadAd();
        }

        public void ShowAd()
        {
            _showAddButton.interactable = false;

            Debug.Log("Ad is showing: " + _adID);
            Advertisement.Show(_adID, this);
        }

        private void LoadAd()
        {
            Debug.Log("Loading Ad: " + _adID);
            Advertisement.Load(_adID, this);
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            Debug.LogAssertion($"Unity Ad is failed to show: input - {_adID}; output - {placementId}; {error} - {message}");
        }

        public void OnUnityAdsShowStart(string placementId)
        {
            Debug.Log($"Unity Ad showing: Input - {_adID}; Output - {placementId}");
        }

        public void OnUnityAdsShowClick(string placementId)
        {
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (placementId.Equals(_adID) && showCompletionState.Equals(UnityAdsCompletionState.COMPLETED))
            {
                //REWARD HACK
                Vector3 pos = new Vector3();
                
                for (int i = 0; i < 10; i++)
                {
                    pos.x++;
                    pos.y++;
                }
            }
        }

        public void OnUnityAdsAdLoaded(string placementId)
        {
            Debug.Log("Ad loaded: " + placementId);

            if (placementId.Equals(_adID))
            {
                _showAddButton.onClick.AddListener(ShowAd);

                _showAddButton.interactable = true;
            }
        }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            Debug.LogAssertion($"Unity Ad is failed to load: input - {_adID}; output - {placementId}; {error} - {message}");
        }

        private void OnDestroy()
        {
            //_showAddButton.onClick.RemoveAllListeners();
        }
    }
}