using MouseInteraction;
using System;

namespace Select
{
    public interface ISelect : IMouseDown
    {
        bool IsSelected { get; }

        void Select();
        void DeSelect();

        event Action<ISelect> OnSelected;
        event Action<ISelect> OnDeSelected;
    }
}
