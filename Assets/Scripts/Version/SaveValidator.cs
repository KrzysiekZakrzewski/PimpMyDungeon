using Levels.Data;
using Saves;
using Saves.Object;
using Settings;
using UnityEngine;
using Zenject;

namespace Saves
{
    public class SaveValidator : MonoBehaviour
    {
        private LevelSaveObject levelSaveObject;
        private SerializedDictionary<int, LevelSaveData> saveLevelDatabase;

        private LevelsDatabaseSO levelDatabase;

        public bool InitializeFinished { get; private set; }

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

        private void UpdateLastLevelUnlocked()
        {
            var lastUnlockedLevel = 0;

            for (int i = 0; i < saveLevelDatabase.Count; i++)
            {
                if (!saveLevelDatabase[i].isUnlocked)
                    break;

                lastUnlockedLevel = i;
            }

            levelSaveObject.SetValue(SaveKeyUtilities.LastUnlockedLevelKey, lastUnlockedLevel);
        }

        private void SaveLevelData()
        {
            UpdateLastLevelUnlocked();

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

            InitializeFinished = true;
        }

        private void ValidateSaveData()
        {
            GetSaveObject();

            if (saveLevelDatabase == null || saveLevelDatabase.Count == 0)
            {
                CreateNewGameSave();
                InitializeFinished = true;
                return;
            }

            if (saveLevelDatabase.Count == levelDatabase.GetLevelsCount())
            {
                InitializeFinished = true;
                return;
            }

            UpdateSaveData();
        }

        public void PrepeareSaveData()
        {
            ValidateSaveData();
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

            UpdateLastLevelUnlocked();
        }

        public bool CheckLevelUnlocked(int levelId)
        {
            if (levelId > GetLastUnlockedLevel())
                return false;

            return true;
        }

        public bool CheckLevelCompleted(int levelId)
        {
            if (levelId > GetLastUnlockedLevel())
                return false;

            saveLevelDatabase.TryGetValue(levelId, out var level);

            return level.isCompleted;
        }

        public bool CheckStartReached(int levelId)
        {
            if (levelId > GetLastUnlockedLevel())
                return false;

            saveLevelDatabase.TryGetValue(levelId, out var level);

            return level.starReached;
        }

        public void SaveData()
        {
            SaveLevelData();

            SaveManager.Save();
        }

        public void OnApplicationQuit()
        {
            SaveData();
        }
    }
}
