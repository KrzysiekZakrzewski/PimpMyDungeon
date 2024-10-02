using ViewSystem.Implementation;

namespace Game.View
{
    public class TutorialViewController : SingleViewTypeStackController
    {
        protected override void Awake()
        {
            base.Awake();

            TryOpenSafe<TutorialView>();
        }
    }
}