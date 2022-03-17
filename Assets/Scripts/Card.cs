using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardTypes
{
    Attack = 0,
    Skill = 1,
    Equipment=2,
    Curse = 3
}

public enum Rarity
{
    Rare,
    Equipment,
    Common
}

public class Card
{
    public string cardName;

    public CardTypes cardType;

    //block
    //power
    //vulnerablity
    //armor
    //exhaust
    //weakness
    //working onit yet

    public string affect; //need work on it;

    public int actionPoint;

    public Rarity rarity;

    public Card()
    {

    }
    public Card(string cardName, CardTypes cardType, string affect, int actionPoint, Rarity rarity)
    {
        this.cardName = cardName;
        this.cardType = cardType;
        this.affect = affect;
        this.actionPoint = actionPoint;
        this.rarity = rarity;
    }
}
