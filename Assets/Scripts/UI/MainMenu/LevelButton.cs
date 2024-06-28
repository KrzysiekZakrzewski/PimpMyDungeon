using Levels;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField]
    private Button levelButton;

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

    private int levelId;

    private void Awake()
    {
        levelButton = GetComponent<Button>();
    }

    public void SetupButton(UnityAction<int> buttonEvent)
    {
        levelButton.onClick.AddListener(() => buttonEvent?.Invoke(levelId));
    }

    public void RefreshButton(int levelId, LevelManager levelManager)
    {
        this.levelId = levelId;

        numberTxt.text = (levelId + 1).ToString();

        var isUnlocked = levelManager.CheckLevelUnlocked(levelId);

        lockedImage.gameObject.SetActive(!isUnlocked);

        if (!isUnlocked)
        {
            starImage.gameObject.SetActive(isUnlocked);
            return;
        }       

        var starReached = levelManager.CheckStartReached(levelId);

        starImage.gameObject.SetActive(starReached);
    }
}
