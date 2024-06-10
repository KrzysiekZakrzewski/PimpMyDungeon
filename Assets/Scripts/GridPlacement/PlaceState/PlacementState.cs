using Item;
using UnityEngine;

namespace GridPlacement.PlaceState
{
    public class PlacementState : IPlacementState
    {
        private GridData gridData;
        private PlaceableItem item;

        public PlacementState(GridData gridData, PlaceableItem item)
        {
            this.gridData = gridData;
            this.item = item;

            StartState();
        }

        private void StartState()
        {

        }

        public bool CheckPlacementValidity(Vector2Int gridPosition)
        {
            return gridData.CheckValidation(gridPosition, item.ItemPoints, item.RotationState);
        }

        public void EndState()
        {

        }

        public bool OnAction(Vector2Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition);

            if (!placementValidity)
                return false;

            gridData.PlaceObject(gridPosition, item.ItemPoints, item.RotationState);

            item.OnPlaced(gridPosition);

            return true;
        }
    }
}
