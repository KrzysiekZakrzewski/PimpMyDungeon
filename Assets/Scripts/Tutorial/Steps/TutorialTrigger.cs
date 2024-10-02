using System.Collections.Generic;
using Tutorial.Data;
using UnityEngine;
using Zenject;

namespace Tutorial
{
    public class TutorialTrigger : MonoBehaviour
    {
        [SerializeField]
        private List<DefaultTutorialStepDataSO> quests = new List<DefaultTutorialStepDataSO>();

        private TutorialManager tutorialManager;

        private Queue<DefaultTutorialStepDataSO> questQueue = new Queue<DefaultTutorialStepDataSO>();

        [Inject]
        private void Inject(TutorialManager tutorialManager)
        {
            this.tutorialManager = tutorialManager;
        }

        private void Awake()
        {
            LoadQuestQueue();
        }

        private void LoadQuestQueue()
        {
            if (quests == null || quests.Count == 0) return;

            foreach(DefaultTutorialStepDataSO quest in quests) 
            {
                //TO DO tu dodajemy weryfikacjê czy quest ju¿ zosta³ ukoñczony z save 
                //je¿eli zosta³ to go nie dodajemy

                questQueue.Enqueue(quest);
            }
        }

        private bool CheckCanTakeQuest()
        {
            if(questQueue.Count == 0) return false;

            return true;
        }

        public void TakeQuest()
        {
            if (!CheckCanTakeQuest()) return;

            var quest = questQueue.Dequeue();

           
        }
    }
}