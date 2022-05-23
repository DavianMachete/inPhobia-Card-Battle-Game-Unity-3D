using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


public class UIController : MonoBehaviour
{
    public static UIController instance;

    public List<RectTransform> therapistCards;
    public List<RectTransform> patientCards;

    public GameObject bigCardUI;

    #region Serialized Fields 

    [Header("                Before Main Game Game UIs            ")]
    [SerializeField]
    private GameObject backGroundUI;
    [SerializeField]
    private GameObject beforStartUI;
    [SerializeField]
    private GameObject mainGameUI;


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
    [Header("            Card Movement settings        ")]
    [SerializeField]
    private List<float> screenPartBoundsOnX;
   
    public CardUI firstSelectedCard;
    public CardUI secondSelectedCard;


    #endregion

    #region Private Fields


    #endregion


    #region Public Methods

    public void InitializeUIController()
    {
        MakeInstance();
        UpdateCardsUI(false);
        foreach (var card in therapistCards)
        {
            card.GetComponent<CardUI>().DestroyCard();
        }
        therapistCards.Clear();
        foreach (var card in patientCards)
        {
            card.GetComponent<CardUI>().DestroyCard();
        }
        patientCards.Clear();

        mainGameUI.SetActive(false);
        beforStartUI.SetActive(true);
        backGroundUI.SetActive(true);

        therapistCards = new List<RectTransform>();
        patientCards = new List<RectTransform>();
    }

