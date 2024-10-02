using Ads;
using Audio.Manager;
using Engagement.UI;
using Game.SceneLoader;
using Haptics;
using Item.Guide;
using Network;
using Saves;
using Settings;
using System.Collections;
using Tutorial;
using UnityEngine;
using UnityEngine.Video;
using Zenject;

namespace Engagement
{
    public class EngagementController : MonoBehaviour
    {
        [SerializeField]
        private EngagementUIController bootUIController;
        [SerializeField]
        private SceneDataSo sceneToLoadData;

        [SerializeField]
        private float delayTime;

        private SceneLoadManagers sceneLoadManagers;
        private AdsManager adsManager;
        private SaveValidator saveValidator;
        private SettingsManager settingsManager;
        private TutorialManager tutorialManager;
        private AudioManager audioManager;
        private NetworkManager networkManager;
        private ItemGuideController itemGuide;
        private bool isInitialized;

        #region VideoPrivateVerbs
        private VideoPlayer video;
        private bool videoEnded;
        private float delayOffset = 0.1f;
        #endregion

        [Inject]
        private void Inject(SceneLoadManagers sceneLoadManagers, AdsManager adsManager, 
            SaveValidator saveValidator, SettingsManager settingsManager, 
            TutorialManager tutorialManager, AudioManager audioManager, 
            NetworkManager networkManager, ItemGuideController itemGuide)
        {
            this.sceneLoadManagers = sceneLoadManagers;
            this.adsManager = adsManager;
            this.saveValidator = saveValidator;
            this.settingsManager = settingsManager;
            this.tutorialManager = tutorialManager;
            this.audioManager = audioManager;
            this.networkManager = networkManager;
            this.itemGuide = itemGuide;
        }

        private void Awake()
        {
            isInitialized = false;

            video = GetComponent<VideoPlayer>();

            video.loopPointReached += EndVideoAction;
        }

        private void Start()
        {
            StartCoroutine(StartEngagement());
        }

        private void EndVideoAction(VideoPlayer vp)
        {
            videoEnded = true;
            bootUIController.TryOpenSafe<EngagementView>();
        }

        private IEnumerator PlayVideoWithDelay()
        {
            yield return new WaitForSeconds(delayTime + delayOffset);

            video.Play();
        }

        private IEnumerator StartEngagement()
        {
            yield return null;

            StartCoroutine(PlayVideoWithDelay());

            networkManager.InitializeNetwork();

            adsManager.InitializeAds();

            HapticsManager.Init();
            saveValidator.PrepeareSaveData();
            settingsManager.LoadSettings();
            tutorialManager.Initialize();
            itemGuide.Initialize();

            yield return new WaitUntil(CheckEngagemntWasFinished);

            audioManager.PlayRandomMusic();

            var engagement = bootUIController.GetEngagementView();

            engagement.ShowContinueText();

            isInitialized = true;
        }

        private bool CheckEngagemntWasFinished()
        {
            return videoEnded && adsManager.InitializeFinished 
                && saveValidator.InitializeFinished && tutorialManager.InitializeFinished 
                && settingsManager.InitializeFinished && itemGuide.InitializeFinished;
        }

        public void FinishEngagement()
        {
            if (!isInitialized) return;

            sceneLoadManagers.LoadLocation(sceneToLoadData);
        }
    }
}