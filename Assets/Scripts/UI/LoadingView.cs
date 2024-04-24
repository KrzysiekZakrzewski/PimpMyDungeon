using Game.SceneLoader;
using UnityEngine;
using ViewSystem;
using ViewSystem.Implementation;
using Zenject;

namespace Game.View
{
    public class LoadingView : BasicView
    {
        [SerializeField]
        private Camera loadingViewCamera;

        private SceneLoadManagers sceneLoadManagers;

        public bool IsShowPresentationComplete { get; private set; }

        public override bool Absolute => false;

        [Inject]
        private void Inject(SceneLoadManagers sceneLoadManagers)
        {
            this.sceneLoadManagers = sceneLoadManagers;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void Presentation_OnShowPresentationComplete(IAmViewPresentation presentation)
        {
            base.Presentation_OnShowPresentationComplete(presentation);

            IsShowPresentationComplete = true;

            loadingViewCamera.gameObject.SetActive(true);
        }

        public override void NavigateTo(IAmViewStackItem previousViewStackItem)
        {
            base.NavigateTo(previousViewStackItem);

            IsShowPresentationComplete = false;
        }

        public override void NavigateFrom(IAmViewStackItem nextViewStackItem)
        {
            base.NavigateFrom(nextViewStackItem);

            loadingViewCamera.gameObject.SetActive(false);
        }

        private void Continue_OnPerformed()
        {
            sceneLoadManagers.OpenScenes();
        }

        public void ShowContinueText()
        {
        }
    }
}