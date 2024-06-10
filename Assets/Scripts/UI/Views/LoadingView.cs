using Game.SceneLoader;
using Inputs;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
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

        [NonSerialized]
        private Inputs.PlayerInput playerInput;

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

            playerInput = InputManager.GetPlayer(0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected override void Presentation_OnShowPresentationComplete(IAmViewPresentation presentation)
        {
            base.Presentation_OnShowPresentationComplete(presentation);

            IsShowPresentationComplete = true;

            playerInput.AddInputEventDelegate(Continue_OnPerformed, InputActionEventType.ButtonUp, InputUtilities.AnyKey);

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

            playerInput.RemoveInputEventDelegate(Continue_OnPerformed);

            loadingViewCamera.gameObject.SetActive(false);
        }

        private void Continue_OnPerformed(InputAction.CallbackContext callback)
        {
            sceneLoadManagers.OpenScenes();
        }

        public void ShowContinueText()
        {
        }
    }
}