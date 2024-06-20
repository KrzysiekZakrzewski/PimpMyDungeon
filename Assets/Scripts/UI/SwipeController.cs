using Levels;
using UnityEngine;
using UnityEngine.UI;
using ViewSystem;
using ViewSystem.Implementation;
using Zenject;

public class SwipeController : SingleViewTypeStackController
{
    [SerializeField]
    private Button nextButton, prevButton;
    [SerializeField]
    private ScrollLevelLeftView leftPage;
    [SerializeField]
    private ScrollLevelRightView rightPage;

    private int lastPageId;
    private bool isMoving;

    private LevelManager levelManager;

    public int PageId { private set; get; }

    public static readonly int ButtonsPerPage = 10;

    [Inject]
    private void Inject(LevelManager levelManager)
    {
        this.levelManager = levelManager;
    }

    protected override void Awake()
    {
        base.Awake();

        Init();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        leftPage.Presentation.OnShowPresentationComplete -= MovingComplete;
        rightPage.Presentation.OnShowPresentationComplete -= MovingComplete;
    }

    private void Init()
    {
        leftPage.Init(this, levelManager, MovingComplete);
        rightPage.Init(this, levelManager, MovingComplete);

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PrevPage);

        var levelsCount = levelManager.GetLevelsCount();

        lastPageId = levelsCount / ButtonsPerPage;
    }

    private void MovingComplete(IAmViewPresentation presentation)
    {
        isMoving = false;
    }

    private void NextPage()
    {
        if (PageId == lastPageId)
            return;

        leftPage.SwipePageToLeft();
        rightPage.SwipePageToLeft();

        PageId++;

        SwipePage();
    }

    private void PrevPage()
    {
        if (PageId == 0)
            return;

        leftPage.SwipePageToRight();
        rightPage.SwipePageToRight();

        PageId--;

        SwipePage();
    }

    public void SwipePage()
    {
        if (isMoving)
            return;

        isMoving = true;

        var currentPage = Peek();

        if(currentPage == null)
        {
            Open<ScrollLevelRightView>();
            return;
        }

        System.Action<IAmViewParameters> nextPageAction = currentPage is ScrollLevelLeftView ? Open<ScrollLevelRightView> : Open<ScrollLevelLeftView>;

        nextPageAction?.Invoke(null);
    }
}
