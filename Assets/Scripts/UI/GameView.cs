using ViewSystem;
using ViewSystem.Implementation;

namespace Game.View 
{
    public class GameView : BasicView
    {
        public override bool Absolute => false;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            RemoveAllInputs();
        }

        private void RemoveAllInputs()
        {
        }

        private void PauseButton_OnPerformed()
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
            RemoveAllInputs();
        }
    }
}