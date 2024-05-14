using GridPlacement.PlaceState;
using Inputs;
using Item;
using System;
using UnityEngine;

namespace GridPlacement
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField]
        private PreviewSystem previewSystem;

        private GridData gridData;
        private IPlacementState placeState;

        private event Action OnPlaced;

        public void SetupGridData(GridData gridData)
        {
            this.gridData = gridData;
        }

        public void StartPlacement(ItemBase item)
        {
            StopPlacement();

            placeState = new PlacementState(gridData, item);

            //ADD Rotation Evenet
        }

        public void StopPlacement()
        {
            if (placeState == null)
                return;

            placeState.EndState();

            placeState = null;
        }

        public void OnPlaceItem(Vector2Int gridPosition)
        {
            if (placeState == null)
                return;

            if (!placeState.OnAction(gridPosition))
            {
                Debug.Log("Wrong place!!!");
                return;
            }

            OnPlaced?.Invoke();
        }

        public void AddPlacementEvent(Action onPlaced, Action onExit)
        {
            OnPlaced += onPlaced;
        }

        public void RemovePlacementEvent(Action onPlaced, Action onExit)
        {
            OnPlaced -= onPlaced;
        }
    }
}