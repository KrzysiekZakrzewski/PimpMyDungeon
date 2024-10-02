using Game.SceneLoader;
using Generator;
using Levels.Data;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Animation.DoTween;
using Saves;
using Tutorial;
using Audio.Manager;
using Audio.SoundsData;
using Haptics;
using Ads;
using Ads.Data;
using Random = UnityEngine.Random;
using Tips;
using System.Collections;

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
        [SerializeField]
        private SceneDataSo tutorialScene;
        [SerializeField]
        private SoundSO completeLevelSfx;
        [SerializeField]
        private AddDataBase[] interstitialAds;

        private int currentLevelId;

        private int toNextAdd;
        private int minAmountToNextAdd = 3;
        private int maxAmountToNextAdd = 5;

        private LevelsDatabaseSO levelDatabase;
        private SceneLoadManagers sceneLoadManagers;
        private SaveValidator saveValidator;
        private TutorialManager tutorialManager;
        private AudioManager audioManager;
        private AdsManager adsManager;
        private TipManager tipManager;

        public static event Action LevelCompletedEvent;

        public static bool IsPaused { private set; get; }

        [Inject]
        private void Inject(SceneLoadManagers sceneLoadManagers, LevelsDatabaseSO levelDatabase, 
            SaveValidator saveValidator, TutorialManager tutorialManager, AudioManager audioManager,
            AdsManager adsManager, TipManager tipManager)
        {
            this.sceneLoadManagers = sceneLoadManagers;
            this.levelDatabase = levelDatabase;
            this.saveValidator = saveValidator;
            this.tutorialManager = tutorialManager;
            this.audioManager = audioManager;
            this.adsManager = adsManager;
            this.tipManager = tipManager;

            toNextAdd = Random.Range(minAmountToNextAdd, maxAmountToNextAdd);
        }

        private AddDataBase GetRandomAdd()
        {
            return interstitialAds[Random.Range(0, interstitialAds.Length)];
        }

        private LevelDataSO GetLevelData(int levelId)
        {
            return levelDatabase.GetLevelData(levelId);
        }

        private void BuildLevelAfterLoadScene()
        {
            levelBuilder.BuildLevel(GetLevelData(currentLevelId));

            sceneLoadManagers.OnSceneChanged -= BuildLevelAfterLoadScene;
        }

        public bool ShowAddBeforeNextLevel(Action onAddCompleted)
        {
            toNextAdd--;

            if (toNextAdd > 0)
                return false;

            toNextAdd = Random.Range(minAmountToNextAdd, maxAmountToNextAdd);

            return adsManager.ShowAd(GetRandomAdd(), onAddCompleted);
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
            if (!isGridFilled || tutorialManager.IsPlaying)
                return;

            audioManager.Play(completeLevelSfx);
            HapticsManager.PlayHaptics(HapticsType.Standard);

            saveValidator.UnlockLevel(currentLevelId);

            saveValidator.UpdateCompletedLevelData(currentLevelId, false);

            saveValidator.SaveData();

            LevelCompletedEvent?.Invoke();

            tipManager.ClearTips();
        }

        public void LoadLevel(int levelId, LevelLoadAnimation loadAnimation = null)
        {
            currentLevelId = levelId;

            if(currentLevelId == 0 && !tutorialManager.IsFinished)
            {
                LoadTutorialLevel(loadAnimation);
                return;
            }

            sceneLoadManagers.OnSceneChanged += BuildLevelAfterLoadScene;

            if(loadAnimation == null)
            {
                sceneLoadManagers.LoadLocation(gameScene);
                return;
            }

            loadAnimation.PlayAnimation(() => sceneLoadManagers.LoadLocation(gameScene));
        }

        public void LoadTutorialLevel(LevelLoadAnimation loadAnimation)
        {
            sceneLoadManagers.OnSceneChanged += tutorialManager.SetupTutorial;

            loadAnimation.PlayAnimation(() => sceneLoadManagers.LoadLocation(tutorialScene));
        }

        public int GetLevelsCount()
        {
            return levelDatabase.GetLevelsCount();
        }

        public void LoadNextLevel()
        {
            currentLevelId++;

            if(currentLevelId >= GetLevelsCount())
            {
                BackToMainMenu();

                return;
            }

            levelBuilder.BuildLevel(GetLevelData(currentLevelId));
        }

        public void BackToMainMenu()
        {
            levelBuilder.ToMainMenuClear();
            tipManager.ClearTips();

            var adsShowed = adsManager.ShowAd(GetRandomAdd(), () => sceneLoadManagers.LoadLocation(mainMenuScene));

            if (adsShowed)
                return;

            sceneLoadManagers.LoadLocation(mainMenuScene);
        }

        public void RestartLevel()
        {
            levelBuilder.StopAllCoroutines();

            tipManager.ClearTips();

            levelBuilder.BuildLevel(GetLevelData(currentLevelId));
        }

        public void SetupLevelBuilder(Tilemap floorTilemap, Tilemap wallTilemap, Transform obstacleMagazine, Transform itemMagazine, Transform tipObjectsMagazine, SpriteRenderer backgroundRenderer)
        {
            levelBuilder.SetupBuildReferences(floorTilemap, wallTilemap, obstacleMagazine, itemMagazine, tipObjectsMagazine, backgroundRenderer);
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