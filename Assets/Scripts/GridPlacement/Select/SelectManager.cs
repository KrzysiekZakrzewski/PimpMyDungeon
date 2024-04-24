using Select;
using UnityEngine;

namespace MouseInteraction.Manager
{
    [System.Serializable]
    internal class SelectManager
    {
        private ISelect currentSelected;
        private bool selectionGuard;
        public bool alreadySelecting
        {
            get { return selectionGuard; }
        }

        private void DeSelect()
        {
            if(currentSelected == null) return;

            currentSelected?.DeSelect();

            currentSelected = null;
        }

        public void SetSelectedGameObject(ISelect selected)
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

            selected?.Select();

            selectionGuard = false;
        }
    }
}
