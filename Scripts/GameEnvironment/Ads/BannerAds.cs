using UnityEngine;
using UnityEngine.Advertisements;

namespace GameEnvironment.Ads
{
    public class BannerAds : MonoBehaviour
    {
        [SerializeField] private BannerPosition _BannerPosition;

        public const string AndroidAdID = "Banner_Android";
        public const string IOSAdID = "Banner_iOS";

        private string _adID;

        private void Awake()
        {
            _adID = (Application.platform == RuntimePlatform.IPhonePlayer) ? IOSAdID : AndroidAdID;
        }

        private void Start()
        {
            Advertisement.Banner.SetPosition(_BannerPosition);
        }

        public void LoadBanner()
        {
            BannerLoadOptions options = new BannerLoadOptions()
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError,
            };

            Advertisement.Banner.Load(_adID, options);
        }

        public void ShowBannerAd()
        {
            BannerOptions options = new BannerOptions()
            {
                clickCallback = OnBannerClicked,
                hideCallback = OnBannerHidden,
                showCallback = OnBannerShown
            };

            Advertisement.Banner.Show(_adID, options);
        }

        private void OnBannerClicked()
        {

        }

        private void OnBannerShown()
        {

        }

        private void OnBannerHidden() 
        {
            
        }

        private void OnBannerError(string message)
        {
            Debug.Log("Banner failed to load: " + message);
        }

        private void OnBannerLoaded()
        {
            Debug.Log("Banner is loaded: " + _adID);
            ShowBannerAd();
        }
    }
}