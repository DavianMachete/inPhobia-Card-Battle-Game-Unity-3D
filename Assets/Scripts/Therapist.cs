using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Therapist : MonoBehaviour
{
    public List<Card> staticDeck;

    [SerializeField]
    private TMP_Text cardsCountInDeck;
    [SerializeField]
    private TMP_Text actionPointsText;


    private List<Card> deck;
    private List<Card> cardsInHand;

    private int therapistMaxAP = 5;
    private int therapistCurrentAP = 5;

    public void InitializeTherapist()
    {
        if (deck == null)
            deck = new List<Card>();
        if (cardsInHand == null)
            cardsInHand = new List<Card>();

        deck = staticDeck;

        
        cardsInHand.Clear();
        for (int i = 0; i < 4; i++)
        {
            int index = Random.Range(0, deck.Count);
            cardsInHand.Add(deck[index]);
            deck.RemoveAt(index);
        }
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            int index = i;
            UIController.instance.AddCardForTherapist(cardsInHand[index], index);
        }
        UIController.instance.UpdateCards();
        cardsCountInDeck.text = deck.Count.ToString();
        SetActionPoint(therapistCurrentAP, therapistMaxAP);
    }

    private void SetActionPoint(int current,int max)
    {
        actionPointsText.text = current.ToString() + "/" + max.ToString();
    }
}
