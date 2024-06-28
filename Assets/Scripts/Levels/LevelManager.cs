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
using Version;
using System.Collections;
using Animation.DoTween;

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

        public void LevelCompleted(bool isGridFilled)
        {
            if (!isGridFilled)
                return;

            saveValidator.UnlockLevel(currentLevelId);

            saveValidator.UpdateCompletedLevelData(currentLevelId, false);

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

        public bool CheckStartReached(int levelId)
        {
            return saveValidator.CheckStartReached(levelId);
        }
    }
}