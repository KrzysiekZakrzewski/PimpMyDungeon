using Ads.Data;
using Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using Zenject;

namespace Ads
{
    public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField]
        private string androidGameId;
        [SerializeField]
        private string iOSGameId;
        [SerializeField]
        private bool testMode = true;

        private string gameId;
        private string adUnitId;

        private event Action endAdsEvent;

        private bool isInitialized;

        private NetworkManager networkManager;

        private bool IsInitialized => isInitialized && InitializeFinished;

        public bool InitializeFinished { get; private set; }
        public bool CanShowAd => IsInitialized && networkManager.IsNetworkConnection;

        [Inject]
        private void Inject(NetworkManager networkManager)
        {
            this.networkManager = networkManager;
        }

        private void OnDestroy()
        {
            
        }

        private void OnConnected(NetworkManager networkManager)
        {
            if (IsInitialized)
                return;

            InitializeAds();
        }

        private void OnDisconnected(NetworkManager networkManager)
        {
            isInitialized = false;
            InitializeFinished = false;
        }

        private void OnBannerClicked() { }

        private void OnBannerShown() { }

        private void OnBannerHidden() { }

        private void OnBannerLoaded()
        {
            ShowBannerAd();
        }

        private void OnBannerError(string message)
        {
            Debug.Log($"Banner Error: {message}");
        }

        private bool LoadBanner()
        {
            if (!CanShowAd)
                return false;

            BannerLoadOptions options = new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            };

            Advertisement.Banner.Load(adUnitId, options);

            return true;
        }

        private void ShowBannerAd()
        {
            BannerOptions options = new BannerOptions
            {
                clickCallback = OnBannerClicked,
                hideCallback = OnBannerHidden,
                showCallback = OnBannerShown
            };

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
            isInitialized = false;
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

        }

        public void OnUnityAdsShowClick(string placementId)
        {
            throw new System.NotImplementedException();
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
        {
            if (!placementId.Equals(adUnitId) || showCompletionState.Equals(UnityAdsShowCompletionState.UNKNOWN))
                return;

            endAdsEvent?.Invoke();
        }

        public bool ShowAd(AddDataBase adData, Action endAdsEvent)
        {
            if (!CanShowAd)
                return false;

#if UNITY_ANDROID
            adUnitId = adData.AddAndroidId;
#elif UNITY_IOS
            adUnitId = addData.AddIOSId;
#elif UNITY_EDITOR
            adUnitId = addData.AddAndroidId;
#endif

            this.endAdsEvent += endAdsEvent;
            this.endAdsEvent += () => UnsubscribeEvent(endAdsEvent);

            if (adData is BannerAdsData)
                return LoadBanner();

            Advertisement.Load(adUnitId, this);

            return true;
        }

        public void InitializeAds()
        {
#if UNITY_ANDROID
            gameId = androidGameId;
#elif UNITY_IOS
            gameId = iOSGameId;
#elif UNITY_EDITOR
            gameId = androidGameId;
#endif

            networkManager.OnConected += OnConnected;
            networkManager.OnDisconnected += OnDisconnected;

            if (Advertisement.isInitialized || !Advertisement.isSupported || !networkManager.IsNetworkConnection)
            {
                InitializeFinished = true;
                return;
            }

            Advertisement.Initialize(gameId, testMode, this);
        }

        public void UnsubscribeEvent(Action endAdsEvent)
        {
            this.endAdsEvent -= endAdsEvent;
            this.endAdsEvent -= () => UnsubscribeEvent(endAdsEvent);
        }
    }
}