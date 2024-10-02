using UnityEngine;
using Zenject;

namespace Tutorial.Objectives
{
    public abstract class TutorialObjectiveBase : MonoBehaviour, ITutorialObjective
    {
        private TutorialManager tutorialManager;

        public abstract ITutorialObjectiveData TutorialObjectiveData { get; }

        [Inject]
        protected void Inject(TutorialManager tutorialManager)
        {
            this.tutorialManager = tutorialManager;
        }

        public void UpdateTutorial()
        {
            tutorialManager.UpdateTutorialStep(this);
        }
    }
}
