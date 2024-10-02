using Item;
using Item.Guide;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.View
{
    public class DiaryPage : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI nameTxt;
        [SerializeField]
        private Image icon;
        [SerializeField]
        private Image sizeIcon;
        [SerializeField]
        private TextMeshProUGUI descTxt;
        [SerializeField]
        private TextMeshProUGUI pageNumberTxt;
        [SerializeField]
        private Sprite lockedIcon;
        [SerializeField]
        private string lockedText;

        private void FillLockedItem()
        {
            icon.sprite = lockedIcon;
            sizeIcon.sprite = lockedIcon;
            nameTxt.text = lockedText;
            descTxt.text = lockedText;

            icon.transform.localScale = Vector3.one;
            sizeIcon.transform.localScale = Vector3.one;

            this.icon.SetNativeSize();
            this.sizeIcon.SetNativeSize();
        }

        private void FillUnlockedItem(PlaceItemData data)
        {
            icon.sprite = data.ItemIcon;
            sizeIcon.sprite = data.ItemSizeIcon;
            nameTxt.text = data.ItemName;
            descTxt.text = data.ItemDescription;

            var itemSize = data.CalculateSize();

            float calculateSize = (float)itemSize.x / 10;

            var iconSize = new Vector3(1 - calculateSize, 1 - calculateSize);

            icon.transform.localScale = iconSize;
            sizeIcon.transform.localScale = iconSize;

            this.icon.SetNativeSize();
            this.sizeIcon.SetNativeSize();
        }

        public void SetupPage(int id, ItemGuideController guideController)
        {
            var item = guideController.GetUnlockedItemById(id);

            var pageNumber = (id + 1).ToString();

            pageNumberTxt.text = pageNumber;

            if (item == null)
            {
                FillLockedItem();
                return;
            }

            FillUnlockedItem(item);
        }
    }
}
