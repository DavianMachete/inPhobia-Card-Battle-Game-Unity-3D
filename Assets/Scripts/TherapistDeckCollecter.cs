using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TherapistDeckCollecter : MonoBehaviour
{
    [SerializeField]
    private GameObject cardPrefab;
    [Space(20f)]

    [Header("Card Collecter Part")]
    [SerializeField]
    private Transform threeCardsParent;


    private List<Card> selectedCards;

    [SerializeField] private List<Card> therapistCardsToSelect;

    [SerializeField] private List<Card> rareCards;
    [SerializeField] private List<Card> equipmentCards;
    [SerializeField] private List<Card> commonCards;

    #region Public Methods

    public void InitializeCollecter()
    {
        therapistCardsToSelect = new List<Card>(Cards.TherapistCardsToSelect());

        PrepareCardsToSelect();
    }

    public void PrepareCardsToSelect()
    {
        if (rareCards == null)
            rareCards = new List<Card>();
        if (equipmentCards == null)
            equipmentCards = new List<Card>();
        if (commonCards == null)
            commonCards = new List<Card>();

        rareCards.Clear();
        equipmentCards.Clear();
        commonCards.Clear();

        foreach (Card card in therapistCardsToSelect)
        {
            if (card.rarity == Rarity.Rare)
            {
                rareCards.Add(card);
            }
            else if (card.rarity == Rarity.Equipment)
            {
                equipmentCards.Add(card);
            }
            else
            {
                commonCards.Add(card);
            }
        }
    }

    public void RandomizeThreeCards()
    {
        List<Card> rareAndEquipment = new List<Card>();
        rareAndEquipment.AddRange(rareCards);
        rareAndEquipment.AddRange(equipmentCards);

        Card leftCard = rareAndEquipment[Random.Range(0, rareAndEquipment.Count)];
        if (leftCard.rarity == Rarity.Rare)
        {
            rareCards.Remove(leftCard);
        }
        else if(leftCard.rarity==Rarity.Equipment)
        {
            equipmentCards.Remove(leftCard);
        }
        threeCardsParent.GetChild(0).GetComponent<CardController>().SetCardParametersToGameObject(leftCard);

        Card medianCard = commonCards[Random.Range(0, commonCards.Count)];
        commonCards.Remove(medianCard);
        threeCardsParent.GetChild(1).GetComponent<CardController>().SetCardParametersToGameObject(medianCard);

        Card rightCard;
        if (rareCards.Count > 0)
        {
            int ff = Random.Range(0, 2);
            if (ff == 0)
            {
                rightCard = rareCards[Random.Range(0, rareCards.Count)];
                rareCards.Remove(rightCard);
            }
            else
            {
                rightCard = commonCards[Random.Range(0, commonCards.Count)];
                commonCards.Remove(rightCard);
            }
        }
        else
        {
            rightCard = commonCards[Random.Range(0, commonCards.Count)];
            commonCards.Remove(rightCard);
        }
        threeCardsParent.GetChild(2).GetComponent<CardController>().SetCardParametersToGameObject(rightCard);
    }

    public void AddCardToTherapistDeck(CardController cardGameObject)
    {
        Card selectedCard = cardGameObject.card;

        therapistCardsToSelect.Remove(selectedCard);

        Therapist.instance.AddToDeck(selectedCard);

        PrepareCardsToSelect();
    }

    #endregion

    #region Private Methods

    #endregion
}
