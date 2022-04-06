using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Therapist : MonoBehaviour
{
    public List<Card> staticDeck;

    [SerializeField]
    private TMP_Text cardsCountInDeck;


    private List<Card> deck;
    private List<Card> cardsInHand;


    public void InitializeDeck()
    {
        if (deck == null)
            deck = new List<Card>();
        if (cardsInHand == null)
            cardsInHand = new List<Card>();

        deck = staticDeck;

        cardsCountInDeck.text = deck.Count.ToString();
        cardsInHand.Clear();
        for (int i = 0; i < 4; i++)
        {
            int index=Random.Range(0, deck.Count);
            cardsInHand.Add(deck[index]);
            deck.RemoveAt(index);
        }
        foreach (var card in cardsInHand)
        {
            UIController.instance.AddCardForTherapist(card);
        }
        UIController.instance.UpdateCards();
    }
}
