using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField]
    private int maxPages;
    [SerializeField]
    private Vector2 pageStep;
    [SerializeField]
    private RectTransform levelPagesRect;
    [SerializeField]
    private float tweenTime;
    [SerializeField]
    private Ease ease = Ease.OutCubic;

    private int currentPage;
    private Vector2 targetPosition;
    private float dragTreshould;

    private void Awake()
    {
        currentPage = 1;
        targetPosition = levelPagesRect.anchoredPosition;
        dragTreshould = Screen.width / 15;
    }

    private void Next()
    {
        if(currentPage < maxPages)
        {
            currentPage++;
            targetPosition += pageStep;
            MovePage();
        }
    }

    private void Prev()
    {
        if (currentPage > 1)
        {
            currentPage--;
            targetPosition -= pageStep;
            MovePage();
        }
    }

    private void MovePage()
    {
        levelPagesRect.DOAnchorPos(targetPosition, tweenTime).SetEase(ease);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragTreshould)
        {
            MovePage();
            return;
        }

        var direction = eventData.position.x > eventData.pressPosition.x;

        Action action = direction ? Next : Prev;

        action?.Invoke();
    }
}
