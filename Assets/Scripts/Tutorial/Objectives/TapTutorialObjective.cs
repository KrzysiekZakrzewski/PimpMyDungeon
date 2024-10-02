using UnityEngine;

namespace Tutorial.Objectives
{
    public class TapTutorialObjective : TutorialObjectiveBase
    {
        [SerializeField]
        private TutorialObjectiveDataSO tutorialObjectiveDataSO;

        public override ITutorialObjectiveData TutorialObjectiveData => tutorialObjectiveDataSO;
    }
}
