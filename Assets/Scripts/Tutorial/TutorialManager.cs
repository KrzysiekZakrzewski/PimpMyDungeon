using Saves.Object;
using Saves;
using UnityEngine;
using Generator;
using Levels.Data;
using Game.SceneLoader;
using Zenject;
using System.Collections.Generic;
using Tutorial.Implementation;
using Tutorial.Objectives;
using Levels;
using Game.View;
using Inject;
using Item;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField]
        private LevelBuilder levelBuilder;
        [SerializeField]
        private LevelDataSO tutorialLevel;

        [SerializeField]
        private TutorialStepFactorySO[] tutorialSteps;

        private Queue<ITutorialStepRuntimeLogic> tutorialDeclarationToRuntimeLogicLut;
        private ITutorialStepRuntimeLogic currentTutorialStep;
        private TutorialSaveObject tutorialSaveObject;
        private SceneLoadManagers sceneLoadManagers;
        private LevelManager levelManager;
        private TutorialView tutorialView;

        public bool IsFinished { private set; get; }
        public bool InitializeFinished { get; private set; }
        public bool IsPlaying { get; private set; }

        [Inject]
        private void Inject(SceneLoadManagers sceneLoadManagers, LevelManager levelManager)
        {
            this.sceneLoadManagers = sceneLoadManagers;
            this.levelManager = levelManager;
        }

        private void GetSaveObject()
        {
            SaveManager.TryGetSaveObject(out tutorialSaveObject);
        }

        private void LoadSaves()
        {
            GetSaveObject();

            IsFinished = tutorialSaveObject.GetValue<bool>(SaveKeyUtilities.TutorialKey).Value;

            InitializeFinished = true;
        }

        private void CreateTutorialQueue()
        {
            tutorialDeclarationToRuntimeLogicLut = new();

            foreach (TutorialStepFactorySO stepFactory in tutorialSteps)
            {
                var tutorialRuntime = stepFactory.CreateTutorialStep();

                tutorialRuntime.Inject(this);

                tutorialDeclarationToRuntimeLogicLut.Enqueue(tutorialRuntime);
            }
        }

        private void StartTutorial()
        {
            IsPlaying = true;

            NextTutorialStep();
        }

        private void CompleteStep()
        {
            NextTutorialStep();
        }

        private void NextTutorialStep()
        {
            if(tutorialDeclarationToRuntimeLogicLut.Count == 0)
            {
                TutorialFinished();
                return;
            }

            currentTutorialStep = tutorialDeclarationToRuntimeLogicLut.Dequeue();

            currentTutorialStep.StartStep();
        }

        private void TutorialFinished()
        {
            IsFinished = true;

            tutorialSaveObject.SetValue(SaveKeyUtilities.TutorialKey, IsFinished);

            levelManager.LoadLevel(0);

            IsPlaying = false;
        }

        public void SetupTutorial()
        {
            sceneLoadManagers.OnSceneChanged -= SetupTutorial;

            CreateTutorialQueue();

            levelBuilder.BuildLevel(tutorialLevel, StartTutorial);
        }

        public void Initialize()
        {
            GetSaveObject();

            LoadSaves();
        }

        public void UpdateTutorialStep(ITutorialObjective objective = null)
        {
            if (IsFinished || currentTutorialStep == null)
                return;

            bool isCompleted = currentTutorialStep.UpdateStepGoals(objective);

            if (!isCompleted)
                return;

            CompleteStep();
        }

        public void SpawnTutorialWindow(GameObject windowPrefab, ItemData onInteractableItems)
        {     
            var window = tutorialView.SpawnWindow(windowPrefab);

            levelBuilder.OffAllItemInteractable();

            window.Init(() => UpdateTutorialStep(null));

            if (onInteractableItems == null)
                return;

            var item = levelBuilder.GetItemByData(onInteractableItems);

            item.ChangeInteractableState(true);

            item.SubscribeOnPlacedEvent(
                () => 
                    {
                        window.Hide();
                        item.UnSubscribeOnPlacedEvent(window.Hide);
                    }
                );
        }

        public void InjectTutorialView(TutorialView tutorialView)
        {
            this.tutorialView = tutorialView;
        }
    }
}