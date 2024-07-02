using Saves;
using UnityEngine;

namespace Assets.Scripts.Save.SaveObjects
{
    [CreateAssetMenu(fileName = nameof(SettingsSaveObject), menuName = nameof(Saves) + "/" + "Assets/Objects" + "/" + nameof(SettingsSaveObject))]
    internal class SettingsSaveObject : SaveObject
    {
        public SaveValue<bool> MusicValue = new (SaveKeyUtilities.MusicSettingsKey);
        public SaveValue<bool> SfxValue = new (SaveKeyUtilities.SFXSettingsKey);
        public SaveValue<bool> VibrationValue = new (SaveKeyUtilities.VibrationSettingsKey);
    }
}
