using MouseInteraction.Manager;

namespace GridPlacement.EventData
{
    public class SelectEventData : BaseEventData
    {
        private SelectManager selectManager;

        public SelectEventData(SelectManager selectManager)
        {
            this.selectManager = selectManager;
        }
    }
}
