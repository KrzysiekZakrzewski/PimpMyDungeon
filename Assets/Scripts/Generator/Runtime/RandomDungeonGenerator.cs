using Generator.Data;
using Levels.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generator
{
    public class RandomDungeonGenerator : AbstractDungeonGenerator
    {
        [SerializeField]
        protected SimpleLevelGeneratorData levelGeneratorData;

        private HashSet<Vector2Int> floorPositions;

        protected override LevelDataSO RunProceduralGeneration()
        {
            floorPositions = RunRandomWalk(levelGeneratorData, startPosition);
            tilemapVisualizer.Clear();
            itemVisualizer.Clear();
            tilemapVisualizer.PaintFloorTiles(floorPositions);
            WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);

            var emptyPosition = new HashSet<Vector2Int>(floorPositions);

            ItemPlacementHalper itemPlacer = new ItemPlacementHalper(
                emptyPosition,
                itemVisualizer,
                levelGeneratorData.PlaceItem, 
                levelGeneratorData.ObstacleItemDatabase);

            var placeableItems = itemPlacer.GeneratePlacableItems();

            var obstacles = itemPlacer.GenerateObstacles();

            Vector2 centerPoint = tilemapVisualizer.GetCenterPoint();

            return LevelDataSO.CreateInstance(name, floorPositions, centerPoint, obstacles, placeableItems);
        }

        protected HashSet<Vector2Int> RunRandomWalk(SimpleLevelGeneratorData parameters, Vector2Int position)
        {
            var currentPosition = position;
            HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
            for (int i = 0; i < parameters.Iterations; i++)
            {
                var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.WalkLength);
                floorPositions.UnionWith(path);
                if (parameters.StartRandomlyEachIteration)
                    currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
            return floorPositions;
        }
    }
}