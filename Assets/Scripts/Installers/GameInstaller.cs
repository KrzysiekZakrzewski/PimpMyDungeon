using Audio.Manager;
using Game.SceneLoader;
using GridPlacement;
using Levels;
using Levels.Data;
using Saves;
using Settings;
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
        }
    }
}