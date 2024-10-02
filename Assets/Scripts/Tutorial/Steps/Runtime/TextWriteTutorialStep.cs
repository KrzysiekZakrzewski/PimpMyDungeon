using Tutorial.Data;

namespace Tutorial.Implementation
{
    public class TextWriteTutorialStep : TutorialStepBase
    {
        private TextWriteStepFactorySO initialData;

        public override int Id => initialData.Id;

        public TextWriteTutorialStep(TextWriteStepFactorySO initialData)
        {
            this.initialData = initialData;

            foreach (ITutorialStepGoalFactory questGoalFactory in initialData.GoalsFactories)
            {
                Goals.Add(questGoalFactory.CreateQuestGoal());
            }
        }
    }
}