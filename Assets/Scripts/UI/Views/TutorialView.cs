using ViewSystem.Implementation;
using UnityEngine;
using Zenject;
using Tutorial;
using Tutorial.UI;
using System;

namespace Game.View
{
    public class TutorialView : BasicView
    {
        [SerializeField]
        private RectTransform spawnedMagazine;

        private ITutorialWindow tutorialWindow;

        public override bool Absolute => false;

        [Inject]
        private void Inject(TutorialManager tutorialManager)
        {
            tutorialManager.InjectTutorialView(this);
        }

        public ITutorialWindow SpawnWindow(GameObject windowPrefab)
        {
            var windowGO = Instantiate(windowPrefab, spawnedMagazine);

            return windowGO.GetComponent<ITutorialWindow>();
        }
    }
}