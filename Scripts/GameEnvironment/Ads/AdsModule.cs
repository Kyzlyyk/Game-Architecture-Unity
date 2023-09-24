using UnityEngine;

namespace GameEnvironment.Ads
{
    [CreateAssetMenu(menuName = "GAssembly/Ads Module")]
    public sealed class AdsModule : ScriptableObject
    {
        [SerializeField] RewardedAds _rewardedAds;
        [SerializeField] private InterstitialAds _interstitialAds;
        [SerializeField] private BannerAds _bannerAds;
        [SerializeField] private AdsInitializer _adsInitializer;
    }
}