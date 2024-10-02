using Tutorial.Data;

namespace Tutorial.Implementation
{
    public class DefaultTutorialStep : TutorialStepBase
    {
        private DefaultTutorialStepDataSO initialData;

        public override int Id => initialData.Id;

        public DefaultTutorialStep(DefaultTutorialStepDataSO initialData)
        {
            this.initialData = initialData;

            foreach (ITutorialStepGoalFactory questGoalFactory in initialData.GoalsFactories)
            {
                Goals.Add(questGoalFactory.CreateQuestGoal());
            }

            foreach (ITutorialRewardFactory questRewardFactory in initialData.RewardsFactories)
            {
                Rewards.Add(questRewardFactory.CreateReward());
            }
        }
    }
}