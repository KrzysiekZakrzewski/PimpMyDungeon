using Engagement.UI;
using Game.SceneLoader;
using Saves;
using Settings;
using System.Collections;
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
        private bool isInitialized;

        #region VideoPrivateVerbs
        private VideoPlayer video;
        private bool videoEnded;
        private float delayOffset = 0.1f;
        #endregion

        [Inject]
        private void Inject(SceneLoadManagers sceneLoadManagers, AdsManager adsManager, SaveValidator saveValidator, SettingsManager settingsManager)
        {
            this.sceneLoadManagers = sceneLoadManagers;
            this.adsManager = adsManager;
            this.saveValidator = saveValidator;
            this.settingsManager = settingsManager;
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

            adsManager.InitializeAds();

            saveValidator.PrepeareSaveData();
            settingsManager.LoadSettings();

            yield return new WaitUntil(CheckEngagemntWasFinished);

            var engagement = bootUIController.GetEngagementView();

            engagement.ShowContinueText();

            isInitialized = true;
        }

        private bool CheckEngagemntWasFinished()
        {
            return videoEnded && adsManager.InitializeFinished;
        }

        public void FinishEngagement()
        {
            if (!isInitialized) return;

            sceneLoadManagers.LoadLocation(sceneToLoadData);
        }
    }
}