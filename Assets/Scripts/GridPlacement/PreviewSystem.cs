using DG.Tweening;
using GridPlacement.PlaceState;
using Item;
using System.Collections;
using UnityEngine;

namespace GridPlacement
{
    public class PreviewSystem : MonoBehaviour
    {
        [SerializeField]
        private Color positiveColor, negativeColor;

        private SpriteRenderer previewRenderer;
        private Vector2 size;
        private IPlacementState placementState;
        private Color sequnceColor = Color.white;

        private Sequence scaleSequence;
        private Coroutine colorCorutine;

        private Vector3 previewMinScale = new(0.9f, 0.9f);
        private Vector3 previewMaxScale = new(1.2f, 1.2f);

        private float duration = 0.5f;

        public bool IsShow { private set; get; }

        public Vector3 PreviewOffSet => new(size.x /2, size.y /2);

        private void ColorSequence()
        {
            previewRenderer?.DOKill();

            previewRenderer.DOColor(sequnceColor, duration).OnComplete(() =>
            {
                previewRenderer.DOColor(Color.white, duration).OnComplete(ColorSequence);
            });
        }

        private void SetupScaleSequence()
        {
            scaleSequence = DOTween.Sequence();
            scaleSequence.Append(previewRenderer.transform.DOScale(previewMaxScale, duration));
            scaleSequence.Append(previewRenderer.transform.DOScale(previewMinScale, duration));
            scaleSequence.SetLoops(-1, LoopType.Yoyo);
        }

        public void SetupPreview(IPlacementState placementState, PlaceableItem placeItem)
        {
            this.placementState = placementState;
            previewRenderer = placeItem.SpriteRenderer;
            size = placeItem.Size;

            SetupScaleSequence();

            scaleSequence.Play();
        }

        public void ShowPreview()
        {
            if (IsShow)
                return;

            IsShow = true;
        }

        public void HidePreview()
        {
            if (!IsShow)
                return;

            IsShow = false;

            previewRenderer?.DOKill();

            previewRenderer.DOColor(Color.white, duration);
        }

        public void OffPreview()
        {
            if(placementState == null)
                return;

            HidePreview();

            scaleSequence?.Kill();

            previewRenderer.transform.DOScale(Vector3.one, duration);

            placementState = null;
            previewRenderer = null;
        }

        public void UpdatePreview(Vector2Int gridPosition, int rotationState)
        {
            if(!IsShow)
                return;

            var validate = placementState.CheckPlacementValidity(gridPosition);

            sequnceColor = validate ? positiveColor : negativeColor;

            ColorSequence();
        }
    }
}
