using GridPlacement;
using GridPlacement.EventData;
using GridPlacement.Hold;
using MouseInteraction.Drag;
using MouseInteraction.Manager;
using MouseInteraction.Select;
using TimeTickSystems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Item
{
    public abstract class ItemBase : MonoBehaviour, ISelectObject, IPlaceItem, IDragObject, IHoldObject
    {
        protected PlacementSystem placementSystem;

        private bool isSelected;
        private bool isPrepeareToHold;
        private Vector2 size;

        public Vector2 Size => size;
        public bool IsSelected => isSelected;
        
        private void TimeTickSystem_OnTickPrepeareToHold(object sender, OnTickEventArgs e)
        {
            Debug.Log($"Tick: {TimeTickSystem.GetTick()}");
        }

        public void Setup(ItemData data, PlacementSystem placementSystem)
        {
            size = data.Size;
            this.placementSystem = placementSystem;
        }

        public void OnSelect(SelectEventData eventData)
        {
            if (isSelected)
                return;

            isSelected = true;

            TimeTickSystem.OnTick += TimeTickSystem_OnTickPrepeareToHold;

            Debug.Log("Select");
        }

        public void OnDeSelect(SelectEventData eventData)
        {
            if (!isSelected)
                return;

            isSelected = false;

            TimeTickSystem.OnTick -= TimeTickSystem_OnTickPrepeareToHold;

            Debug.Log("DeSelect");
        }

        public void OnPointerEnter(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SelectManager.Current.SetSelectedGameObject(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnPlaced()
        {

        }

        public void OnExit()
        {

        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("Drag");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("DragEnd");
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("DragBegin");
        }

        public void OnHold()
        {
            throw new System.NotImplementedException();
        }

        public void OnBeginHold()
        {
            throw new System.NotImplementedException();
        }

        public void OnEndHold()
        {
            throw new System.NotImplementedException();
        }
    }
}
