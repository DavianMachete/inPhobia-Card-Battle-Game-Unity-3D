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

    [HideInInspector]
    public List<RectTransform> therapistCards;
    [HideInInspector]
    public List<RectTransform> patientCards;

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
    [Header("            Card Movement settings        ")]
    [SerializeField]
    private List<float> screenPartBoundsOnX;
   
    public CardUI firstSelectedCard;
    public CardUI secondSelectedCard;


    #endregion

    #region Private Fields


    #endregion

    #region Public Methods

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

    public void UpdateCards(bool setPosition = true)
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
            return;
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
        //Need to Insert and remove card in nessary abstract holders
        //............
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

    //Поменять местами 2 карты (в руке пациента или одиниз руки пациентаа другой из руки игрока)
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

            int indexHolder = firstSelectedCard.index;
            Vector2 centerPosHolder = firstSelectedCard.centeredCardPos;
            Vector2 lowestPosHolder = firstSelectedCard.lowestPosOfCard;
            Vector2 highestPosHolder = firstSelectedCard.highestPosOfCard;
            RectTransform transformHolder = firstSelectedCard.GetComponent<RectTransform>();

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
                //change deck belonging
            }
            else
            {
                Therapist.instance.SetActionPoint(Therapist.instance.therapistCurrentAP - 1, Therapist.instance.therapistMaxAP);
            }

            //reset selectedes
            ResetSelectedes();
        }
    }

    public void PutCardInPlace(CardUI cardUI)
    {

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
