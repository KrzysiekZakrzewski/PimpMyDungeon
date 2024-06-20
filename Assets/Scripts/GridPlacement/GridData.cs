using System.Collections.Generic;
using UnityEngine;

namespace GridPlacement
{
    [System.Serializable]
    public class GridData
    {
        private readonly HashSet<Vector2Int> emptyPositions;
        private readonly Dictionary<Vector2Int, HashSet<Vector2Int>> occupiedPositions;

        public GridData(HashSet<Vector2Int> floorPositions, IEnumerable<Vector2Int> obstaclesPostition)
        {
            emptyPositions = new HashSet<Vector2Int>(floorPositions);
            emptyPositions.ExceptWith(obstaclesPostition);

            occupiedPositions = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
        }

        private Vector2Int GetPositionToCheck(Vector2Int originPosition, List<Vector2Int> itemPoints, int rotationId, int i)
        {
            return originPosition + PositionCalculator.CalculateGridRotation(itemPoints[i], rotationId);
        }

        public void PlaceObject(Vector2Int originPosition, List<Vector2Int> itemPoints, int rotationId)
        {
            HashSet<Vector2Int> positions = new();

            for (int i = 0; i < itemPoints.Count; i++)
            {
                Vector2Int positionToCheck = GetPositionToCheck(originPosition, itemPoints, rotationId, i);

                positions.Add(positionToCheck);
            }

            emptyPositions.ExceptWith(positions);

            occupiedPositions.Add(originPosition, positions);
        }

        public void RemoveObject(Vector2Int originPosition)
        {
            occupiedPositions.TryGetValue(originPosition, out HashSet<Vector2Int> positions);

            emptyPositions.UnionWith(positions);

            occupiedPositions.Remove(originPosition);
        }

        public bool CheckValidation(Vector2Int originPosition, List<Vector2Int> itemPoints, int rotationId)
        {
            for (int i = 0; i < itemPoints.Count; i++)
            {
                Vector2Int positionToCheck = GetPositionToCheck(originPosition, itemPoints, rotationId, i);

                if (!emptyPositions.Contains(positionToCheck))
                    return false;
            }

            return true;
        }

        public bool IsGridFilled()
        {
            return emptyPositions.Count == 0;
        }
    }
}
