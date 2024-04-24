using Extensions;
using Generator.Data;
using GridPlacement;
using Item;
using Item.Obstacle;
using Levels.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

namespace Generator
{
    public class LevelBuilder : MonoBehaviour
    {
        [SerializeField]
        private LevelDataSO levelDataSO;

        [SerializeField]
        private Tilemap floorTilemap, wallTilemap;

        [SerializeField]
        private TileMapVisualizerDatabase database;

        [SerializeField]
        private Transform obstacleMagazine, itemMagazine;

        private HashSet<Vector2Int> floorInHash;
        private HashSet<Vector2Int> obstaclesInHash;
        private List<Bounds> objectsBounds;
        private Camera currentCamera;
        private GridData gridData;

        private void Awake()
        {
            currentCamera = Camera.main;

            objectsBounds = new List<Bounds>();
            objectsBounds.Clear();

            Vector3 cameraPosition = 
                new Vector3(levelDataSO.CenterPoint.x, 
                levelDataSO.CenterPoint.y,
                currentCamera.transform.position.z);

            currentCamera.transform.position = cameraPosition;

            StartCoroutine(BuildlevelSequence());
        }

        private void PaintSingleBasicWall(Vector2Int position, string binaryType)
        {
            int typeAsInt = Convert.ToInt32(binaryType, 2);
            TileBase tile = null;
            if (WallTypesHelper.wallTop.Contains(typeAsInt))
            {
                tile = database.WallTop;
            }
            else if (WallTypesHelper.wallSideRight.Contains(typeAsInt))
            {
                tile = database.WallSideRight;
            }
            else if (WallTypesHelper.wallSideLeft.Contains(typeAsInt))
            {
                tile = database.WallSiderLeft;
            }
            else if (WallTypesHelper.wallBottm.Contains(typeAsInt))
            {
                tile = database.WallBottom;
            }
            else if (WallTypesHelper.wallFull.Contains(typeAsInt))
            {
                tile = database.WallFull;
            }

            if (tile != null)
                PaintSingleTile(wallTilemap, tile, position);
        }

        private void PaintSingleCornerWall(Vector2Int position, string binaryType)
        {
            int typeASInt = Convert.ToInt32(binaryType, 2);
            TileBase tile = null;

            if (WallTypesHelper.wallInnerCornerDownLeft.Contains(typeASInt))
            {
                tile = database.WallInnerCornerDownLeft;
            }
            else if (WallTypesHelper.wallInnerCornerDownRight.Contains(typeASInt))
            {
                tile = database.WallInnerCornerDownRight;
            }
            else if (WallTypesHelper.wallDiagonalCornerDownLeft.Contains(typeASInt))
            {
                tile = database.WallDiagonalCornerDownLeft;
            }
            else if (WallTypesHelper.wallDiagonalCornerDownRight.Contains(typeASInt))
            {
                tile = database.WallDiagonalCornerDownRight;
            }
            else if (WallTypesHelper.wallDiagonalCornerUpRight.Contains(typeASInt))
            {
                tile = database.WallDiagonalCornerUpRight;
            }
            else if (WallTypesHelper.wallDiagonalCornerUpLeft.Contains(typeASInt))
            {
                tile = database.WallDiagonalCornerUpLeft;
            }
            else if (WallTypesHelper.wallFullEightDirections.Contains(typeASInt))
            {
                tile = database.WallFull;
            }
            else if (WallTypesHelper.wallBottmEightDirections.Contains(typeASInt))
            {
                tile = database.WallBottom;
            }

            if (tile != null)
                PaintSingleTile(wallTilemap, tile, position);
        }

        private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
        {
            var tilePosition = tilemap.WorldToCell((Vector3Int)position);
            tilemap.SetTile(tilePosition, tile);
        }

        private bool IsOccupiedPosition(Vector2 position)
        {
            foreach(Bounds bound in objectsBounds)
            {
                if(bound.Contains(position))
                    return true;
            }

            return false;
        }

        private Vector2 CalculatePositionForItem(Bounds objectsArea, Vector2Int size)
        {
            bool succesfull = false;

            Vector3Int offSetSize = new Vector3Int(size.x, size.y);

            Bounds levelBounds = new Bounds(wallTilemap.cellBounds.center, wallTilemap.cellBounds.size + offSetSize);

            Vector2 position = Vector2.zero;

            while (!succesfull)
            {
                position = RandomExtension.RandomPointInBounds(objectsArea);

                bool inLevelBounds = levelBounds.Contains(position);

                succesfull = !inLevelBounds && !IsOccupiedPosition(position);
            }

            Bounds objectBound = new Bounds(position, new Vector3(size.x, size.y, 0));

            objectsBounds.Add(objectBound);

            return position;
        }

