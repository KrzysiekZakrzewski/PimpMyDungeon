using Game.SceneLoader;
using UnityEngine;
using ViewSystem.Implementation;
using Zenject;

public class LevelSelectorView : BasicView
{
    [SerializeField]
    private SceneDataSo gameScene;

    public override bool Absolute => false;

    private MainMenuViewController mainMenuViewController;
    private SceneLoadManagers sceneLoadManagers;

    [Inject]
    private void Inject(MainMenuViewController mainMenuViewController, SceneLoadManagers sceneLoadManagers)
    {
        this.mainMenuViewController = mainMenuViewController;
        this.sceneLoadManagers = sceneLoadManagers;
    }

    private void LoadSelectedLevel(int id)
    {
        sceneLoadManagers.LoadLocation(gameScene);
    }
}
