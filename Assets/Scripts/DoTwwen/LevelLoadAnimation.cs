using UnityEngine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;

namespace Animation.DoTween
{
    public class LevelLoadAnimation : MonoBehaviour
    {
        [SerializeField]
        private Image background;
        [SerializeField] 
        private RectTransform title;
        [SerializeField]
        private RectTransform[] extraItemToAnimate;

        [SerializeField]
        private float minDaleyBetween, maxDaleyBetween;
        [SerializeField]
        private float minRotateAngle, maxRotateAngle;
        [SerializeField]
        private float shakeDuration;
        [SerializeField]
        private float moveDuration = 0.5f;
        [SerializeField]
        private float scaleTarget;

        private List<int> itemsId;
        private List<RectTransform> items;
        private Action endAction;

        private void Awake()
        {
            items = new();

            for (int i = 0; i < transform.childCount; i++)
                items.Add(transform.GetChild(i).GetComponent<RectTransform>());

            for (int i = 0; i < extraItemToAnimate.Length; i++)
                items.Add(extraItemToAnimate[i]);
        }

        private Sequence StartSingleSequence(RectTransform item)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.AppendInterval(Random.Range(minDaleyBetween, maxDaleyBetween));

            sequence.Append(item.DOShakeScale(shakeDuration));

            var angle = new Vector3(0, 0, Random.Range(minRotateAngle, maxRotateAngle));

            sequence.Append(item.DORotate(angle, moveDuration));

            sequence.Append(item.DOAnchorPosY(item.anchoredPosition.y - Screen.height, moveDuration));

            return sequence;
        }

        private Sequence SequnceTitle()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.AppendInterval(Random.Range(minDaleyBetween, maxDaleyBetween));

            sequence.Append(title.DOShakeScale(shakeDuration));

            var angle = new Vector3(0, 0, Random.Range(minRotateAngle, maxRotateAngle));

            sequence.Append(title.DOAnchorPosY(title.anchoredPosition.y + Screen.height, moveDuration));

            return sequence;
        }

        private void StartSequnces()
        {
            Sequence loadSequence = DOTween.Sequence();

            itemsId = new List<int>();

            for(int j = 0; j < items.Count; j++)
                itemsId.Add(j);

            for (int i = 0; i < items.Count; i++)
            {
                int id = itemsId[Random.Range(0, itemsId.Count)];

                itemsId.Remove(id);

                var sequence = StartSingleSequence(items[id]);

                if (i != items.Count - 1)
                    continue;

                loadSequence.Append(sequence);
            }

            loadSequence.Append(SequnceTitle());

            loadSequence.Append(background.rectTransform.DOScale(new Vector3(scaleTarget, scaleTarget, scaleTarget), moveDuration));

            loadSequence.Append(background.DOColor(Color.black, moveDuration));

            loadSequence.OnComplete(() => endAction?.Invoke());
        }

        public void PlayAnimation(Action endAction)
        {
            this.endAction = endAction;

            StartSequnces();
        }
    }
}
