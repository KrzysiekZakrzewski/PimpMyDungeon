using Ads;
using Audio.Manager;
using Game.SceneLoader;
using Generator;
using GridPlacement;
using Item.Guide;
using Levels;
using Levels.Data;
using Network;
using Saves;
using Settings;
using Tips;
using Tutorial;
using UnityEngine;
using Zenject;

namespace Game.Installer
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private SceneLoadManagers sceneLoadManagers;
        [SerializeField]
        private LevelsDatabaseSO levelsDatabaseSO;
        [SerializeField]
        private PlacementSystem placementSystem;
        [SerializeField]
        private LevelManager levelManager;
        [SerializeField]
        private AdsManager adsManager;
        [SerializeField]
        private SaveValidator saveValidator;
        [SerializeField]
        private SettingsManager settingsManager;
        [SerializeField]
        private AudioManager audioManager;
        [SerializeField]
        private TutorialManager tutorialManager;
        [SerializeField]
        private NetworkManager networkManager;
        [SerializeField]
        private LevelBuilder levelBuilder;
        [SerializeField]
        private TipManager tipManager;
        [SerializeField]
        private ItemGuideController itemGuide;

        public override void InstallBindings()
        {
            Container.BindInstance(sceneLoadManagers).AsSingle();
            Container.BindInstance(levelsDatabaseSO).AsSingle();
            Container.BindInstance(placementSystem).AsSingle();
            Container.BindInstance(levelManager).AsSingle();
            Container.BindInstance(adsManager).AsSingle();
            Container.BindInstance(saveValidator).AsSingle();
            Container.BindInstance(settingsManager).AsSingle();
            Container.BindInstance(audioManager).AsSingle();
            Container.BindInstance(tutorialManager).AsSingle();
            Container.BindInstance(networkManager).AsSingle();
            Container.BindInstance(levelBuilder).AsSingle();
            Container.BindInstance(tipManager).AsSingle();
            Container.BindInstance(itemGuide).AsSingle();
        }
    }
}