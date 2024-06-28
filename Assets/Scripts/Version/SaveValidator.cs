using Levels.Data;
using Saves;
using Saves.Object;
using TMPro;
using UnityEngine;
using Zenject;

namespace Version
{
    public class SaveValidator : MonoBehaviour
    {
        private LevelSaveObject levelSaveObject;
        private SerializedDictionary<int, LevelSaveData> saveLevelDatabase;

        private LevelsDatabaseSO levelDatabase;

        [Inject]
        private void Inject(LevelsDatabaseSO levelDatabase)
        {
            this.levelDatabase = levelDatabase;
        }

        private void GetSaveObject()
        {
            var result = SaveManager.TryGetSaveObject(out levelSaveObject);

            if (!result)
                return;

            GetSaveLevelDataBase();
        }

        private void GetSaveLevelDataBase()
        {
            saveLevelDatabase = levelSaveObject.GetValue<SerializedDictionary<int, LevelSaveData>>(SaveKeyUtilities.SavedLevelsKey).Value;
        }

        private void CreateNewGameSave()
        {
            saveLevelDatabase = new();

            for (int i = 0; i < levelDatabase.GetLevelsCount(); i++)
            {
                saveLevelDatabase.Add(i, new LevelSaveData());
            }

            saveLevelDatabase[0].UnlockLevel();

            SaveLevelData();
        }

        private void SaveLevelData()
        {
            var lastUnlockedLevel = 0;

            for (int i = 0; i < saveLevelDatabase.Count; i++)
            {
                if (!saveLevelDatabase[i].isUnlocked)
                    break;

                lastUnlockedLevel = i;
            }

            levelSaveObject.SetValue(SaveKeyUtilities.LastUnlockedLevelKey, lastUnlockedLevel);

            levelSaveObject.SetValue(SaveKeyUtilities.SavedLevelsKey, saveLevelDatabase);
        }

        private int GetLastUnlockedLevel()
        {
            return levelSaveObject.GetValue<int>(SaveKeyUtilities.LastUnlockedLevelKey).Value;
        }

        private void UpdateSaveData()
        {
            for (int i = saveLevelDatabase.Count; i < levelDatabase.GetLevelsCount(); i++)
            {
                saveLevelDatabase.Add(i, new LevelSaveData());
            }

            saveLevelDatabase[0].UnlockLevel();

            SaveLevelData();
        }

        public void ValidateLevelSaveData()
        {
            GetSaveObject();

            if (saveLevelDatabase == null || saveLevelDatabase.Count == 0)
            {
                CreateNewGameSave();
                return;
            }

            if (saveLevelDatabase.Count == levelDatabase.GetLevelsCount())
                return;

            UpdateSaveData();
        }

        public void UpdateCompletedLevelData(int levelId, bool starReached)
        {
            var result = saveLevelDatabase.TryGetValue(levelId, out var levelData);

            if (!result)
                return;

            levelData.LevelCompleted(starReached);
        }

        public void UnlockLevel(int levelId)
        {
            var result = saveLevelDatabase.TryGetValue(levelId + 1, out var levelData);

            if (!result)
                return;

            levelData.UnlockLevel();
        }

        public bool CheckLevelUnlocked(int levelId)
        {
            if (levelId > GetLastUnlockedLevel())
                return false;

            return true;
        }

        public bool CheckStartReached(int levelId)
        {
            if (levelId > GetLastUnlockedLevel())
                return false;

            saveLevelDatabase.TryGetValue(levelId, out var level);

            return level.starReached;
        }
    }
}
