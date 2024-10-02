using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = nameof(PlaceItemData), menuName = nameof(Generator) + "/" + nameof(Generator.Item) + "/" + nameof(PlaceItemData))]
    public class PlaceItemData : ItemData
    {
        [field: SerializeField]
        public int Id { private set; get; }

        [field: SerializeField]
        public string ItemName { private set; get; }

        [field: SerializeField]
        public string ItemDescription { private set; get; }

        [field: SerializeField]
        public Sprite ItemIcon { private set; get; }

        [field: SerializeField]
        public Sprite ItemSizeIcon { private set; get; }
    }
}
