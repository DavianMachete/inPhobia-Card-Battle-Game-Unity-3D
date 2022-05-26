using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Patient : NPC
{
    public static Patient instance;

    //public Card nextAttackCard;
    public int patientMaxAP = 3;
    public int patientCurrentAP = 3;



    private Affect affect;
    private bool patientCardPlayed = false;



    [SerializeField] private TMP_Text cardsCountInDeck;
    [SerializeField] private TMP_Text actionPointsText;

    [SerializeField] private TMP_Text healthTxtp;

    [SerializeField] private Image healthBarImage;

    [SerializeField] private float maxHealth;

    [SerializeField] private int nextAttackCount = 1;

    [SerializeField] private float block;
    [SerializeField] private bool blockSaved;

    [SerializeField] private bool blockGot;
    [SerializeField] private bool attackWhenGetBlock;

    [SerializeField] private List<Card> deck;
    [SerializeField] private List<Card> cardsInHand;
    [SerializeField] private List<Card> playedCards;

    [SerializeField] private List<Card> patientStandartCards;

    public void InitializePatient()
    {
        MakeInstance();



        patientMaxAP = 3;
        patientCurrentAP = 3;

        InitializeDeck();
        UpdateHealthBar();
        SetActionPoint(patientCurrentAP, patientMaxAP);
    }

    public void StartTurn()
    {
        if (IStartTurnHelper == null)
        {
            IStartTurnHelper = StartCoroutine(IStartTurn());
        }
    }

    public void InitializeDeck()
    {
        if (deck == null)
            deck = new List<Card>();
        if (cardsInHand == null)
            cardsInHand = new List<Card>();
        if (playedCards == null)
            playedCards = new List<Card>();


        if (patientStandartCards == null || patientStandartCards.Count < 1)
            patientStandartCards = Cards.PatientStandartCards();

        deck = patientStandartCards;
        playedCards.Clear();
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

    public void DoubleNextAttack()
    {
        nextAttackCount *= 2;
    }

    private void Attack()
    {
        Phobia.instance.MakeTheDamage(Mathf.RoundToInt(AttackForce));
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

    public void SetAttackForce(float force)
    {
        AttackForce = force;
        //Debug.Log($"<color=teal>NPC:</color> attackForce =  {force}");
    }

    public void MakeTheDamage(float damage)
    {
        affect.inPhobia.OnDefense?.Invoke();

        if (block > 0)
        {
            damage -= block;
        }
        if (damage < 0)
            damage = 0f;
        Health -= damage;
        UpdateHealthBar();
    }





    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = Health / maxHealth;
        healthTxtp.text = Mathf.RoundToInt(Health).ToString();
    }



    private Coroutine IStartTurnHelper;
    private IEnumerator IStartTurn()
    {
        playedCards.Clear();
        if (affect != null && affect.inPhobia != null && affect.inPhobia.OnTurnStart != null)
            affect.inPhobia.OnTurnStart.Invoke();

        //Get nextAttackCard
        //nextAttackCard = GetNextAttackCard();

        Debug.Log($"<color=cyan>Turn Started</color>");

        foreach (Card card in cardsInHand)
        {
            if (patientCurrentAP < card.actionPoint)
                break;
            patientCurrentAP -= card.actionPoint;
            actionPointsText.text = patientCurrentAP.ToString() + "/" + patientMaxAP.ToString();

            affect = card.affect;

            affect.inPhobia.OnStepStart?.Invoke();
            Debug.Log($"<color=cyan>Step Started </color>_{card.cardID}_");

            patientCardPlayed = false;
            UIController.instance.PlayPatientTopCard(() => { patientCardPlayed = true; });



            yield return new WaitUntil(()=>patientCardPlayed);//only for testing. the time will be controled in future
                                                //by patient animation and(or) card animation durations;

            if (card.cardType == CardTypes.Attack)
            {
                affect.inPhobia.OnAttack?.Invoke();
                for (int i = 0; i < nextAttackCount; i++)
                {
                    Attack();
                    Debug.Log($"<color=cyan>Attacked</color>_{card.cardID}_");
                    yield return new WaitForSeconds(1f);
                }
                nextAttackCount = 1;//is Next Attack count saved, when turn ended?
            }

            affect.inPhobia.OnStepEnd?.Invoke();

            //yield return new WaitForSeconds(2f);//only for testing. the time will be controled in future
                                                //by patient animation and(or) card animation durations;

            Debug.Log($"<color=cyan>Step Ended</color>_{card.cardID}_");
            playedCards.Add(card);
            //cardsInHand.Remove(card);
            //Debug.Break();
        }


        Debug.Log($"<color=cyan>Turn Ended</color>");

        RemovePlayedCards();

        Phobia.instance.StartTurn(()=>
        {
            affect.inPhobia.OnTurnEnd?.Invoke();
        });

        IStartTurnHelper = null;
    }


    private void RemovePlayedCards()
    {
        foreach (var playedCard in playedCards)
        {
            foreach (var cardInHand in cardsInHand)
            {
                if (playedCard.cardID == cardInHand.cardID)
                {
                    cardsInHand.Remove(playedCard);
                    break;
                }
            }
        }
    }


    private void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
