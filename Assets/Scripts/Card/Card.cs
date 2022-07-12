using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New card", menuName = "ScriptableObjects/Card", order = 1)]
[Serializable]
public class Card : ScriptableObject
{
    public Sprite cardImageSprite;

    public CardTypes cardType;

    public string affectDescription;

    public int actionPoint;

    public Rarity rarity;

    public CardUIType cardBelonging = CardUIType.defaultCard;

    [HideInInspector] public string cardID;


    [NonReorderable] public List<AffectHolder> affects;


    private static int id;

    private List<Affect> _affects;

    public Card(Card card)
    {
        cardID = card.cardID;//Guid.NewGuid().ToString("N");
        name = card.name;
        cardImageSprite = card.cardImageSprite;
        cardType = card.cardType;
        affects = card.affects;
        affectDescription = card.affectDescription;
        actionPoint = card.actionPoint;
        rarity = card.rarity;
        cardBelonging = card.cardBelonging;
    }

    public Card(string cardName,Sprite cardImageSprite, CardTypes cardType, List<AffectHolder> affects, string affectDescription, int actionPoint, Rarity rarity, CardUIType cardBelonging)
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

    public List<Affect> GetAffects()
    {
        if (_affects == null)
            _affects = new List<Affect>();
        _affects.Clear();

        foreach (AffectHolder ah in affects)
        {
            _affects.Add(ah.affect);
        }
        return _affects;
    }
}
