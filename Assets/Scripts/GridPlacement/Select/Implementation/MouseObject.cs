using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MouseInteraction
{
    public abstract class MouseObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        protected event Action OnPointerDownE;
        protected event Action OnPointerUpE;
        protected event Action OnPointerEnterE;
        protected event Action OnPointerExitE;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnPointerDownE?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterE?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitE?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnPointerUpE?.Invoke();
        }
    }
}
