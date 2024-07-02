using DG.Tweening;
using Game.SceneLoader;
using Inputs;
using Loading.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using ViewSystem;
using ViewSystem.Implementation;
using Zenject;

namespace Game.View
{
    public class LoadingView : BasicView
    {
        [SerializeField]
        private Camera loadingViewCamera;
        [SerializeField]
        private TextMeshProUGUI hintText;
        [SerializeField]
        private Image backgroundImage;
        [SerializeField]
        private TextMeshProUGUI continueText;
        [SerializeField]
        private float changeTextDuration;

        private Sequence changeTextSequnce;

        private Color transparentColor = new(1f,1f,1f,0f);

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

            CreateSequence();
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

        private void Continue_OnPerformed(InputAction.CallbackContext callback)
        {
            sceneLoadManagers.OpenScenes();
        }

        private void CreateSequence()
        {
            changeTextSequnce = DOTween.Sequence();

            changeTextSequnce.SetAutoKill(false);

            changeTextSequnce.Append(hintText.DOFade(0f, changeTextDuration));

            changeTextSequnce.Append(continueText.DOFade(1f, changeTextDuration));

            changeTextSequnce.Rewind();
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

        public void ShowContinueText()
        {
            hintText.DOKill();

            changeTextSequnce.Play();
        }

        public void SetupLoadingScreen(string hint, Sprite background)
        {
            continueText.color = transparentColor;
            hintText.color = transparentColor;
            hintText.text = hint;
            backgroundImage.sprite = background;
            hintText.DOFade(1f, 1f);

            changeTextSequnce.Rewind();
        }
    }
}