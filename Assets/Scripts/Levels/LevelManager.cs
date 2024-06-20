using Game.SceneLoader;
using Generator;
using Levels.Data;
using Saves.Object;
using Saves;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Inputs;

namespace Levels
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField]
        private LevelBuilder levelBuilder;
        [SerializeField]
        private SceneDataSo gameScene;
        [SerializeField]
        private SceneDataSo mainMenuScene;

        private int currentLevelId;

        private LevelsDatabaseSO levelDatabase;
        private SceneLoadManagers sceneLoadManagers;

        private LevelSaveObject saveObject;

        public static event Action LevelCompletedEvent;

        [Inject]
        private void Inject(SceneLoadManagers sceneLoadManagers, LevelsDatabaseSO levelDatabase)
        {
            this.sceneLoadManagers = sceneLoadManagers;
            this.levelDatabase = levelDatabase;
        }

        private void Awake()
        {
            GetSaveObject();
        }

        private void GetSaveObject()
        {
            var result = SaveManager.TryGetSaveObject(out saveObject);

            if (!result)
                return;
        }

        private int GetLastUnlockedLevel()
        {
            return saveObject.GetValue<int>(SaveKeyUtilities.LastUnlockedLevelKey).Value;
        }

        private LevelDataSO GetLevelData(int levelId)
        {
            return levelDatabase.GetLevelData(levelId);
        }

        private void BuildLevelAfterLoadScene()
        {
            sceneLoadManagers.OnSceneChanged -= BuildLevelAfterLoadScene;

            levelBuilder.BuildLevel(GetLevelData(currentLevelId));
        }

        public void LevelCompleted(bool isGridFilled)
        {
            if (!isGridFilled)
                return;

            LevelCompletedEvent?.Invoke();
        }

        public void LoadLevel(int levelId)
        {
            currentLevelId = levelId;

            sceneLoadManagers.OnSceneChanged += BuildLevelAfterLoadScene;

            sceneLoadManagers.LoadLocation(gameScene);
        }

        public int GetLevelsCount()
        {
            return levelDatabase.GetLevelsCount();
        }

        public void LoadNextLevel()
        {
            currentLevelId++;

            levelBuilder.BuildLevel(GetLevelData(currentLevelId));
        }

        public void BackToMainMenu()
        {
            sceneLoadManagers.LoadLocation(mainMenuScene);
        }

        public void RestartLevel()
        {
            levelBuilder.BuildLevel(GetLevelData(currentLevelId));
        }

        public void SetupLevelBuilder(Tilemap floorTilemap, Tilemap wallTilemap, Transform obstacleMagazine, Transform itemMagazine)
        {
            levelBuilder.SetupBuildReferences(floorTilemap, wallTilemap, obstacleMagazine, itemMagazine);
        }

        public bool CheckLevelUnlocked(int levelId)
        {
            if(levelId > GetLastUnlockedLevel())
                return false;

            return true;
        }

        public bool CheckStartReached(int levelId)
        {
            if(levelId > GetLastUnlockedLevel())
                return false;

            var levels = saveObject.GetValue<SerializedDictionary<int, LevelSaveData>>(SaveKeyUtilities.SavedLevelsKey).Value;

            levels.TryGetValue(levelId, out var level);

            return level.starReached;
        }
    }
}