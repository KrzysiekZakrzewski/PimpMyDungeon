using GridPlacement;
using UnityEngine;

namespace Item
{
    public interface IPlaceItem
    {
        Vector2 Size { get; }

        void Setup(ItemData data, PlacementSystem placementSystem);

        void OnPlaced();

        void OnExit();
    }
}
