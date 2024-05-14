using DG.Tweening;
using GridPlacement;
using Select;
using Select.Implementation;
using UnityEngine;

namespace Item
{
    public abstract class ItemBase : SelectObjectBase, IItem
    {
        protected PlacementSystem placementSystem;

        private bool isPlaced;
        private Sequence selectSequence;
        private Vector3 putScale = new(1.2f, 1.2f);
        private readonly float selectSequenceDuration = 0.5f;

        public SpriteRenderer Renderer { get; private set; }
        public Sprite Sprite { get; private set; }
        public Vector2 Size { get; private set; }

        protected override void SelectObject_Select(ISelect callback)
        {
            base.SelectObject_Select(callback);

            selectSequence.Restart();
            selectSequence.Play();

            placementSystem.StartPlacement(this);

            placementSystem.AddPlacementEvent(OnPlaced, OnExit);
        }

        protected override void SelectObject_DeSelect(ISelect callback)
        {
            base.SelectObject_DeSelect(callback);

            selectSequence.Pause();
            transform.localScale = Vector3.one;

            placementSystem.StopPlacement();

            placementSystem.RemovePlacementEvent(OnPlaced, OnExit);
        }

        public void Setup(ItemData data, PlacementSystem placementSystem)
        {
            Renderer = GetComponent<SpriteRenderer>();
            Sprite = data.Sprite; 
            Size = data.Size;
            this.placementSystem = placementSystem;

            selectSequence = DOTween.Sequence();
            selectSequence.Append(transform.DOScale(putScale, selectSequenceDuration));
            selectSequence.Append(transform.DOScale(Vector3.one, selectSequenceDuration));
            selectSequence.SetLoops(-1, LoopType.Restart);
            selectSequence.Pause();
        }

        public void OnPlaced()
        {
            isPlaced = true;

            placementSystem.RemovePlacementEvent(OnPlaced, OnExit);
        }

        public void OnExit()
        {
            Debug.Log("OnExit");
        }

        public void MoveToPlacePosition(Vector2 gridPosition, float duration = 1f)
        {
            var placePosition = gridPosition + new Vector2(Size.x / 2, Size.y / 2);

            transform.DOMove(placePosition, duration);
        }
    }
}
