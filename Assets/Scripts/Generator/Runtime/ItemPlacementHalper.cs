using Generator.Item;
using Item;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Generator
{
    public class ItemPlacementHalper
    {
        private HashSet<Vector2Int> emptyTiles;

        private Queue<ItemLevelGeneratorData> placeableItemToGenerate;
        private Queue<ObstacleItemData> obstacles;
        private ItemLevelGeneratorData currentItem;

        private int currentItemAmount = 0;

        private ItemVisualizer itemVisualizer;

        public ItemPlacementHalper(HashSet<Vector2Int> roomFloor,
            ItemVisualizer itemVisualizer,
            List<ItemLevelGeneratorData> itemGeneratorData,
            ObstacleItemDatabase obstacles)
        {
            emptyTiles = roomFloor;
            this.itemVisualizer = itemVisualizer;
            this.obstacles = new Queue<ObstacleItemData>(obstacles.ObstacleItems);

            SortItems(itemGeneratorData);
        }

        private (bool, List<Vector2Int>) PlaceBigItem(Vector2Int originPosition, Vector2Int size)
        {
            List<Vector2Int> positions = new List<Vector2Int>() { originPosition };

            for (int row = 0; row < size.x; row++)
            {
                for (int col = 0; col < size.y; col++)
                {
                    if (col == 0 && row == 0)
                        continue;

                    Vector2Int newPosToCheck
                        = new Vector2Int(originPosition.x + row, originPosition.y + col);

                    if (!emptyTiles.Contains(newPosToCheck))
                        return (false, positions);

                    positions.Add(newPosToCheck);
                }
            }

            return (true, positions);
        }

        private (bool, Vector2Int, List<Vector2Int>) GetItemPosition(Vector2Int size)
        {
            var potentialPositions = GetPotentialItemPositions(size);

            var originPosition = Vector2Int.zero;
            var occupedPosition = new List<Vector2Int>();

            if (potentialPositions.Count == 0)
                return (false, originPosition, occupedPosition);

            var id = Random.Range(0, potentialPositions.Count);

            originPosition = potentialPositions.Keys.ElementAt(id);
            occupedPosition = potentialPositions.Values.ElementAt(id);

            return (true, originPosition, occupedPosition);
        }

        private Dictionary<Vector2Int, List<Vector2Int>> GetPotentialItemPositions(Vector2Int size)
        {
            Dictionary<Vector2Int, List<Vector2Int>> potentialPositions = new Dictionary<Vector2Int, List<Vector2Int>>();

            foreach (var tile in emptyTiles)
            {
                var (result, placementPositions) = PlaceBigItem(tile, size);

                if (!result)
                    continue;

                potentialPositions.Add(tile, placementPositions);
            }

            return potentialPositions;
        }

        private void SortItems(List<ItemLevelGeneratorData> itemGeneratorData)
        {
            var sortedList = itemGeneratorData.OrderBy(x => x.ItemData.GetItemSizeArea()).ToList();

            sortedList.Reverse();

            placeableItemToGenerate = new Queue<ItemLevelGeneratorData>(sortedList);
        }

        private void GetNextPlaceableItem()
        {
            currentItemAmount = 0;

            currentItem = placeableItemToGenerate.Count == 0 ? null : placeableItemToGenerate.Dequeue();
        }

        public Dictionary<Vector2, PlaceItemData> GeneratePlacableItems()
        {
            Dictionary<Vector2, PlaceItemData> generatedItems = new Dictionary<Vector2, PlaceItemData>();

            GetNextPlaceableItem();

            while (currentItem != null)
            {
                var (result, originPosition, ocupedPosition) = GetItemPosition(currentItem.ItemData.Size);

                if (!result)
                {
                    GetNextPlaceableItem();
                    continue;
                }

                currentItemAmount++;

                var spawnedPosition = itemVisualizer.SpawnPlaceableItem(originPosition, currentItem.ItemData);

                generatedItems.Add(spawnedPosition, currentItem.ItemData);

                for (int i = 0; i < ocupedPosition.Count; i++)
                {
                    emptyTiles.Remove(ocupedPosition[i]);
                }

                if (currentItemAmount >= currentItem.MaxItemAmount)
                    GetNextPlaceableItem();
            }

            return generatedItems;
        }

        public Dictionary<Vector2Int, ObstacleItemData> GenerateObstacles()
        {
            Dictionary<Vector2Int, ObstacleItemData> generatedObstacles = new Dictionary<Vector2Int, ObstacleItemData>();

            ObstacleItemData currentObstacleItem = obstacles.Dequeue();

            while (emptyTiles.Count > 0)
            {
                var (result, originPosition, ocupedPosition) = GetItemPosition(currentObstacleItem.Size);

                if (!result)
                {
                    currentObstacleItem = obstacles.Dequeue();
                    continue;
                }

                itemVisualizer.SpawnObstacle(originPosition, currentObstacleItem);

                generatedObstacles.Add(originPosition, currentObstacleItem);

                for (int i = 0; i < ocupedPosition.Count; i++)
                {
                    emptyTiles.Remove(ocupedPosition[i]);
                }
            }

            return generatedObstacles;
        }
    }
}