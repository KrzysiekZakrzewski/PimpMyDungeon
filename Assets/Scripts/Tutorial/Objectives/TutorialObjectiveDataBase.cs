using UnityEngine;

namespace Tutorial.Objectives
{
    public abstract class TutorialObjectiveDataBase : ScriptableObject, ITutorialObjectiveData
    {
        public abstract int ID { get; }
    }
}
