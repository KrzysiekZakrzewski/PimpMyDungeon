using Levels;
using Tips;
using UnityEngine;
using ViewSystem;
using ViewSystem.Implementation;
using Zenject;

public class PauseView : BasicView
{
    [SerializeField]
    private UIButton resumeButton;
    [SerializeField]
    private UIButton mainMenuButton;
    [SerializeField]
    private UIButton restartLevelButton;
    [SerializeField]
    private UIButton tipGenerator;

    public override bool Absolute => false;

    private LevelManager levelManager;
    private TipManager tipManager;

    [Inject]
    private void Inject(LevelManager levelManager, TipManager tipManager)
    {
        this.levelManager = levelManager;
        this.tipManager = tipManager;
    }

    protected override void Awake()
    {
        base.Awake();

        resumeButton.SetupButtonEvent(OnResumePerformed);
        mainMenuButton.SetupButtonEvent(OnMainMenuPerformed);
        restartLevelButton.SetupButtonEvent(OnRestartLevelPerformed);
    }

    protected override void Presentation_OnHidePresentationComplete(IAmViewPresentation presentation)
    {
        base.Presentation_OnHidePresentationComplete(presentation);

        levelManager.UnPauseGame();
    }

    public override void NavigateTo(IAmViewStackItem previousViewStackItem)
    {
        base.NavigateTo(previousViewStackItem);

        levelManager.PauseGame();
    }

    private void OnResumePerformed()
    {
        ParentStack.TryPopSafe();
    }

    private void OnMainMenuPerformed()
    {
        ParentStack.Pop();

        ParentStack.Pop();

        levelManager.BackToMainMenu();
    }

    private void OnRestartLevelPerformed()
    {
        ParentStack.TryPopSafe();

        levelManager.RestartLevel();
    }
}