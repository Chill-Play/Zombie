#define HC_ADS


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvertisementManager : SingletonMono<AdvertisementManager>
{


    [SerializeField] float interstitialCooldown = 30.0f;

    const string adUnitId = "YOUR_AD_UNIT_ID";
    int retryAttempt;
    DateTime lastInterstitialShown;

    // Start is called before the first frame update
    void Start()
    {
        #if HC_ADS
        DontDestroyOnLoad(this);
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            // AppLovin SDK is initialized, start loading ads
        };

        MaxSdk.SetSdkKey("6AQkyPv9b4u7yTtMH9PT40gXg00uJOTsmBOf7hDxa_-FnNZvt_qTLnJAiKeb5-2_T8GsI_dGQKKKrtwZTlCzAR");
        MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
        InitializeInterstitialAds();
        #endif
    }

    public void TryShowInterstitial()
    {
        #if HC_ADS
        if (lastInterstitialShown + TimeSpan.FromSeconds(interstitialCooldown) <= DateTime.Now)
        {
            lastInterstitialShown = DateTime.Now;
            if (MaxSdk.IsInterstitialReady(adUnitId))
            {
                MaxSdk.ShowInterstitial(adUnitId);
            }
        }
        #endif
    }


    public void ShowRewardedVideo(System.Action<bool> callback)
    {
        callback?.Invoke(true);
    }


    public void InitializeInterstitialAds()
    {
        #if HC_ADS
        // Attach callback
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
        MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;

        // Load the first interstitial
        LoadInterstitial();
        #endif
    }

    private void LoadInterstitial()
    {
        #if HC_ADS
        MaxSdk.LoadInterstitial(adUnitId);
        #endif
    }


    #if HC_ADS
    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'

        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        LoadInterstitial();
    }
    #endif
}
