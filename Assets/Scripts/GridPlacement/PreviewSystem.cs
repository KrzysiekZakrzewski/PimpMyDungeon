using DG.Tweening;
using GridPlacement.PlaceState;
using Item;
using UnityEngine;

namespace GridPlacement
{
    public class PreviewSystem : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer previewRenderer;

        [SerializeField]
        private Color positiveColor, negativeColor;

        private IPlacementState placementState;
        private Vector2 size;
        private bool isShow;

        public void SetupPreview(IPlacementState placementState, IPlaceItem item)
        {
            this.placementState = placementState;
            size = item.Size;

            previewRenderer.gameObject.transform.localScale = size;

            UpdateRotate(item.GameObject.transform.localEulerAngles);
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
            placementState = null;

            HidePreview();
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

        public void UpdateRotate(Vector3 localEulerAngles)
        {
            if (!isShow)
                return;

            previewRenderer.gameObject.transform.localEulerAngles = localEulerAngles;
        }
    }
}
