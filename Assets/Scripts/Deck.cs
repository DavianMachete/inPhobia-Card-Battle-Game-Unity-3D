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

    private List<Card> deck;
    private List<Card> selectedCards;
    private List<Card> cardsToUnlock;
    

    public Phobia phobia;
    public Patient patient;
    private Card cardRage;
    private Card cardInspiration;

    private void Start()
    {
        deck = new List<Card>();
        selectedCards = new List<Card>();

        foreach (var item in cardsToUnlock)
        {
            deck.Add(item);
        }
    }


    #region PrivateMethods


    #endregion

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

    public void AddCardAsSelected(CardGameObject cardGO)
    {
        Card newSelected = new Card(cardGO.name, cardGO.cardType, cardGO.affect, cardGO.affectDescription, cardGO.actionPoint, cardGO.rarity);

        selectedCards.Add(newSelected);
    }

    public void StartGame()
    {
        Vector2 stepForSum = new Vector2(320f, 0f);
        Vector2 firstLeftPos = new Vector2(-160 * (selectedCards.Count - 1),0f);

        for (int i = 0; i < selectedCards.Count; i++)
        {
            var goUI = Instantiate(cardPrefab, selectedCardsParent);
            goUI.GetComponent<CardGameObject>().ApplyToCardGameObject(selectedCards[i]);
            goUI.GetComponent<RectTransform>().anchoredPosition = firstLeftPos + stepForSum * i;
            goUI.GetComponent<RectTransform>().localScale = Vector3.one * 0.8f;
        }
    }

    public void RestartGame()
    {
        selectedCards.Clear();

        for (int i = selectedCardsParent.childCount - 1; i >= 0; i--)
        {
            selectedCardsParent.GetChild(i).GetComponent<CardGameObject>().DestroyCard();
        }
    }

    public void RemoveCardFromDeck(Card card)
    {
        deck.Remove(card);
    }

    #endregion
}
