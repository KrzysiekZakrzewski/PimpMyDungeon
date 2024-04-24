using TMPro;
using UnityEngine;
using ViewSystem.Implementation;

namespace Engagement.UI
{
    public class EngagementView : BasicView
    {
        [SerializeField]
        private EngagementController engagementController;
        [SerializeField]
        private TextMeshProUGUI continueText;

        public override bool Absolute => false;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void Continue_OnPerformed()
        {
            engagementController.FinishEngagement();
        }

        public void ShowContinueText()
        {

        }
    }
}