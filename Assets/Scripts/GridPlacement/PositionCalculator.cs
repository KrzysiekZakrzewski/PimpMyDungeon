using System.Drawing;
using UnityEngine;

namespace GridPlacement
{
    public static class PositionCalculator
    {
        private static Vector3 CalculateOffSet(int rotationState, Vector2 size)
        {
            var offSet = new Vector3(size.x / 2, size.y / 2);

            float y;
            float x;
            float maxStep = 4f;
            float step;

            switch (rotationState)
            {
                case 0:
                    break;
                case 1:
                    offSet = new Vector3(offSet.y, -offSet.x + 1, offSet.z);
                    break;
                case 2:
                    step = offSet.y / 0.5f;
                    y = 1 - (step * 0.5f);
                    step = offSet.x / 0.5f;
                    step = maxStep - step;
                    x = -1 + (step * 0.5f);
                    offSet = new Vector3(x, y, offSet.z);
                    break;
                case 3:
                    step = offSet.y / 0.5f;
                    x = 1 - (step * 0.5f);
                    step = offSet.x / 0.5f;
                    y = step * 0.5f;
                    offSet = new Vector3(x, y, offSet.z);
                    break;
            }

            return offSet;
        }

        public static Vector3 CalculatePosition(Vector2 gridPosition, Vector2 size, int rotationState = 0)
        {
            var offSet = CalculateOffSet(rotationState, size);

            Vector3 spawnPosition = new Vector3(gridPosition.x, gridPosition.y) + offSet;

            return spawnPosition;
        }

        public static Vector2Int CalculateGridRotation(Vector2Int point, int rotationState)
        {
            Vector2Int newPoint = point;

            switch (rotationState)
            {
                case 0:
                    break;
                case 1:
                    newPoint = new Vector2Int(point.y, -point.x);
                    break;
                case 2:
                    newPoint = new Vector2Int(-point.x, -point.y);
                    break;
                case 3:
                    newPoint = new Vector2Int(-point.y, point.x);
                    break;
            }

            return newPoint;
        }
    }
}
