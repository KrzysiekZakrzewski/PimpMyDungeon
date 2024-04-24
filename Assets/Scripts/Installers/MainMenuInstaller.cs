using Zenject;
using UnityEngine;

namespace Game.Installer
{
    public class MainMenuInstaller : MonoInstaller
    {
        [SerializeField]
        private MainMenuViewController mainMenuViewController;

        public override void InstallBindings()
        {
            Container.BindInstance(mainMenuViewController).AsSingle();
        }
    }
}