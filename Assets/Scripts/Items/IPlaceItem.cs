using GridPlacement;
using System.Collections.Generic;
using UnityEngine;

namespace Item
{
    public interface IPlaceItem : IGameObject
    {
        int RotationState { get; }
        Vector2 Size { get; }
        List<Vector2Int> ItemPoints { get; }
        SpriteRenderer PreviewRenderer { get; }

        void Setup(ItemData data, PlacementSystem placementSystem);
        void OnPlaced(Vector2Int gridPosition);
        void OnExit();
        void Rotate();
    }
}
