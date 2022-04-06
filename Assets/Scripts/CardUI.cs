using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardUI : MonoBehaviour
{
    public string cardName;
    public CardTypes cardType;
    public Affect affect;
    public string affectDescription;
    public int actionPoint;
    public Rarity rarity;

    [SerializeField]
    private int index;//first card is 0

    [SerializeField]
    private Vector2 lowestPosOfCard = new Vector2(0f, -253);
    [SerializeField]
    private Vector2 highestPosOfCard = new Vector2(0f, 248);

    [SerializeField]
    private Vector2 centeredCardPos;
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

    public void ApplyToCardGameObject(Card card)
    {
        cardName = card.cardName;
        cardType = card.cardType;
        affect = card.affect;
        affectDescription = card.affectDescription;
        actionPoint = card.actionPoint;
        rarity = card.rarity;

        if (card.cardType == CardTypes.Equipment)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            transform.GetChild(0).GetChild(0).GetComponent<TMPro.TMP_Text>().text = $"{card.actionPoint}";
        }
        transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = card.cardName;
        //2
        transform.GetChild(3).GetChild(0).GetComponent<TMPro.TMP_Text>().text = card.cardType.ToString();
        transform.GetChild(4).GetComponent<TMPro.TMP_Text>().text = card.affectDescription;
    }

    public void ApplyCardMetrics(int index, Vector2 centeredCardPos, Vector2 highestPosOfCard, Vector2 lowestPosOfCard)
    {
        isUI = true;
        this.index = index;
        this.centeredCardPos = centeredCardPos;
        this.highestPosOfCard = highestPosOfCard;
        this.lowestPosOfCard = lowestPosOfCard;

        cardRect = GetComponent<RectTransform>();
        otherCardsUI = new List<CardUI>();
    }
   
    public void DestroyCard()
    {
        Destroy(gameObject);
    }

    public void AnimateZoomIn()
    {
        if (!isUI)
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
        if (!isUI)
            return;
        float s = 0;
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

    public void UpdateCard()
    {
        SetCardPosition();
        SetCardsScales();

        otherCardsUI.Clear();
        for (int i = 0; i < transform.parent.childCount; i++)
        {
            if (i != index)
                otherCardsUI.Add(transform.parent.GetChild(i).GetComponent<CardUI>());
        }
    }



    private void SetCardPosition()
    {
        float step = 1f / (transform.parent.childCount - 1f);
        cardRect.anchoredPosition = Vector2.Lerp(highestPosOfCard, lowestPosOfCard, index * step);
    }
    private void SetCardsScales()
    {
        cardRect.localScale = cardMinScale;
    }


    private Coroutine IAnimateZoomInHelper;
    private IEnumerator IAnimateZoomIn()
    {
        while (Vector2.Distance(cardRect.anchoredPosition, centeredCardPos) > 0.002f ||
            Vector3.Distance(cardRect.localScale, cardMaxScale) > 0.002f)
        {
            cardRect.anchoredPosition = Vector2.Lerp(cardRect.anchoredPosition, centeredCardPos, Time.fixedDeltaTime * cardAnimationSpeed);
            cardRect.localScale = Vector3.Lerp(cardRect.localScale, cardMaxScale, Time.fixedDeltaTime * cardAnimationSpeed);

            yield return new WaitForFixedUpdate();
        }
        IAnimateZoomInHelper = null;
    }


    private Coroutine IAnimateZoomOutHelper;
    private IEnumerator IAnimateZoomOut(float s)
    {
        yield return new WaitForSeconds(s);

        if (IAnimateZoomInHelper != null)
        {
            StopCoroutine(IAnimateZoomInHelper);
        }

        float step = 1f / (transform.parent.childCount - 1f);

        while (Vector2.Distance(cardRect.anchoredPosition, Vector2.Lerp(highestPosOfCard, lowestPosOfCard, index * step)) > 0.002f ||
            Vector3.Distance(cardRect.localScale, cardMinScale) > 0.002f)
        {
            cardRect.anchoredPosition = Vector2.Lerp(cardRect.anchoredPosition, Vector2.Lerp(highestPosOfCard, lowestPosOfCard, index * step), Time.fixedDeltaTime * cardAnimationSpeed);
            cardRect.localScale = Vector3.Lerp(cardRect.localScale, cardMinScale, Time.fixedDeltaTime * cardAnimationSpeed);

            yield return new WaitForFixedUpdate();
        }
        IAnimateZoomOutHelper = null;
    }
}
