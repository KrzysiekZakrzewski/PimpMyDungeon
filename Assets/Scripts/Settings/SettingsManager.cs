using Saves;
using Assets.Scripts.Save.SaveObjects;
using UnityEngine;
using Audio.Manager;
using Zenject;

namespace Settings
{
    public class SettingsManager : MonoBehaviour
    {
        private SettingsSaveObject settingsSaveObject;
        private AudioManager audioManager;

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
            var isMuted = audioManager.SetSoundGroupMuted(Audio.SoundsData.AudioTypes.Music);

            SetSettingsValue(SaveKeyUtilities.MusicSettingsKey, isMuted);
        }

        public void SetSfxValue()
        {
            var isMuted = audioManager.SetSoundGroupMuted(Audio.SoundsData.AudioTypes.SFX);

            SetSettingsValue(SaveKeyUtilities.SFXSettingsKey, isMuted);
        }

        public void SetVibrationValue()
        {
            SetSettingsValue(SaveKeyUtilities.SFXSettingsKey, true);
        }

        public void SaveSettings()
        {

        }
    }
}
