using UnityEngine;

namespace Tutorial.Implementation
{
    public abstract class TutorialStepFactorySO : ScriptableObject, ITutorialStepFactory
    {
        public abstract ITutorialStepRuntimeLogic CreateTutorialStep();
    }
}
