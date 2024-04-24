using Item;
using UnityEngine;

namespace Generator.Item
{
    [System.Serializable]
    public class ItemLevelGeneratorData 
    {
        [field: SerializeField]
        public int MaxItemAmount { private set; get; }
        [field: SerializeField]
        public PlaceItemData ItemData { private set; get; }
    }
}
