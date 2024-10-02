using Item.Guide;
using UnityEngine;
using ViewSystem;
using ViewSystem.Implementation;
using Zenject;

namespace Game.View
{
    public class DiaryView : BasicView
    {
        [SerializeField]
        private DiaryPage leftPage, rightPage;
        [SerializeField]
        private UIButton backButton;
        [SerializeField]
        private UIButton nextPageLeft, nextPageRight;

        private const int pageToView = 2;

        private ItemGuideController itemGuide;

        private int currentPageNumber;
        private int maxPageNumber;

        public override bool Absolute => false;

        [Inject]
        private void Inject(ItemGuideController itemGuide)
        {
            this.itemGuide = itemGuide;
        }

        protected override void Awake()
        {
            base.Awake();

            SetupButtons();

            maxPageNumber = itemGuide.ItemsAmount / 2;
        }

        private void SetupButtons()
        {
            backButton.SetupButtonEvent(OnBackPerformed);
            nextPageLeft.SetupButtonEvent(OnLeftArrowPerformed);
            nextPageRight.SetupButtonEvent(OnRightArrowPerformed);
        }

        private void OnBackPerformed()
        {
            ParentStack.TryPopSafe();
        }

        private void OnRightArrowPerformed()
        {
            if (currentPageNumber == maxPageNumber - 1)
                return;

            currentPageNumber++;

            SwipePage();
        }

        private void OnLeftArrowPerformed()
        {
            if (currentPageNumber <= 0)
                return;

            currentPageNumber--;

            SwipePage();
        }

        private void SwipePage()
        {
            int leftItemId = currentPageNumber * 2;
            int rightItemId = currentPageNumber * pageToView + 1;

            leftPage.SetupPage(leftItemId, itemGuide);
            rightPage.SetupPage(rightItemId, itemGuide);
        }

        public override void NavigateTo(IAmViewStackItem previousViewStackItem)
        {
            base.NavigateTo(previousViewStackItem);

            SwipePage();
        }
    }
}