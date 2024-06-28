using Animation.DoTween;
using Levels;
using System;
using ViewSystem;
using ViewSystem.Implementation;
using ViewSystem.Implementation.ViewPresentations;

public class ScrollLevelView : BasicView
{
    private LevelButton[] pageButtons;
    private SwipeController swipeController;
    private LevelLoadAnimation loadAnimation;

    private LevelManager levelManager;

    public override bool Absolute => true;

    public void Init(SwipeController swipeController, LevelManager levelManager, Action<IAmViewPresentation> action)
    {
        loadAnimation = GetComponent<LevelLoadAnimation>();

        this.swipeController = swipeController;
        this.levelManager = levelManager;

        Presentation.OnShowPresentationComplete += action;

        pageButtons = GetComponentsInChildren<LevelButton>();

        for (int i = 0; i < pageButtons.Length; i++)
        {
            pageButtons[i].SetupButton(LoadLevel);
        }
    }

    private void LoadLevel(int levelId)
    {
        levelManager.LoadLevel(levelId, loadAnimation);
    }

    private void RefreshButtons()
    {
        var levelId = swipeController.PageId * SwipeController.ButtonsPerPage;

        for (int i = 0; i < pageButtons.Length; i++)
        {
            pageButtons[i].RefreshButton(levelId + i, levelManager);
        }
    }

    public override void NavigateTo(IAmViewStackItem previousViewStackItem)
    {
        base.NavigateTo(previousViewStackItem);

        RefreshButtons();
    }

    public void SwipePageToLeft()
    {
        var slidePresentation = Presentation as DotweenSlideViewPresentation;

        slidePresentation.SetDirection(ViewSystem.Utils.DotweenViewAnimationUtil.Direction.Left);
    }

    public void SwipePageToRight() 
    {
        var slidePresentation = Presentation as DotweenSlideViewPresentation;

        slidePresentation.SetDirection(ViewSystem.Utils.DotweenViewAnimationUtil.Direction.Right);
    }
}
