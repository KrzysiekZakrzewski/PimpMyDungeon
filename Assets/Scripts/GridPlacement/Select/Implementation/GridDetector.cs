using GridPlacement;
using MouseInteraction.Manager;
using UnityEngine;
using Zenject;

namespace MouseInteraction.Implementation
{
    public class GridDetector : MouseDownBase
    {
        [SerializeField]
        private Grid grid;

        private PlacementSystem placementSystem;
        private MouseManager mouseManager;

        [Inject]
        private void Inject(PlacementSystem placementSystem, MouseManager mouseManager)
        {
            this.placementSystem = placementSystem;
            this.mouseManager = mouseManager;
        }

        protected override void MouseObject_MouseDown(IMouseDown callback)
        {
            base.MouseObject_MouseDown(callback);

            var hit = mouseManager.GetRayCastHit();

            if (hit.collider == null)
                return;

            var gridPosition = grid.WorldToCell(hit.point);

            Vector2Int gridPosition2D = new (gridPosition.x, gridPosition.y);

            //placementSystem.OnPlaceItem(gridPosition2D);
        }

        protected override void MouseObject_MouseUp(IMouseDown callback)
        {
            base.MouseObject_MouseUp(callback);
        }
    }
}