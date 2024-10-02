using DG.Tweening;
using Item.Guide;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.View
{
    public class GuideNotification : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private TextMeshProUGUI itemNameTxt;
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private float duration = 0.33f;
        [SerializeField]
        private float showDuration = 2f;

        private ItemGuideController itemGuide;

        [Inject]
        private void Inject(ItemGuideController itemGuide)
        {
            this.itemGuide = itemGuide;
        }

        private void Awake()
        {
            itemGuide.OnItemUnlocked += SetupNotification;
        }

        private void OnDestroy()
        {
            itemGuide.OnItemUnlocked -= SetupNotification;
        }

        private IEnumerator WaitToHide()
        {
            yield return new WaitForSeconds(showDuration);

            HideNotification();
        }

        private void ShowNotification()
        {
            canvasGroup.DOFade(1f, duration).OnComplete(() => StartCoroutine(WaitToHide()));
        }

        private void HideNotification()
        {
            canvasGroup.DOFade(0f, duration);
        }

        public void SetupNotification(string itemName, Sprite itemIcon)
        {
            //itemNameTxt.text = itemName;
            itemImage.sprite = itemIcon;

            ShowNotification();
        }
    }
}