using Engagement.UI;
using Game.SceneLoader;
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
        private bool isInitialized;

        #region VideoPrivateVerbs
        private VideoPlayer video;
        private bool videoEnded;
        private float delayOffset = 0.1f;
        #endregion

        [Inject]
        private void Inject(SceneLoadManagers sceneLoadManagers)
        {
            this.sceneLoadManagers = sceneLoadManagers;
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

            yield return new WaitUntil(CheckEngagemntWasFinished);

            var engagement = bootUIController.GetEngagementView();

            engagement.ShowContinueText();

            isInitialized = true;
        }

        private bool CheckEngagemntWasFinished()
        {
            return videoEnded;
        }

        public void FinishEngagement()
        {
            if (!isInitialized) return;

            sceneLoadManagers.LoadLocation(sceneToLoadData);
        }
    }
}