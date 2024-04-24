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

        [field: SerializeField]
        public List<Vector2Int> FloorPositions { private set; get; }

        [field: SerializeField]
        public SerializedDictionary<Vector2Int, ObstacleItemData> Obstacles { private set; get; }

        [field: SerializeField]
        public SerializedDictionary<Vector2, PlaceItemData> PlaceableItems { private set; get; }

        [field: SerializeField]
        public Vector2 CenterPoint { private set; get; }

        public void Init(string name,
            HashSet<Vector2Int> floorPositions,
            Vector2 centerPoint,
            Dictionary<Vector2Int, ObstacleItemData> obstacles,
            Dictionary<Vector2, PlaceItemData> placeableItem)
        {
            Name = name;
            CenterPoint = centerPoint;
            FloorPositions = floorPositions.ToList();
            Obstacles = obstacles;
            PlaceableItems = placeableItem;
        }

        public static LevelDataSO CreateInstance(string name,
            HashSet<Vector2Int> floorPositions,
            Vector2 centerPoint,
            Dictionary<Vector2Int, ObstacleItemData> obstacles, 
            Dictionary<Vector2, PlaceItemData> placeableItem)
        {
            var data = CreateInstance<LevelDataSO>();
            data.Init(name, floorPositions, centerPoint, obstacles, placeableItem);
            return data;
        }
    }
}
