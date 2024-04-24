using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Item
{
    [CreateAssetMenu(fileName = nameof(ObstacleItemDatabase), menuName = nameof(Generator) + "/" + nameof(Generator.Item) + "/" + nameof(ObstacleItemDatabase))]
    public class ObstacleItemDatabase : ScriptableObject
    {
        [field: SerializeField]
        public List<ObstacleItemData> ObstacleItems { private set; get; }

        public void SortItem()
        {
            if(ObstacleItems == null || ObstacleItems.Count == 0) return;

            var sortedList = ObstacleItems.OrderBy(x => x.GetItemSizeArea()).ToList();
            sortedList.Reverse();

            ObstacleItems = sortedList;
        }
    }
}
