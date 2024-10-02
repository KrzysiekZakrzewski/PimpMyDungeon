using UnityEngine;

namespace Tutorial.Objectives
{
    [CreateAssetMenu(fileName = nameof(TutorialObjectiveDataSO), menuName = nameof(Tutorial) + "/" + nameof(Tutorial.Objectives) + "/" + nameof(TutorialObjectiveDataSO))]
    public class TutorialObjectiveDataSO : TutorialObjectiveDataBase
    {
        [SerializeField]
        private int id;

        public override int ID => id;
    }
}
