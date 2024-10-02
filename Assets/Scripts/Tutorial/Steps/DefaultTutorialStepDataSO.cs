using System.Collections.Generic;
using Tutorial.Implementation;
using UnityEngine;

namespace Tutorial.Data
{
    [CreateAssetMenu(fileName = nameof(DefaultTutorialStepDataSO), menuName = nameof(Tutorial) + "/" + nameof(Tutorial.Data) + "/" + nameof(DefaultTutorialStepDataSO))]
    public class DefaultTutorialStepDataSO : TutorialStepFactorySO
    {
        [field: SerializeField]
        public int Id { get; private set; }
        [field: SerializeField]
        public string NameLocKey { get; private set; }

        [field: SerializeField]
        public string DescriptionLocKey { get; private set; }

        [field: SerializeField]
        public List<TutorialStepGoalFactorySO> GoalsFactories { get; private set; }

        [field: SerializeField]
        public List<TutorialRewardFactorySO> RewardsFactories { get; private set; }

        public override ITutorialStepRuntimeLogic CreateTutorialStep()
        {
            return new DefaultTutorialStep(this);
        }
    }
}