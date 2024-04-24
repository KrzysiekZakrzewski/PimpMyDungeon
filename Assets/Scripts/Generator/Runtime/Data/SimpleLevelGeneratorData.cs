using Generator.Item;
using Item;
using System.Collections.Generic;
using UnityEngine;

namespace Generator.Data
{
    [CreateAssetMenu(fileName = nameof(SimpleLevelGeneratorData), menuName = nameof(Generator) + "/" + nameof(Generator.Data) + "/" + nameof(SimpleLevelGeneratorData))]
    public class SimpleLevelGeneratorData : ScriptableObject
    {
        [field: SerializeField]
        public int Iterations { private set; get; } = 10;
        [field: SerializeField]
        public int WalkLength { private set; get; } = 10;
        [field: SerializeField]
        public bool StartRandomlyEachIteration { private set; get; } = true;
        [field: SerializeField]
        public List<ItemLevelGeneratorData> PlaceItem { private set; get; } = new List<ItemLevelGeneratorData>();
        [field: SerializeField]
        public ObstacleItemDatabase ObstacleItemDatabase { private set; get; }
    }
}