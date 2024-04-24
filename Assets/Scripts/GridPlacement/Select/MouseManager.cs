using Select;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace MouseInteraction.Manager
{
    public class MouseManager : MonoBehaviour
    {
        [SerializeField]
        private InputAction leftMouse;

        [SerializeField]
        private LayerMask placementLayermask;
        private Camera mainCamera;

        private IMouseDown mouseDown;

        private SelectManager selectManager;

        private void Awake()
        {
            selectManager = new SelectManager();
            mainCamera = Camera.main;

            leftMouse.performed += ReadMouse;
            leftMouse.canceled += MouseUp;
        }

        private void OnEnable()
        {
            leftMouse.Enable();
        }

        private void OnDisable()
        {
            leftMouse.Disable();
        }

        private bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

        private void ReadMouse(InputAction.CallbackContext ctx)
        {
            if (IsPointerOverUI())
                return;

            var hit = GetRayCastHit();

            if(hit.collider == null)
            {
                selectManager.SetSelectedGameObject(null);

                return;
            }

            mouseDown = hit.collider.GetComponent<IMouseDown>();

            mouseDown?.MouseDown();

            if (mouseDown is not ISelect) return;

            selectManager.SetSelectedGameObject((ISelect)mouseDown);
        }

        private void MouseUp(InputAction.CallbackContext ctx)
        {
            if (mouseDown == null) return;

            mouseDown?.MouseUp();
        }

        public RaycastHit2D GetRayCastHit()
        {
            var mousePos = GetMousePosition();

            Ray ray = mainCamera.ScreenPointToRay(mousePos);

            return Physics2D.GetRayIntersection(ray, Mathf.Infinity, placementLayermask);
        }

        public static Vector2 GetMousePosition()
        {
            return Mouse.current.position.ReadValue();
        }
    }
}
