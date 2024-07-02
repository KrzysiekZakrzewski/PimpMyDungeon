using Levels;
using UnityEngine;
using UnityEngine.UI;
using ViewSystem;
using ViewSystem.Implementation;
using Zenject;

public class PauseView : BasicView
{
    [SerializeField]
    private Button resumeButton;
    [SerializeField]
    private Button mainMenuButton;
    [SerializeField]
    private Button restartLevelButton;

    public override bool Absolute => false;

    private LevelManager levelManager;

    [Inject]
    private void Inject(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    protected override void Awake()
    {
        base.Awake();

        resumeButton.onClick.AddListener(OnResumePerformed);
        mainMenuButton.onClick.AddListener(OnMainMenuPerformed);
        restartLevelButton.onClick.AddListener(OnRestartLevelPerformed);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();

        resumeButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        restartLevelButton.onClick.RemoveAllListeners();
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

        levelManager.LoadNextLevel();
    }

    private void OnMainMenuPerformed()
    {
        ParentStack.TryPopSafe();

        levelManager.BackToMainMenu();
    }

    private void OnRestartLevelPerformed()
    {
        ParentStack.TryPopSafe();

        levelManager.RestartLevel();
    }
}