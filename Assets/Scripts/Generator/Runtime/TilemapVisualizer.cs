using Generator.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generator
{
    public class TilemapVisualizer : MonoBehaviour
    {
        [SerializeField]
        private Tilemap floorTilemap, wallTilemap;
        [SerializeField] 
        private TileMapVisualizerDatabase database;

        private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
        {
            foreach (var position in positions)
            {
                PaintSingleTile(tilemap, tile, position);
            }
        }

        internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
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

        private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
        {
            var tilePosition = tilemap.WorldToCell((Vector3Int)position);
            tilemap.SetTile(tilePosition, tile);
        }

        internal void PaintSingleCornerWall(Vector2Int position, string binaryType)
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

        public void Clear()
        {
            floorTilemap.ClearAllTiles();
            wallTilemap.ClearAllTiles();
        }

        public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
        {
            PaintTiles(floorPositions, floorTilemap, database.FloorTile);
        }

        public Vector2 GetCenterPoint()
        {
            Vector2 centerPoint = wallTilemap.cellBounds.center;

            return centerPoint;
        }
    }
}