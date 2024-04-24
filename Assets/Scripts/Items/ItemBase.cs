using GridPlacement;
using Select;
using Select.Implementation;
using UnityEngine;

namespace Item
{
    public abstract class ItemBase : SelectObjectBase, IItem
    {
        public SpriteRenderer Renderer { get; private set; }

        public Sprite Sprite { get; private set; }

        public Vector2 Size { get; private set; }

        protected override void SelectObject_Select(ISelect callback)
        {
            base.SelectObject_Select(callback);

            transform.localScale = new Vector3(1.2f, 1.2f);

            PlacementSystem.Instance.StartPlacement(this);
        }

        protected override void SelectObject_DeSelect(ISelect callback)
        {
            base.SelectObject_DeSelect(callback);

            transform.localScale = Vector3.one;
        }

        public void Setup(ItemData data)
        {
            Renderer = GetComponent<SpriteRenderer>();
            Sprite = data.Sprite; 
            Size = data.Size;
        }
    }
}
