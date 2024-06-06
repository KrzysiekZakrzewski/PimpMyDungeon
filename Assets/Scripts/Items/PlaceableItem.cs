using DG.Tweening;
using GridPlacement;
using GridPlacement.Hold;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Item
{
    public class PlaceableItem : HoldObject, IPlaceItem, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private ItemData itemData;

        [SerializeField]
        private Transform gridDetectorPoint;

        [SerializeField]
        protected PlacementSystem placementSystem;

        private Vector2 size;
        private List<Vector2Int> itemPoints;
        private Sprite sprite;
        private int rotationState;
        private bool isRotating;

        public int RotationState => rotationState;
        public Vector2 Size => size;
        public List<Vector2Int> ItemPoints => itemPoints;
        public Sprite Sprite => sprite;
        public GameObject GameObject => gameObject;


        private void Awake()
        {
            sprite = GetComponent<SpriteRenderer>().sprite;

            if (itemData == null)
                return;

            Setup(itemData, placementSystem);
        }

        private void OnHoldEvent()
        {

        }

        private void OnHoldEndEvent()
        {

        }

        public void Setup(ItemData data, PlacementSystem placementSystem)
        {
            size = data.Size;
            itemPoints = data.ItemPoints;
            this.placementSystem = placementSystem;
        }

        public void OnExit()
        {

        }

        public void OnPlaced(Vector2Int gridPosition)
        {
            gameObject.transform.position = PositionCalculator.CalculatePosition(gridPosition, Size, rotationState);
        }

        public void OnDrag(PointerEventData eventData)
        {
            var gridValue = placementSystem.OnItemMove(gridDetectorPoint.position, rotationState);

            transform.position = gridValue.Item2 ? PositionCalculator.CalculatePosition(gridValue.Item1, Size, rotationState) : gridValue.Item1;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            placementSystem.StartPlacement(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            placementSystem.OnPlaceItem(gridDetectorPoint.position);
        }

        public void Rotate(RotateCallback rotateCallback)
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
                    rotateCallback.Invoke(gameObject.transform.localEulerAngles);
                }
            );
        }
    }
}
