using Inject;
using Tutorial.Objectives;

namespace Tutorial
{
    public interface ITutorialStepGoalRuntimeLogic
    {
        bool IsCompleted { get; }
        bool IsStarted { get; }

        void StartGoal(ITutorialStepRuntimeLogic tutorialStep);

        bool UpdateGoal(ITutorialStepRuntimeLogic tutorialStep, ITutorialObjective tutorialObjective);

        void CompleteGoal(ITutorialStepRuntimeLogic tutorialStep);
    }
}