using UnityEngine;

namespace Saves.Object
{
    [CreateAssetMenu(fileName = nameof(LevelSaveObject), menuName = nameof(Saves) + "/" + "Assets/Objects" + "/" + nameof(LevelSaveObject))]
    public class LevelSaveObject : SaveObject
    {
        public SaveValue<SerializedDictionary<int, LevelSaveData>> Levels = new(SaveKeyUtilities.SavedLevelsKey);

        public SaveValue<int> LastUnlockedLevel = new(SaveKeyUtilities.LastUnlockedLevelKey);

        public void ClearSaves()
        {
            Levels.Value.Clear();
            LastUnlockedLevel.Value = 0;
        }
        public void UnlockAll()
        {
            for (int i = 0; i < Levels.Value.Count; i++)
            {
                Levels.Value[i].isUnlocked = true;
            }

            LastUnlockedLevel.Value = Levels.Value.Count - 1;
        }
    }
}