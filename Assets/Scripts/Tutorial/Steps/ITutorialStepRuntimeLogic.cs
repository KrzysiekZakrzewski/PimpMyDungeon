using Inject;
using System.Collections.Generic;
using Tutorial.Objectives;

namespace Tutorial
{
    public interface ITutorialStepRuntimeLogic : IInjectParam<TutorialManager>
    {
        int Id { get; }

        List<ITutorialStepGoalRuntimeLogic> Goals { get; }
        List<ITutorialStepReward> Rewards { get; }

        bool IsCompleted { get; }
        bool IsStarted { get; }
        int CurrentQuestGoalID { get; }

        TutorialManager TutorialManager { get; }

        void StartStep();
        void UpdateStep();
        bool UpdateStepGoals(ITutorialObjective tutorialObjective);
        void CompleteStep();
    }
}