using Audio.Manager;
using Audio.SoundsData;
using DG.Tweening;
using GridPlacement;
using Haptics;
using MouseInteraction.Select;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Item
{
    public class PlaceableItem : SelectObject, IPlaceItem, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private SoundSO putSfx, rotateSFX, correctPlaceSfx, wrongPlaceSfx;

        [SerializeField]
        private SpriteRenderer squareVisualize;

        private Color baseSquareVisualizeColor = new (0.45f, 0.45f, 0.45f, 1f);
        private Vector2 basePosition;
        private Vector2 size;
        private List<Vector2Int> itemPoints;
        private int rotationState;
        private bool isRotating;
        private bool isMovingToBasePosition;
        private bool onGrid;
        private bool isDragging;
        private SpriteRenderer spriteRenderer;
        private int baseLayerID;
        private int movableSortingLayerID = 1;
        private AudioManager audioManager;
        private event Action onPlaced;
        private bool isInteractable = true;
        private ItemData itemData;

        private Sequence squareSequance;

        protected PlacementSystem placementSystem;

        public int RotationState => rotationState;
        public Vector2 Size => size;
        public List<Vector2Int> ItemPoints => itemPoints;
        public GameObject GameObject => gameObject;
        public bool OnGrid => onGrid;

        public SpriteRenderer SpriteRenderer => spriteRenderer;

        private void OnDestroy()
        {
            OnPointerUpE -= Rotate;
        }

        private void SetSquareSequnce()
        {
            squareVisualize.color = new Color(baseSquareVisualizeColor.r, baseSquareVisualizeColor.g, baseSquareVisualizeColor.b, 0f);

            squareSequance = DOTween.Sequence();

            squareSequance.SetLoops(-1, LoopType.Yoyo).SetAutoKill(false);

            squareSequance.Pause();

            squareSequance.Append(squareVisualize.DOFade(1f, 0.5f));
            squareSequance.Append(squareVisualize.DOFade(0f, 0.5f));
        }

        private void ShowSquareVisualize()
        {
            squareSequance.Restart();

            squareSequance.Play();
        }

        private void HideSquareVisualize()
        {
            squareSequance.Pause();

            squareVisualize.DOFade(0f, 0.1f);
        }

        private void OnCorrectPlace()
        {
            onGrid = true;

            audioManager.Play(correctPlaceSfx);

            onPlaced?.Invoke();

            spriteRenderer.sortingOrder = baseLayerID;
        }

        private void OnWrongPlace()
        {
            isMovingToBasePosition = true;

            audioManager.Play(wrongPlaceSfx);
            HapticsManager.PlayHaptics(HapticsType.Standard);

            Camera.main.DOShakePosition(1f, 0.1f);
            gameObject.transform.DOShakePosition(1f, 0.1f).OnComplete(
                () => gameObject.transform.DOMove(basePosition, 1f).OnComplete(() => 
                {
                    isMovingToBasePosition = false;
                    spriteRenderer.sortingOrder = baseLayerID;
                }));
        }

        public void OnGridChanged(bool onGrid) => this.onGrid = onGrid;

        public void Setup(ItemData data, PlacementSystem placementSystem, AudioManager audioManager, Vector2 basePosition)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            baseLayerID = spriteRenderer.sortingOrder;

            itemData = data;

            this.basePosition = basePosition;
            size = data.CalculateSize();
            itemPoints = data.ItemPoints;
            this.placementSystem = placementSystem;
            this.audioManager = audioManager;

            SetSquareSequnce();

            OnPointerUpE += Rotate;
        }

        public void OnPlaced(Vector2Int gridPosition)
        {
            if (!isInteractable || isMovingToBasePosition)
                return;

            gameObject.transform.position = PositionCalculator.CalculatePosition(gridPosition, Size, rotationState);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isInteractable || isMovingToBasePosition)
                return;

            var gridValue = placementSystem.OnItemMove(GetGridDetectorPosition(), rotationState);

            transform.position = gridValue.Item2 ? PositionCalculator.CalculatePosition(gridValue.Item1, Size, rotationState) : gridValue.Item1;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!isInteractable || isMovingToBasePosition)
                return;

            spriteRenderer.sortingOrder = movableSortingLayerID;

            isDragging = true;

            if (!onGrid)
                placementSystem.StartPlacement(this);
            else
                placementSystem.RemovePlacement(this);

            audioManager.Play(putSfx);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isInteractable || isMovingToBasePosition)
                return;
#if UNITY_STANDALONE
            placementSystem.UnSubscribeRotateEvent(Rotate);
#endif

            var isPlaced = placementSystem.OnPlaceItem(GetGridDetectorPosition(), this);

            if (isPlaced)
                OnCorrectPlace();
            else
                OnWrongPlace();

            isDragging = false;
        }

        public void Rotate(InputAction.CallbackContext callbackContext)
        {
            if (isRotating || !isInteractable || isMovingToBasePosition)
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

        public void Rotate()
        {
            if (isRotating || isDragging || !IsAfterFirstTouch() || !isInteractable || onGrid || isMovingToBasePosition)
                return;

            audioManager.Play(rotateSFX);

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

        public void ChangeInteractableState(bool isInteractable, bool ignoreOnGrid = true)
        {
            if(ignoreOnGrid && onGrid)
            {
                this.isInteractable = true;
                return;
            }

            this.isInteractable = isInteractable;
        }

        public void SubscribeOnPlacedEvent(Action action)
        {
            onPlaced += action;
        }

        public void UnSubscribeOnPlacedEvent(Action action)
        {
            onPlaced -= action;
        }

        public bool IsSame(ItemData itemData)
        {
            return this.itemData == itemData;
        }

        public Vector3 GetGridDetectorPosition()
        {
            Vector3 tileOffset = new(0.5f, 0.5f);

            return rotationState switch
            {
                0 => transform.position - new Vector3(size.x / 2, size.y / 2) + tileOffset,
                1 => transform.position - new Vector3(size.y / 2, -size.x / 2) + new Vector3(tileOffset.x, -tileOffset.y),
                2 => transform.position + new Vector3(size.x / 2, size.y / 2) - tileOffset,
                3 => transform.position + new Vector3(size.y / 2, -size.x / 2) - new Vector3(tileOffset.x, -tileOffset.y),
                _ => transform.position - new Vector3(size.x / 2, size.y / 2) + tileOffset,
            };
        }

        public void OnOffSquareVisualize(bool onOff)
        {
            if(onOff)
                ShowSquareVisualize();
            else
                HideSquareVisualize();
        }
    }
}
