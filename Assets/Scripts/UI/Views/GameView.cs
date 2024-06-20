using DG.Tweening;
using Generator;
using Levels;
using Saves;
using Saves.Object;
using UnityEngine;
using UnityEngine.UI;
using ViewSystem;
using ViewSystem.Implementation;

namespace Game.View 
{
    public class GameView : BasicView
    {
        [SerializeField]
        private CanvasGroup skipPanel;
        [SerializeField]
        private Image skipImage;
        [SerializeField]
        private Button pauseButton;
        [SerializeField]
        private LevelCompletedView levelCompletedView;
        [SerializeField]
        private PauseView pauseView;

        private readonly float showSkipPanelDuration = 0.5f;

        public override bool Absolute => false;

        protected override void Awake()
        {
            base.Awake();
            pauseButton.onClick.AddListener(OnPausePerformend);

            LevelManager.LevelCompletedEvent += ShowLevelCompleted;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            RemoveHoldActions();

            pauseButton.onClick.RemoveAllListeners();

            LevelManager.LevelCompletedEvent -= ShowLevelCompleted;
        }

        private void OnPausePerformend()
        {
            ParentStack.TryPushSafe(pauseView);
        }

        private void RemoveHoldActions()
        {
            LevelBuilder.StartHoldAction -= ShowSkipPanel;
            LevelBuilder.EndHoldAction -= HideSkipPanel;
            LevelBuilder.HeldUpdateAction -= UpdateSkipPanel;
        }

        private void ShowSkipPanel()
        {
            if (skipPanel == null)
                return;

            skipPanel.DOFade(1f, showSkipPanelDuration);
        }

        private void HideSkipPanel()
        {
            if (skipPanel == null)
                return;

            ResetSkipPanel();

            skipPanel.DOFade(0f, showSkipPanelDuration);
        }

        private void ResetSkipPanel(bool force = false)
        {
            if (skipPanel == null)
                return;

            if(force)
            {
                skipImage.fillAmount = 0f;
                return;
            }

            skipImage.DOFillAmount(0f, showSkipPanelDuration);
        }

        private void UpdateSkipPanel(float value)
        {
            if (skipPanel == null)
                return;

            skipImage.fillAmount = value;
        }

        protected override void Presentation_OnShowPresentationComplete(IAmViewPresentation presentation)
        {
            base.Presentation_OnShowPresentationComplete(presentation);

            ResetSkipPanel(true);

            LevelBuilder.StartHoldAction += ShowSkipPanel;
            LevelBuilder.EndHoldAction += HideSkipPanel;
            LevelBuilder.HeldUpdateAction += UpdateSkipPanel;
        }

        public void ShowLevelCompleted()
        {
            ParentStack.TryPushSafe(levelCompletedView);
        }
    }
}