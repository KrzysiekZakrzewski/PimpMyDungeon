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
    private string iOsAdUnitId;

    private string gameId;
    private string adUnitId;

    private void InitializeAds()
    {
#if UNITY_ANDROID
            gameId = androidGameId;
            adUnitId = androindAdUnitId;
#elif UNITY_IOS
            gameId = iOSGameId;
            adUnitId = iOsAdUnitId;
#elif UNITY_EDITOR
        gameId = androidGameId;
        adUnitId = androindAdUnitId;
#endif

        if (Advertisement.isInitialized)
            return;

        Advertisement.Initialize(gameId, testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads initialization failed: {error} - {message}");
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
}
