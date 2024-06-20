using UnityEngine;
using Zenject;

namespace Levels
{
    public class MainMenuLevelManager : MonoBehaviour
    {
        private LevelManager levelManager;

        [Inject]
        private void Inject(LevelManager levelManager)
        {
            this.levelManager = levelManager;
        }
    }
}