using UnityEngine;
using ViewSystem;
using ViewSystem.Implementation;

public class LevelSelectorView : BasicView
{
    [SerializeField]
    private SwipeController swipeController;

    public override bool Absolute => false;

    public override void NavigateTo(IAmViewStackItem previousViewStackItem)
    {
        base.NavigateTo(previousViewStackItem);

        swipeController.SwipePage();
    }
}
