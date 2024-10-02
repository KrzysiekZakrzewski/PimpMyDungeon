using UnityEngine;

namespace Tutorial.Implementation
{
    public abstract class TutorialRewardFactorySO : ScriptableObject, ITutorialRewardFactory
    {
        public abstract ITutorialStepReward CreateReward();
    }
}