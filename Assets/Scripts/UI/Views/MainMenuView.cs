using ViewSystem;
using ViewSystem.Implementation;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class MainMenuView : BasicView
    {
        [SerializeField]
        private UIButton playButton;
        [SerializeField]
        private UIButton settingsButton;
        [SerializeField]
        private UIButton guideButton;
        [SerializeField]
        private LevelSelectorView levelSelectorView;
        [SerializeField]
        private SettingsView settingsView;
        [SerializeField]
        private DiaryView guideView;

        public override bool Absolute => false;

        protected override void Awake()
        {
            base.Awake();

            SetupButtons();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void SetupButtons()
        {
            playButton.SetupButtonEvent(PlayButton_OnPerformed);
            settingsButton.SetupButtonEvent(SettingsButton_OnPerformed);
            guideButton.SetupButtonEvent(GuideButton_OnPerformed);
        }

        private void SettingsButton_OnPerformed()
        {
            ParentStack.TryPushSafe(settingsView);
        }

        private void PlayButton_OnPerformed()
        {
            ParentStack.TryPushSafe(levelSelectorView);
        }

        private void GuideButton_OnPerformed()
        {
            ParentStack.TryPushSafe(guideView);
        }

        protected override void Presentation_OnShowPresentationComplete(IAmViewPresentation presentation)
        {
            base.Presentation_OnShowPresentationComplete(presentation);
        }

        public override void NavigateTo(IAmViewStackItem previousViewStackItem)
        {
            base.NavigateTo(previousViewStackItem);
        }

        public override void NavigateFrom(IAmViewStackItem nextViewStackItem)
        {
            base.NavigateFrom(nextViewStackItem);
        }
    }
}