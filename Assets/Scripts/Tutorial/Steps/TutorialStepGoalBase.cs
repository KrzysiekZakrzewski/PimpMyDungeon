using Tutorial.Objectives;

namespace Tutorial
{
    public abstract class TutorialStepGoalBase : ITutorialStepGoalRuntimeLogic
    {
        private bool isCompleted;
        private bool isStarted;

        public bool IsCompleted => isCompleted;

        public bool IsStarted => isStarted;
        
        protected virtual void Complete(ITutorialStepRuntimeLogic tutorialStep)
        {
            isCompleted = true;
        }

        protected virtual void Start(ITutorialStepRuntimeLogic tutorialStep)
        {
            isStarted = true;
        }

        protected abstract bool Update(ITutorialStepRuntimeLogic tutorialStep, ITutorialObjective tutorialObjective);

        public void CompleteGoal(ITutorialStepRuntimeLogic tutorialStep)
        {
            if (!isStarted || isCompleted)
                return;

            Complete(tutorialStep);
        }

        public void StartGoal(ITutorialStepRuntimeLogic tutorialStep)
        {
            if (isStarted || isCompleted)
                return;

            Start(tutorialStep);
        }

        public bool UpdateGoal(ITutorialStepRuntimeLogic tutorialStep, ITutorialObjective tutorialObjective)
        {
            if (!isStarted || isCompleted)
                return false;

            isCompleted = Update(tutorialStep, tutorialObjective);

            return isCompleted;
        }
    }
}