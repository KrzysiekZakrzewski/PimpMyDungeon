using GridPlacement.PlaceState;
using Item;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GridPlacement
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField]
        private LayerMask placeableLayers;
        [SerializeField]
        private Grid grid;
        [SerializeField]
        private PreviewSystem previewSystem;
        [SerializeField]
        private InputAction testRotateAction;

        private readonly Vector2Int resetLastCheckedPosition = new(-10000, -10000);

        private GridData gridData;
        private IPlacementState placeState;
        private Vector2Int lastChackedPosition;

        private event Action OnPlaced;
        private event Action OnRotate;

        private void OnEnable()
        {
            testRotateAction.performed -= Rotate;
            testRotateAction.Enable();
        }

        private void OnDisable()
        {
            testRotateAction.performed -= Rotate;
            testRotateAction.Disable();
        }

        private void Start()
        {
            lastChackedPosition = resetLastCheckedPosition;
            testRotateAction.performed += Rotate;
        }

        private void Rotate(InputAction.CallbackContext callbackContext)
        {
            OnRotate?.Invoke();
        }

        private bool IsSameGridPosition(Vector2Int gridPosition)
        {
            return lastChackedPosition == gridPosition;
        }

        private Vector2Int ConvertToGridPosition(Vector2 position)
        {
            return (Vector2Int)grid.WorldToCell(position);
        }

        private RaycastHit2D GetRayCastHit(Vector2 rayPosition)
        {
            rayPosition = Camera.main.WorldToScreenPoint(rayPosition);

            Ray ray = Camera.main.ScreenPointToRay(rayPosition);

            return Physics2D.GetRayIntersection(ray, Mathf.Infinity, placeableLayers);
        }

        private bool IsRayCastHited(RaycastHit2D raycastHit)
        {
            return raycastHit.collider != null;
        }

        private Vector3 GetMouseWorldPosition()
        {
            var mouseValue = Mouse.current.position.ReadValue();

            return Camera.main.ScreenToWorldPoint(mouseValue);
        }

        public void SetupGridData(GridData gridData)
        {
            this.gridData = gridData;
        }

        public void StartPlacement(IPlaceItem item)
        {
            StopPlacement();

            placeState = new PlacementState(gridData, item);

            previewSystem.SetupPreview(placeState, item);

            OnRotate += item.Rotate;

            //ADD Rotation Evenet
        }

        public void StopPlacement()
        {
            if (placeState == null)
                return;

            placeState.EndState();

            placeState = null;

            lastChackedPosition = resetLastCheckedPosition;

            previewSystem.OffPreview();
        }

        public void OnPlaceItem(Vector2 position)
        {
            var rayCastHit = GetRayCastHit(position);

            var rayCastPosition = IsRayCastHited(rayCastHit) ? rayCastHit.point : lastChackedPosition;

            Vector2Int gridPosition = ConvertToGridPosition(rayCastPosition);

            if (placeState == null)
                return;

            if (!placeState.OnAction(gridPosition))
            {
                StopPlacement();
                return;
            }

            previewSystem.OffPreview();

            OnPlaced?.Invoke();
        }

        public (Vector2, bool) OnItemMove(Vector2 gridDetectorPosition, int rotationState)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();

            var rayCastHit = GetRayCastHit(mouseWorldPosition);

            if (!IsRayCastHited(rayCastHit))
            {
                if(previewSystem.IsShow())
                    previewSystem.HidePreview();

                return (mouseWorldPosition, false);
            }

            if(!previewSystem.IsShow())
                previewSystem.ShowPreview();

            Vector2Int gridValue = ConvertToGridPosition(mouseWorldPosition);
            Vector2Int gridDetectorValue = ConvertToGridPosition(gridDetectorPosition);

            if(!IsSameGridPosition(gridDetectorValue))
                previewSystem.UpdatePreview(gridDetectorValue, rotationState);

            return (gridValue, true);
        }
    }
}