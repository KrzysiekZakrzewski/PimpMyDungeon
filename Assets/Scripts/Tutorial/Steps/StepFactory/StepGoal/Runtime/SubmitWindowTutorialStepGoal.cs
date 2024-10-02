using Item;
using Tutorial.Implementation;
using Tutorial.Objectives;
using UnityEngine;

namespace Tutorial
{
    public class SubmitWindowTutorialStepGoal : TutorialStepGoalBase
    {
        private GameObject windowPrefab;
        private ItemData onInteractableItem;

        public SubmitWindowTutorialStepGoal(SubmitWindowTutorialStepGoalFactorySO data)
        {
            windowPrefab = data.WindowObject;
            onInteractableItem = data.OnInteractableItem;
        }

        protected override void Start(ITutorialStepRuntimeLogic tutorialStep)
        {
            base.Start(tutorialStep);

            tutorialStep.TutorialManager.SpawnTutorialWindow(windowPrefab, onInteractableItem);
        }

        protected override bool Update(ITutorialStepRuntimeLogic tutorialStep, ITutorialObjective tutorialObjective)
        {
            return true;
        }
    }
}
