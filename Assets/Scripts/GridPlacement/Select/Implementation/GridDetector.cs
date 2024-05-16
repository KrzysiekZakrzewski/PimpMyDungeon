using GridPlacement;
using UnityEngine;
using Zenject;

namespace MouseInteraction
{
    public class GridDetector //: MouseObject
    {
        [SerializeField]
        private Grid grid;

        private PlacementSystem placementSystem;

        [Inject]
        private void Inject(PlacementSystem placementSystem)
        {
            this.placementSystem = placementSystem;
        }
    }
}