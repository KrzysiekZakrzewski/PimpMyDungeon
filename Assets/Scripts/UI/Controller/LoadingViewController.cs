using ViewSystem.Implementation;
using UnityEngine;

namespace Game.View
{
    public class LoadingViewController : SingleViewTypeStackController
    {
        [SerializeField]
        private LoadingView loadingView;

        public bool IsShowPresentationComplete() 
        {
            return loadingView.IsShowPresentationComplete;
        }

        public void OpenLoadingView()
        {
            TryOpenSafe<LoadingView>();
        }

        public void OnLoadingCompleted()
        {
            loadingView.ShowContinueText();
        }

        public void CloseLoadingView()
        {
            loadingView.ParentStack.TryPopSafe();
        }
    }
}