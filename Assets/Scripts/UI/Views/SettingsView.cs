using Audio.SoundsData;
using Haptics;
using Saves;
using Settings;
using UnityEngine;
using UnityEngine.UI;
using ViewSystem;
using ViewSystem.Implementation;
using Zenject;

namespace Game.View
{
    public class SettingsView : BasicView
    {
        [SerializeField]
        private UIToogle musicButton;
        [SerializeField]
        private UIToogle SFXButton;
        [SerializeField]
        private UIToogle vibrationButton;
        [SerializeField]
        private UIButton backButton;
        [SerializeField]
        private UIButton creditsButton;

        [SerializeField]
        private CreditsView creditsView;

        private SettingsManager settingsManager;

        public override bool Absolute => false;

        [Inject]
        private void Inject(SettingsManager settingsManager)
        {
            this.settingsManager = settingsManager;
        }

        protected override void Awake()
        {
            base.Awake();

            SetupButtons();
        }

        private void SetupButtons()
        {
            musicButton.SetupButtonEvent(OnMusicPerformed);
            SFXButton.SetupButtonEvent(OnSFXPerformed);
            vibrationButton.SetupButtonEvent(OnVibrationPerformed);
            backButton.SetupButtonEvent(OnBackPerformed);
            creditsButton.SetupButtonEvent(OnCreditsPerformed);
        }

        private void RefreshButtonsState()
        {
            musicButton.LoadButtonState(settingsManager.GetSettingsValue<bool>(SaveKeyUtilities.MusicSettingsKey));
            SFXButton.LoadButtonState(settingsManager.GetSettingsValue<bool>(SaveKeyUtilities.SFXSettingsKey));
            vibrationButton.LoadButtonState(settingsManager.GetSettingsValue<bool>(SaveKeyUtilities.HapticsSettingsKey));
        }

        private void OnBackPerformed()
        {
            ParentStack.TryPopSafe();
        }

        private void OnCreditsPerformed()
        {
            ParentStack.TryPushSafe(creditsView);
        }

        private void OnMusicPerformed()
        {
            settingsManager.SetMusicValue();
        }

        private void OnSFXPerformed()
        {
            settingsManager.SetSfxValue();
        }

        private void OnVibrationPerformed()
        {
            settingsManager.SetVibrationValue();
        }

        public override void NavigateTo(IAmViewStackItem previousViewStackItem)
        {
            base.NavigateTo(previousViewStackItem);

            RefreshButtonsState();
        }
    }
}