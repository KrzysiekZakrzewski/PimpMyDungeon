using System.Collections.Generic;
using UnityEngine;

namespace Saves.Object
{
    [CreateAssetMenu(fileName = nameof(ItemGuideSaveObject), menuName = nameof(Saves) + "/" + "Assets/Objects" + "/" + nameof(ItemGuideSaveObject))]
    public class ItemGuideSaveObject : SaveObject
    {
        public SaveValue<List<int>> UnlockedItems = new(SaveKeyUtilities.UnlockedItems);
    }
}