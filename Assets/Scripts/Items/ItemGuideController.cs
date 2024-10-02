using Saves;
using Saves.Object;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Item.Guide
{
    public class ItemGuideController : MonoBehaviour
    {
        [SerializeField]
        private List<PlaceItemData> allItems = new();

        private List<int> unlockedItemsId = new();

        private ItemGuideSaveObject itemGuideSaveObject;

        public event Action<string, Sprite> OnItemUnlocked;

        public bool InitializeFinished { get; private set; }

        public int ItemsAmount => allItems.Count;

        private void LoadSaves()
        {
            unlockedItemsId = itemGuideSaveObject.GetValue<List<int>>(SaveKeyUtilities.UnlockedItems).Value;

            InitializeFinished = true;
        }

        private void GetSaveObject()
        {
            SaveManager.TryGetSaveObject(out itemGuideSaveObject);
        }

        private bool IsUnlocked(int id)
        {
            return unlockedItemsId.Contains(id);
        }

        private void UnlockItem(int id)
        {
            unlockedItemsId.Add(id);

            PlaceItemData itemData = allItems.Find(x => x.Id == id);

            OnItemUnlocked?.Invoke(itemData.name, itemData.ItemIcon);
        }

        public bool TryUnlockItem(int id)
        {
            if (IsUnlocked(id))
                return false;

            UnlockItem(id);

            return true;
        }

        public PlaceItemData GetUnlockedItemById(int id)
        {
            if(!IsUnlocked(id)) 
                return null;

            return allItems.Find(x => x.Id == id);
        }

        public void Initialize()
        {
            GetSaveObject();

            LoadSaves();
        }
    }
}