using GridPlacement;
using MouseInteraction.Manager;
using UnityEngine;

namespace MouseInteraction.Implementation
{
    public class GridDetector : MouseDownBase
    {
        [SerializeField]
        private Grid grid;

        [SerializeField]
        private MouseManager mouseManager;

        [SerializeField]
        private PlacementSystem placementSystem;

        private PreviewSystem previewSystem;

        private void Awake()
        {
            previewSystem = placementSystem.PreviewSystem;
        }

        protected override void MouseObject_MouseDown(IMouseDown callback)
        {
            base.MouseObject_MouseDown(callback);

            var hit = mouseManager.GetRayCastHit();

            if (hit.collider == null)
                return;

            var gridPosition = grid.WorldToCell(hit.point);

            previewSystem.MovePreview(gridPosition);
        }

        protected override void MouseObject_MouseUp(IMouseDown callback)
        {
            base.MouseObject_MouseUp(callback);
        }
    }
}