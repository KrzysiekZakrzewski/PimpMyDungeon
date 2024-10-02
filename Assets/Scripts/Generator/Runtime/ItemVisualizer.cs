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

        private Vector3 CalculateOffSet(int rotationState,Vector3 offSet)
        {
            Vector3 newOffSet = offSet;

            float y;
            float x;
            float maxStep = 4f;
            float step;
            switch (rotationState)
            {
                case 0:
                    break;
                case 1:
                    newOffSet = new Vector3(offSet.y, -offSet.x + 1, offSet.z);
                    break;
                case 2:
                    step = offSet.y / 0.5f;
                    y = 1 - (step * 0.5f);
                    step = offSet.x / 0.5f;
                    step = maxStep - step;
                    x = -1 + (step * 0.5f);
                    newOffSet = new Vector3(x, y, offSet.z);
                    break;
                case 3:
                    step = offSet.y / 0.5f;
                    x = 1 - (step * 0.5f);
                    step = offSet.x / 0.5f;
                    y = step * 0.5f;
                    newOffSet = new Vector3(x, y, offSet.z);
                    break;
            }

            return newOffSet;
        }

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
            var size = obstacle.CalculateSize();

            float x = size.x;
            float y = size.y;

            var offSet = new Vector3(x / 2, y / 2);

            Vector3 spawnPosition = new Vector3(position.x, position.y) + offSet;

            var newObject = Instantiate(obstacle.Prefab, spawnPosition, Quaternion.identity, generatedItemsMagazine);

            generatedItems.Add(newObject);
        }

        public Vector2 SpawnPlaceableItem(ItemGeneratedData potentialPosition, PlaceItemData placeItem)
        {
            var size = placeItem.CalculateSize();

            float x = size.x;
            float y = size.y;

            var offSet = new Vector3(x / 2, y / 2);

            offSet = CalculateOffSet(potentialPosition.RotationID, offSet);

            var position = potentialPosition.OriginPosition;
            var rotation = new Vector3(0, 0, potentialPosition.RotationID * -90);

            Vector3 spawnPosition = new Vector3(position.x, position.y) + offSet;

            var newObject = Instantiate(placeItem.Prefab, spawnPosition, Quaternion.identity, generatedItemsMagazine);

            newObject.transform.eulerAngles = rotation;

            generatedItems.Add(newObject);

            return spawnPosition;
        }
    }
}