using System;

namespace MouseInteraction
{
    public interface IMouseDown
    {
        bool MouseWasDown { get; }

        void MouseDown();
        void MouseUp();

        event Action<IMouseDown> OnMouseDown;
        event Action<IMouseDown> OnMouseUp;
    }
}