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

        [field: SerializeField, HideInInspector]
        public SerializedDictionary<Vector2Int, ObstacleItemData> Obstacles { private set; get; }

        [field: SerializeField, HideInInspector]
        public List<ItemGeneratedData> PlaceableItems { private set; get; }

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
    }
}
