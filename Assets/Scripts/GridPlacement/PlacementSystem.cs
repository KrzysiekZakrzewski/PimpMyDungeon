using GridPlacement.PlaceState;
using Inputs;
using Item;
using Levels;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace GridPlacement
{
    public class PlacementSystem : MonoBehaviour
    {
        [SerializeField]
        private LayerMask placeableLayers;
        [SerializeField]
        private PreviewSystem previewSystem;

        [NonSerialized]
        private Inputs.PlayerInput playerInput;

        private readonly Vector2Int resetLastCheckedPosition = new(-10000, -10000);
        private GridData gridData;
        private IPlacementState placeState;
        private Vector2Int lastChackedPosition;
        private LevelManager levelManager;
        private Grid grid;

        [Inject]
        private void Inject(LevelManager levelManager)
        {
            this.levelManager = levelManager;
        }

        private void Start()
        {
            playerInput = InputManager.GetPlayer(0);

            lastChackedPosition = resetLastCheckedPosition;
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
            var mouseValue = playerInput.GetCoordinates();

            return Camera.main.ScreenToWorldPoint(mouseValue);
        }

        public void SetupGridData(GridData gridData)
        {
            this.gridData = gridData;
        }

        public void StartPlacement(PlaceableItem item)
        {
            StopPlacement();

            placeState = new PlacementState(gridData, item);

            previewSystem.SetupPreview(placeState, item);

#if UNITY_STANDALONE
            SubscribeRotateEvent(item.Rotate);
#endif
        }

        public void RemovePlacement(PlaceableItem item)
        {
            StopPlacement();

            placeState = new RemoveState(gridData, item);

            Vector2Int gridDetectorValue = ConvertToGridPosition(item.GetGridDetectorPosition());

            var removeResults = placeState.OnAction(gridDetectorValue);

            item.OnGridChanged(!removeResults);

            if (!removeResults) return;

            StartPlacement(item);
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

        public bool OnPlaceItem(Vector2 position, PlaceableItem item)
        {
            var rayCastHit = GetRayCastHit(position);

            var rayCastPosition = IsRayCastHited(rayCastHit) ? rayCastHit.point : lastChackedPosition;

            Vector2Int gridPosition = ConvertToGridPosition(rayCastPosition);

            if (placeState == null)
                return false;

            if (!placeState.OnAction(gridPosition))
            {
                StopPlacement();
                return false;
            }

            previewSystem.OffPreview();

            levelManager.LevelCompleted(gridData.IsGridFilled());

            return true;
        }

        public (Vector2, bool) OnItemMove(Vector2 gridDetectorPosition, int rotationState)
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition() + previewSystem.PreviewOffSet;

            var rayCastHit = GetRayCastHit(mouseWorldPosition);

            if (!IsRayCastHited(rayCastHit))
            {
                if(previewSystem.IsShow)
                    previewSystem.HidePreview();
                
                return (mouseWorldPosition, false);
            }
 
            if(!previewSystem.IsShow)
                previewSystem.ShowPreview();
            
            Vector2Int gridValue = ConvertToGridPosition(mouseWorldPosition);
            Vector2Int gridDetectorValue = ConvertToGridPosition(gridDetectorPosition);

            if(!IsSameGridPosition(gridDetectorValue))
                previewSystem.UpdatePreview(gridDetectorValue, rotationState);

            return (gridValue, true);
        }

        public void InjectGrid(Grid grid)
        {
            this.grid = grid;
        }

#if UNITY_STANDALONE
        public void SubscribeRotateEvent(Action<InputAction.CallbackContext> action)
        {
            playerInput.AddInputEventDelegate(action, InputActionEventType.ButtonPressed, InputUtilities.Rotate);
        }

        public void UnSubscribeRotateEvent(Action<InputAction.CallbackContext> action)
        {
            playerInput.RemoveInputEventDelegate(action);
        }
#endif
    }
}