using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private Vector2 lowestPosOfCard;
    [SerializeField]
    private Vector2 highestPosOfCard;

    [SerializeField]
    private Vector2 centeredCardPos;
    //[SerializeField]
    //private Vector3 cardMaxScale;
    //[SerializeField]
    //private Vector3 cardMinScale;

    [SerializeField]
    private float cardAnimationSpeed;

    #endregion

    #region Private Fields

    private List<RectTransform> therapistCards;
    private List<RectTransform> patientCards;

    #endregion

    #region Public Methods

    public void UpdateCards()
    {
        therapistCards.Clear();
        patientCards.Clear();
        for (int i = 0; i < therapistCardsParent.childCount; i++)
        {
            therapistCards.Add(therapistCardsParent.GetChild(i) as RectTransform);
        }
        for (int i = 0; i < patientCardsParent.childCount; i++)
        {
            patientCards.Add(patientCardsParent.GetChild(i) as RectTransform);
        }

        SetCardsPositions(therapistCards);
        SetCardsPositions(patientCards);

        SetCardsScales(therapistCards);
        SetCardsScales(patientCards);


        //need to work on this part
        //foreach (var card in therapistCards)
        //{
        //    EventTrigger trigger = card.GetComponent<EventTrigger>();
        //    EventTrigger.Entry entry = new EventTrigger.Entry();
        //    entry.eventID = EventTriggerType.PointerEnter;
        //    entry.callback.AddListener((data) => { OnPointerEnterDelegate((PointerEventData)data); });
        //    trigger.triggers.Add(entry);

        //    entry.eventID = EventTriggerType.PointerExit;
        //    entry.callback.AddListener((data) => { OnPointerExitDelegate((PointerEventData)data); });
        //    trigger.triggers.Add(entry);
        //}
    }

    public void AddCardForTherapist(Card card)
    {
        GameObject newCard = Instantiate(cardPrefab, therapistCardsParent);
        newCard.GetComponent<CardUI>().ApplyToCardGameObject(card);
    }

    #endregion


    #region Private Methods

    private void SetCardsPositions(List<RectTransform> cards)
    {
        float step = 1f / cards.Count;
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].anchoredPosition = Vector2.Lerp(highestPosOfCard, lowestPosOfCard, i * step);
        }
    }

    private void SetCardsScales(List<RectTransform> cards)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            //cards[i].localScale = cardMinScale;
        }
    }

    //private IEnumerator IAnimateZoomIn(RectTransform card)
    //{
    //    while (Vector2.Distance(card.anchoredPosition, centeredCardPos) > 0.002f||
    //        Vector3.Distance(card.localScale, cardMaxScale) >0.002f)
    //    {
    //        card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, centeredCardPos, Time.fixedDeltaTime * cardAnimationSpeed);
    //        card.localScale = Vector3.Lerp(card.localScale, cardMaxScale, Time.fixedDeltaTime * cardAnimationSpeed);

    //        yield return new WaitForFixedUpdate();
    //    }
    //}

    //private IEnumerator IAnimateZoomOut(RectTransform card)
    //{
    //    float step = 1f / therapistCards.Count;
    //    int indexer = 0;
    //    for (int i = 0; i < therapistCards.Count; i++)
    //    {
    //        if (therapistCards[i].gameObject==card.gameObject)
    //        {
    //            indexer = i;
    //            break;
    //        }
    //    }
    //    while (Vector2.Distance(card.anchoredPosition, Vector2.Lerp(highestPosOfCard, lowestPosOfCard, indexer * step)) > 0.002f ||
    //        Vector3.Distance(card.localScale, cardMinScale) > 0.002f)
    //    {
    //        card.anchoredPosition = Vector2.Lerp(card.anchoredPosition, Vector2.Lerp(highestPosOfCard, lowestPosOfCard, indexer * step), Time.fixedDeltaTime * cardAnimationSpeed);
    //        card.localScale = Vector3.Lerp(card.localScale, cardMinScale, Time.fixedDeltaTime * cardAnimationSpeed);

    //        yield return new WaitForFixedUpdate();
    //    }
    //}

    //private void OnPointerEnterDelegate(PointerEventData eventData)
    //{
    //    StopCoroutine(IAnimateZoomOut(eventData.pointerEnter.transform as RectTransform));
    //    StartCoroutine(IAnimateZoomIn(eventData.pointerEnter.transform as RectTransform));
    //}

    //private void OnPointerExitDelegate(PointerEventData eventData)
    //{
    //    StopCoroutine(IAnimateZoomIn(eventData.pointerEnter.transform as RectTransform));
    //    StartCoroutine(IAnimateZoomOut(eventData.pointerEnter.transform as RectTransform));
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

        //UpdateCards();
    }

    #endregion
}
