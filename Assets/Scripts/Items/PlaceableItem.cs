using DG.Tweening;
using GridPlacement;
using MouseInteraction.Select;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Item
{
    public class PlaceableItem : SelectObject, IPlaceItem, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private ItemData itemData;
        [SerializeField]
        private SpriteRenderer previewRenderer;

        [field: SerializeField]
        public Transform GridDetectorPoint { private set; get; }

        [SerializeField]
        protected PlacementSystem placementSystem;

        private Vector2 basePosition;
        private Vector2 size;
        private List<Vector2Int> itemPoints;
        private int rotationState;
        private bool isRotating;
        private bool isMoving;
        private bool onGrid;

        public int RotationState => rotationState;
        public Vector2 Size => size;
        public List<Vector2Int> ItemPoints => itemPoints;
        public GameObject GameObject => gameObject;
        public bool OnGrid => onGrid;

        public SpriteRenderer PreviewRenderer => previewRenderer;

        private void Awake()
        {
            if (itemData == null)
                return;

            Setup(itemData, placementSystem, gameObject.transform.position);
        }

        private void OnCorrectPlace()
        {
            onGrid = true;
        }

        private void OnWrongPlace()
        {
            isMoving = true;

            Camera.main.DOShakePosition(1f, 0.1f);
            gameObject.transform.DOShakePosition(1f, 0.1f).OnComplete(
                () => gameObject.transform.DOMove(basePosition, 1f).OnComplete(() => isMoving = false));
        }

        public void OnGridChanged(bool onGrid) => this.onGrid = onGrid;

        public void Setup(ItemData data, PlacementSystem placementSystem, Vector2 basePosition)
        {
            previewRenderer.color = new Color(1f, 1f, 1f, 0f);

            this.basePosition = basePosition;
            size = data.Size;
            itemPoints = data.ItemPoints;
            this.placementSystem = placementSystem;
        }

        public void OnPlaced(Vector2Int gridPosition)
        {
            gameObject.transform.position = PositionCalculator.CalculatePosition(gridPosition, Size, rotationState);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var gridValue = placementSystem.OnItemMove(GridDetectorPoint.position, rotationState);

            transform.position = gridValue.Item2 ? PositionCalculator.CalculatePosition(gridValue.Item1, Size, rotationState) : gridValue.Item1;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(!onGrid)
                placementSystem.StartPlacement(this);
            else
                placementSystem.RemovePlacement(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            placementSystem.UnSubscribeRotateEvent(Rotate);
            var isPlaced = placementSystem.OnPlaceItem(GridDetectorPoint.position, this);

            if (isPlaced)
                OnCorrectPlace();
            else
                OnWrongPlace();
        }

        public void Rotate(InputAction.CallbackContext callbackContext)
        {
            if (isRotating)
                return;

            isRotating = true;

            rotationState++;

            rotationState = rotationState >= 4 ? 0 : rotationState;

            float angle = rotationState * -90f;

            gameObject.transform.DORotate(new Vector3(0, 0, angle), 0.2f).OnComplete
            (
                () =>
                {
                    isRotating = false;
                }
            );
        }
    }
}
