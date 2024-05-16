using GridPlacement;
using UnityEngine;
using Zenject;

namespace Game.Installer
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField]
        private PlacementSystem placementSystem;

        public override void InstallBindings()
        {
            Container.BindInstance(placementSystem).AsSingle();
        }
    }
}