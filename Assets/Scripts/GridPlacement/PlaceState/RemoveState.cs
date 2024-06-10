using Item;
using UnityEngine;

namespace GridPlacement.PlaceState
{
    public class RemoveState : IPlacementState
    {
        private GridData gridData;
        private PlaceableItem item;

        public RemoveState(GridData gridData, PlaceableItem item)
        {
            this.gridData = gridData;
            this.item = item;

            StartState();
        }

        private void StartState()
        {

        }

        public void EndState()
        {

        }

        public bool OnAction(Vector2Int gridPosition)
        {
            if(gridData == null)
                return false;

            gridData.RemoveObject(gridPosition);

            return true;
        }

        public bool CheckPlacementValidity(Vector2Int gridPosition)
        {
            return true;
        }
    }
}
