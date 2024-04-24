using System;
using UnityEngine;

namespace MouseInteraction.Implementation
{
    public class MouseDownBase : MonoBehaviour, IMouseDown
    {
        private bool mouseWasDown = false;
        public bool MouseWasDown => mouseWasDown;

        public event Action<IMouseDown> OnMouseDown;
        public event Action<IMouseDown> OnMouseUp;

        private void OnEnable()
        {
            OnMouseDown += MouseObject_MouseDown;
            OnMouseUp += MouseObject_MouseUp;
        }

        private void OnDisable()
        {
            OnMouseDown -= MouseObject_MouseDown;
            OnMouseUp -= MouseObject_MouseUp;
        }

        protected virtual void MouseObject_MouseDown(IMouseDown callback)
        {

        }

        protected virtual void MouseObject_MouseUp(IMouseDown callback)
        {

        }

        public void MouseDown()
        {
            if (MouseWasDown) return;

            mouseWasDown = true;

            OnMouseDown?.Invoke(this);
        }
        public void MouseUp()
        {
            if (!MouseWasDown) return;

            mouseWasDown = false;

            OnMouseUp?.Invoke(this);
        }
    }
}
