using GridPlacement;
using MouseInteraction.Manager;
using UnityEngine;
using Zenject;

namespace Game.Installer
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField]
        private PlacementSystem placementSystem;
        [SerializeField]
        private MouseManager mouseManager;

        public override void InstallBindings()
        {
            Container.BindInstance(placementSystem).AsSingle();
            Container.BindInstance(mouseManager).AsSingle();
        }
    }
}