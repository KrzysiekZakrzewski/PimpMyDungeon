using Audio.Manager;
using DG.Tweening;
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
        private SettingsToogle musicButton;
        [SerializeField]
        private SettingsToogle SFXButton;
        [SerializeField]
        private SettingsToogle vibrationButton;
        [SerializeField]
        private Button backButton;

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
            backButton.onClick.AddListener(OnClickBackPerformed);
        }

        private void RefreshButtonsState()
        {
            musicButton.LoadButtonState(settingsManager.GetSettingsValue<bool>(SaveKeyUtilities.MusicSettingsKey));
            SFXButton.LoadButtonState(settingsManager.GetSettingsValue<bool>(SaveKeyUtilities.SFXSettingsKey));
            vibrationButton.LoadButtonState(true);
        }

        private void OnClickBackPerformed()
        {
            ParentStack.TryPopSafe();
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