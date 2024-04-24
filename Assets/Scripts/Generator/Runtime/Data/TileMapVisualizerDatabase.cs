using Levels.Data;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Generator.Data
{
    [CreateAssetMenu(fileName = nameof(TileMapVisualizerDatabase), menuName = nameof(Generator) + "/" + nameof(Generator.Data) + "/" + nameof(TileMapVisualizerDatabase))]
    public class TileMapVisualizerDatabase : ScriptableObject
    {
        [field: SerializeField]
        public TileBase FloorTile { private set; get; }
        [field: SerializeField]
        public TileBase WallTop { private set; get; }
        [field: SerializeField]
        public TileBase WallSideRight { private set; get; }
        [field: SerializeField]
        public TileBase WallSiderLeft { private set; get; }
        [field: SerializeField]
        public TileBase WallBottom { private set; get; }
        [field: SerializeField]
        public TileBase WallFull { private set; get; }
        [field: SerializeField]
        public TileBase WallInnerCornerDownLeft { private set; get; }
        [field: SerializeField]
        public TileBase WallInnerCornerDownRight { private set; get; }
        [field: SerializeField]
        public TileBase WallDiagonalCornerDownRight { private set; get; }
        [field: SerializeField]
        public TileBase WallDiagonalCornerUpRight { private set; get; }
        [field: SerializeField]
        public TileBase WallDiagonalCornerDownLeft { private set; get; }
        [field: SerializeField]
        public TileBase WallDiagonalCornerUpLeft { private set; get; }
    }
}
