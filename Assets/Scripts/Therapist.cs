using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Therapist : MonoBehaviour
{
    public static Therapist instance;

    public List<Card> staticDeck;
    public int therapistMaxAP = 5;
    public int therapistCurrentAP = 5;

    [SerializeField]
    private TMP_Text cardsCountInDeck;
    [SerializeField]
    private TMP_Text actionPointsText;


    private List<Card> deck;
    private List<Card> cardsInHand;

    public void InitializeTherapist()
    {
        MakeInstance();
    }

    public void InitializeTherapistDeck(List<Card> staticDeck)
    {
        if (deck == null)
            deck = new List<Card>();
        if (cardsInHand == null)
            cardsInHand = new List<Card>();

        this.staticDeck = staticDeck;

        deck = this.staticDeck;

        therapistMaxAP = 5;
        therapistCurrentAP = 5;

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
        cardsCountInDeck.text = deck.Count.ToString();
        actionPointsText.text = therapistCurrentAP.ToString() + "/" + therapistMaxAP.ToString();
        UIController.instance.UpdateCardsUI(true);
    }

    public void SetActionPoint(int current,int max)
    {
        therapistCurrentAP = current;
        therapistMaxAP = max;
        actionPointsText.text = current.ToString() + "/" + max.ToString();
    }


    private void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
