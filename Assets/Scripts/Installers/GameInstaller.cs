using Game.SceneLoader;
using UnityEngine;
using Zenject;

namespace Game.Installer
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField]
        private SceneLoadManagers sceneLoadManagers;

        public override void InstallBindings()
        {
            Container.BindInstance(sceneLoadManagers).AsSingle();
        }
    }
}