        private void ResizeCamera(Bounds objectsArea)
        {
            var orthographicSize = objectsArea.size.x * Screen.height / Screen.width * 0.75f;

            currentCamera.orthographicSize = orthographicSize;
        }

        private void Clear()
        {
            floorTilemap.ClearAllTiles();
            wallTilemap.ClearAllTiles();
        }

        #region Sequences
        private IEnumerator BuildlevelSequence()
        {
            floorInHash = new HashSet<Vector2Int>(levelDataSO.FloorPositions);

            Clear();

            yield return new WaitForSeconds(1f);

            yield return StartCoroutine(PaintWalls());
            yield return StartCoroutine(PaintFloor());
            yield return StartCoroutine(SpawnObstacles());
            yield return StartCoroutine(SpawnPlaceableItem());

            gridData = new GridData(floorInHash, obstaclesInHash);

            PlacementSystem.Instance.SetupGridData(gridData);
        }

        private IEnumerator PaintFloor()
        {
            yield return StartCoroutine(PaintFloorTiles(floorInHash, floorTilemap, database.FloorTile));
        }

        private IEnumerator PaintWalls()
        {
            var basicWallPositions = WallGenerator.FindWallsInDirections(floorInHash, Direction2D.cardinalDirectionsList);
            var cornerWallPositions = WallGenerator.FindWallsInDirections(floorInHash, Direction2D.diagonalDirectionsList);

            StartCoroutine(CreateBasicWall(basicWallPositions));

            yield return StartCoroutine(CreateCornerWalls(cornerWallPositions));
        }

        private IEnumerator CreateCornerWalls(HashSet<Vector2Int> cornerWallPositions)
        {
            foreach (var position in cornerWallPositions)
            {
                string neighboursBinaryType = "";
                foreach (var direction in Direction2D.eightDirectionsList)
                {
                    var neighbourPosition = position + direction;
                    if (floorInHash.Contains(neighbourPosition))
                    {
                        neighboursBinaryType += "1";
                    }
                    else
                    {
                        neighboursBinaryType += "0";
                    }
                }
                PaintSingleCornerWall(position, neighboursBinaryType);

                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator CreateBasicWall(HashSet<Vector2Int> basicWallPositions)
        {
            foreach (var position in basicWallPositions)
            {
                string neighboursBinaryType = "";
                foreach (var direction in Direction2D.cardinalDirectionsList)
                {
                    var neighbourPosition = position + direction;
                    if (floorInHash.Contains(neighbourPosition))
                    {
                        neighboursBinaryType += "1";
                    }
                    else
                    {
                        neighboursBinaryType += "0";
                    }
                }
                PaintSingleBasicWall(position, neighboursBinaryType);

                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator PaintFloorTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
        {
            foreach (var position in positions)
            {
                PaintSingleTile(tilemap, tile, position);

                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator SpawnObstacles()
        {
            obstaclesInHash = new HashSet<Vector2Int>();

            var obstacles = levelDataSO.Obstacles;

            var arrayOfAllKeys = obstacles.Keys.ToArray();
            var arrayOfAllValues = obstacles.Values.ToArray();

            for(int i = 0; i < obstacles.Count; i++)
            {
                var obstacleData = arrayOfAllValues[i];

                var offSet = new Vector3((float)obstacleData.Size.x / 2, (float)obstacleData.Size.y / 2);

                Vector3 spawnPosition = new Vector3(arrayOfAllKeys[i].x, arrayOfAllKeys[i].y) + offSet;

                var newObject = Instantiate(obstacleData.Prefab, spawnPosition, Quaternion.identity, obstacleMagazine);

                for (int x = 0; x < obstacleData.Size.x; x++)
                {
                    for (int y = 0; y < obstacleData.Size.y; y++)
                    {
                        obstaclesInHash.Add(arrayOfAllKeys[i] + new Vector2Int(x, y));
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator SpawnPlaceableItem()
        {
            Vector3Int levelResize = new Vector3Int(wallTilemap.cellBounds.size.x / 2, 0);

            Bounds objectsArea = new Bounds(wallTilemap.cellBounds.center, wallTilemap.cellBounds.size + levelResize);

            foreach (var item in levelDataSO.PlaceableItems)
            {
                var position = CalculatePositionForItem(objectsArea, item.Value.Size);

                var newObject = Instantiate(item.Value.Prefab, position, Quaternion.identity, itemMagazine);

                var newItem = newObject.GetComponent<ItemBase>();

                newItem.Setup(item.Value);

                yield return new WaitForSeconds(0.1f);
            }

            ResizeCamera(objectsArea);
        }
        #endregion
    } 
}
