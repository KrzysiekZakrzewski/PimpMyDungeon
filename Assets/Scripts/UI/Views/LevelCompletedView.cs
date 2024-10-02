using Levels;
using UnityEngine;
using ViewSystem;
using ViewSystem.Implementation;
using Zenject;
using DG.Tweening;
using UnityEngine.UI;

namespace Game.View
{
    public class LevelCompletedView : BasicView
    {
        [SerializeField]
        private CanvasGroup levelCompletedPanel;

        [SerializeField]
        private UIButton nextLevelButton;
        [SerializeField]
        private UIButton mainMenuButton;
        [SerializeField]
        private UIButton restartLevelButton;
        [SerializeField]
        private Image starIcon;

        [SerializeField]
        private Material noStarMaterial;

        private float levelPanelDelay = 0.5f;
        private LevelManager levelManager;

        public override bool Absolute => false;

        [Inject]
        private void Inject(LevelManager levelManager)
        {
            this.levelManager = levelManager;
        }

        protected override void Awake()
        {
            base.Awake();

            nextLevelButton.SetupButtonEvent(OnNextLevelPerformed);
            mainMenuButton.SetupButtonEvent(OnMainMenuPerformed);
            restartLevelButton.SetupButtonEvent(OnRestartLevelPerformed);

            levelCompletedPanel.interactable = false;
            levelCompletedPanel.alpha = 0f;

            //starIcon.material = noStarMaterial;
        }

        public override void NavigateTo(IAmViewStackItem previousViewStackItem)
        {
            base.NavigateTo(previousViewStackItem);

            levelCompletedPanel.DOFade(1f, levelPanelDelay).SetDelay(levelPanelDelay).OnComplete(() => levelCompletedPanel.interactable = true);
        }

        public override void NavigateFrom(IAmViewStackItem nextViewStackItem)
        {
            base.NavigateFrom(nextViewStackItem);

            levelCompletedPanel.interactable = false;

            levelCompletedPanel.DOFade(0f, levelPanelDelay);
        }

        private void OnNextLevelPerformed()
        {
            if (levelManager.ShowAddBeforeNextLevel(OnNextLevel))
                return;

            OnNextLevel();
        }

        private void OnNextLevel()
        {
            ParentStack.TryPopSafe();

            levelManager.LoadNextLevel();
        }

        private void OnMainMenuPerformed()
        {
            ParentStack.Pop();

            ParentStack.Pop();

            levelManager.BackToMainMenu();
        }

        private void OnRestartLevelPerformed()
        {
            ParentStack.TryPopSafe();

            levelManager.RestartLevel();
        }
    }
}