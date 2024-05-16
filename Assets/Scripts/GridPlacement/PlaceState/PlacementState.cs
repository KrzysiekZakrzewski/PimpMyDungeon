using Item;
using UnityEngine;
using DG.Tweening;

namespace GridPlacement.PlaceState
{
    public class PlacementState : IPlacementState
    {
        private GridData gridData;
        private ItemBase item;

        public PlacementState(GridData gridData, ItemBase item)
        {
            this.gridData = gridData;
            this.item = item;

            StartState();
        }

        private void StartState()
        {

        }

        private bool CheckPlacementValidity(Vector2Int gridPosition)
        {
            return gridData.CheckValidation(gridPosition, item.Size);
        }

        public void EndState()
        {

        }

        public bool OnAction(Vector2Int gridPosition)
        {
            bool placementValidity = CheckPlacementValidity(gridPosition);

            if (!placementValidity)
                return false;

            gridData.PlaceObject(gridPosition, item.Size);

            //item.MoveToPlacePosition(gridPosition);

            return true;
        }
    }
}
