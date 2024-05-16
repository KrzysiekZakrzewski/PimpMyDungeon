using UnityEngine.EventSystems;

namespace MouseInteraction.Drag
{
    public interface IDragObject : IDragHandler, IEndDragHandler, IBeginDragHandler
    {
    }
}
