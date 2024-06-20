namespace Saves.Object
{
    [System.Serializable]
    public class LevelSaveData
    {
        public int levelId;

        public bool starReached;

        public LevelSaveData(int levelId, bool starReached) 
        { 
            this.levelId = levelId;
            this.starReached = starReached;
        }
    }
}
