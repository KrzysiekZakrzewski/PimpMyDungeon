using ViewSystem.Implementation;

namespace Game.View
{
    public class GameViewController : SingleViewTypeStackController
    {
        protected override void Awake()
        {
            base.Awake();

            TryOpenSafe<GameView>();
        }
    }
}