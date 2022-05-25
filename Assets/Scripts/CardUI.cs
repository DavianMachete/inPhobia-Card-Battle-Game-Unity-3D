using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class CardUI : MonoBehaviour
{
    public Card card;
    [Space(40f)]
    public CardUIType cardUIType;
    public int index;//first card is 0

    public Vector2 lowestPosOfCard;
    public Vector2 highestPosOfCard;
    public Vector2 centeredCardPos;

    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Vector3 cardMaxScale = Vector3.one * 0.8f;
    [SerializeField] private Vector3 cardMinScale = Vector3.one * 0.5f;

    [SerializeField] private float cardAnimationSpeed;

    [SerializeField] private float animDuration = 2f;

    private RectTransform cardRect;
    private List<CardUI> otherCardsUI;
    private float draggedTime = 0;
    private Vector2 startDragPos;
    private bool rightButtonClicked;



    public void OnPointerDown()
    {
        draggedTime = Time.time;
        startDragPos = cardRect.anchoredPosition;
        if (Input.GetMouseButton(1))
            rightButtonClicked = true;
    }

    public void OnPointerUP()
    {
        float deltaT = Mathf.Abs(Time.time - draggedTime);
        if (deltaT < 0.3f&& Vector2.Distance(startDragPos, cardRect.anchoredPosition)<20f)
            OnClicked(rightButtonClicked);
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

        ScreenPart screenPart = UIController.instance.GetScreenPart(Input.mousePosition);

        if(screenPart == ScreenPart.PatientHand)
        {
            float mouseY = Input.mousePosition.y * 1080f / (float)Screen.height;
            mouseY -= 1080f / 2f;

            UIController.instance.AnimatePatientCardsBeforeDrop(mouseY);
        }
    }

    public void OnDragBegin()
    {
        if (cardUIType != CardUIType.TherapistCard)
            return;
        StopCardMoving();
    }

    public void OnDragEnd()
    {
        if (cardUIType != CardUIType.TherapistCard)
            return;

        ScreenPart screenPart = UIController.instance.GetScreenPart(Input.mousePosition);

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
        foreach (var item in otherCardsUI)
        {
            item.AnimateZoomOut();
        }

        float tPart = Mathf.InverseLerp(cardMinScale.magnitude, cardMaxScale.magnitude, cardRect.localScale.magnitude);
        float duration = animDuration - tPart / animDuration;

        Debug.Log("");///////asdasds\fsdfghjskj dfgasldjfhgasldfgaslwdfhygalkgelaweygkjggkhgahgasfrlagkaehaflsakjfgasdjfghhhhhhhhhhhhhhhhhhhhhhljkkkkkkkkkkkkkkkkkkasdffffffffffffff

        ScaleCardIn(duration);
        MoveCardTo(duration, centeredCardPos);
    }

    public void AnimateZoomOut()
    {
        float tPart = Mathf.InverseLerp(cardMaxScale.magnitude, cardMinScale.magnitude, cardRect.localScale.magnitude);
        float duration = animDuration - tPart / animDuration;

        ScaleCardOut(duration);
        MoveCardToPlace(duration);
    }

    public void UpdateCard(bool setPosition,bool setScale = true)
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
        if (setScale)
        {
            SetCardsScales();
        }

        cardRect.SetSiblingIndex(index);
        otherCardsUI.Clear();
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (transform.parent.GetChild(i).GetComponent<CardUI>().index != index)
                otherCardsUI.Add(transform.parent.GetChild(i).GetComponent<CardUI>());
        }
    }


    public void MoveCardToPlace(float duration, float t = -1f)
    {
        if (t <= 0)
            t = GetCardT();
        MoveCardTo(duration, Vector2.Lerp(highestPosOfCard, lowestPosOfCard, t));
    }

    public void MoveCardToCenter(UnityAction onDone)
    {
        SetInteractable(false);
        MoveCardTo(animDuration, new Vector2(800, 0), onDone);
    }




    private void OnClicked(bool isRightButton)
    {
        if(!isRightButton)
        {
            if (cardUIType == CardUIType.TherapistCard)
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
        else
        {
            UIController.instance.bigCardUI.SetActive(true);
            UIController.instance.bigCardUI.transform.GetChild(0).GetComponent<CardUI>().ApplyToCardGameObject(card);
        }
        rightButtonClicked = false;
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

    private void MoveCardTo(float duration, Vector2 anchoredPosition, UnityAction OnDone = null)
    {
        StopCardMoving();
        isCardMoving = true;
        IMoveCardToHelper = StartCoroutine(IMoveCardTo(anchoredPosition, duration, OnDone));
    }

    private void StopCardMoving()
    {
        isCardMoving = false;
        if (IMoveCardToHelper != null)
            StopCoroutine(IMoveCardToHelper);
    }

    private void ScaleCardIn(float duration, UnityAction onDone = null)
    {
        StopCardScaling();
        isCardScaling = true;

        cardRect.SetAsLastSibling();

        IScaleCardToHelper = StartCoroutine(IScaleCardTo(cardMaxScale, duration, onDone));
    }

    private void ScaleCardOut(float duration, UnityAction onDone = null)
    {
        StopCardScaling();
        isCardScaling = true;

        foreach (CardUI cardUI in otherCardsUI)
        {
            cardUI.cardRect.SetSiblingIndex(cardUI.index);
        }
        cardRect.SetSiblingIndex(index);

        IScaleCardToHelper = StartCoroutine(IScaleCardTo(cardMinScale, duration, onDone));
    }

    private void StopCardScaling()
    {
        isCardScaling = false;
        if (IScaleCardToHelper != null)
            StopCoroutine(IScaleCardToHelper);
    } 

    private void SetInteractable(bool value)
    {
        canvasGroup.interactable = value;
    }


    private bool isCardScaling = false;
    private Coroutine IScaleCardToHelper;
    private IEnumerator IScaleCardTo(Vector3 scale, float duration, UnityAction onDone = null)
    {
        if (duration <= 0f)
            duration = Time.fixedDeltaTime;

        float t = 0f;

        Vector3 startScale = cardRect.localScale;

        while (isCardScaling)
        {
            //yield return new WaitUntil(() => !IsBeingDrag);
            cardRect.localScale = Vector2.Lerp(startScale, scale, t / duration);

            if (t / duration >= 1f)
                isCardMoving = false;
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (onDone != null)
            onDone();
        IScaleCardToHelper = null;
    }


    private bool isCardMoving = false;
    private Coroutine IMoveCardToHelper;
    private IEnumerator IMoveCardTo(Vector2 anchoredPosition, float duration, UnityAction onDone = null)
    {
        if (duration <= 0f)
            duration = Time.fixedDeltaTime;

        float t = 0f;
        Vector2 startPos = cardRect.anchoredPosition;

        while (isCardMoving)
        {
            //yield return new WaitUntil(() => !IsBeingDrag);
            cardRect.anchoredPosition = Vector2.Lerp(startPos, anchoredPosition, t/ duration);

            if (t / duration >= 1f)
                isCardMoving = false;
            t += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        if (onDone != null)
            onDone();
        IMoveCardToHelper = null;
    }
}
