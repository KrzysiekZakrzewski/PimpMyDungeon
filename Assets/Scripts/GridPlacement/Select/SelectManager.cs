using GridPlacement.EventData;
using MouseInteraction.Select;
using System.Collections.Generic;
using TimeTickSystems;
using UnityEngine;

namespace MouseInteraction.Manager
{
    public class SelectManager : MonoBehaviour
    {
        private ISelectObject currentSelected;
        private bool selectionGuard;
        private SelectEventData selectEventData;

        private SelectEventData SelectEventDataCache
        {
            get
            {
                if (selectEventData == null)
                {
                    selectEventData = new SelectEventData(this);
                }

                return selectEventData;
            }
        }

        private static readonly List<SelectManager> selectManagers = new();

        public bool AlreadySelecting
        {
            get { return selectionGuard; }
        }

        private void OnEnable()
        {
            TimeTickSystem.Create();
            selectManagers.Add(this);
        }

        public static SelectManager Current
        {
            get
            {
                return (selectManagers.Count > 0) ? selectManagers[0] : null;
            }
            set
            {
                int num = selectManagers.IndexOf(value);
                if (num > 0)
                {
                    selectManagers.RemoveAt(num);
                    selectManagers.Insert(0, value);
                }
                else if (num < 0)
                {
                    Debug.LogError("Failed setting SelectManager.current to unknown SelectManager " + value);
                }
            }
        }

        private void DeSelect()
        {
            if(currentSelected == null) return;

            currentSelected?.OnDeSelect(SelectEventDataCache);

            selectEventData = null;
            currentSelected = null;
        }

        public void SetSelectedGameObject(ISelectObject selected)
        {
            if(selected == null && currentSelected == null) return;

            if (selectionGuard)
            {
                Debug.LogError("Attempting to select " + selected + "while already selecting an object.");
                return;
            }

            selectionGuard = true;

            if (selected == currentSelected)
            {
                selectionGuard = false;
                return;
            }

            DeSelect();

            currentSelected = selected;

            currentSelected.OnSelect(SelectEventDataCache);

            selectionGuard = false;
        }
    }
}
