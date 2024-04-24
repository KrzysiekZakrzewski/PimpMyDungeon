using ViewSystem;
using ViewSystem.Implementation;
using Zenject;

namespace Game.View
{
    public class MainMenuView : BasicView
    {
        public override bool Absolute => false;

        private MainMenuViewController mainMenuViewController;

        [Inject]
        private void Inject(MainMenuViewController mainMenuViewController)
        {
            this.mainMenuViewController = mainMenuViewController;
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        private void SettingsButton_OnPerformed()
        {

        }

        private void PlayButton_OnPerformed()
        {
        }

        protected override void Presentation_OnShowPresentationComplete(IAmViewPresentation presentation)
        {
            base.Presentation_OnShowPresentationComplete(presentation);
        }

        public override void NavigateTo(IAmViewStackItem previousViewStackItem)
        {
            base.NavigateTo(previousViewStackItem);
        }

        public override void NavigateFrom(IAmViewStackItem nextViewStackItem)
        {
            base.NavigateFrom(nextViewStackItem);
        }
    }
}