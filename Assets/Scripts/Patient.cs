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

    public List<Card> deck;
    public List<Card> Hand;
    public List<Card> discard;



    [SerializeField]private Affect affect;

    [SerializeField] private TMP_Text cardsCountInDeck;
    [SerializeField] private TMP_Text actionPointsText;

    [SerializeField] private TMP_Text healthTxtp;

    [SerializeField] private Image healthBarImage;

    [SerializeField] private float maxHealth;

    [SerializeField] private int nextAttackCount = 1;

    [SerializeField] private float armor = 0;

    [SerializeField] private float block;
    [SerializeField] private bool blockSaved;

    [SerializeField] private bool attackWhenDamaged;
    [SerializeField] private bool attackWhenGetBlock;

    private bool patientCardPlayed = false;
    private bool removeCurrentCardFromDeck = false;

    public void InitializePatient()
    {
        MakeInstance();

        if (affect == null)
            affect = new Affect();
        affect.Clear();

        patientMaxAP = 3;
        patientCurrentAP = 3;

        Health = maxHealth;

        InitializeDeck();
        UpdateHealthBar();
        SetActionPoint(patientCurrentAP, patientMaxAP);
    }

    public void StartTurn()
    {
        UIController.instance.SetInteractable(false);
        if (IStartTurnHelper == null)
        {
            IStartTurnHelper = StartCoroutine(IStartTurn());
        }
    }

    public void InitializeDeck()
    {
        if (Hand == null)
            Hand = new List<Card>();
        Hand.Clear();

        if (discard == null)
            discard = new List<Card>();
        discard.Clear();
        
        deck = new List<Card>(Cards.PatientStandartCards());


        PrepareNewTurn();
    }

    public void PrepareNewTurn()
    {
        SetActionPoint(patientMaxAP, patientMaxAP);

        Discard();
        PullCard(3);

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
            Cards.SortDiscards(false);
            for (int i = 0; i < count - deckCount; i++)
            {
                PullACard();
            }
        }

    }

    public void Discard()
    {
        discard.AddRange(Hand);
        Hand.Clear();
        UIController.instance.Discard(CardUIType.PatientCard);
    }

    public void SaveBlock() 
    {
        blockSaved = true;
    }

    public void ActivateAttackWhenGetBlock()
    {
        attackWhenGetBlock = true;
    }
    public void ActivateAttackWhenDamaged()
    {
        attackWhenDamaged = true;
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
        Debug.Log($"<color=cyan>Block added </color>value = {block} ");
        this.block += block;

        if (attackWhenGetBlock)
        {
            SetAttackForce(5);
            Attack();
        }
    }

    public int GetBlock()
    {
        return Mathf.RoundToInt(block);
    }

    public void AddArmor(float armor)
    {
        Debug.Log($"<color=cyan>Armor added </color>value = {armor} ");
        this.armor += armor;
        if (this.armor < 0)
            this.armor = 0;
    }

    public void RemoveCardFromHand(Card card)
    {
        if (!Hand.Contains(card))
        {
            Debug.Log($"<color=red>Can't</color> remove card({card.cardID}) from patient in hand cards cause it doesnt contain that");
            return;
        }
        Hand.Remove(card);   
    }

    public void AddCardToHand(int index, Card card)
    {
        if (index < 0 || index >= Hand.Count+1)
        {
            Debug.Log($"<color=red>Can't</color> add card ({card.cardID}) to patient in hand cards by index {index}");
        }
        Hand.Insert(index,card);
    }

    public void RemoveCardFromDeck()//callonly after played
    {
        removeCurrentCardFromDeck = true;
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
        affect.Invoke(affect.OnDefense);

        damage -= block;
        damage -= armor;

        if (damage > 0)
        {
            Health -= damage;
            if (attackWhenDamaged)
            {
                Attack();
            }
        }
        if (Health <= 0)
        {
            GameManager.instance.LevelFailed();
        }
        UpdateHealthBar();
    }




    private void PullACard()
    {
        int index = Random.Range(0, deck.Count);

        UIController.instance.PullCardForPatient(deck[index]);
        deck.RemoveAt(index);
        cardsCountInDeck.text = deck.Count.ToString();
    }
    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = Health / maxHealth;
        healthTxtp.text = Mathf.RoundToInt(Health).ToString();
    }



    private Coroutine IStartTurnHelper;
    private IEnumerator IStartTurn()
    {
        if (!blockSaved)
        {
            block = 0;
        }

        affect.Invoke(InPhobiaEventType.OnTurnStart);
        Debug.Log($"<color=cyan>Turn Started</color>");



        //foreach (Card card in Hand)
        int cardCountInHand = Hand.Count;
        //while()
        for (int i = 0; i < cardCountInHand; i++)
        {
            if (patientCurrentAP < Hand[i].actionPoint)//card.actionPoint)
                break;

            string currentCardID = Hand[i].cardID;

            Debug.Log($"Current card is {currentCardID}");
            if (i + 1 < cardCountInHand)
                Debug.Log($"Next card will be {currentCardID}");
            else
                Debug.Log($"And itsthe last card in Hand");

            affect += Hand[i].affect;// card.affect;
            //Debug.Break();
            affect.Update();
            yield return new WaitForFixedUpdate();
            //Debug.Break();

            Debug.Log($"<color=cyan>Step Started </color>with card -> {currentCardID}");// card.cardID}");

            patientCurrentAP -= Hand[i].actionPoint;// card.actionPoint;
            actionPointsText.text = patientCurrentAP.ToString() + "/" + patientMaxAP.ToString();

            patientCardPlayed = false;
            UIController.instance.PlayPatientTopCard(() => 
            { 
                patientCardPlayed = true; 
            });


            yield return new WaitUntil(()=>patientCardPlayed);
            yield return new WaitForFixedUpdate();


            affect.Invoke(InPhobiaEventType.OnStepStart);
            cardCountInHand = Hand.Count;

            if (Hand.Count > 0)
            {
                if (Hand[i].cardType /*card.cardType*/ == CardTypes.Attack)
                {
                    affect.Invoke(InPhobiaEventType.OnAttack);
                    for (int j = 0; j < nextAttackCount; j++)
                    {
                        Attack();
                        Debug.Log($"<color=cyan>Attacked</color>_{currentCardID/*card.cardID*/}_");
                        yield return new WaitForSeconds(1f);
                    }
                    nextAttackCount = 1;//is Next Attack count saved, when turn ended?
                }
            }
            affect.Invoke(InPhobiaEventType.OnStepEnd);

            Debug.Log($"<color=cyan>Step Ended</color>_{currentCardID/*card.cardID*/}_");
            if (!removeCurrentCardFromDeck && Hand.Count > 0)
                discard.Add(Hand[i]);
            else
                removeCurrentCardFromDeck = false;
        }

        Debug.Log($"<color=cyan>Turn Ended</color>");

        RemovePlayedCards();

        Phobia.instance.StartTurn(()=>
        {
            affect.Invoke(InPhobiaEventType.OnTurnEnd);

            UIController.instance.SetInteractable(true);

            GameManager.instance.PlayNextTurn();
        });


        IStartTurnHelper = null;
    }


    private void RemovePlayedCards()
    {
        foreach (var playedCard in discard)
        {
            foreach (var cardInHand in Hand)
            {
                if (playedCard.cardID == cardInHand.cardID)
                {
                    Hand.Remove(playedCard);
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
