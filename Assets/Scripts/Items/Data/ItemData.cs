using System.Collections.Generic;
using UnityEngine;

namespace Item
{    public class ItemData : ScriptableObject
    {
        [field: SerializeField]
        public List<Vector2Int> ItemPoints { private set; get; }

        [field: SerializeField]
        public GameObject Prefab { private set; get; }

        private Vector2Int size;

        public Vector2Int Size => size;

        private void OnValidate()
        {
            CalculateSize();
        }

        public int GetItemSizeArea()
        {
            return size.x * size.y;
        }

        public void CalculateSize()
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

            size.x = (maxX - minX) + 1;
            size.y = (maxY - minY) + 1;
        }
    }
}