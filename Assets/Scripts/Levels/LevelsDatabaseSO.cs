using UnityEngine;

namespace Levels.Data
{
    [CreateAssetMenu(fileName = nameof(LevelsDatabaseSO), menuName = nameof(Levels) + "/" + nameof(Levels.Data) + "/" + nameof(LevelsDatabaseSO))]
    public class LevelsDatabaseSO : ScriptableObject
    {
        [SerializeField]
        private LevelDataSO[] levelsData;

        public LevelDataSO GetLevelData(int levelId)
        {
            return levelsData[levelId];
        }

        public int GetLevelsCount()
        {
            return levelsData.Length;
        }
    }
}