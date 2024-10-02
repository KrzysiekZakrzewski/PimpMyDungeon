using System.Collections.Generic;
using Tutorial.Objectives;
using UnityEngine;

namespace Tutorial.Implementation 
{
    public abstract class TutorialStepBase : ITutorialStepRuntimeLogic
    {
        public abstract int Id { get; }

        public List<ITutorialStepGoalRuntimeLogic> Goals { get; } = new List<ITutorialStepGoalRuntimeLogic>();

        public List<ITutorialStepReward> Rewards { get; } = new List<ITutorialStepReward>();

        public bool IsCompleted { get; private set; }

        public bool IsStarted { get; private set; }

        public int CurrentQuestGoalID { get; private set; }

        public TutorialManager TutorialManager { get; private set; }

        public void Inject(TutorialManager param)
        {
            TutorialManager = param;
        }

        public void CompleteStep()
        {
            if(IsCompleted || !IsStarted) return;

            IsCompleted = true;
        }

        public void StartStep()
        {
            if (IsCompleted || IsStarted) return;

            IsStarted = true;

            CurrentQuestGoalID = 0;

            Goals[CurrentQuestGoalID].StartGoal(this);
        }

        public void UpdateStep()
        {
            if (IsCompleted || !IsStarted) return;

            CurrentQuestGoalID++;

            if (CurrentQuestGoalID < Goals.Count)
            {
                Goals[CurrentQuestGoalID].StartGoal(this);
                return;
            }

            CompleteStep();
        }

        public bool UpdateStepGoals(ITutorialObjective tutorialObjective)
        {
            if (IsCompleted || !IsStarted)
                return false;

            var goalCompleted = Goals[CurrentQuestGoalID].UpdateGoal(this, tutorialObjective);

            if(goalCompleted)
                UpdateStep();

            return IsCompleted;
        }
    }
}