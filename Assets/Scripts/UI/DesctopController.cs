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
    [Header("Desctop Info Properties")]
    [SerializeField] private List<Patient> patients;
    [SerializeField] private TMPro.TMP_Text infoText;
    [SerializeField] private Image patientImage;
    [SerializeField] private Image phobiaImage;

    [Space(20f)]
    [Header("Scrollview Properties")]
    [SerializeField] private RectTransform top;
    [SerializeField] private RectTransform bottom;
    [SerializeField] private Scrollbar scrollBar;
    [SerializeField] private RectTransform content;
    [SerializeField] private float scrollBarElasticity = 10f;
    [SerializeField] private float contentMaxYPos;

    #endregion

    #region Private Fields

    private int patientIndexHolder;
    private float contentTargetT;
    private Vector2 contentAnchordPosition;

    #endregion

    #region Unity Behaviour

    private void Awake()
    {
    }

    #endregion

    #region Public Methods

    public void ShowNextPatient(bool fromFirst)
    {
        if (fromFirst)
            patientIndexHolder = 0;

        scrollBar.value = 1f;

        Patient patient = patients[patientIndexHolder];
        infoText.text = patient.info;
        patientImage.sprite = patient.image;
        phobiaImage.sprite = patient.phobia.image;

        patientIndexHolder++;
        if (patientIndexHolder == patients.Count)
            patientIndexHolder = 0;
    }

    public void LeafThrough(int value)
    {
        patientIndexHolder += value;
        if (patientIndexHolder == patients.Count)
            patientIndexHolder = 0;
        if (patientIndexHolder < 0)
            patientIndexHolder = patients.Count - 1;
        scrollBar.value = 1f;

        Patient patient = patients[patientIndexHolder];
        infoText.text = patient.info;
        patientImage.sprite = patient.image;
        phobiaImage.sprite = patient.phobia.image;
    }

    public void OpenGlossary()
    {
        Patient patient = patients[patientIndexHolder];
        infoText.text = patient.glossary;
    }

    public void OpenInfo()
    {
        Patient patient = patients[patientIndexHolder];
        infoText.text = patient.info;
    }

    public void MoveScrollViewBy(float value)
    {
        float currentT = scrollBar.value;
        float currentHeightPos = Mathf.Lerp(0f,contentMaxYPos,currentT);
        float heightPos = Mathf.Clamp(currentHeightPos + value, 0f, contentMaxYPos);

        contentTargetT = Mathf.InverseLerp(0f, contentMaxYPos, heightPos);
        StartScrollContentByButton();
    }

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

    private void StartScrollContentByButton()
    {
        scrollContentByButton = true;
        if (IScrollContentByButtonHelper == null)
            IScrollContentByButtonHelper = StartCoroutine(IScrollContentByButton());
    }
    private void StopScrollContentByButton()
    {
        scrollContentByButton = false;
        if (IScrollContentByButtonHelper != null)
            StopCoroutine(IScrollContentByButtonHelper);
        IScrollContentByButtonHelper = null;
    }

    private void StartDragScrollView()
    {
        StopScrollContentByButton();

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
        StopScrollContentByButton();

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
            y *= 1080f / Screen.height;
            //Debug.Log(y);
            t = Mathf.InverseLerp(bottom.anchoredPosition.y, top.anchoredPosition.y, y);

            scrollBar.value = Mathf.Lerp(scrollBar.value, t, Time.fixedDeltaTime * scrollBarElasticity);

            yield return new WaitForFixedUpdate();
        }
        IScrollBarDragHelper = null;
    }


    private bool scrollContentByButton= false;
    private Coroutine IScrollContentByButtonHelper;
    private IEnumerator IScrollContentByButton()
    {
        while (scrollContentByButton)
        {
            scrollBar.value = Mathf.Lerp(scrollBar.value, contentTargetT, Time.fixedDeltaTime * scrollBarElasticity);

            yield return new WaitForFixedUpdate();
        }
        IScrollContentByButtonHelper = null;
    }

    #endregion
}
