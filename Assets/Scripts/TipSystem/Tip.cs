using UnityEngine;
using DG.Tweening;
using System;

namespace Tips
{
    public class Tip
    {
        private readonly SpriteRenderer spriteRenderer;
        private readonly float duration = 0.5f;
        private readonly int loops = 4;
        private Sequence sequence;
        private Vector3 previewMinScale = new(0.7f, 0.7f);
        private Vector3 previewMaxScale = new(0.9f, 0.9f);
        private event Action endEvent;

        public Tip(Vector2 tipPosition, int rotationID, GameObject itemObject, Sprite tipMaterial) 
        {
            //this.endEvent = endEvent;

            var rotation = new Vector3(0, 0, rotationID * -90);

            spriteRenderer = itemObject.GetComponent<SpriteRenderer>();

            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);

            spriteRenderer.sprite = tipMaterial;

            itemObject.transform.eulerAngles = rotation;
            itemObject.transform.position = tipPosition;

            CreateSequnce();
        }

        private void CreateSequnce()
        {
            sequence = DOTween.Sequence();

            sequence = DOTween.Sequence();
            sequence.Append(spriteRenderer.transform.DOScale(previewMaxScale, duration));
            sequence.Append(spriteRenderer.transform.DOScale(previewMinScale, duration));
            sequence.SetLoops(loops, LoopType.Yoyo).SetAutoKill(false).OnComplete(HideTip);

            sequence.Pause();
        }

        public void ShowTip()
        {
            if (sequence.IsPlaying())
            {
                sequence.Pause();
                sequence.Restart();
            }

            spriteRenderer.DOFade(0.33f, duration);

            sequence.Play();
        }

        public void HideTip()
        {
            sequence.Pause();

            sequence.Restart();

            spriteRenderer.DOFade(0, duration);
        }

        public void DestroyTip()
        {
            sequence.Kill();
        }

        public GameObject TipObject => spriteRenderer.gameObject;

        public bool IsShow() => sequence.IsPlaying();
    }
}
