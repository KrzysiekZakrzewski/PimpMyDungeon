using JetBrains.Annotations;
using Tutorial.Objectives;
using UnityEngine;

namespace Tutorial
{
    public class TapTutorialStepGoal : TutorialStepGoalBase
    {
        private TutorialObjectiveDataSO tutorialObjectiveDataSO;
        private int tapAmountRequire;
        private int tapCount;

        public TapTutorialStepGoal(TutorialObjectiveDataSO tutorialObjectiveDataSO, int tapAmountRequire) 
        { 
            this.tutorialObjectiveDataSO = tutorialObjectiveDataSO;
            this.tapAmountRequire = tapAmountRequire;

            tapCount = 0;
        }

        protected override void Start(ITutorialStepRuntimeLogic tutorialStep)
        {
            base.Start(tutorialStep);
        }

        protected override void Complete(ITutorialStepRuntimeLogic tutorialStep)
        {
            base.Complete(tutorialStep);
        }

        protected override bool Update(ITutorialStepRuntimeLogic tutorialStep, ITutorialObjective tutorialObjective)
        {
            if (tutorialObjectiveDataSO.ID != tutorialObjective.GetTutorialObjectiveId)
                return false;

            tapCount++;

            if (tapCount >= tapAmountRequire)
                return true;

            return false;
        }
    } 
}