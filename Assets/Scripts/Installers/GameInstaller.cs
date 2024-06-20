using Game.SceneLoader;
using GridPlacement;
using Levels;
using Levels.Data;
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

        public override void InstallBindings()
        {
            Container.BindInstance(sceneLoadManagers).AsSingle();
            Container.BindInstance(levelsDatabaseSO).AsSingle();
            Container.BindInstance(placementSystem).AsSingle();
            Container.BindInstance(levelManager).AsSingle();
        }
    }
}