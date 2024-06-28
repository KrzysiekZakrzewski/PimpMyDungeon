using ViewSystem;
using ViewSystem.Implementation;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class MainMenuView : BasicView
    {
        [SerializeField]
        private Button playButton;
        [SerializeField]
        private Button settingsButton;
        [SerializeField]
        private Button creditsButton;
        [SerializeField]
        private LevelSelectorView levelSelectorView;

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
            playButton.onClick.AddListener(PlayButton_OnPerformed);
            settingsButton.onClick.AddListener(SettingsButton_OnPerformed);
            creditsButton.onClick.AddListener(CreditsButton_OnPerformed);
        }

        private void SettingsButton_OnPerformed()
        {

        }

        private void PlayButton_OnPerformed()
        {
            ParentStack.TryPushSafe(levelSelectorView);
        }

        private void CreditsButton_OnPerformed()
        {

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