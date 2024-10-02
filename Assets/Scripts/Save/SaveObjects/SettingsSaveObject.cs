using UnityEngine;

namespace Saves.Object
{
    [CreateAssetMenu(fileName = nameof(SettingsSaveObject), menuName = nameof(Saves) + "/" + "Assets/Objects" + "/" + nameof(SettingsSaveObject))]
    public class SettingsSaveObject : SaveObject
    {
        public SaveValue<bool> MusicValue = new (SaveKeyUtilities.MusicSettingsKey);
        public SaveValue<bool> SfxValue = new (SaveKeyUtilities.SFXSettingsKey);
        public SaveValue<bool> VibrationValue = new (SaveKeyUtilities.HapticsSettingsKey);
    }
}
