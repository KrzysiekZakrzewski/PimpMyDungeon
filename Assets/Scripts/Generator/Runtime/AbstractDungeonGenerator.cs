using Levels.Data;
using UnityEngine;

namespace Generator
{
    public abstract class AbstractDungeonGenerator : MonoBehaviour
    {
        [SerializeField]
        protected TilemapVisualizer tilemapVisualizer = null;
        [SerializeField]
        protected ItemVisualizer itemVisualizer = null;

        protected Vector2Int startPosition = Vector2Int.zero;

        public LevelDataSO GenerateDungeon()
        {
            tilemapVisualizer.Clear();
            return RunProceduralGeneration();
        }

        protected abstract LevelDataSO RunProceduralGeneration();
    }
}