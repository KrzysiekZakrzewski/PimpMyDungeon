using UnityEngine;

namespace Saves.Object
{
    [CreateAssetMenu(fileName = nameof(LevelSaveObject), menuName = nameof(Saves) + "/" + "Assets/Objects" + "/" + nameof(LevelSaveObject))]
    public class LevelSaveObject : SaveObject
    {
        public SaveValue<SerializedDictionary<int, LevelSaveData>> Levels = new(SaveKeyUtilities.SavedLevelsKey);

        public SaveValue<int> LastUnlockedLevel = new(SaveKeyUtilities.LastUnlockedLevelKey);
    }
}