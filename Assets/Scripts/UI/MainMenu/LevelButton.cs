using DG.Tweening;
using Levels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelButton : UIButton
{
    [SerializeField]
    private TextMeshProUGUI numberTxt;

    [SerializeField]
    private Sprite normalSprite;
    [SerializeField]
    private Sprite completedSprite;
    [SerializeField]
    private Image lockedImage;
    [SerializeField]
    private Image starImage;

    [SerializeField]
    private Material noStarReachedMaterial;

    private int levelId;

    private bool isUnlocked;
    private bool isCompleted;
    private RectTransform rectTransform;
    private Image levelButtonIcon;

    private void Awake()
    {
        levelButtonIcon = button.image;
        rectTransform = levelButtonIcon.rectTransform;
    }

    private void LockedAnimation()
    {
        rectTransform.DOKill();

        rectTransform.DOShakePosition(0.5f, 2f);
    }

    private void OnClickPerformed(UnityAction<int> buttonEvent)
    {
        if (!isUnlocked)
        {
            LockedAnimation();
            return;
        }

        buttonEvent?.Invoke(levelId);
    }

    private void CheckUnlocked(int levelId, LevelManager levelManager)
    {
        isUnlocked = levelManager.CheckLevelUnlocked(levelId);

        lockedImage.gameObject.SetActive(!isUnlocked);
    }

    private void CheckCompleted(int levelId, LevelManager levelManager)
    {
        isCompleted = levelManager.CheckLevelCompleted(levelId);

        levelButtonIcon.sprite = isCompleted ? completedSprite : normalSprite;
    }

    private void CheckStarReached(int levelId, LevelManager levelManager)
    {
        if (!isCompleted)
        {
            //starImage.material = noStarReachedMaterial;
            starImage.gameObject.SetActive(false);
            return;
        }

        
        if (!starImage.gameObject.activeSelf)
            starImage.gameObject.SetActive(true);

        //var starReached = levelManager.CheckStartReached(levelId);

        //starImage.material = starReached ? null : noStarReachedMaterial;
        
    }

    public void SetupButton(UnityAction<int> buttonEvent)
    {
        SetupButtonEvent(() => OnClickPerformed(buttonEvent));
    }

    public void RefreshButton(int levelId, LevelManager levelManager)
    {
        this.levelId = levelId;

        numberTxt.text = (levelId + 1).ToString();

        CheckUnlocked(levelId, levelManager);

        CheckCompleted(levelId, levelManager);

        CheckStarReached(levelId, levelManager);
    }
}
