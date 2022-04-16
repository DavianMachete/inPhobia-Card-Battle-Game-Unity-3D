using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Patient : NPC
{
    public static Patient instance;

    public List<Card> staticDeck;
    public Card nextCard;
    public int patientMaxAP = 3;
    public int patientCurrentAP = 3;

    [SerializeField]
    private TMP_Text cardsCountInDeck;
    [SerializeField]
    private TMP_Text actionPointsText;

    private float block;
    private bool blockSaved;

    private bool blockGot;
    private bool attackWhenGetBlock;

    private List<Card> deck;
    private List<Card> cardsInHand;


    public void InitializePatient()
    {
        MakeInstance();

        patientMaxAP = 3;
        patientCurrentAP = 3;

        InitializeDeck();

        SetActionPoint(patientCurrentAP, patientMaxAP);
    }

    public void InitializeDeck()
    {
        if (deck == null)
            deck = new List<Card>();
        if (cardsInHand == null)
            cardsInHand = new List<Card>();

        staticDeck = Cards.PatientStandartCards();
        deck = staticDeck;

        cardsInHand.Clear();
        for (int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, deck.Count);
            cardsInHand.Add(deck[index]);
            deck.RemoveAt(index);
        }
        for (int i = 0; i < cardsInHand.Count; i++)
        {
            int index = i;
            UIController.instance.AddCardForPatient(cardsInHand[index], index);
        }
        UIController.instance.UpdateCardsUI(true);
        cardsCountInDeck.text = deck.Count.ToString();
    }

    public void PullCard(int count)
    {
        //need add functionality
    }

    public void SaveBlock() 
    {
        blockSaved = true;
    }

    public void ActivateAttackWhenGetBlock()
    {
        attackWhenGetBlock = true;
    }

    public void Attack(int damage, int countInStep = 1)
    {
        if (countInStep < 1)
        {
            countInStep = 1;
        }

        for (int i = 0; i < countInStep; i++)
        {
            OnEveryAttack.Invoke();
            Phobia.instance.OnEveryDefense.Invoke();
            Phobia.instance.Health -= damage;
        }
    }

    public void AddBlock(float block)
    {
        if (block < 0 && blockSaved)
            return;
        this.block += block;
        if (attackWhenGetBlock)
        {
            float newAttackForce = AttackForce;
            AttackForce = 5;
            //Attack();
            AttackForce = newAttackForce;
        }
    }

    public void RemoveCardFromDeck(Card card)
    {
        foreach (Card itemCard in staticDeck)
        {
            if (itemCard == card)
            {
                staticDeck.Remove(itemCard);
                //Continuted
                break;
            }
        }
    }

    public void SetActionPoint(int current, int max)
    {
        patientCurrentAP = current;
        patientMaxAP = max;
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
