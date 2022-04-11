using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum CardUIType 
{
    TherapistCard,
    PatientCard,
    defaultCard
}


public class CardUI : MonoBehaviour
{
    public Card card;
    [Space(40f)]
    public CardUIType cardUIType;
    public int index;//first card is 0

    public Vector2 lowestPosOfCard;
    public Vector2 highestPosOfCard;
    public Vector2 centeredCardPos;

    [SerializeField]
    private Vector3 cardMaxScale = Vector3.one * 0.8f;
    [SerializeField]
    private Vector3 cardMinScale = Vector3.one * 0.5f;

    [SerializeField]
    private float cardAnimationSpeed;
    [SerializeField]
    private float secondsForZoomOut = 1f;

    private RectTransform cardRect;
    private bool isUI = false;
    private List<CardUI> otherCardsUI;
    private bool IsBeingDrag = false;
    private float draggedTime = 0;

    public void OnPointerDown()
    {
        draggedTime = Time.time;
    }

    public void OnPointerUP()
    {
        float deltaT = Mathf.Abs(Time.time - draggedTime);
        if (deltaT < 0.5f)
            OnClicked();
        else
        {
            if(UIController.instance.firstSelectedCard != null)
            {
                UIController.instance.firstSelectedCard.transform.GetChild(0).gameObject.SetActive(false);
                UIController.instance.firstSelectedCard = null;
            }
            if(UIController.instance.secondSelectedCard != null)
            {
                UIController.instance.secondSelectedCard.transform.GetChild(0).gameObject.SetActive(false);
                UIController.instance.secondSelectedCard = null;
            }
        }
        draggedTime = 0f;
    }

    public void OnDrag()
    {
        if (cardUIType != CardUIType.TherapistCard)
            return;

        cardRect.position = Vector3.Lerp(cardRect.position, Input.mousePosition, Time.fixedDeltaTime * 20f); 
    }

    public void OnDragBegin()
    {
        if (cardUIType != CardUIType.TherapistCard)
            return;
        IsBeingDrag = true;
    }
    public void OnDragEnd()
    {
        if (cardUIType != CardUIType.TherapistCard)
            return;
        IsBeingDrag = false;

        ScreenPart screenPart = UIController.instance.GetScreenPart(Input.mousePosition);

        switch (screenPart)
        {
            case ScreenPart.PatientHand:
                break;
            case ScreenPart.Middle:
                UIController.instance.PutCardInRandomPlace(this);
                break;
            case ScreenPart.Therapist:
                break;
            default:
                break;
        }
    }

    public void ApplyToCardGameObject(Card card,CardUIType cardUIType = CardUIType.defaultCard)
    {
        this.card = card;
        this.cardUIType = cardUIType;

        if (card.cardType == CardTypes.Equipment)
        {
            transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = $"{card.actionPoint}";
        }
        transform.GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = card.cardName;
        //2
        transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<TMPro.TMP_Text>().text = card.cardType.ToString();
        transform.GetChild(1).GetChild(4).GetComponent<TMPro.TMP_Text>().text = card.affectDescription;
    }

    public void ApplyCardMetrics(CardUI newCardUI)
    {
        ApplyCardMetrics(newCardUI.index, newCardUI.centeredCardPos, newCardUI.highestPosOfCard, newCardUI.lowestPosOfCard);
    }

    public void ApplyCardMetrics(int index, Vector2 centeredCardPos, Vector2 highestPosOfCard, Vector2 lowestPosOfCard)
    {
        cardRect = GetComponent<RectTransform>();
        isUI = true;
        this.index = index;
        this.centeredCardPos = centeredCardPos;
        this.highestPosOfCard = highestPosOfCard;
        this.lowestPosOfCard = lowestPosOfCard;

        otherCardsUI = new List<CardUI>();
    }
   
    public void DestroyCard()
    {
        Destroy(gameObject);
    }

