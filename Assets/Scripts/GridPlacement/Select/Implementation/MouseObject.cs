using UnityEngine;
using UnityEngine.EventSystems;

namespace MouseInteraction
{
    public interface IMouseObject : IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {

    }
}
