using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Patient : NPC
{
    public static Patient instance;

    public Card nextAttackCard;
    public int patientMaxAP = 3;
    public int patientCurrentAP = 3;

    [SerializeField]
    private TMP_Text cardsCountInDeck;
    [SerializeField]
    private TMP_Text actionPointsText;

    private Affect affect;

    private float block;
    private bool blockSaved;

    private bool blockGot;
    private bool attackWhenGetBlock;

    private List<Card> deck;
    [SerializeField]private List<Card> cardsInHand;

    private List<Card> patientStandartCards;

    public void InitializePatient()
    {
        MakeInstance();

        patientMaxAP = 3;
        patientCurrentAP = 3;

        InitializeDeck();

        SetActionPoint(patientCurrentAP, patientMaxAP);
    }

    public void StartTurn()
    {
        if (IStartTurnHelper == null)
        {
            IStartTurnHelper = StartCoroutine(IStartTurn());
        }
        //cardsInHand = UIController.instance.patientCards;
    }

    public void InitializeDeck()
    {
        if (deck == null)
            deck = new List<Card>();
        if (cardsInHand == null)
            cardsInHand = new List<Card>();

        if(patientStandartCards==null)
            patientStandartCards = Cards.PatientStandartCards();
        deck = patientStandartCards;

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

    private void Attack(int damage=-20, int countInStep = 1)
    {
        if (countInStep < 1)
        {
            countInStep = 1;
        }

        if (damage < 0)
            damage = Mathf.RoundToInt(AttackForce);
        Debug.Log($"AttackForce = {AttackForce}, damage = {damage}");

        for (int i = 0; i < countInStep; i++)
        {
            //Phobia.instance.OnEveryDefense.Invoke();
            Phobia.instance.MakeTheDamage(damage);
        }
    }

    public void AddBlock(float block)
    {
        if (block < 0 && this.block<=0)
            return;
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

    public void AddCardToHand(Card card,int index = -2)
    {
        if (index < 0)
            index = Random.Range(0, cardsInHand.Count + 1);
        cardsInHand.Insert(index, card);
    }

    public void RemoveCardFromHand(int index)
    {
        if (index < 0||index>=cardsInHand.Count)
        {
            Debug.Log($"<color=red>Can't</color> remove patient in hand card by index {index}");
        }
        cardsInHand.RemoveAt(index);   
    }

    public void RemoveCardFromDeck(Card card)
    {
        foreach (Card itemCard in deck)
        {
            if (itemCard == card)
            {
                deck.Remove(itemCard);
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

    private Card GetNextAttackCard()
    {
        foreach (var card in cardsInHand)
        {
            if (card.cardType == CardTypes.Attack)
                return card;
        }
        return null;
    }

    private Coroutine IStartTurnHelper;
    private IEnumerator IStartTurn()
    {
        if (affect != null && affect.inPhobia != null && affect.inPhobia.OnTurnStart != null)
            affect.inPhobia.OnTurnStart.Invoke();

        //Get nextAttackCard
        nextAttackCard = GetNextAttackCard();

        Debug.Log($"<color=cyan>Turn Started</color>");

        foreach (var card in cardsInHand)
        {
            affect = card.affect;

            affect.inPhobia.OnStepStart?.Invoke();
            Debug.Log($"<color=cyan>Step Started </color>_{card.cardID}_");

            yield return new WaitForSeconds(2f);//only for testing. the time will be controled in future
                                                //by patient animation and(or) card animation durations;

            if (card.cardType == CardTypes.Attack)
            {
                affect.inPhobia.OnAttack();
                Attack();
                Debug.Log($"<color=cyan>Attacked</color>_{card.cardID}_");
            }

            affect.inPhobia.OnStepEnd?.Invoke();

            yield return new WaitForSeconds(2f);//only for testing. the time will be controled in future
                                                //by patient animation and(or) card animation durations;

            Debug.Log($"<color=cyan>Step Ended</color>_{card.cardID}_");
            Debug.Break();
        }

        affect.inPhobia.OnTurnEnd?.Invoke();
        Debug.Log($"<color=cyan>Turn Ended</color>");
    }

    private void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
