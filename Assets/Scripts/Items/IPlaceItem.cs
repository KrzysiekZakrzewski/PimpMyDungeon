using Audio.Manager;
using GridPlacement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Item
{
    public interface IPlaceItem : IGameObject
    {
        int RotationState { get; }
        Vector2 Size { get; }
        List<Vector2Int> ItemPoints { get; }
        SpriteRenderer SpriteRenderer { get; }
        bool OnGrid { get; }

        void Setup(ItemData data, PlacementSystem placementSystem, AudioManager audioManager, Vector2 basePosition);
        void OnPlaced(Vector2Int gridPosition);
        void Rotate(InputAction.CallbackContext callbackContext);
        void Rotate();
    }
}
