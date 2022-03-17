using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private Transform threeCardsParent;
    [SerializeField]
    private Transform selectedCardsParent;
    [SerializeField]
    private GameObject cardPrefab;

    private List<Card> selectedCards;
    private List<Card> cardsToUnlock;

    private void Start()
    {
        selectedCards = new List<Card>();

        #region CreateCards

        cardsToUnlock = new List<Card>();

        cardsToUnlock.Add(new Card("Ram", CardTypes.Attack, "Deal damage equal to your block", 1, Rarity.Rare));

        cardsToUnlock.Add(new Card("Entrench", CardTypes.Skill, "Double your current block", 1, Rarity.Rare));

        cardsToUnlock.Add(new Card("Shrug it off", CardTypes.Skill, "Gain 8 block, draw 1 card", 0, Rarity.Rare));

        cardsToUnlock.Add(new Card("Barricade", CardTypes.Equipment, "Block no longer expires at the start of your turn", 0, Rarity.Equipment));

        cardsToUnlock.Add(new Card("Juggernaut", CardTypes.Equipment, "Each time you gain block - deal 5 damage", 0, Rarity.Equipment));

        cardsToUnlock.Add(new Card("Wall of Steel", CardTypes.Equipment, "At the end of your turn: Gain 3 block", 0, Rarity.Equipment));

        cardsToUnlock.Add(new Card("Second Wind",CardTypes.Skill,"Discard your hand. For each card discarded: Gain 5 block",1, Rarity.Common));

        cardsToUnlock.Add(new Card("Inspiration", CardTypes.Skill, "Exhaust. Gain 1 AP, Draw 1 card", 0, Rarity.Common));

        cardsToUnlock.Add(new Card("Parry", CardTypes.Attack, $" Deal 5 damage \n Gain 5 block", 1, Rarity.Common));

        cardsToUnlock.Add(new Card("Dropkick", CardTypes.Attack, "Deal damage equal to your block", 1, Rarity.Common));

        cardsToUnlock.Add(new Card("Wall of Fire", CardTypes.Skill, "Gain 12 Block. This round: Every time an enemy deals damage, deal 4 damage", 2, Rarity.Common));

        cardsToUnlock.Add(new Card("Rage", CardTypes.Skill, "Exhaust. Gain 2 AP", 1, Rarity.Common));

        cardsToUnlock.Add(new Card("Invulnerability", CardTypes.Skill, "Exhaust. Gain 30 block", 0, Rarity.Common));
        #endregion
    }

    #region Public Methods

    public void RandomizeThreeCards() 
    {
        //SelectLeft Card
        List<Card> leftCards = new List<Card>();
        foreach (var item in cardsToUnlock)
        {
            if (item.rarity == Rarity.Equipment ||
                item.rarity == Rarity.Rare) 
            {
                leftCards.Add(item);
            }
        }
        int index = Random.Range(0, 6);
        Card leftCard = leftCards[index];

        threeCardsParent.GetChild(0).GetComponent<CardGameObject>().ApplyToCardGameObject(leftCard);

        //Select median card

        List<Card> medianCards = new List<Card>();
        foreach (var item in cardsToUnlock)
        {
            if (item.rarity == Rarity.Common)
            {
                medianCards.Add(item);
            }
        }
        Card medianCard = medianCards[Random.Range(0, 7)];

        threeCardsParent.GetChild(1).GetComponent<CardGameObject>().ApplyToCardGameObject(medianCard);

        //Select median card

        List<Card> rightCards = new List<Card>();
        int typeForFifty = Random.Range(0, 2);//if 0 then Commo, and if 1 then rare

        foreach (var item in cardsToUnlock)
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

        threeCardsParent.GetChild(2).GetComponent<CardGameObject>().ApplyToCardGameObject(rightCard);
    }

    public void AddSelectedCardToDeck()
    {

    }

    public void AddCardAsSelected(CardGameObject cardGO)
    {
        Card newSelected = new Card(cardGO.name, cardGO.cardType, cardGO.affect, cardGO.actionPoint, cardGO.rarity);

        selectedCards.Add(newSelected);
    }

    public void StartGame() 
    {
        for (int i = selectedCardsParent.childCount-1; i >=0; i--)
        {
            selectedCardsParent.GetChild(i).GetComponent<CardGameObject>().DestroyCard();
        }
        foreach (var item in selectedCards)
        {
            var goUI = Instantiate(cardPrefab, selectedCardsParent);
            goUI.GetComponent<CardGameObject>().ApplyToCardGameObject(item);
        }
    }

    #endregion

}
