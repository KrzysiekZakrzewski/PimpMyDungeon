using System.Collections;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Animation
{
    public class AnimationController : MonoBehaviour
    {
        [SerializeField]
        private bool onlyOneAnimationLoop;
        [SerializeField]
        private bool randomizeAnimation;

        private readonly float minWaitTime = 2f;
        private readonly float maxWaitTime = 5f;

        private readonly float minStartWaitTime = 0.1f;
        private readonly float maxStartWaitTime = 1.7f;

        private Animator animator;
        private string[] clips;

        private void Awake()
        {
            Setup();
        }

        private void OnEnable()
        {
            StartCoroutine(StartAnimation());
        }

        private void Setup()
        {
            animator = GetComponent<Animator>();

            GetAllClips();
        }

        private void GetAllClips()
        {
            if (animator != null)
            {
                clips = animator
                    .runtimeAnimatorController
                    .animationClips
                    .Select(x => x.name)
                    .ToArray();
            }

            if (clips.Length <= 1)
            {
                animator.enabled = false;
                return;
            }

            var listClips = clips.ToList();

            listClips.Remove("Idle");

            clips = listClips.ToArray();
        }

        private string GetRandomClip()
        {
            return clips[Random.Range(0, clips.Length)];
        }

        private float GetRandomWaitTime()
        {
            return Random.Range(minWaitTime, maxWaitTime);
        }

        private IEnumerator StartAnimation()
        {
            yield return new WaitForSeconds(Random.Range(minStartWaitTime, maxStartWaitTime));

            if(onlyOneAnimationLoop)
            {
                animator.enabled = true;
                yield break;
            }

            if (!randomizeAnimation)
                yield break;

            StartCoroutine(AnimationSequnce());
        }

        private IEnumerator AnimationSequnce()
        {
            yield return new WaitForSeconds(GetRandomWaitTime());

            animator.Play(GetRandomClip());

            var info = animator.GetCurrentAnimatorClipInfo(0);

            yield return new WaitForSeconds(info[0].clip.length);

            StartCoroutine(AnimationSequnce());
        }
    }

    public enum AnimationType
    {
        None,
        OnlyOne,
        OnlyOneLoop,
        Randomize
    }
}