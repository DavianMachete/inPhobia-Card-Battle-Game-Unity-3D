using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class CardController : MonoBehaviour
{
    public Card card;
    [Space(40f)]
    public CardUIType cardCurrentType = CardUIType.defaultCard;
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
    private List<CardController> otherCardsUI;
    private Vector2 startDragPos;
    private float draggedTime = 0;
    private bool rightButtonClicked;
    private bool interactable = true;



    public void OnPointerDown()
    {
        if (!UIController.instance.GetCanvasInteractable() || !interactable)
            return;

        draggedTime = Time.time;
        startDragPos = cardRect.anchoredPosition;
        if (Input.GetMouseButton(1))
            rightButtonClicked = true;
    }

    public void OnPointerUP()
    {
        if (!UIController.instance.GetCanvasInteractable() || !interactable)
            return;

        float deltaT = Mathf.Abs(Time.time - draggedTime);
        if (deltaT < 0.3f && Vector2.Distance(startDragPos, cardRect.anchoredPosition) < 18f)
            OnClicked(rightButtonClicked);
        else
        {
            if (UIController.instance.firstSelectedCard != null)
            {
                UIController.instance.firstSelectedCard.transform.GetChild(0).gameObject.SetActive(false);
                UIController.instance.firstSelectedCard = null;
            }
            if (UIController.instance.secondSelectedCard != null)
            {
                UIController.instance.secondSelectedCard.transform.GetChild(0).gameObject.SetActive(false);
                UIController.instance.secondSelectedCard = null;
            }
        }
        draggedTime = 0f;
    }

    public void OnDrag()
    {
        if (cardCurrentType != CardUIType.TherapistCard ||
            !UIController.instance.GetCanvasInteractable() || !interactable)
            return;

        //Debug.Log($"{card.cardID} Draging");
        StopCardMoving();

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
        if (cardCurrentType != CardUIType.TherapistCard ||
            !UIController.instance.GetCanvasInteractable() || !interactable)
            return;

        //Debug.Log($"{card.cardID} Drag Begin");
        StopCardMoving();
    }

    public void OnDragEnd()
    {
        if (cardCurrentType != CardUIType.TherapistCard ||
            !UIController.instance.GetCanvasInteractable() || !interactable)
            return;

        //Debug.Log($"{card.cardID} Drag End");

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

    public void AnimateZoomIn()
    {
        if (!UIController.instance.GetCanvasInteractable() || !interactable)
            return;

        foreach (var item in otherCardsUI)
        {
            item.AnimateZoomOut();
        }

        ScaleCardIn(animDuration);
        MoveCardTo(animDuration, centeredCardPos);
    }

    public void AnimateZoomOut()
    {
        if (!UIController.instance.GetCanvasInteractable() || !interactable)
            return;

        ScaleCardOut(animDuration);
        MoveCardToPlace(animDuration);
    }




    public void SetCardParametersToGameObject(Card card)
    {
        SetInteractable(true);
        this.gameObject.name = card.name;
        this.card = card;

        if (card.cardType == CardTypes.Equipment)
        {
            transform.GetChild(1).GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(1).GetChild(0).gameObject.SetActive(true);
            transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = $"{card.actionPoint}";
        }
        transform.GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = card.name;
        //2
        transform.GetChild(1).GetChild(3).GetChild(0).GetComponent<TMPro.TMP_Text>().text = card.cardType.ToString();
        transform.GetChild(1).GetChild(4).GetComponent<TMPro.TMP_Text>().text = card.affectDescription;
    }

    public void SetCardCurrentType(CardUIType cardUIType)
    {
        cardCurrentType = cardUIType;
    }

    public void SetCardMetrics(CardController newCardUI)
    {
        SetCardMetrics(newCardUI.index, newCardUI.centeredCardPos, newCardUI.highestPosOfCard, newCardUI.lowestPosOfCard);
    }

    public void SetCardMetrics(int index, Vector2 centeredCardPos, Vector2 highestPosOfCard, Vector2 lowestPosOfCard)
    {
        cardRect = GetComponent<RectTransform>();
        cardRect.SetSiblingIndex(index);
        //enableActions = true;
        this.index = index;
        this.centeredCardPos = centeredCardPos;
        this.highestPosOfCard = highestPosOfCard;
        this.lowestPosOfCard = lowestPosOfCard;
    }
   
    public void DestroyCard()
    {
        Destroy(this.gameObject);
    }

    public void UpdateCard(bool smoothly)
    {
        if(otherCardsUI==null)
            otherCardsUI = new List<CardController>();
        otherCardsUI.Clear();

        List<CardController> currentCards = cardCurrentType == CardUIType.PatientCard ? UIController.instance.patientCardsInHand : UIController.instance.therapistCardsInHand;
        foreach (CardController cardC in currentCards)
        {
            if (cardC.index != index)
                otherCardsUI.Add(cardC);
        }

        UpdateCardPosition(smoothly);
        UpdateCardScale(smoothly);
    }

    public void UpdateCardPosition(bool smoothly)
    {
        Vector2 anchPos = Vector2.Lerp(highestPosOfCard, lowestPosOfCard, GetCardT());
        centeredCardPos = new Vector2(centeredCardPos.x, anchPos.y);

        if (smoothly)
        {
            MoveCardToPlace(animDuration);
        }
        else
        {
            MoveCardToPlace(0);
        }
    }

    public void UpdateCardScale(bool smoothly)
    {
        if (smoothly)
        {
            ScaleCardOut(animDuration);
        }
        else
        {
            ScaleCardOut(0);
        }
    }

    public void MoveCardToPlace(float duration, UnityAction onDone = null)
    {
        MoveCardTo(duration, Vector2.Lerp(highestPosOfCard, lowestPosOfCard, GetCardT()), onDone);
    }
    public void MoveCardToPlace(float duration,float t, UnityAction onDone = null)
    {
        MoveCardTo(duration, Vector2.Lerp(highestPosOfCard, lowestPosOfCard, t), onDone);
    }

    public void MoveCardToCenter(UnityAction onDone)
    {
        SetInteractable(false);
        MoveCardTo(animDuration, new Vector2(800, 0),()=> 
        {
            FadeCardIn(onDone);
        });
    }
    public void MoveCardToDiscard(Vector2 toPos)
    {
        SetInteractable(false);
        MoveCardTo(animDuration, toPos, () =>
        {
            FadeCardIn();
        });
    }

    public void FadeCardIn(UnityAction onDone = null)
    {
        isCardFading = true;
        if (IFadeCardHelper == null)
            IFadeCardHelper = StartCoroutine(IFadeCard(0f, animDuration, () =>
            {
                isCardFading = false;
                onDone?.Invoke();
                DestroyCard();
            }));
    }




    private void OnClicked(bool isRightButton)
    {
        if(!isRightButton)
        {
            if (cardCurrentType == CardUIType.TherapistCard)
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
            UIController.instance.bigCardUI.transform.GetChild(0).GetComponent<CardController>().SetCardParametersToGameObject(card);
        }
        rightButtonClicked = false;
    }

    private void SetAsSelectedCardUI(CardController cardUI, int index)
    {
        if (cardUI == null)
        {
            if (cardCurrentType == CardUIType.PatientCard)
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
                    if (UIController.instance.secondSelectedCard.cardCurrentType == CardUIType.TherapistCard)
                    {
                        UIController.instance.secondSelectedCard = null;
                    }
                }
            }
        }
        else
        {
            if (cardCurrentType == CardUIType.PatientCard)
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

    private float GetCardT()
    {
        float step, t;
        int currentCardsCount = cardCurrentType == CardUIType.PatientCard ? UIController.instance.patientCardsInHand.Count : UIController.instance.therapistCardsInHand.Count;
        if (currentCardsCount > 1)
        {
            step = 1f / (currentCardsCount - 1f);
            t = index * step;
        }
        else
            t = 0.5f;
        return t;
    }

    private void MoveCardTo(float duration, Vector2 anchoredPosition, UnityAction OnDone=null)
    {
        StopCardMoving();
        isCardMoving = true;

        if (gameObject.activeInHierarchy)
            IMoveCardToHelper = StartCoroutine(IMoveCardTo(anchoredPosition, duration, OnDone));
        else
            cardRect.anchoredPosition = anchoredPosition;
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

        foreach (CardController cardUI in otherCardsUI)
        {
            cardUI.cardRect.SetSiblingIndex(cardUI.index);
        }
        cardRect.SetSiblingIndex(index);

        if (gameObject.activeInHierarchy)
            IScaleCardToHelper = StartCoroutine(IScaleCardTo(cardMinScale, duration, onDone));
        else
            cardRect.localScale = cardMinScale;
    }

    private void StopCardScaling()
    {
        isCardScaling = false;
        if (IScaleCardToHelper != null)
            StopCoroutine(IScaleCardToHelper);
    } 

    private void SetInteractable(bool value)
    {
        interactable = value;
    }


    private bool isCardScaling = false;
    private Coroutine IScaleCardToHelper;
    private IEnumerator IScaleCardTo(Vector3 scale, float duration, UnityAction onDone = null)
    {
        if (duration <= 0f)
        {
            //Debug.Log($"<color=yellow>Card {card.cardID}'s MoveCardTo animation duration = {duration}</color>");
            duration = 0.1f;
        }

        float t = 0f;

        Vector3 startScale = cardRect.localScale;

        while (isCardScaling)
        {
            //yield return new WaitUntil(() => !IsBeingDrag);
            cardRect.localScale = Vector2.Lerp(startScale, scale, t / duration);

            t += Time.fixedDeltaTime;

            if (t / duration >= 1f)
                isCardScaling = false;

            yield return new WaitForFixedUpdate();
        }

        cardRect.localScale = scale;

        if (onDone != null)
            onDone();
        yield return null;
        IScaleCardToHelper = null;
    }


    private bool isCardMoving = false;
    private Coroutine IMoveCardToHelper;
    private IEnumerator IMoveCardTo(Vector2 anchoredPosition, float duration, UnityAction onDone = null)
    {
        if (duration <= 0f)
        {
            //Debug.Log($"<color=yellow>Card {card.cardID}'s MoveCardTo animation duration = {duration}</color>");
            duration = 0.1f;
        }

        float t = 0f;
        Vector2 startPos = cardRect.anchoredPosition;

        while (isCardMoving)
        {
            //yield return new WaitUntil(() => !IsBeingDrag);
            cardRect.anchoredPosition = Vector2.Lerp(startPos, anchoredPosition, t/ duration);

            t += Time.fixedDeltaTime;

            if (t / duration >= 1f)
                isCardMoving = false;
            yield return new WaitForFixedUpdate();
        }

        cardRect.anchoredPosition = anchoredPosition;

        if (onDone != null)
            onDone();
        yield return null;
        IMoveCardToHelper = null;
    }


    private bool isCardFading = false;
    private Coroutine IFadeCardHelper;
    private IEnumerator IFadeCard(float value, float duration, UnityAction onDone = null)
    {
        if (duration <= 0f)
        {
            //Debug.Log($"<color=yellow>Card {card.cardID}'s IFadeCard animation duration = {duration}</color>");
            duration = 0.1f;
        }

        float t = 0f;
        float startFadeValue = canvasGroup.alpha;

        while (isCardFading)
        {
            canvasGroup.alpha = Mathf.Lerp(startFadeValue, value, t / duration);

            t += Time.fixedDeltaTime;

            if (t / duration >= 1f)
                isCardFading = false;

            yield return new WaitForFixedUpdate();
        }

        canvasGroup.alpha = value;

        if (onDone != null)
            onDone();
        yield return null;
        IFadeCardHelper = null;
    }
}
