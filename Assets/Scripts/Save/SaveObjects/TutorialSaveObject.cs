using UnityEngine;

namespace Saves.Object
{
    [CreateAssetMenu(fileName = nameof(TutorialSaveObject), menuName = nameof(Saves) + "/" + "Assets/Objects" + "/" + nameof(TutorialSaveObject))]
    public class TutorialSaveObject : SaveObject
    {
        public SaveValue<bool> TutorialFinished = new(SaveKeyUtilities.TutorialKey);
    }
}
