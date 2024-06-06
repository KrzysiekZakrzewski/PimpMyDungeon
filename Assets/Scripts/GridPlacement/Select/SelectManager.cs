using MouseInteraction.Select;
using System.Collections.Generic;
using TimeTickSystems;
using UnityEngine;

namespace MouseInteraction.Manager
{
    public class SelectManager : MonoBehaviour
    {
        private bool selectionGuard;

        public SelectObject CurrentSelected { private set; get; }

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

        private void Deselect()
        {
            if(CurrentSelected == null) return;

            CurrentSelected.Deselect();
            CurrentSelected = null;
        }

        public void SetSelectedObject(SelectObject selected)
        {
            if(selected == null && CurrentSelected == null) return;

            if (selectionGuard)
            {
                Debug.LogError("Attempting to select " + selected + "while already selecting an object.");
                return;
            }

            selectionGuard = true;

            if (selected == CurrentSelected)
            {
                selectionGuard = false;
                return;
            }

            Deselect();

            CurrentSelected = selected;

            CurrentSelected.Select();

            selectionGuard = false;
        }
    }
}
