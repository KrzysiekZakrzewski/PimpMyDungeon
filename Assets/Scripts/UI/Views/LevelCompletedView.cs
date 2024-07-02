using Game.View;
using Levels;
using UnityEngine;
using UnityEngine.UI;
using ViewSystem;
using ViewSystem.Implementation;
using Zenject;

public class LevelCompletedView : BasicView
{
    [SerializeField]
    private Button nextLevelButton;
    [SerializeField]
    private Button mainMenuButton;
    [SerializeField]
    private Button restartLevelButton;

    [SerializeField]
    private Material noStarMaterial;

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

        nextLevelButton.onClick.AddListener(OnNextLevelPerformed);
        mainMenuButton.onClick.AddListener(OnMainMenuPerformed);
        restartLevelButton.onClick.AddListener(OnRestartLevelPerformed);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        nextLevelButton.onClick.RemoveAllListeners();
        mainMenuButton.onClick.RemoveAllListeners();
        restartLevelButton.onClick.RemoveAllListeners();
    }

    private void OnNextLevelPerformed()
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
