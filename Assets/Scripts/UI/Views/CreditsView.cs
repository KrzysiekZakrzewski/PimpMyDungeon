using ViewSystem.Implementation;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class CreditsView : BasicView
    {
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
            ParentStack.TryPopSafe();
        }
    }
}