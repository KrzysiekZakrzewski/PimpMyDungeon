using MouseInteraction.Manager;
using System;
using UnityEngine;

namespace MouseInteraction.Select
{
    public abstract class SelectObject : MouseObject
    {
        protected event Action OnSelectE;
        protected event Action OnDeselectE;

        private bool isSelected;
        private bool isFirstTouch;

        public bool IsSelected => isSelected;

        protected virtual void OnEnable()
        {
            OnPointerUpE += SetFirtstTouch;
            OnPointerDownE += Select;
        }

        protected virtual void OnDisable()
        {
            OnPointerDownE -= Select;
            OnPointerUpE -= SetFirtstTouch;
        }

        private void SetFirtstTouch()
        {
            isFirstTouch = false;
        }

        public void Select()
        {
            if (isSelected)
                return;

            isSelected = true;
            isFirstTouch = true;

            SelectManager.Current.SetSelectedObject(this);

            OnSelectE?.Invoke();
        }

        public void Deselect()
        {
            if (!isSelected)
                return;

            isSelected = false;

            OnDeselectE?.Invoke();
        }

        public bool IsAfterFirstTouch()
        {
            return isSelected && !isFirstTouch;
        }
    }
}
