using DG.Tweening;
using System;
using UnityEngine;

namespace Tutorial.UI
{
    public class TutorialWindowBase : MonoBehaviour, ITutorialWindow
    {
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private float showDuration;

        private event Action hideEvent;

        private void Awake()
        {
            canvasGroup.alpha = 0f;
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }

        private void ChangeInteractable(bool isOn)
        {
            if (canvasGroup == null)
                return;

            canvasGroup.interactable = isOn;
        }

        public virtual void Init(Action hideEvent)
        {
            ChangeInteractable(false);

            this.hideEvent = hideEvent;

            Show();
        }

        public void Show()
        {
            canvasGroup.DOFade(1f, showDuration).OnComplete(OnShowed);
        }

        public void Hide()
        {
            ChangeInteractable(false);

            canvasGroup.DOFade(0f, showDuration).OnComplete(OnHideed);
        }

        public virtual void OnShowed()
        {
            ChangeInteractable(true);
        }

        public virtual void OnHideed()
        {
            hideEvent?.Invoke();

            DestroySelf();
        }
    }
}