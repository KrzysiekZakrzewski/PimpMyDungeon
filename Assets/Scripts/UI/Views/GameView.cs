using DG.Tweening;
using Generator;
using Item.Guide;
using Levels;
using Tips;
using UnityEngine;
using UnityEngine.UI;
using ViewSystem;
using ViewSystem.Implementation;
using Zenject;

namespace Game.View 
{
    public class GameView : BasicView
    {
        [SerializeField]
        private CanvasGroup skipPanel;
        [SerializeField]
        private Image skipImage;
        [SerializeField]
        private UIButton pauseButton;
        [SerializeField] 
        private UIButton tipButton;
        [SerializeField]
        private UIButton showSquareButton;
        [SerializeField]
        private LevelCompletedView levelCompletedView;
        [SerializeField]
        private PauseView pauseView;
        [SerializeField]
        private GuideNotification guideNotification;

        private readonly float showSkipPanelDuration = 0.5f;
        private LevelBuilder levelBuilder;
        private TipManager tipManager;

        private RectTransform[] rects = new RectTransform[3];

        public override bool Absolute => false;

        [Inject]
        private void Inject(LevelBuilder levelBuilder, TipManager tipManager)
        {
            this.levelBuilder = levelBuilder;
            this.tipManager = tipManager;
        }

        protected override void Awake()
        {
            base.Awake();
            pauseButton.SetupButtonEvent(OnPausePerformend);
            tipButton.SetupButtonEvent(OnTipPerformed);
            showSquareButton.SetupButtonEvent(OnShowSquarePerformed);

            LevelManager.LevelCompletedEvent += ShowLevelCompleted;

            rects[0] = pauseButton.GetComponent<RectTransform>();
            rects[1] = tipButton.GetComponent<RectTransform>();
            rects[2] = showSquareButton.GetComponent<RectTransform>();

            levelBuilder.SetRects(rects);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            LevelManager.LevelCompletedEvent -= ShowLevelCompleted;
        }

        private void OnTipPerformed()
        {
            var adShowed = tipManager.TryGenerateTip();
        }

        private void OnShowSquarePerformed()
        {
            levelBuilder.ShowTipSquare();
        }

        private void OnPausePerformend()
        {
            ParentStack.TryPushSafe(pauseView);
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
        }

        public void ShowLevelCompleted()
        {
            ParentStack.TryPushSafe(levelCompletedView);
        }
    }
}