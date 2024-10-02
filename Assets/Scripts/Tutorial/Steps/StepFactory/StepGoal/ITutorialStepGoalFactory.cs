using Zenject;

namespace Tutorial
{
    public interface ITutorialStepGoalFactory
    {
        ITutorialStepGoalRuntimeLogic CreateQuestGoal();
    }
}