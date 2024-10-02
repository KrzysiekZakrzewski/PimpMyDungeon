using Game.View;
using UnityEngine;
using Zenject;

public class TutorialInstaller : MonoInstaller
{
    [SerializeField]
    private TutorialView tutorialView;

    public override void InstallBindings()
    {
        Container.BindInstance(tutorialView).AsSingle();
    }
}
