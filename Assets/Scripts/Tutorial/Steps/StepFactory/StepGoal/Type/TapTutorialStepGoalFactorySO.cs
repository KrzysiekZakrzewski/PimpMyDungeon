using Tutorial.Objectives;
using UnityEngine;

namespace Tutorial.Implementation
{
    [CreateAssetMenu(fileName = nameof(TapTutorialStepGoalFactorySO), menuName = nameof(Tutorial) + "/" + nameof(Tutorial.Data) + "/" + nameof(TapTutorialStepGoalFactorySO))]
    public class TapTutorialStepGoalFactorySO : TutorialStepGoalFactorySO
    {
        [SerializeField]
        private TutorialObjectiveDataSO tutorialObjectiveData;

        [SerializeField]
        private int tapAmountRequire;

        public override ITutorialStepGoalRuntimeLogic CreateQuestGoal()
        {
            return new TapTutorialStepGoal(tutorialObjectiveData, tapAmountRequire);
        }
    }
}