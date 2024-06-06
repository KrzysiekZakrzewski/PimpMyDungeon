using Generator.Item;
using GridPlacement;
using Item;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

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

        private (bool, List<Vector2Int>) PlaceBigItem(Vector2Int originPosition, List<Vector2Int> itemPoints, int rotationId)
        {
            List<Vector2Int> positions = new();

            for (int i = 0; i < itemPoints.Count; i++)
            {
                Vector2Int newPosToCheck
                        = originPosition + PositionCalculator.CalculateGridRotation(itemPoints[i], rotationId);

                if (!emptyTiles.Contains(newPosToCheck))
                    return (false, positions);

                positions.Add(newPosToCheck);
            }

            return (true, positions);
        }

        private (bool, ItemGeneratedData, List<Vector2Int>) GetItemPosition(List<Vector2Int> itemPoints, bool useRotation)
        {
            var potentialPositions = GetPotentialItemPositions(itemPoints, useRotation);

            var itemPotentialPosition = new ItemGeneratedData();
            var occupedPosition = new List<Vector2Int>();

            if (potentialPositions.Count == 0)
                return (false, itemPotentialPosition, occupedPosition);

            var id = Random.Range(0, potentialPositions.Count);

            itemPotentialPosition = potentialPositions.Keys.ElementAt(id);
            occupedPosition = potentialPositions.Values.ElementAt(id);

            return (true, itemPotentialPosition, occupedPosition);
        }

        private Dictionary<ItemGeneratedData, List<Vector2Int>> GetPotentialItemPositions(List<Vector2Int> itemPoints, bool useRotation)
        {
            Dictionary<ItemGeneratedData, List<Vector2Int>> potentialPositions = new();

            foreach (var tile in emptyTiles)
            {
                int rotationIteration = itemPoints.Count > 1 ? 3 : 1;

                int rotationID = useRotation ? Random.Range(0, 4) : 0;

                var (result, placementPositions) = PlaceBigItem(tile, itemPoints, rotationID);

                if (!result)
                    continue;

                potentialPositions.Add(new ItemGeneratedData(rotationID, tile), placementPositions);
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

        public List<ItemGeneratedData> GeneratePlacableItems()
        {
            List<ItemGeneratedData> generatedItems = new();

            GetNextPlaceableItem();

            while (currentItem != null)
            {
                var (result, generatedData, ocupedPosition) = GetItemPosition(currentItem.ItemData.ItemPoints, true);

                if (!result)
                {
                    GetNextPlaceableItem();
                    continue;
                }

                currentItemAmount++;

                var spawnedPosition = itemVisualizer.SpawnPlaceableItem(generatedData, currentItem.ItemData);

                generatedData.SetSpawnPosition(spawnedPosition);
                generatedData.SetPlaceableItemData(currentItem.ItemData);

                generatedItems.Add(generatedData);

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
            Dictionary<Vector2Int, ObstacleItemData> generatedObstacles = new();

            ObstacleItemData currentObstacleItem = obstacles.Dequeue();

            while (emptyTiles.Count > 0)
            {
                var (result, itemPositionData, ocupedPosition) = GetItemPosition(currentObstacleItem.ItemPoints, false);

                if (!result)
                {
                    currentObstacleItem = obstacles.Dequeue();
                    continue;
                }

                var originPosition = itemPositionData.OriginPosition;

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

    [Serializable]
    public class ItemGeneratedData
    {
        [field: SerializeField]
        public int RotationID { private set; get; }
        [field: SerializeField]
        public Vector2Int OriginPosition { private set; get; }
        [field: SerializeField]
        public Vector2 SpawnPosition { private set; get; }
        [field: SerializeField]
        public PlaceItemData PlaceItemData { private set; get; }

        public ItemGeneratedData()
        {
            RotationID = 0;
            OriginPosition = Vector2Int.zero;
            SpawnPosition = Vector2.zero;
        }

        public ItemGeneratedData(int rotationID, Vector2Int originPosition)
        {
            RotationID = rotationID;
            OriginPosition = originPosition;
            SpawnPosition = Vector2.zero;
        }

        public void SetSpawnPosition(Vector2 spawnPosition)
        {
            SpawnPosition = spawnPosition;
        }

        public void SetPlaceableItemData(PlaceItemData placeItemData)
        {
            PlaceItemData = placeItemData;
        }
    }
}