using DG.Tweening;
using GridPlacement.PlaceState;
using Item;
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
        private bool isShow;

        public void SetupPreview(IPlacementState placementState, PlaceableItem placeItem)
        {
            this.placementState = placementState;
            previewRenderer = placeItem.PreviewRenderer;
            size = placeItem.Size;
        }

        public bool IsShow()
        {
            return isShow;
        }

        public void ShowPreview()
        {
            if (isShow)
                return;

            isShow = true;

            previewRenderer.DOFade(1f, 0.5f);
        }

        public void HidePreview()
        {
            if (!isShow)
                return;

            previewRenderer.DOFade(0f, 0.5f);

            isShow = false;
        }

        public void OffPreview()
        {
            HidePreview();

            placementState = null;
            previewRenderer = null;
        }

        public void UpdatePreview(Vector2Int gridPosition, int rotationState)
        {
            if(!isShow)
                return;

            var validate = placementState.CheckPlacementValidity(gridPosition);

            Color resultColor = validate ? positiveColor : negativeColor;

            previewRenderer.DOKill();

            previewRenderer.DOColor(resultColor, 0.2f);

            previewRenderer.transform.position = PositionCalculator.CalculatePosition(gridPosition, size, rotationState);
        }
    }
}
