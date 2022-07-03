using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New card", menuName = "ScriptableObjects/Card", order = 1)]
[Serializable]
public class Card : ScriptableObject
{
    public Sprite cardImageSprite;
 
    public string cardID;

    public CardTypes cardType;

    public List<Affect> affects; 

    public string affectDescription;

    public int actionPoint;

    public Rarity rarity;

    public CardUIType cardBelonging = CardUIType.defaultCard;

    private static int id;

    public Card(Card card)
    {
        cardID = card.cardID;//Guid.NewGuid().ToString("N");
        name = card.name;
        cardImageSprite = card.cardImageSprite;
        cardType = card.cardType;
        affect = card.affect;
        affectDescription = card.affectDescription;
        actionPoint = card.actionPoint;
        rarity = card.rarity;
        cardBelonging = card.cardBelonging;
    }

    public Card(string cardName,Sprite cardImageSprite, CardTypes cardType, List<Affect> affects, string affectDescription, int actionPoint, Rarity rarity, CardUIType cardBelonging)
    {
        id++;
        cardID = cardName.ToLower() + $"_{id}";//Guid.NewGuid().ToString("N");
        //Debug.Log(cardID);
        name = cardName;
        this.cardImageSprite = cardImageSprite;
        this.cardType = cardType;
        this.affects = affects;
        this.affectDescription = affectDescription;
        this.actionPoint = actionPoint;
        this.rarity = rarity;
        this.cardBelonging = cardBelonging;
    }
}
