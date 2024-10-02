using UnityEngine;

namespace Tutorial.Implementation
{
    public abstract class TutorialStepGoalFactorySO : ScriptableObject, ITutorialStepGoalFactory
    {
        public abstract ITutorialStepGoalRuntimeLogic CreateQuestGoal();
    }
}