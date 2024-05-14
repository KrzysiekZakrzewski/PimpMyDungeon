using Item;
using UnityEngine;

namespace GridPlacement.PlaceState
{
    public interface IPlacementState
    {
        void EndState();
        bool OnAction(Vector2Int gridPosition);
    }
}
