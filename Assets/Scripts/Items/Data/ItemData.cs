using System.Collections.Generic;
using UnityEngine;

namespace Item
{    public class ItemData : ScriptableObject
    {
        [field: SerializeField]
        public List<Vector2Int> ItemPoints { private set; get; }

        [field: SerializeField]
        public GameObject Prefab { private set; get; }

        public int GetItemSizeArea()
        {
            var size = CalculateSize();

            return size.x * size.y;
        }

        public Vector2Int CalculateSize()
        {
            var maxX = 0;
            var maxY = 0;
            var minX = 0;
            var minY = 0;

            for (int i = 0; i < ItemPoints.Count; i++)
            {
                var point = ItemPoints[i];

                maxX = point.x > maxX ? point.x : maxX;
                minX = point.x < minX ? point.x : minX;
                maxY = point.y > maxY ? point.y : maxY;
                minY = point.y < minY ? point.y : minY;
            }

            var x = (maxX - minX) + 1;
            var y = (maxY - minY) + 1;

            return new Vector2Int(x, y);
        }
    }
}