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

    private List<Card> therapistCardsToSelect;
    private List<Card> therapistStandartCards;

    #region Public Methods

    public void RandomizeThreeCards()
    {
        if (selectedCards == null)
            selectedCards = new List<Card>();
        selectedCards.Clear();

        //SelectLeft Card
        List<Card> leftCards = new List<Card>();

        if(therapistCardsToSelect==null)
            therapistCardsToSelect = Cards.TherapistCardsToSelect();
        foreach (var item in therapistCardsToSelect)
        {
            if (item.rarity == Rarity.Equipment ||
                item.rarity == Rarity.Rare)
            {
                leftCards.Add(item);
            }
        }
        int index = Random.Range(0, 6);
        Card leftCard = leftCards[index];

        threeCardsParent.GetChild(0).GetComponent<CardUI>().ApplyToCardGameObject(leftCard);

        //Select median card

        List<Card> medianCards = new List<Card>();
        foreach (var item in Cards.TherapistCardsToSelect())
        {
            if (item.rarity == Rarity.Common)
            {
                medianCards.Add(item);
            }
        }
        Card medianCard = medianCards[Random.Range(0, 7)];

        threeCardsParent.GetChild(1).GetComponent<CardUI>().ApplyToCardGameObject(medianCard);

        //Select median card

        List<Card> rightCards = new List<Card>();
        int typeForFifty = Random.Range(0, 2);//if 0 then Commo, and if 1 then rare

        foreach (var item in Cards.TherapistCardsToSelect())
        {
            if (typeForFifty == 0)
            {
                if (item.rarity == Rarity.Common)
                {
                    rightCards.Add(item);
                }
            }
            else
            {
                if (item.rarity == Rarity.Rare)
                {
                    rightCards.Add(item);
                }
            }
        }

        Card rightCard = rightCards[Random.Range(0, rightCards.Count)];

        threeCardsParent.GetChild(2).GetComponent<CardUI>().ApplyToCardGameObject(rightCard);
    }

    public void AddCardAsSelected(CardUI cardGO)
    {
        Card newSelected = new Card(cardGO.card.cardName, cardGO.card.cardType, cardGO.card.affect, cardGO.card.affectDescription, cardGO.card.actionPoint, cardGO.card.rarity);

        selectedCards.Add(newSelected);
    }

    public void StartGame()
    {
        if(therapistStandartCards==null)
            therapistStandartCards = Cards.TherapistStandartCards();
        if (selectedCards == null)
            selectedCards = new List<Card>();
        List<Card>  staticDeck = new List<Card>(therapistStandartCards.Count + selectedCards.Count);

        staticDeck.AddRange(therapistStandartCards);
        staticDeck.AddRange(selectedCards);

        Therapist.instance.InitializeTherapistDeck(staticDeck);
    }

    #endregion
}
