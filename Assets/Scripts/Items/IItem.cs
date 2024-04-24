using UnityEngine;

namespace Item
{
    public interface IItem
    {
        SpriteRenderer Renderer { get; }
        Sprite Sprite { get; }
        Vector2 Size { get; }

        void Setup(ItemData data);
    }
}
