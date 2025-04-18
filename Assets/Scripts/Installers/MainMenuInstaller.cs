using Zenject;
using UnityEngine;
using Levels;

namespace Game.Installer
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField]
        private MainMenuViewController mainMenuViewController;
        [SerializeField]
        private LevelSelectorView levelSelectorView;

        public override void InstallBindings()
        {
            Container.BindInstance(mainMenuViewController).AsSingle();
            Container.BindInstance(levelSelectorView).AsSingle();
        }
    }
}