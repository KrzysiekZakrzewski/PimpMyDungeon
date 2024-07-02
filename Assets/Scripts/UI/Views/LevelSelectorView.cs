using UnityEngine;
using UnityEngine.UI;
using ViewSystem;
using ViewSystem.Implementation;

public class LevelSelectorView : BasicView
{
    [SerializeField]
    private SwipeController swipeController;
    [SerializeField]
    private Button backButton;

    public override bool Absolute => false;

    protected override void Awake()
    {
        base.Awake();
        backButton.onClick.AddListener(OnClickBackPerformed);
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
}
