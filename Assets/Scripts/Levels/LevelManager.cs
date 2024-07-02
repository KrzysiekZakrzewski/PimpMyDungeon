using Game.SceneLoader;
using Generator;
using Levels.Data;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Animation.DoTween;
using Saves;

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
        private SaveValidator saveValidator;

        public static event Action LevelCompletedEvent;

        public static bool IsPaused { private set; get; }

        [Inject]
        private void Inject(SceneLoadManagers sceneLoadManagers, LevelsDatabaseSO levelDatabase, SaveValidator saveValidator)
        {
            this.sceneLoadManagers = sceneLoadManagers;
            this.levelDatabase = levelDatabase;
            this.saveValidator = saveValidator;
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

        public void PauseGame()
        {
            IsPaused = true;

            Time.timeScale = 0.0f;
        }

        public void UnPauseGame()
        {
            IsPaused = false;

            Time.timeScale = 1.0f;
        }

        public void LevelCompleted(bool isGridFilled)
        {
            if (!isGridFilled)
                return;

            saveValidator.UnlockLevel(currentLevelId);

            saveValidator.UpdateCompletedLevelData(currentLevelId, false);

            saveValidator.SaveData();

            LevelCompletedEvent?.Invoke();
        }

        public void LoadLevel(int levelId, LevelLoadAnimation loadAnimation)
        {
            currentLevelId = levelId;

            sceneLoadManagers.OnSceneChanged += BuildLevelAfterLoadScene;

            loadAnimation.PlayAnimation(() => sceneLoadManagers.LoadLocation(gameScene));
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
            return saveValidator.CheckLevelUnlocked(levelId);
        }

        public bool CheckLevelCompleted(int levelId)
        {
            return saveValidator.CheckLevelCompleted(levelId);
        }

        public bool CheckStartReached(int levelId)
        {
            return saveValidator.CheckStartReached(levelId);
        }
    }
}