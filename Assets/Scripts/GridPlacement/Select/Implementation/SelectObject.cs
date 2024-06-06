using MouseInteraction.Manager;
using System;

namespace MouseInteraction.Select
{
    public abstract class SelectObject : MouseObject
    {
        protected event Action OnSelectE;
        protected event Action OnDeselectE;

        private bool isSelected;

        public bool IsSelected => isSelected;

        protected virtual void OnEnable()
        {
            OnPointerDownE += Select;
        }

        protected virtual void OnDisable()
        {
            OnPointerDownE -= Select;
        }

        public void Select()
        {
            if (isSelected)
                return;

            isSelected = true;

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
    }
}