    public ScreenPart GetScreenPart(Vector2 mousePosition)
    {
        float leftBound, rightBound;

        float mouseX = mousePosition.x * 1920f / (float)Screen.width;
        mouseX -= 1920f / 2f;

        int index = 0; 
        for (int i = 0; i < screenPartBoundsOnX.Count+1; i++)
        {
            if (i == 0)
                leftBound = -1920f / 2f; 
            else
                leftBound = screenPartBoundsOnX[i - 1];

            if (i == screenPartBoundsOnX.Count)
                rightBound = 1920f / 2f;
            else
                rightBound = screenPartBoundsOnX[i];

            //Debug.Log($"leftBound= {leftBound}, rightBound = {rightBound}, Mouse Posotion.x = {mouseX}");

            if (mouseX >= leftBound && mouseX < rightBound)
            {
                index = i;
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
        }
        return ScreenPart.Therapist;
    }

    public void UpdateCardsUI(bool setPosition = true)
    {
        therapistCards.Clear();
        patientCards.Clear();
        for (int i = 0; i < therapistCardsParent.childCount; i++)
        {
            therapistCards.Add(therapistCardsParent.GetChild(i) as RectTransform);
            therapistCardsParent.GetChild(i).GetComponent<CardUI>().UpdateCard(setPosition);
        }
        for (int i = 0; i < patientCardsParent.childCount; i++)
        {
            patientCards.Add(patientCardsParent.GetChild(i) as RectTransform);
            patientCardsParent.GetChild(i).GetComponent<CardUI>().UpdateCard(setPosition);
        }
        List<RectTransform> sortedPatientCardsList = patientCards.OrderBy(o => o.GetComponent<CardUI>().index).ToList();
        patientCards = sortedPatientCardsList;
        List<RectTransform> sortedTherapistCardsList = therapistCards.OrderBy(o => o.GetComponent<CardUI>().index).ToList();
        therapistCards= sortedTherapistCardsList;
    }

    public void AddCardForTherapist(Card card,int index)
    {
        GameObject newCard = (GameObject)Instantiate(cardPrefab, therapistCardsParent);
        CardUI newCardUI = newCard.GetComponent<CardUI>();
        newCardUI.ApplyToCardGameObject(card, CardUIType.TherapistCard);
        newCardUI.ApplyCardMetrics(index, therapistCenteredCardPos, therapistHighestPosOfCard, therapistLowestPosOfCard);
    }

    public void AddCardForPatient(Card card,int index)
    {
        GameObject newCard = Instantiate(cardPrefab, patientCardsParent);
        CardUI newCardUI = newCard.GetComponent<CardUI>();
        newCardUI.ApplyToCardGameObject(card, CardUIType.PatientCard);
        newCardUI.ApplyCardMetrics(index, patientCenteredCardPos, patientHighestPosOfCard, patientLowestPosOfCard);
    }

    //Поставить свою карту в случайное место в руке пациента
    public void PutCardInRandomPlace(CardUI cardUI)
    {
        if (Therapist.instance.therapistCurrentAP - 1 < 0)
        {
            cardUI.AnimateZoomOut(0);
            return;
        }

        UpdateCardsUI(false);

        int tookCardIndex = cardUI.index;
        foreach (var cardT in therapistCards)
        {
            if (cardT.GetComponent<CardUI>().index > tookCardIndex)
                cardT.GetComponent<CardUI>().index--;
        }

        int index = Random.Range(0, patientCardsParent.childCount+1);
        cardUI.ApplyToCardGameObject(cardUI.card, CardUIType.PatientCard);
        cardUI.ApplyCardMetrics(index, patientCenteredCardPos, patientHighestPosOfCard, patientLowestPosOfCard);

        //Debug.Log($"index = {index}");

        for (int i = 0; i < patientCardsParent.childCount; i++)
        {
            int indexnow = patientCardsParent.GetChild(i).GetComponent<CardUI>().index;
            if (indexnow>= index)
            {
                indexnow++;
            }
            patientCardsParent.GetChild(i).GetComponent<CardUI>().ApplyCardMetrics(indexnow, patientCenteredCardPos, patientHighestPosOfCard, patientLowestPosOfCard);
        }

        cardUI.transform.SetParent(patientCardsParent);

        for (int i = 0; i < patientCardsParent.childCount; i++)
        {
            patientCardsParent.GetChild(i).GetComponent<CardUI>().UpdateCard(false);
            patientCardsParent.GetChild(i).GetComponent<CardUI>().MoveCardToPlace();
        }

        for (int i = 0; i < therapistCardsParent.childCount; i++)
        {
            therapistCardsParent.GetChild(i).GetComponent<CardUI>().UpdateCard(false);
            therapistCardsParent.GetChild(i).GetComponent<CardUI>().MoveCardToPlace();
        }
        //Need to Insert and remove card in nessary abstract holders

        Patient.instance.AddCardToHand(cardUI.card, index);
        UpdateCardsUI(false);

        //Update TherapistActionPoints
        if (cardUI.card.cardType == CardTypes.Equipment)
        {
            Therapist.instance.SetActionPoint(Therapist.instance.therapistCurrentAP - 1, Therapist.instance.therapistMaxAP - 1);
        }
        else
        {
            Therapist.instance.SetActionPoint(Therapist.instance.therapistCurrentAP - 1, Therapist.instance.therapistMaxAP);
        }
    }

    //Поменять местами 2 карты (в руке пациента или один из руки пациентаа другой из руки игрока,
    //firstSelectedCard всегда находится в руке пациента, secondSelectedCard может быть или из руки пацента, или из руки теропевта)
    public void CheckSelectedCardUIs()
    {
        if (firstSelectedCard != null && secondSelectedCard != null)
        {
            if (firstSelectedCard.cardUIType != secondSelectedCard.cardUIType)
            {
                if (Therapist.instance.therapistCurrentAP - 3 < 0)
                {
                    ResetSelectedes();
                    return;
                }
            }
            else
            {
                if (Therapist.instance.therapistCurrentAP - 1 < 0)
                {
                    ResetSelectedes();
                    return;
                }
            }

            UpdateCardsUI(false);

            Patient.instance.RemoveCardFromHand(firstSelectedCard.index);
            if (secondSelectedCard.cardUIType == CardUIType.PatientCard)
            {
                Patient.instance.AddCardToHand(firstSelectedCard.card, secondSelectedCard.index);
                Patient.instance.RemoveCardFromHand(secondSelectedCard.index);
            }
            Patient.instance.AddCardToHand(secondSelectedCard.card, firstSelectedCard.index);

            int indexHolder = firstSelectedCard.index;
            Vector2 centerPosHolder = firstSelectedCard.centeredCardPos;
            Vector2 lowestPosHolder = firstSelectedCard.lowestPosOfCard;
            Vector2 highestPosHolder = firstSelectedCard.highestPosOfCard;

            /////Set Patient card settings as Therapist
            firstSelectedCard.ApplyToCardGameObject(firstSelectedCard.card, secondSelectedCard.cardUIType);
            firstSelectedCard.ApplyCardMetrics(secondSelectedCard.index, secondSelectedCard.centeredCardPos, secondSelectedCard.highestPosOfCard, secondSelectedCard.lowestPosOfCard);

            //Debug.Log($"secondSelectedCard.transform.parent name is {transformHolder.parent.gameObject.name}");
            firstSelectedCard.transform.SetParent(secondSelectedCard.transform.parent);

            firstSelectedCard.UpdateCard(false);
            firstSelectedCard.MoveCardToPlace();


            /////Set Therapist card settings as Patient
            secondSelectedCard.ApplyToCardGameObject(secondSelectedCard.card, CardUIType.PatientCard);
            secondSelectedCard.ApplyCardMetrics(indexHolder,centerPosHolder,highestPosHolder,lowestPosHolder);

            //Debug.Log($"transformHolder.parent name is {transformHolder.parent.gameObject.name}");
            secondSelectedCard.transform.SetParent(patientCardsParent);

            secondSelectedCard.UpdateCard(false);
            secondSelectedCard.MoveCardToPlace();

            if (firstSelectedCard.cardUIType != CardUIType.PatientCard)
            {
                Therapist.instance.SetActionPoint(Therapist.instance.therapistCurrentAP - 3, Therapist.instance.therapistMaxAP);
                //NEED TO change deck belonging
            }
            else
            {
                Therapist.instance.SetActionPoint(Therapist.instance.therapistCurrentAP - 1, Therapist.instance.therapistMaxAP);
            }

            UpdateCardsUI(false);
            //reset selectedes
            ResetSelectedes();
        }
    }

    public void AnimatePatientCardsBeforeDrop(float mouseY) 
    {
        float t = Mathf.InverseLerp(patientHighestPosOfCard.y, patientLowestPosOfCard.y, mouseY);
        //float indexByT = t * patientCards.Count;

        float step = 1f / patientCards.Count;

        for (int i = 0; i < patientCards.Count; i++)
        {
            float tForCard = step * i;
            if (tForCard > t)
                tForCard += step;
            patientCards[i].GetComponent<CardUI>().MoveCardToPlace(tForCard);
        }
    }

    //Добавить свою карту в руку пациента в любое место, по желанию игрока
    public void DropCardToPatientHand(CardUI cardUI)
    {
        if (Therapist.instance.therapistCurrentAP - 2 < 0)
        {
            cardUI.AnimateZoomOut(0);
            return;
        }


        UpdateCardsUI(false);

        //detect index
        float t = Mathf.InverseLerp(patientHighestPosOfCard.y, patientLowestPosOfCard.y, cardUI.GetComponent<RectTransform>().anchoredPosition.y);
        float step = 1f / patientCards.Count;
        int index = 0;
        for (int i = 0; i < patientCards.Count; i++)
        {
            float tForCard = step * i;
            if (t >= tForCard && t < tForCard + step)
                index = i + 1;
        }
        cardUI.ApplyToCardGameObject(cardUI.card, CardUIType.PatientCard);
        cardUI.ApplyCardMetrics(index, patientCenteredCardPos, patientHighestPosOfCard, patientLowestPosOfCard);
        
        //
        for (int i = 0; i < patientCardsParent.childCount; i++)
        {
            int indexnow = patientCardsParent.GetChild(i).GetComponent<CardUI>().index;
            if (indexnow >= index)
            {
                indexnow++;
            }
            patientCardsParent.GetChild(i).GetComponent<CardUI>().ApplyCardMetrics(indexnow, patientCenteredCardPos, patientHighestPosOfCard, patientLowestPosOfCard);
        }


        cardUI.transform.SetParent(patientCardsParent);

        for (int i = 0; i < patientCardsParent.childCount; i++)
        {
            patientCardsParent.GetChild(i).GetComponent<CardUI>().UpdateCard(false);
            patientCardsParent.GetChild(i).GetComponent<CardUI>().MoveCardToPlace();
        }

        for (int i = 0; i < therapistCardsParent.childCount; i++)
        {
            therapistCardsParent.GetChild(i).GetComponent<CardUI>().UpdateCard(false);
            therapistCardsParent.GetChild(i).GetComponent<CardUI>().MoveCardToPlace();
        }
        //Need to Insert and remove card in nessary abstract holders

        UpdateCardsUI(false);
        Patient.instance.AddCardToHand(cardUI.card, index);

        Therapist.instance.SetActionPoint(Therapist.instance.therapistCurrentAP - 2, Therapist.instance.therapistMaxAP);
    }

    public void AddPsychosisToPatient()
    {
        UpdateCardsUI(false);

        int index = Random.Range(0, patientCardsParent.childCount + 1);

        GameObject newCard = Instantiate(cardPrefab, patientCardsParent);
        newCard.GetComponent<RectTransform>().localScale = 0.4f * Vector3.one;
        CardUI newCardUI = newCard.GetComponent<CardUI>();
        newCardUI.ApplyToCardGameObject(Cards.Psychosis, CardUIType.PatientCard);
        newCardUI.ApplyCardMetrics(index, patientCenteredCardPos, patientHighestPosOfCard, patientLowestPosOfCard);

        //Debug.Log($"index = {index}");

        for (int i = 0; i < patientCardsParent.childCount; i++)
        {
            int indexnow = patientCardsParent.GetChild(i).GetComponent<CardUI>().index;
            if (indexnow >= index)
            {
                indexnow++;
            }
            patientCardsParent.GetChild(i).GetComponent<CardUI>().ApplyCardMetrics(indexnow, patientCenteredCardPos, patientHighestPosOfCard, patientLowestPosOfCard);
        }

        for (int i = 0; i < patientCardsParent.childCount; i++)
        {
            patientCardsParent.GetChild(i).GetComponent<CardUI>().UpdateCard(false,false);
            patientCardsParent.GetChild(i).GetComponent<CardUI>().AnimateZoomOut(0f);
        }
        //Need to Insert and remove card in nessary abstract holders

        UpdateCardsUI(false);
    }

    #endregion


    #region Private Methods

    private void ResetSelectedes() 
    {
        for (int i = 0; i < patientCardsParent.childCount; i++)
        {
            patientCardsParent.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
        for (int i = 0; i < therapistCardsParent.childCount; i++)
        {
            therapistCardsParent.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
        secondSelectedCard = null;
        firstSelectedCard = null;
    }

    #endregion


    private void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