    public void AnimateZoomIn()
    {

        if (!isUI)//|| interactable)
            return;

        foreach (var item in otherCardsUI)
        {
            item.AnimateZoomOut(0);
        }

        if (IAnimateZoomOutHelper != null)
        {
            StopCoroutine(IAnimateZoomOutHelper);
        }
        if (IAnimateZoomInHelper == null)
        {
            IAnimateZoomInHelper = StartCoroutine(IAnimateZoomIn());
        }
        else
        {
            StopCoroutine(IAnimateZoomInHelper);
            IAnimateZoomInHelper = StartCoroutine(IAnimateZoomIn());
        }
    }

    public void AnimateZoomOut(float seconds = -1f)
    {
        //interactable = false;
        if (!isUI)
            return;
        float s;
        if (seconds >= 0)
            s = seconds;
        else
            s = secondsForZoomOut;


        if (IAnimateZoomOutHelper == null)
        {
            IAnimateZoomOutHelper = StartCoroutine(IAnimateZoomOut(s));
        }
        else
        {
            StopCoroutine(IAnimateZoomOutHelper);
            IAnimateZoomOutHelper = StartCoroutine(IAnimateZoomOut(s));
        }
    }

    public void UpdateCard(bool setPosition)
    {
        if (setPosition)
        {
            SetCardPosition();
        }
        SetCardsScales();

        cardRect.SetSiblingIndex(index);
        otherCardsUI.Clear();
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).GetComponent<CardUI>().index != index)
                otherCardsUI.Add(transform.parent.GetChild(i).GetComponent<CardUI>());
        }
    }


    public void MoveCardToPlace()
    {
        float step = 1f / (transform.parent.childCount - 1f);
        MoveCardToPlace(Vector2.Lerp(highestPosOfCard, lowestPosOfCard, index * step));
    }



    private void OnClicked()
    {
        if(cardUIType== CardUIType.TherapistCard)
        {
            foreach (var cardUI in otherCardsUI)
            {
                cardUI.transform.GetChild(0).gameObject.SetActive(false);
            }
        }

        bool active = !transform.GetChild(0).gameObject.activeSelf;
        transform.GetChild(0).gameObject.SetActive(active);

        if (active)
        {
            SetAsSelectedCardUI(this, index);
            UIController.instance.CheckSelectedCardUIs();
        }
        else
        {
            SetAsSelectedCardUI(null, index);
        }
    }

    private void SetAsSelectedCardUI(CardUI cardUI, int index)
    {
        if (cardUI == null)
        {
            if (cardUIType == CardUIType.PatientCard)
            {
                if (UIController.instance.firstSelectedCard != null)
                {
                    if (UIController.instance.firstSelectedCard.index == index)
                    {
                        UIController.instance.firstSelectedCard = null;
                    }
                }
                
                if(UIController.instance.secondSelectedCard != null)
                {
                    if (UIController.instance.secondSelectedCard.index == index)
                    {
                        UIController.instance.secondSelectedCard = null;
                    }
                }
            }
            else
            {
                if (UIController.instance.secondSelectedCard != null)
                {
                    if (UIController.instance.secondSelectedCard.cardUIType == CardUIType.TherapistCard)
                    {
                        UIController.instance.secondSelectedCard = null;
                    }
                }
            }
        }
        else
        {
            if (cardUIType == CardUIType.PatientCard)
            {
                if (UIController.instance.firstSelectedCard == null)
                {
                    UIController.instance.firstSelectedCard = cardUI;
                }
                else
                {
                    UIController.instance.secondSelectedCard = cardUI;
                }
            }
            else
            {
                UIController.instance.secondSelectedCard = cardUI;
            }
        }
    }

    private void SetCardPosition()
    {
        float step = 1f / (transform.parent.childCount - 1f);
        //MoveCardToPlace(Vector2.Lerp(highestPosOfCard, lowestPosOfCard, index * step));
        cardRect.anchoredPosition = Vector2.Lerp(highestPosOfCard, lowestPosOfCard, index * step);
        centeredCardPos = new Vector2(centeredCardPos.x, cardRect.anchoredPosition.y);
    }

    private void SetCardsScales()
    {
        cardRect.localScale = cardMinScale;
    }

    private void MoveCardToPlace(Vector2 anchoredPosition, UnityAction OnMoved = null)
    {
        if (IMoveCardToPlaceHelper == null)
        {
            IMoveCardToPlaceHelper = StartCoroutine(IMoveCardToPlace(anchoredPosition, OnMoved));
        }
        else
        {
            StopCoroutine(IMoveCardToPlaceHelper);
            IMoveCardToPlaceHelper = StartCoroutine(IMoveCardToPlace(anchoredPosition, OnMoved));
        }
    }

    private Coroutine IAnimateZoomInHelper;
    private IEnumerator IAnimateZoomIn()
    {
        while (Vector2.Distance(cardRect.anchoredPosition, centeredCardPos) > 0.002f &&
            Vector3.Distance(cardRect.localScale, cardMaxScale) > 0.002f)
        {
            cardRect.SetAsLastSibling();//Need to check this in profiler
            cardRect.anchoredPosition = Vector2.Lerp(cardRect.anchoredPosition, centeredCardPos, Time.fixedDeltaTime * cardAnimationSpeed);
            cardRect.localScale = Vector3.Lerp(cardRect.localScale, cardMaxScale, Time.fixedDeltaTime * cardAnimationSpeed);

            yield return new WaitUntil(() => !IsBeingDrag);
            yield return new WaitForFixedUpdate();
        }
        IAnimateZoomInHelper = null;
    }


    private Coroutine IAnimateZoomOutHelper;
    private IEnumerator IAnimateZoomOut(float s)
    {

        yield return new WaitUntil(() => !IsBeingDrag);
        yield return new WaitForSeconds(s);

        if (IAnimateZoomInHelper != null)
        {
            StopCoroutine(IAnimateZoomInHelper);
        }

        float step = 1f / (transform.parent.childCount - 1f);

        foreach (var otherC in otherCardsUI)
        {
            otherC.cardRect.SetSiblingIndex(otherC.index);
        }
        cardRect.SetSiblingIndex(index);

        //for (int i = 0; i < transform.parent.childCount; i++)
        //{
        //    transform.parent.GetChild(i).SetSiblingIndex(i);
        //}

        while (Vector2.Distance(cardRect.anchoredPosition, Vector2.Lerp(highestPosOfCard, lowestPosOfCard, index * step)) > 0.002f &&
            Vector3.Distance(cardRect.localScale, cardMinScale) > 0.002f)
        {
            cardRect.anchoredPosition = Vector2.Lerp(cardRect.anchoredPosition, Vector2.Lerp(highestPosOfCard, lowestPosOfCard, index * step), Time.fixedDeltaTime * cardAnimationSpeed);
            cardRect.localScale = Vector3.Lerp(cardRect.localScale, cardMinScale, Time.fixedDeltaTime * cardAnimationSpeed);


            yield return new WaitUntil(() => !IsBeingDrag);
            yield return new WaitForFixedUpdate();
        }

        //interactable = true;
        IAnimateZoomOutHelper = null;
    }



    private Coroutine IMoveCardToPlaceHelper;
    private IEnumerator IMoveCardToPlace(Vector2 anchoredPosition, UnityAction OnMoved)
    {
        while (Vector2.Distance(cardRect.anchoredPosition, anchoredPosition) > 0.002f)
        {
            cardRect.anchoredPosition = Vector2.Lerp(cardRect.anchoredPosition, anchoredPosition, Time.fixedDeltaTime * cardAnimationSpeed * 2f);
            yield return new WaitForFixedUpdate();
        }
        if (OnMoved != null)
        {
            OnMoved();
        }
        IMoveCardToPlaceHelper = null;
    }
}
