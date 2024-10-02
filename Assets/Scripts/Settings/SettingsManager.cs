using Saves;
using UnityEngine;
using Audio.Manager;
using Zenject;
using Haptics;
using Saves.Object;

namespace Settings
{
    public class SettingsManager : MonoBehaviour
    {
        private SettingsSaveObject settingsSaveObject;
        private AudioManager audioManager;

        public bool InitializeFinished { get; private set; }

        [Inject]
        private void Inject(AudioManager audioManager)
        {
            this.audioManager = audioManager;
        }

        public void LoadSettings() 
        {
            GetSaveObject();

            audioManager.SetSoundGroupMuted(Audio.SoundsData.AudioTypes.Music, GetSettingsValue<bool>(SaveKeyUtilities.MusicSettingsKey));
            audioManager.SetSoundGroupMuted(Audio.SoundsData.AudioTypes.SFX, GetSettingsValue<bool>(SaveKeyUtilities.SFXSettingsKey));
            HapticsManager.Load(GetSettingsValue<bool>(SaveKeyUtilities.HapticsSettingsKey));

            InitializeFinished = true;
        }

        private void GetSaveObject()
        {
            SaveManager.TryGetSaveObject(out settingsSaveObject);
        }

        private void SetSettingsValue(string key, object value)
        {
            settingsSaveObject.SetValue(key, value);
        }

        public T GetSettingsValue<T>(string key)
        {
            return settingsSaveObject.GetValue<T>(key).Value;
        }

        public void SetMusicValue()
        {
            var isOn = audioManager.SetSoundGroupMuted(Audio.SoundsData.AudioTypes.Music);

            SetSettingsValue(SaveKeyUtilities.MusicSettingsKey, isOn);
        }

        public void SetSfxValue()
        {
            var isOn = audioManager.SetSoundGroupMuted(Audio.SoundsData.AudioTypes.SFX);

            SetSettingsValue(SaveKeyUtilities.SFXSettingsKey, isOn);
        }

        public void SetVibrationValue()
        {
            var isOn = HapticsManager.OnOffHaptics();

            SetSettingsValue(SaveKeyUtilities.HapticsSettingsKey, isOn);
        }

        public void SaveSettings()
        {

        }
    }
}
