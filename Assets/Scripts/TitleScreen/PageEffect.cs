using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PageEffect : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject content;

    private ScrollRect scroll;
    private bool isDraging;

    private float pageWidthPercentage;
    private int pageAmount;

    private int targetPageIndex = 0;


    private void Awake()
    {
        scroll = GetComponent<ScrollRect>();
    }

    private void Start()
    {
        pageAmount = content.transform.childCount;
        //minus 1 here because there will be gaps between pages, these gaps should also get taken into account of the page size
        //or in other words, page 1 to page 5, user should swipe 4 times
        //and if don't minues 1 here, user should swipe 5 times, which is wrong
        pageWidthPercentage = 1f / (pageAmount - 1); 
    }

    private void Update()
    {
        if (!isDraging && scroll.horizontalNormalizedPosition != targetPageIndex * pageWidthPercentage)
        {
            scroll.horizontalNormalizedPosition = Mathf.Lerp(scroll.horizontalNormalizedPosition, targetPageIndex * pageWidthPercentage, 10 * Time.deltaTime);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDraging = false;

        float currentHorizontalPostionPercentage = scroll.horizontalNormalizedPosition;
        float closestPageDistancePercentage = Mathf.Abs(currentHorizontalPostionPercentage - 0);

        for (int i = 0; i < pageAmount; i++)
        {
            float distancePercentageToCurrentPage = Mathf.Abs(currentHorizontalPostionPercentage - i * pageWidthPercentage);

            if (distancePercentageToCurrentPage <= closestPageDistancePercentage)
            {
                closestPageDistancePercentage = distancePercentageToCurrentPage;
                targetPageIndex = i;
            }
        }
    }

}
