using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Patient : NPC
{
    public Phobia enemy;
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


    private void Start()
    {
        staticDeck = Cards.PatientStandartCards(this,enemy);
        InitializeDeck();
    }

    public void InitializeDeck()
    {
        if (deck == null)
            deck = new List<Card>();
        if (cardsInHand == null)
            cardsInHand = new List<Card>();

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
        UIController.instance.UpdateCards();
        cardsCountInDeck.text = deck.Count.ToString();
        SetActionPoint(patientCurrentAP, patientMaxAP);
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
            enemy.OnEveryDefense.Invoke();
            enemy.Health -= damage;
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

    private void SetActionPoint(int current, int max)
    {
        actionPointsText.text = current.ToString() + "/" + max.ToString();
    }
}
