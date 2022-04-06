using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Patient : NPC
{
    public Phobia enemy;
    public List<Card> staticDeck;
    public Card nextCard;

    private float block;
    private bool blockSaved;

    private bool blockGot;
    private bool attackWhenGetBlock;
    private List<Card> deck;


    private void Start()
    {
        
    }

    public void InitializeDeck()
    {

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
}
