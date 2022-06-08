using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DesctopController : MonoBehaviour
{
    #region Public Fields
    #endregion

    #region Serialized Fields

    [SerializeField] private RectTransform top;
    [SerializeField] private RectTransform bottom;
    [SerializeField] private Scrollbar scrollBar;

    [SerializeField] private RectTransform content;

    [SerializeField] private float scrollBarElasticity = 10f;

    #endregion

    #region Private Fields

    Vector2 contentAnchordPosition;

    #endregion

    #region Unity Behaviour
    #endregion

    #region Public Methods

    public void OnScrollViewDragBegin()
    {
        StartDragScrollView();
    }

    public void OnScrollViewDragEnd()
    {
        StopDragScrollView();
    }

    public void OnScrollBarDragBegin()
    {
        StartDragScrollBar();
    }

    public void OnScrollBarDragEnd()
    {
        StopDragScrollBar();
    }

    #endregion

    #region Private Methods

    private void StartDragScrollView()
    {
        scrollViewDraging = true;
        if (IScrollViewDragHelper == null)
            IScrollViewDragHelper = StartCoroutine(IScrollViewDrag());
    }
    private void StopDragScrollView()
    {
        scrollViewDraging = false;
        if (IScrollViewDragHelper != null)
            StopCoroutine(IScrollViewDragHelper);
        IScrollViewDragHelper = null;
    }

    private void StartDragScrollBar()
    {
        scrollBarDraging = true;
        if (IScrollBarDragHelper == null)
            IScrollBarDragHelper = StartCoroutine(IScrollBarDrag());
    }
    private void StopDragScrollBar()
    {
        scrollBarDraging = false;
        if (IScrollBarDragHelper != null)
            StopCoroutine(IScrollBarDragHelper);
        IScrollBarDragHelper = null;
    }

    #endregion

    #region Coroutines

    private bool scrollViewDraging = false;
    private Coroutine IScrollViewDragHelper;
    private IEnumerator IScrollViewDrag()
    {

        Vector2 mousePrevPose = Input.mousePosition;
        yield return new WaitForFixedUpdate();
        Vector2 mousePose; 
        Vector2 deltaPos;
        while (scrollViewDraging)
        {
            mousePose = Input.mousePosition;
            deltaPos = mousePose - mousePrevPose;

            contentAnchordPosition = content.anchoredPosition;
            contentAnchordPosition.y += deltaPos.y;

            content.anchoredPosition = contentAnchordPosition;

            yield return new WaitForFixedUpdate();
            mousePrevPose = mousePose;
        }
        IScrollViewDragHelper = null;
    }

    private bool scrollBarDraging = false;
    private Coroutine IScrollBarDragHelper;
    private IEnumerator IScrollBarDrag()
    {
        float y, t;

        while (scrollBarDraging)
        {
            y = Input.mousePosition.y;
            t = Mathf.InverseLerp(top.anchoredPosition.y, bottom.anchoredPosition.y, y);

            scrollBar.value = Mathf.Lerp(scrollBar.value, t, Time.fixedDeltaTime * scrollBarElasticity);

            yield return new WaitForFixedUpdate();
        }
        IScrollBarDragHelper = null;
    }

    #endregion
}
