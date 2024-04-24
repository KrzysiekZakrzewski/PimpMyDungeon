using ViewSystem.Implementation;
using UnityEngine;
using System.Collections.Generic;
using Game.View;

public class MainMenuViewController : SingleViewTypeStackController
{
    [SerializeField]
    private List<BasicView> mainMenuViews;

    private void Start()
    {
        TryOpenSafe<MainMenuView>();
    }
}
