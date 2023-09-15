using GoogleMobileAds.Api;
using UnityEngine;

public class InterstitialAdController : MonoBehaviour
{
    public PlayerController playerController;
    private InterstitialAd interstitialAd;

#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3036661189646093/1206597547";
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3036661189646093/9296748660";
#else
    private string _adUnitId = "unused";
#endif

    public void Start()
    {
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // Callback upon SDK initialized.
        });

        LoadInterstitialAd();
    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(string.Format("Interstitial ad paid {0} {1}.", adValue.Value, adValue.CurrencyCode));
        };

        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };

        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };

        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };

        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            LoadInterstitialAd(); // Prepare the next ad
        };

        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            LoadInterstitialAd(); // Prepare the next ad
        };
    }

    public void LoadInterstitialAd()
    {
        var adRequest = new AdRequest();

        InterstitialAd.Load(_adUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
        {
            if (error != null)
            {
                Debug.LogError("Interstitial ad failed to load with error: " + error);
                return;
            }

            interstitialAd = ad;
            Debug.Log("Interstitial ad loaded.");
            RegisterEventHandlers(interstitialAd);
        });
    }

    public void ShowAdIfReady()
    {
        if (!IAPManager.AdsRemoved)
        {
            // Always attempt to show the ad if it's ready.
            if (interstitialAd != null && interstitialAd.CanShowAd())
            {
                Debug.Log("Showing interstitial ad.");
                interstitialAd.Show();
            }
            else
            {
                Debug.LogError("Interstitial ad is not ready yet.");
            }
        }
        
    }
}



