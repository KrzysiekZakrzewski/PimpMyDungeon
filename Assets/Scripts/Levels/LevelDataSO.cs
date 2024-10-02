using Generator;
using Item;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Levels.Data
{
    [CreateAssetMenu(fileName = nameof(LevelDataSO), menuName = nameof(Levels) + "/" + nameof(Levels.Data) + "/" + nameof(LevelDataSO))]
    public class LevelDataSO : ScriptableObject
    {
        [field: SerializeField]
        public string Name { private set; get; }

        [field: SerializeField, HideInInspector]
        public List<Vector2Int> FloorPositions { private set; get; }

        [field: SerializeField]
        public Vector2 CenterPoint { private set; get; }

        [field: SerializeField]
        public SerializedDictionary<Vector2Int, ObstacleItemData> Obstacles { private set; get; }

        [field: SerializeField]
        public List<ItemGeneratedData> PlaceableItems { private set; get; }

        [field: SerializeField, HideInInspector]
        public bool IsOverride { set; get; }

        [field: SerializeField, HideIf("PlaceablePositionOverride", false)]
        public List<ItemGeneratedDataOverride> PlaceableItemOverride { private set; get; }

        public void Init(
            HashSet<Vector2Int> floorPositions,
            Vector2 centerPoint,
            Dictionary<Vector2Int, ObstacleItemData> obstacles,
            List<ItemGeneratedData> placeableItem)
        {
            CenterPoint = centerPoint;
            FloorPositions = floorPositions.ToList();
            Obstacles = obstacles;
            PlaceableItems = placeableItem;
        }

        public static LevelDataSO CreateInstance(
            HashSet<Vector2Int> floorPositions,
            Vector2 centerPoint,
            Dictionary<Vector2Int, ObstacleItemData> obstacles, 
            List<ItemGeneratedData> placeableItem)
        {
            var data = CreateInstance<LevelDataSO>();
            data.Init(floorPositions, centerPoint, obstacles, placeableItem);
            return data;
        }

        public void SetupName(string name)
        {
            Name = name;
        }

        public void GeneratePlaceableItemData()
        {
            PlaceableItemOverride = new List<ItemGeneratedDataOverride>();

            for (int i = 0; i < PlaceableItems.Count; i++)
            {
                PlaceableItemOverride.Add(new ItemGeneratedDataOverride(PlaceableItems[i]));
            }
        }
    }

    [System.Serializable]
    public class ItemGeneratedDataOverride
    {
        [field: SerializeField]
        public ItemGeneratedData ItemGeneratedData { private set; get; }

        [field: SerializeField]
        public Vector2 OverrideSpawnPosition { private set; get; }

        public ItemGeneratedDataOverride(ItemGeneratedData itemGeneratedData)
        {
            ItemGeneratedData = itemGeneratedData;
            OverrideSpawnPosition = Vector2.zero;
        }
    }
}
