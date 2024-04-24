using Item;
using System.Collections.Generic;
using UnityEngine;

namespace Generator
{
    public class ItemVisualizer : MonoBehaviour
    {
        [SerializeField]
        private Transform generatedItemsMagazine = null;

        [SerializeField, HideInInspector]
        private List<GameObject> generatedItems = new List<GameObject>();

        public void Clear()
        {
            for (int i = 0; i < generatedItems.Count; i++)
            {
                var item = generatedItems[i].gameObject;

                if(item == null) continue;

                DestroyImmediate(item);
            }

            generatedItems.Clear();
        }

        public void SpawnObstacle(Vector2Int position, ObstacleItemData obstacle)
        {
            float x = obstacle.Size.x;
            float y = obstacle.Size.y;

            var offSet = new Vector3(x / 2, y / 2);

            Vector3 spawnPosition = new Vector3(position.x, position.y) + offSet;

            var newObject = Instantiate(obstacle.Prefab, spawnPosition, Quaternion.identity, generatedItemsMagazine);

            generatedItems.Add(newObject);
        }

        public Vector2 SpawnPlaceableItem(Vector2Int position, PlaceItemData placeItem)
        {
            float x = placeItem.Size.x;
            float y = placeItem.Size.y;

            var offSet = new Vector3(x / 2, y / 2);

            Vector3 spawnPosition = new Vector3(position.x, position.y) + offSet;

            var newObject = Instantiate(placeItem.Prefab, spawnPosition, Quaternion.identity, generatedItemsMagazine);

            generatedItems.Add(newObject);

            return spawnPosition;
        }
    }
}