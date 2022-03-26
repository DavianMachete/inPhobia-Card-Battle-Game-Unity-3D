using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField]
    private Transform threeCardsParent;
    [SerializeField]
    private Transform selectedCardsParent;
    [SerializeField]
    private GameObject cardPrefab;

    private List<Card> selectedCards;
    private List<Card> cardsToUnlock;

    private Phobia phobia;
    private Patient patient;

    private void Start()
    {
        selectedCards = new List<Card>();

        #region CreateCards

        cardsToUnlock = new List<Card>();

        cardsToUnlock.Add(new Card("Ram", CardTypes.Attack, Affects.Block(phobia.damage, patient), "Deal damage equal to your block", 1, Rarity.Rare));//"Deal damage equal to your block"

        cardsToUnlock.Add(new Card("Entrench", CardTypes.Skill, 2 * Affects.Block(phobia.damage, patient), "Double your current block", 1, Rarity.Rare));//"Double your current block"

        cardsToUnlock.Add(new Card("Shrug it off", CardTypes.Skill, Affects.Block(8, patient) + new Affect(null, () => patient.SuggestPullCard(1)), "Gain 8 block, draw 1 card", 0, Rarity.Rare));//"Gain 8 block, draw 1 card"

        cardsToUnlock.Add(new Card("Barricade", CardTypes.Equipment, new Affect(null,()=>patient.SaveBlock()),"Block no longer expires at the start of your turn", 0, Rarity.Equipment));//"Block no longer expires at the start of your turn"

        //cardsToUnlock.Add(new Card("Juggernaut", CardTypes.Equipment, patient.OnEveryStepStart+= "Each time you gain block - deal 5 damage", 0, Rarity.Equipment));//"Each time you gain block - deal 5 damage"

        //cardsToUnlock.Add(new Card("Wall of Steel", CardTypes.Equipment, "At the end of your turn: Gain 3 block", 0, Rarity.Equipment));//"At the end of your turn: Gain 3 block"

        //cardsToUnlock.Add(new Card("Second Wind",CardTypes.Skill,"Discard your hand. For each card discarded: Gain 5 block",1, Rarity.Common));//"Discard your hand. For each card discarded: Gain 5 block"

        //cardsToUnlock.Add(new Card("Inspiration", CardTypes.Skill, "Exhaust. Gain 1 AP, Draw 1 card", 0, Rarity.Common));//"Exhaust. Gain 1 AP, Draw 1 card"

        //cardsToUnlock.Add(new Card("Parry", CardTypes.Attack, $" Deal 5 damage \n Gain 5 block", 1, Rarity.Common));//$" Deal 5 damage \n Gain 5 block"

        //cardsToUnlock.Add(new Card("Dropkick", CardTypes.Attack, "Deal damage equal to your block", 1, Rarity.Common));//"Deal damage equal to your block"

        //cardsToUnlock.Add(new Card("Wall of Fire", CardTypes.Skill, "Gain 12 Block. This round: Every time an enemy deals damage, deal 4 damage", 2, Rarity.Common));//"Gain 12 Block. This round: Every time an enemy deals damage, deal 4 damage"

        //cardsToUnlock.Add(new Card("Rage", CardTypes.Skill, "Exhaust. Gain 2 AP", 1, Rarity.Common));//"Exhaust. Gain 2 AP"

        //cardsToUnlock.Add(new Card("Invulnerability", CardTypes.Skill, "Exhaust. Gain 30 block", 0, Rarity.Common));//"Exhaust. Gain 30 block"
        #endregion
    }

    #region Public Methods

    public void RandomizeThreeCards() 
    {
        //SelectLeft Card
        List<Card> leftCards = new List<Card>();
        foreach (var item in cardsToUnlock)
        {
            if (item.rarity == Rarity.Equipment ||
                item.rarity == Rarity.Rare) 
            {
                leftCards.Add(item);
            }
        }
        int index = Random.Range(0, 6);
        Card leftCard = leftCards[index];

        threeCardsParent.GetChild(0).GetComponent<CardGameObject>().ApplyToCardGameObject(leftCard);

        //Select median card

        List<Card> medianCards = new List<Card>();
        foreach (var item in cardsToUnlock)
        {
            if (item.rarity == Rarity.Common)
            {
                medianCards.Add(item);
            }
        }
        Card medianCard = medianCards[Random.Range(0, 7)];

        threeCardsParent.GetChild(1).GetComponent<CardGameObject>().ApplyToCardGameObject(medianCard);

        //Select median card

        List<Card> rightCards = new List<Card>();
        int typeForFifty = Random.Range(0, 2);//if 0 then Commo, and if 1 then rare

        foreach (var item in cardsToUnlock)
        {
            if (typeForFifty == 0)
            {
                if (item.rarity == Rarity.Common)
                {
                    rightCards.Add(item);
                }
            }
            else 
            {
                if (item.rarity == Rarity.Rare)
                {
                    rightCards.Add(item);
                }
            }
        }

        Card rightCard = rightCards[Random.Range(0, rightCards.Count)];

        threeCardsParent.GetChild(2).GetComponent<CardGameObject>().ApplyToCardGameObject(rightCard);
    }

    public void AddCardAsSelected(CardGameObject cardGO)
    {
        //Card newSelected = new Card(cardGO.name, cardGO.cardType,cardGO.af, cardGO.affect, cardGO.actionPoint, cardGO.rarity);

        //selectedCards.Add(newSelected);
    }

    public void StartGame()
    {
        Vector2 stepForSum = new Vector2(320f, 0f);
        Vector2 firstLeftPos = new Vector2(-160 * (selectedCards.Count - 1),0f);

        for (int i = 0; i < selectedCards.Count; i++)
        {
            var goUI = Instantiate(cardPrefab, selectedCardsParent);
            goUI.GetComponent<CardGameObject>().ApplyToCardGameObject(selectedCards[i]);
            goUI.GetComponent<RectTransform>().anchoredPosition = firstLeftPos + stepForSum * i;
            goUI.GetComponent<RectTransform>().localScale = Vector3.one * 0.8f;
        }
    }

    public void RestartGame()
    {
        selectedCards.Clear();

        for (int i = selectedCardsParent.childCount - 1; i >= 0; i--)
        {
            selectedCardsParent.GetChild(i).GetComponent<CardGameObject>().DestroyCard();
        }
    }

    #endregion
}
