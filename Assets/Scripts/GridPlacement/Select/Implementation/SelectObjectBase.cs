using MouseInteraction;
using MouseInteraction.Implementation;
using System;

namespace Select.Implementation
{
    [Serializable]
    public class SelectObjectBase : MouseDownBase, ISelect
    {
        protected bool isSelected = false;

        public bool IsSelected => isSelected;

        public event Action<ISelect> OnSelected;
        public event Action<ISelect> OnDeSelected;

        private void OnEnable()
        {
            OnSelected += SelectObject_Select;
            OnDeSelected += SelectObject_DeSelect;
        }

        private void OnDisable()
        {
            OnSelected -= SelectObject_Select;
            OnDeSelected -= SelectObject_DeSelect;
        }

        protected virtual void SelectObject_Select(ISelect callback)
        {

        }

        protected virtual void SelectObject_DeSelect(ISelect callback)
        {

        }

        protected override void MouseObject_MouseDown(IMouseDown callback)
        {
            base.MouseObject_MouseDown(callback);

            Select();
        }

        public void Select()
        {
            if (IsSelected) return;

            isSelected = true;

            OnSelected?.Invoke(this);
        }
        public void DeSelect()
        {
            if (!IsSelected) return;

            isSelected = false;

            OnDeSelected?.Invoke(this);
        }
    }
}
