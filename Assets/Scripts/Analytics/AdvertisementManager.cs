#define HC_ADS


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvertisementManager : SingletonMono<AdvertisementManager>
{


    [SerializeField] float interstitialCooldown = 30.0f;

    const string INTERSTITIAL_UNIT = "ebd4fbbdef2bad80";
    const string REWARDED_UNIT = "4e339487e26a9c31";
    int retryAttempt;
    DateTime lastAdShown;

    System.Action<bool> onRewardedClosed;
    string cachedPlacement;

    enum AdResult
    {
        Watched,
        Clicked,
        Canceled
    }

    AdResult adResult;

    public bool RewardedAvailable => MaxSdk.IsRewardedAdReady(REWARDED_UNIT);


    // Start is called before the first frame update
    void Start()
    {
        #if HC_ADS
        DontDestroyOnLoad(this);
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            InitializeInterstitialAds();
            InitializeRewardedAds();
        };

        MaxSdk.SetSdkKey("6AQkyPv9b4u7yTtMH9PT40gXg00uJOTsmBOf7hDxa_-FnNZvt_qTLnJAiKeb5-2_T8GsI_dGQKKKrtwZTlCzAR");
        MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
        #endif
    }

    public void TryShowInterstitial(string placement)
    {
#if HC_ADS
        cachedPlacement = placement;
        if (lastAdShown + TimeSpan.FromSeconds(interstitialCooldown) <= DateTime.Now)
        {
            if (MaxSdk.IsInterstitialReady(INTERSTITIAL_UNIT))
            {
                adResult = AdResult.Watched;
                var result = "success";
                ReportAnalytics("video_ads_available", "interstitial", placement, result);
                ReportAnalytics("video_ads_started", "interstitial", cachedPlacement, "start");
                MaxSdk.ShowInterstitial(INTERSTITIAL_UNIT);
            }
            else
            {
                var result = "not_available";
                ReportAnalytics("video_ads_available", "interstitial", placement, result);
            }
        }
        #endif
    }


    public void ShowRewardedVideo(System.Action<bool> callback, string placement)
    {

#if HC_ADS  && !UNITY_EDITOR
        adResult = AdResult.Canceled;
        cachedPlacement = placement;
        onRewardedClosed = callback;
        string result;
        if (MaxSdk.IsRewardedAdReady(REWARDED_UNIT))
        {
            rewardReceived = false;
            result = "success";
            ReportAnalytics("video_ads_available", "rewarded", placement, result);
            ReportAnalytics("video_ads_started", "rewarded", cachedPlacement, "start");
            MaxSdk.ShowRewardedAd(REWARDED_UNIT);
        }
        else
        {
            result = "not_available";
            ReportAnalytics("video_ads_available", "rewarded", placement, result);
        }
        return;
#endif
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


    public void InitializeRewardedAds()
    {
        #if HC_ADS
        // Attach callback
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first rewarded ad
        LoadRewardedAd();
        #endif
    }


    private void LoadInterstitial()
    {
        #if HC_ADS
        MaxSdk.LoadInterstitial(INTERSTITIAL_UNIT);
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

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) 
    {
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        adResult = AdResult.Clicked;
    }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        string result;
        switch (adResult)
        {
            case AdResult.Watched:
                result = "watched";
                break;
            case AdResult.Clicked:
                result = "clicked";
                break;
            case AdResult.Canceled:
                result = "canceled";
                break;
            default:
                result = "";
                break;
        }

        // Interstitial ad is hidden. Pre-load the next ad.
        ReportAnalytics("video_ads_watch", "interstitial", cachedPlacement, result);
        lastAdShown = DateTime.Now;
        LoadInterstitial();
    }


    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(REWARDED_UNIT);
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.
        // Reset retry attempt

        retryAttempt = 0;
    }


    void ReportAnalytics(string eventType, string adType, string placement, string result)
    {
        var p = new Dictionary<string, object>();
        p.Add("ad_type", adType);
        p.Add("placement", placement);
        p.Add("result", result);
        p.Add("connection", Application.internetReachability != NetworkReachability.NotReachable ? 1 : 0);
        AnalyticsManager.Instance.ReportEvent(eventType, p);
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load 
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        Debug.Log("Appmetrica : Clicked");
        adResult = AdResult.Clicked;
    }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        string result;
        switch (adResult)
        {
            case AdResult.Watched:
                result = "watched";
                break;
            case AdResult.Clicked:
                result = "clicked";
                break;
            case AdResult.Canceled:
                result = "canceled";
                break;
            default:
                result = "";
                break;
        }
        ReportAnalytics("video_ads_watch", "rewarded", cachedPlacement, result);
        onRewardedClosed?.Invoke(rewardReceived);
        lastAdShown = DateTime.Now;
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }
    bool rewardReceived;
    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        if (adResult != AdResult.Clicked)
        {
            adResult = AdResult.Watched;
        }
        // The rewarded ad displayed and the user should receive the reward.
        rewardReceived = true;
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }

#endif
}
