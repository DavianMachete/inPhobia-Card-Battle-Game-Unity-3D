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
    //private bool enableActions = false;
    private List<CardUI> otherCardsUI;
    private bool IsBeingDrag = false;
    private float draggedTime = 0;
    private Vector2 startDragPos;



    public void OnPointerDown()
    {
        //if (!enableActions)
        //    return;
        draggedTime = Time.time;
        startDragPos = cardRect.anchoredPosition;
    }

    public void OnPointerUP()
    {
        //if (!enableActions)
        //    return;
        float deltaT = Mathf.Abs(Time.time - draggedTime);
        if (deltaT < 0.3f&& Vector2.Distance(startDragPos, cardRect.anchoredPosition)<20f)
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
        //if (!enableActions)
        //    return;
        if (cardUIType != CardUIType.TherapistCard)
            return;

        cardRect.position = Vector3.Lerp(cardRect.position, Input.mousePosition, Time.fixedDeltaTime * 20f);

        ScreenPart screenPart = UIController.instance.GetScreenPart(Input.mousePosition);

        if(screenPart == ScreenPart.PatientHand)
        {
            float mouseY = Input.mousePosition.y * 1080f / (float)Screen.height;
            mouseY -= 1080f / 2f;

            //Debug.Log(mouseY);

            UIController.instance.AnimatePatientCardsBeforeDrop(mouseY);
        }
    }

    public void OnDragBegin()
    {
        //if (!enableActions)
        //    return;
        if (cardUIType != CardUIType.TherapistCard)
            return;
        IsBeingDrag = true;
        //foreach (var cUI in otherCardsUI)
        //{
        //    cUI.EnableActions();
        //}
    }

    public void OnDragEnd()
    {
        //if (!enableActions)
        //    return;
        if (cardUIType != CardUIType.TherapistCard)
            return;
        IsBeingDrag = false;

        ScreenPart screenPart = UIController.instance.GetScreenPart(Input.mousePosition);
        //foreach (var cUI in otherCardsUI)
        //{
        //    cUI.EnableActions();
        //}
        switch (screenPart)
        {
            case ScreenPart.PatientHand:
                UIController.instance.DropCardToPatientHand(this);
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
        this.gameObject.name = card.cardName;
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
        //enableActions = true;
        this.index = index;
        this.centeredCardPos = centeredCardPos;
        this.highestPosOfCard = highestPosOfCard;
        this.lowestPosOfCard = lowestPosOfCard;

        otherCardsUI = new List<CardUI>();
    }
   
    public void DestroyCard()
    {
        Destroy(this.gameObject);
    }

    public void AnimateZoomIn()
    {
        //if (!enableActions)//|| interactable)
        //    return;

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
        //if (!enableActions)
        //    return;
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
        else
        {
            Vector2 anchPos = Vector2.Lerp(highestPosOfCard, lowestPosOfCard, GetCardT());
            centeredCardPos = new Vector2(centeredCardPos.x, anchPos.y);
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
        //if (!enableActions)
        //    return;
        MoveCardToPlace(Vector2.Lerp(highestPosOfCard, lowestPosOfCard, GetCardT()));
    }

    public void MoveCardToPlace(float t)
    {
        //if (!enableActions)
        //    return;
        MoveCardToPlace(Vector2.Lerp(highestPosOfCard, lowestPosOfCard, t));
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
        //MoveCardToPlace(Vector2.Lerp(highestPosOfCard, lowestPosOfCard, index * step));
        cardRect.anchoredPosition = Vector2.Lerp(highestPosOfCard, lowestPosOfCard, GetCardT());
        centeredCardPos = new Vector2(centeredCardPos.x, cardRect.anchoredPosition.y);
    }

    private void SetCardsScales()
    {
        cardRect.localScale = cardMinScale;
    }

    private float GetCardT()
    {
        float step, t;
        if (transform.parent.childCount > 1)
        {
            step = 1f / (transform.parent.childCount - 1f);
            t = index * step;
        }
        else
            t = 0.5f;
        return t;
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
            yield return new WaitUntil(() => !IsBeingDrag);

            cardRect.SetAsLastSibling();//Need to check this in profiler
            cardRect.anchoredPosition = Vector2.Lerp(cardRect.anchoredPosition, centeredCardPos, Time.fixedDeltaTime * cardAnimationSpeed);
            cardRect.localScale = Vector3.Lerp(cardRect.localScale, cardMaxScale, Time.fixedDeltaTime * cardAnimationSpeed);

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

        foreach (var otherC in otherCardsUI)
        {
            otherC.cardRect.SetSiblingIndex(otherC.index);
        }
        cardRect.SetSiblingIndex(index);

        //for (int i = 0; i < transform.parent.childCount; i++)
        //{
        //    transform.parent.GetChild(i).SetSiblingIndex(i);
        //}

        while (Vector2.Distance(cardRect.anchoredPosition, Vector2.Lerp(highestPosOfCard, lowestPosOfCard, GetCardT())) > 0.002f &&
            Vector3.Distance(cardRect.localScale, cardMinScale) > 0.002f)
        {
            yield return new WaitUntil(() => !IsBeingDrag);
            
            cardRect.anchoredPosition = Vector2.Lerp(cardRect.anchoredPosition, Vector2.Lerp(highestPosOfCard, lowestPosOfCard, GetCardT()), Time.fixedDeltaTime * cardAnimationSpeed);
            cardRect.localScale = Vector3.Lerp(cardRect.localScale, cardMinScale, Time.fixedDeltaTime * cardAnimationSpeed);

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
            yield return new WaitUntil(() => !IsBeingDrag);
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
