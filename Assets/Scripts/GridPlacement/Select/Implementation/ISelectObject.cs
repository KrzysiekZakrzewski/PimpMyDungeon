using GridPlacement.EventData;

namespace MouseInteraction.Select
{
    public interface ISelectObject : IMouseObject
    {
        bool IsSelected { get; }
        void OnSelect(SelectEventData selectObject);
        void OnDeSelect(SelectEventData selectObject);
    }
}
