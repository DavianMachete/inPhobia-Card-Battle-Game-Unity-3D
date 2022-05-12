using System;
using UnityEditor;
using UnityEngine;


[CreateAssetMenu(fileName ="New card",menuName ="Card")]
[Serializable]
public class Card : ScriptableObject
{
    public string cardName;

    public CardTypes cardType;

    public Affect affect; //need work on it;

    public string affectDescription;

    public int actionPoint;

    public Rarity rarity;

    public Card()
    {

    }

    public Card(string cardName, CardTypes cardType, Affect affect,string affectDescription, int actionPoint, Rarity rarity)
    {
        this.cardName = cardName;
        this.cardType = cardType;
        this.affect = affect;
        this.affectDescription = affectDescription;
        this.actionPoint = actionPoint;
        this.rarity = rarity;
    }
}
