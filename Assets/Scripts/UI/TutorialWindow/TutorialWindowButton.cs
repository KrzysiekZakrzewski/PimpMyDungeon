using System;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial.UI
{
    public class TutorialWindowButton : TutorialWindowBase
    {
        [SerializeField]
        private Button submitButton;

        public override void Init(Action hideEvent)
        {
            base.Init(hideEvent);

            submitButton.onClick.AddListener(Hide);
        }
    }
}