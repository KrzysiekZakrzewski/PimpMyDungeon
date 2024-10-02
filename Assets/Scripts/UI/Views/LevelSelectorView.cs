using UnityEngine;
using ViewSystem;
using ViewSystem.Implementation;

public class LevelSelectorView : BasicView
{
    [SerializeField]
    private SwipeController swipeController;
    [SerializeField]
    private UIButton backButton;

    public override bool Absolute => false;

    protected override void Awake()
    {
        base.Awake();
        backButton.SetupButtonEvent(OnClickBackPerformed);
    }

    private void OnClickBackPerformed()
    {
        swipeController.Close();

        ParentStack.TryPopSafe();
    }

    public override void NavigateTo(IAmViewStackItem previousViewStackItem)
    {
        base.NavigateTo(previousViewStackItem);

        swipeController.SwipePage();
    }

    public void OffInteractable()
    {
        CanvasGroup.interactable = false;
    }
}
