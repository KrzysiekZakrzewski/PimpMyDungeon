using Item;
using UnityEngine;

namespace Tutorial.Implementation
{
    public class SubmitWindowTutorialStepGoalFactorySO : TutorialStepGoalFactorySO
    {
        [field: SerializeField]
        public GameObject WindowObject { private set; get; }

        [field: SerializeField]
        public ItemData OnInteractableItem { private set; get; }

        public override ITutorialStepGoalRuntimeLogic CreateQuestGoal()
        {
            return new SubmitWindowTutorialStepGoal(this);
        }
    }
}
