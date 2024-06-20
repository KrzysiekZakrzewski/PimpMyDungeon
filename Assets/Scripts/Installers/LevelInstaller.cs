using Zenject;
using UnityEngine;
using Game.View;

namespace Game.Installer
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField]
        private GameView gameView;

        public override void InstallBindings()
        {
            Container.BindInstance(gameView).AsSingle();
        }
    }
}