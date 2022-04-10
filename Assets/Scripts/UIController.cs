using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum ScreenPart
{
    PatientHand,
    Middle,
    Therapist
}

public class UIController : MonoBehaviour
{
    public static UIController instance;

    #region Serialized Fields 

    [Header("                The Main Game UIs            ")]
    [SerializeField]
    private GameObject cardPrefab;
    [SerializeField]
    private RectTransform therapistCardsParent;
    [SerializeField]
    private RectTransform patientCardsParent;
    [SerializeField]
    private Vector2 patientLowestPosOfCard;
    [SerializeField]
    private Vector2 patientHighestPosOfCard;
    [SerializeField]
    private Vector2 therapistLowestPosOfCard;
    [SerializeField]
    private Vector2 therapistHighestPosOfCard;

    [SerializeField]
    private Vector2 therapistCenteredCardPos;
    [SerializeField]
    private Vector2 patientCenteredCardPos;

    [Space(40f)]
    [Header("            Cards drag and drop        ")]
    //[SerializeField]
    //private RectTransform canvas;
    //[SerializeField]
    //private List<CanvasGroup> screenParts;
    [SerializeField]
    private List<float> screenPartBoundsOnX;

    #endregion

    #region Private Fields

    private List<RectTransform> therapistCards;
    private List<RectTransform> patientCards;

    #endregion

    #region Public Methods

    public ScreenPart GetScreenPart(Vector2 mousePosition)
    {
        float leftBound, rightBound;
        int index = 0; 
        for (int i = 0; i < screenPartBoundsOnX.Count; i++)
        {
            if (i == 0)
                leftBound = -1920f / 2f;
            else
                leftBound = screenPartBoundsOnX[i - 1];

            if (i == screenPartBoundsOnX.Count - 1)
                rightBound = 1920f / 2f;
            else
                rightBound = screenPartBoundsOnX[i];

            if (mousePosition.x < rightBound && mousePosition.x >= leftBound)
            {
                index = i;
                break;
            }
        }
        switch (index)
        {
            case 0:
                return ScreenPart.PatientHand;
            case 1:
                return ScreenPart.Middle;
            case 2:
                return ScreenPart.Therapist;
            default:
                return ScreenPart.Therapist;
        }
    }

    public void UpdateCards()
    {
        therapistCards.Clear();
        patientCards.Clear();
        for (int i = 0; i < therapistCardsParent.childCount; i++)
        {
            therapistCards.Add(therapistCardsParent.GetChild(i) as RectTransform);
            therapistCardsParent.GetChild(i).GetComponent<CardUI>().UpdateCard();
        }
        for (int i = 0; i < patientCardsParent.childCount; i++)
        {
            patientCards.Add(patientCardsParent.GetChild(i) as RectTransform);
            patientCardsParent.GetChild(i).GetComponent<CardUI>().UpdateCard();
        }
    }

    public void AddCardForTherapist(Card card,int index)
    {
        GameObject newCard = Instantiate(cardPrefab, therapistCardsParent);
        CardUI newCardUI = newCard.GetComponent<CardUI>();
        newCardUI.ApplyToCardGameObject(card);
        newCardUI.ApplyCardMetrics(index, therapistCenteredCardPos, therapistHighestPosOfCard, therapistLowestPosOfCard);
    }

    public void AddCardForPatient(Card card,int index)
    {
        GameObject newCard = Instantiate(cardPrefab, patientCardsParent);
        CardUI newCardUI = newCard.GetComponent<CardUI>();
        newCardUI.ApplyToCardGameObject(card);
        newCardUI.ApplyCardMetrics(index, patientCenteredCardPos, patientHighestPosOfCard, patientLowestPosOfCard);
    }

    //public void SetScreenPartActive(bool value)
    //{
    //    if (IScreenPartActiveHelper != null)
    //        StopCoroutine(IScreenPartActiveHelper);
    //    IScreenPartActiveHelper=StartCoroutine(IScreenPartActive(value));
    //}

    #endregion


    #region Private Methods

    //private Coroutine IScreenPartActiveHelper;

    //private IEnumerator IScreenPartActive(bool value)
    //{
    //    if (value)
    //    {
    //        while (screenParts[0].alpha< 9.99f)
    //        {
    //            foreach (var screenPart in screenParts)
    //            {
    //                screenPart.alpha = Mathf.Lerp(screenPart.alpha, 1f, Time.fixedDeltaTime * 8f);
    //            }

    //            yield return new WaitForFixedUpdate();
    //        }
    //    }
    //    else
    //    {
    //        while (screenParts[0].alpha> 0.01f)
    //        {
    //            foreach (var screenPart in screenParts)
    //            {
    //                screenPart.alpha = Mathf.Lerp(screenPart.alpha, 0f, Time.fixedDeltaTime * 8f);
    //            }

    //            yield return new WaitForFixedUpdate();
    //        }
    //    }
    //    IScreenPartActiveHelper = null;
    //}

    //private void PrepareScreenParts()
    //{
    //    float leftBound, rightBound, height, width;

    //    for (int i = 0; i < screenParts.Count; i++)
    //    {
    //        RectTransform rectOfPart = screenParts[i].GetComponent<RectTransform>();
    //        height = 1080f;
    //        if (i == 0)
    //            leftBound = -1920f / 2f;
    //        else
    //            leftBound = screenPartBoundsOnX[i - 1];

    //        if (i == screenParts.Count - 1) 
    //            rightBound = 1920f / 2f;
    //        else
    //            rightBound = screenPartBoundsOnX[i];

    //        width = rightBound - leftBound;

    //        rectOfPart.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    //        rectOfPart.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
    //        rectOfPart.anchoredPosition = new Vector2(rightBound - width / 2f, 0f);
    //    }
    //}

    #endregion


    #region Unity Monobehaviour

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        therapistCards = new List<RectTransform>();
        patientCards = new List<RectTransform>();

        //PrepareScreenParts();
    }

    #endregion
}
