using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

namespace GoogleMobileAds.Sample
{
    [AddComponentMenu("GoogleMobileAds/Samples/RewardedAdController")]
    public class RewardedAdController : MonoBehaviour
    {
        public PowerUpController powerupcontroller;
        public Button rewardButton; // Reference to the button

        public bool IsAdLoaded
        {
            get { return _adLoaded; }
        }

#if UNITY_ANDROID
        private const string _adUnitId = "ca-app-pub-3036661189646093/2426673270";
#elif UNITY_IPHONE
        private const string _adUnitId = "ca-app-pub-3036661189646093/1199283711";
#else
        private const string _adUnitId = "unused";
#endif

        private RewardedAd _rewardedAd;
        private bool _adLoaded = false;

        private void Awake()
        {
            StartCoroutine(CheckInternetConnection());
            LoadAd();
        }

        public void LoadAd()
        {
            if (_rewardedAd != null)
            {
                DestroyAd();
            }

            Debug.Log("Loading rewarded ad.");

            var adRequest = new AdRequest();

            RewardedAd.Load(_adUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad with error : " + error);
                    return;
                }

                if (ad == null)
                {
                    Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : " + ad.GetResponseInfo());
                _rewardedAd = ad;
                _adLoaded = true;
                RegisterEventHandlers(ad);
            });
        }

        public void ShowAd()
        {
            if (!IAPManager.AdsRemoved)
            {
                if (_rewardedAd != null && _adLoaded)
                {
                    Debug.Log("Showing rewarded ad.");
                    _rewardedAd.Show((Reward reward) =>
                    {
                        Debug.Log(string.Format("Rewarded ad granted a reward: {0} {1}",
                                                reward.Amount,
                                                reward.Type));
                        powerupcontroller.EnableShield();
                        _adLoaded = false;  // Reset this flag only when ad is shown successfully
                        LoadAd();  // Load next ad immediately after showing the current one
                    });
                }
                else
                {
                    Debug.LogError("Rewarded ad is not ready yet.");
                    // You might consider popping up a message to the user here.
                }
            }
        }

        public void DestroyAd()
        {
            if (_rewardedAd != null)
            {
                Debug.Log("Destroying rewarded ad.");
                _rewardedAd.Destroy();
                _rewardedAd = null;
                _adLoaded = false;
            }
        }

        private void RegisterEventHandlers(RewardedAd ad)
        {
            ad.OnAdPaid += (AdValue adValue) =>
            {
                Debug.Log(string.Format("Rewarded ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
            };

            ad.OnAdImpressionRecorded += () =>
            {
                Debug.Log("Rewarded ad recorded an impression.");
            };

            ad.OnAdClicked += () =>
            {
                Debug.Log("Rewarded ad was clicked.");
            };

            ad.OnAdFullScreenContentOpened += () =>
            {
                Debug.Log("Rewarded ad full screen content opened.");
            };

            ad.OnAdFullScreenContentClosed += () =>
            {
                Debug.Log("Rewarded ad full screen content closed.");
                _adLoaded = false;
                LoadAd();  // Load a new ad when the old one is closed
            };

            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.LogError("Rewarded ad failed to open full screen content with error : " + error);
            };
        }

        IEnumerator CheckInternetConnection()
        {
            using (UnityWebRequest www = UnityWebRequest.Get("https://google.com"))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("Internet Connection Error: " + www.error);
                    rewardButton.interactable = false; // Disable the button
                }
                else
                {
                    Debug.Log("Internet Connection Successful");
                    rewardButton.interactable = true; // Enable the button
                }
            }
        }
    }
}


