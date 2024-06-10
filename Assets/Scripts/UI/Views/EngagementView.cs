using Inputs;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using ViewSystem;
using ViewSystem.Implementation;

namespace Engagement.UI
{
    public class EngagementView : BasicView
    {
        [SerializeField]
        private EngagementController engagementController;
        [SerializeField]
        private TextMeshProUGUI continueText;

        private Inputs.PlayerInput playerInput;

        public override bool Absolute => false;

        protected override void Awake()
        {
            base.Awake();

            playerInput = InputManager.GetPlayer(0);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            playerInput.RemoveInputEventDelegate(Continue_OnPerformed);
        }

        protected override void Presentation_OnShowPresentationComplete(IAmViewPresentation presentation)
        {
            base.Presentation_OnShowPresentationComplete(presentation);

            playerInput.AddInputEventDelegate(Continue_OnPerformed, Inputs.InputActionEventType.ButtonUp, InputUtilities.AnyKey);
        }

        private void Continue_OnPerformed(InputAction.CallbackContext callback)
        {
            engagementController.FinishEngagement();
        }

        public void ShowContinueText()
        {

        }
    }
}