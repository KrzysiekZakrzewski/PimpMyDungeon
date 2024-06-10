
using ViewSystem;
using ViewSystem.Implementation;

namespace Game.View
{
    public class CreditsView : BasicView
    {
        public override bool Absolute => false;

        protected override void Awake()
        {
            base.Awake();
        }

        private void SettingsButton_OnPerformed()
        {
            ParentStack.TryPopSafe();
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