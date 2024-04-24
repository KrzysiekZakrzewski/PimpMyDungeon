using UnityEngine;

namespace Item
{    public class ItemData : ScriptableObject
    {
        [field: SerializeField]
        public Sprite Sprite { private set; get; }

        [field: SerializeField]
        public Vector2Int Size { private set; get; }

        [field: SerializeField]
        public GameObject Prefab { private set; get; }

        public int GetItemSizeArea()
        {
            return Size.x * Size.y;
        }
    }
}