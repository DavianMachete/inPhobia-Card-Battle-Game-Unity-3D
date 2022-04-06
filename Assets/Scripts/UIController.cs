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

    //[SerializeField]
    //private float cardAnimationSpeed;

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

    #endregion


    #region Private Methods

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
    }

    #endregion
}
