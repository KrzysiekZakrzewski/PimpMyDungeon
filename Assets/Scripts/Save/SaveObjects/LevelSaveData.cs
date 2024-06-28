namespace Saves.Object
{
    [System.Serializable]
    public class LevelSaveData
    {
        public bool isUnlocked;

        public bool isCompleted;

        public bool starReached;

        public LevelSaveData() 
        { 
            isUnlocked = false;
            isCompleted = false;
            starReached = false;
        }

        public void LevelCompleted(bool starReached)
        {
            isUnlocked = true;
            isCompleted = true;
            this.starReached = starReached;
        }
        public void UnlockLevel()
        {
            isUnlocked = true;
        }
    }
}
