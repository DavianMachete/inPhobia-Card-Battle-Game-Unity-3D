using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Therapist : MonoBehaviour
{
    public static Therapist instance;

    public int therapistMaxAP = 5;
    public int therapistCurrentAP = 5;

    public List<Card> deck;
    public List<Card> hand;
    public List<Card> discard;

    [SerializeField]
    private TMP_Text cardsCountInDeck;
    [SerializeField]
    private TMP_Text actionPointsText;

    public void InitializeTherapist()
    {
        MakeInstance();
    }

    public void InitializeTherapistDeck(List<Card> staticDeck)
    {
        if (deck == null)
            deck = new List<Card>();
        deck.Clear();

        if (hand == null)
            hand = new List<Card>();
        hand.Clear();

        if (discard == null)
            discard = new List<Card>();
        discard.Clear();

        deck = staticDeck;

        therapistMaxAP = 5;
        therapistCurrentAP = 5;

        PrepareNewTurn();
    }

    public void PrepareNewTurn()
    {
        SetActionPoint(therapistMaxAP, therapistMaxAP);

        Discard();
        PullCard(5);

        UIController.instance.UpdateCards(true);
    }

    public void PullCard(int count)
    {
        if (deck.Count >= count)
        {
            for (int i = 0; i < count; i++)
            {
                PullACard();
            }
        }
        else
        {
            int deckCount = deck.Count;
            for (int i = 0; i < deckCount; i++)
            {
                PullACard();
            }
            Cards.SortDiscards();
            for (int i = 0; i < count - deckCount; i++)
            {
                PullACard();
            }
        }

    }

    public void Discard()
    {
        discard.AddRange(hand);
        hand.Clear();
        UIController.instance.Discard(CardUIType.TherapistCard);
    }

    public void RemoveCardFromHand(Card card)
    {
        if (!hand.Contains(card))
        {
            Debug.Log($"<color=red>Can't</color> remove card({card.cardID}) from therapist in hand cards cause it doesnt contain that");
            return;
        }
        hand.Remove(card);
    }

    public void AddCardToHand(int index, Card card)
    {
        if (index < 0 || index >= hand.Count + 1)
        {
            Debug.Log($"<color=red>Can't</color> add card ({card.cardID}) to therapist in hand cards by index {index}");
        }
        hand.Insert(index, card);
    }

    public void SetActionPoint(int current,int max)
    {
        therapistCurrentAP = current;
        therapistMaxAP = max;
        actionPointsText.text = current.ToString() + "/" + max.ToString();
    }

    private void PullACard()
    {
        int index = Random.Range(0, deck.Count);

        UIController.instance.PullCardForTherapist(deck[index]);
        deck.RemoveAt(index);
        cardsCountInDeck.text = deck.Count.ToString();
    }

    private void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
