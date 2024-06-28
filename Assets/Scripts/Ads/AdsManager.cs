using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField]
    private string androidGameId;
    [SerializeField]
    private string iOSGameId;

    [SerializeField]
    private bool testMode = true;

    [SerializeField]
    private string androindAdUnitId;
    [SerializeField]
    private string iOSAdUnitId;

    private string gameId;
    private string adUnitId;

    private bool isInitialized;

    public bool InitializeFinished { get; private set; }

    public void InitializeAds()
    {
#if UNITY_ANDROID
        gameId = androidGameId;
        adUnitId = androindAdUnitId;
#elif UNITY_IOS
        gameId = iOSGameId;
        adUnitId = iOSAdUnitId;
#elif UNITY_EDITOR
        gameId = androidGameId;
        adUnitId = androindAdUnitId;
#endif

        if (Advertisement.isInitialized || !Advertisement.isSupported)
        {
            InitializeFinished = true;
            return;
        }

        Advertisement.Initialize(gameId, testMode, this);
    }

    private bool IsInitialized()
    {
        return isInitialized && InitializeFinished;
    }

    private void OnBannerClicked() { }
    private void OnBannerShown() { }
    private void OnBannerHidden() { }

    private void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
    }

    private void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
    }

    private void ShowBannerAd()
    {
        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(adUnitId, options);
    }

    private void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");

        InitializeFinished = true;
        isInitialized = true;
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads initialization failed: {error} - {message}");
        InitializeFinished = true;
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Advertisement.Show(placementId, this);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if(placementId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED)) { }
    }

    public void ShowAd()
    {
        if (!IsInitialized())
            return;

        Advertisement.Load(adUnitId, this);
    }

    public void LoadBanner()
    {
        if (!IsInitialized())
            return;

        // Set up options to notify the SDK of load events:
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Load the Ad Unit with banner content:
        Advertisement.Banner.Load(adUnitId, options);
    }
}
