using UnityEngine;

namespace Ads.Data
{
    public abstract class AddDataBase : ScriptableObject
    {
        [field: SerializeField]
        public string AddAndroidId { private set; get; }
        [field: SerializeField]
        public string AddIOSId{ private set; get; }
    }
}
