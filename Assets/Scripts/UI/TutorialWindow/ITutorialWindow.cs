using System;

namespace Tutorial.UI 
{
    public interface ITutorialWindow
    {
        void Init(Action hideEvent);
        void Show();
        void Hide();
        void OnShowed();
        void OnHideed();
    }
}