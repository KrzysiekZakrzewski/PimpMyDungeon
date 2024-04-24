using Item;
using UnityEngine;

namespace GridPlacement
{
    public class PlacementSystem : MonoBehaviour
    {
        [field: SerializeField]
        public PreviewSystem PreviewSystem { private set; get; }

        public static PlacementSystem Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void SetupGridData(GridData gridData)
        {
            PreviewSystem.SetupGridData(gridData);
        }

        public void StartPlacement(ItemBase item)
        {
            if (item == null)
                return;

            PreviewSystem.StartShow(item);
        }

        public void StopPlacement()
        {
            PreviewSystem.StopShow();
        }
    }
}