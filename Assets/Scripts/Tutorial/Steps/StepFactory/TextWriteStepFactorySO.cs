using System.Collections.Generic;
using Tutorial.Implementation;
using UnityEngine;

namespace Tutorial.Data
{
    [CreateAssetMenu(fileName = nameof(TextWriteStepFactorySO), menuName = nameof(Tutorial) + "/" + nameof(Tutorial.Data) + "/" + nameof(TextWriteStepFactorySO))]
    public class TextWriteStepFactorySO : TutorialStepFactorySO
    {
        [field: SerializeField]
        public int Id { get; private set; }

        [field: SerializeField]
        public List<TutorialStepGoalFactorySO> GoalsFactories { get; private set; }

        public override ITutorialStepRuntimeLogic CreateTutorialStep()
        {
            return new TextWriteTutorialStep(this);
        }
    }
}