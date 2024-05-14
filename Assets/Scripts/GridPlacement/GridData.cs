using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace GridPlacement
{
    [System.Serializable]
    public class GridData
    {
        private HashSet<Vector2Int> emptyPositions;
        private Dictionary<Vector2Int, HashSet<Vector2Int>> occupiedPositions;

        public GridData(HashSet<Vector2Int> floorPositions, IEnumerable<Vector2Int> obstaclesPostition)
        {
            emptyPositions = new HashSet<Vector2Int>(floorPositions);
            emptyPositions.ExceptWith(obstaclesPostition);

            occupiedPositions = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
        }

        public void PlaceObject(Vector2Int placePosition, Vector2 size)
        {
            HashSet<Vector2Int> positions = new();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var position = placePosition + new Vector2Int(x, y);

                    positions.Add(position);
                }
            }

            emptyPositions.ExceptWith(positions);

            occupiedPositions.Add(placePosition, positions);
        }

        public bool CheckValidation(Vector2Int placePos, Vector2 size)
        {
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var positionToCheck = placePos + new Vector2Int(x, y);

                    if(!emptyPositions.Contains(positionToCheck))
                        return false;
                }
            }

            return true;
        }
    }
}
