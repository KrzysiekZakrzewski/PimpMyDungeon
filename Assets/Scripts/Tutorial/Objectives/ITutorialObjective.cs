namespace Tutorial.Objectives
{
    public interface ITutorialObjective
    {
        ITutorialObjectiveData TutorialObjectiveData { get; }

        int GetTutorialObjectiveId => TutorialObjectiveData.ID;
        void UpdateTutorial();
    }
}