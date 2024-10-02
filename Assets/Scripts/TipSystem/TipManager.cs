using Ads;
using Ads.Data;
using Generator;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

namespace Tips
{
    public class TipManager : MonoBehaviour
    {
        [SerializeField]
        private AddDataBase adData;
        [SerializeField]
        private GameObject tipPrefab;

        private List<Tip> tipsGenerated = new();

        private LevelBuilder levelBuilder;
        private AdsManager adsManager;

        private Coroutine coroutine;

        [Inject]
        private void Inject(LevelBuilder levelBuilder, AdsManager adsManager)
        {
            this.levelBuilder = levelBuilder;
            this.adsManager = adsManager;
        }

        private bool IsAllTipsGenerated()
        {
            return tipsGenerated.Count == levelBuilder.LevelItems.Count;
        }

        private IEnumerator TipGeneratedSequnce()
        {
            var levelItems = levelBuilder.LevelItems;

            if (tipsGenerated.Count == levelItems.Count)
                yield break;

            var tipItem = levelItems[tipsGenerated.Count];

            var tipObject = Instantiate(tipPrefab, levelBuilder.TipObjectsMagazine);

            Tip newTip = new(tipItem.SpawnPosition, tipItem.RotationID, tipObject, tipItem.PlaceItemData.ItemIcon);

            tipsGenerated.Add(newTip);

            yield return new WaitForSeconds(0.5f);

            ShowTips();
        }

        private void GenerateTip()
        {
            coroutine = StartCoroutine(TipGeneratedSequnce());
        }

        private void StopTipsSequnce()
        {
            if (coroutine == null)
                return;

            StopCoroutine(coroutine);

            coroutine = null;
        }

        public void ShowTips()
        {
            for (int i = 0; i < tipsGenerated.Count; i++)
                tipsGenerated[i].ShowTip();
        }

        public bool TryGenerateTip()
        {
            if (IsAllTipsGenerated())
            {
                ShowTips();
                return false;
            }

            var adsShowed = adsManager.ShowAd(adData, GenerateTip);

            return adsShowed;
        }

        public void ClearTips()
        {
            for (int i = 0; i < tipsGenerated.Count; i++)
            {
                tipsGenerated[i].DestroyTip();
                Destroy(tipsGenerated[i].TipObject);
            }

            tipsGenerated.Clear();
        }
    }
}