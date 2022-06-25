using System;
using UnityEngine;


//[CreateAssetMenu(fileName ="New card",menuName ="Card")]
[Serializable]
public class Card //: ScriptableObject
{
    public string name;
 
    public string cardID;

    public CardTypes cardType;

    public Affect affect; //need work on it;

    public string affectDescription;

    public int actionPoint;

    public Rarity rarity;

    public CardUIType cardBelonging = CardUIType.defaultCard;

    private static int id;

    public Card(Card card)
    {
        cardID = card.cardID;//Guid.NewGuid().ToString("N");
        name = card.name;
        this.cardType = card.cardType;
        this.affect = card.affect;
        this.affectDescription = card.affectDescription;
        this.actionPoint = card.actionPoint;
        this.rarity = card.rarity;
        this.cardBelonging = card.cardBelonging;
    }

    public Card(string cardName, CardTypes cardType, Affect affect, string affectDescription, int actionPoint, Rarity rarity, CardUIType cardBelonging)
    {
        id++;
        cardID = cardName.ToLower() + $"_{id}";//Guid.NewGuid().ToString("N");
        //Debug.Log(cardID);
        name = cardName;
        this.cardType = cardType;
        this.affect = affect;
        this.affectDescription = affectDescription;
        this.actionPoint = actionPoint;
        this.rarity = rarity;
        this.cardBelonging = cardBelonging;
    }
}
