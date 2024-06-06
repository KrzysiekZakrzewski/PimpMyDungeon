using Item;
using UnityEngine;

namespace GridPlacement.PlaceState
{
    public interface IPlacementState
    {
        bool CheckPlacementValidity(Vector2Int gridPosition);
        void EndState();
        bool OnAction(Vector2Int gridPosition);
    }
}
