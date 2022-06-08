using System.Collections.Generic;
using UnityEngine;

public static class Cards 
{
    private static List<Card> _TherapistCardsToSelect;
    public static List<Card> TherapistCardsToSelect()
    {
        if (_TherapistCardsToSelect != null)
            return _TherapistCardsToSelect;
        else
            _TherapistCardsToSelect = new List<Card>();

        _TherapistCardsToSelect.Add(new Card("Ram", CardTypes.Attack, Affects.Block(PhobiaManager.instance.phobia.attackForce), "Deal damage equal to your block", 1, Rarity.Rare, CardUIType.TherapistCard));//"Deal damage equal to your block"

        _TherapistCardsToSelect.Add(new Card("Entrench", CardTypes.Skill, Affects.Block(PatientManager.instance.GetBlock() * 2), "Double your current block", 1, Rarity.Rare, CardUIType.TherapistCard));//"Double your current block"

        _TherapistCardsToSelect.Add(new Card("Shrug it off", CardTypes.Skill, Affects.Block(8) + Affects.PullCard(1), "Gain 8 block, draw 1 card", 0, Rarity.Rare, CardUIType.TherapistCard));//"Gain 8 block, draw 1 card"

        _TherapistCardsToSelect.Add(new Card("Barricade", CardTypes.Equipment,Affects.SaveBlock() + Affects.Exhaust(), "Block no longer expires at the start of your turn", 0, Rarity.Equipment, CardUIType.TherapistCard));//"Block no longer expires at the start of your turn"

        _TherapistCardsToSelect.Add(new Card("Juggernaut", CardTypes.Equipment, Affects.AttackWhenGetBlock() + Affects.Exhaust(), "Each time you gain block - deal 5 damage", 0, Rarity.Equipment, CardUIType.TherapistCard));//"Each time you gain block - deal 5 damage"

        _TherapistCardsToSelect.Add(new Card("Steel", CardTypes.Equipment, Affects.SteelBlock(3) + Affects.Exhaust(), "At the end of your turn: Gain 3 block", 0, Rarity.Equipment, CardUIType.TherapistCard));//"At the end of your turn: Gain 3 block"

        //NEed to change
        _TherapistCardsToSelect.Add(new Card("Second Wind", CardTypes.Skill, Affects.Block(5 * UIController.instance.patientCardsInHand.Count) + Affects.Discard(), "Discard your hand. For each card discarded: Gain 5 block", 1, Rarity.Common, CardUIType.TherapistCard));//"Discard your hand. For each card discarded: Gain 5 block"

        _TherapistCardsToSelect.Add(new Card("Inspiration", CardTypes.Skill, Affects.AddActionPoint(1) + Affects.PullCard(1) + Affects.Exhaust(), "Exhaust. Gain 1 AP, Draw 1 card", 0, Rarity.Common, CardUIType.TherapistCard));//"Exhaust. Gain 1 AP, Draw 1 card"

        _TherapistCardsToSelect.Add(new Card("Parry", CardTypes.Attack, Affects.Block(5) + Affects.Attack(5), $" Deal 5 damage \n Gain 5 block", 1, Rarity.Common, CardUIType.TherapistCard));//$" Deal 5 damage \n Gain 5 block"

        _TherapistCardsToSelect.Add(new Card("Dropkick", CardTypes.Attack, Affects.Attack(5) + Affects.DropKickWithouAttack(), "Deal damage equal to your block", 1, Rarity.Common, CardUIType.TherapistCard));//"Deal damage equal to your block"

        _TherapistCardsToSelect.Add(new Card("Wall of Fire", CardTypes.Skill, Affects.Block(12) + Affects.AttackOnDefense(4), "Gain 12 Block. This round: Every time an enemy deals damage, deal 4 damage", 2, Rarity.Common, CardUIType.TherapistCard));//"Gain 12 Block. This round: Every time an enemy deals damage, deal 4 damage"

        _TherapistCardsToSelect.Add(new Card("Rage", CardTypes.Skill, Affects.Exhaust() + Affects.AddActionPoint(2), "Exhaust. Gain 2 AP", 1, Rarity.Common, CardUIType.TherapistCard));//"Exhaust. Add 2 AP"

        _TherapistCardsToSelect.Add(new Card("Invulnerability", CardTypes.Skill, Affects.Block(30), "Exhaust. Gain 30 block", 0, Rarity.Common, CardUIType.TherapistCard));//"Exhaust. Gain 30 block"

        return _TherapistCardsToSelect;
    }


    private static List<Card> _TherapistStandartCards;
    public static List<Card> TherapistStandartCards()
    {
        if (_TherapistStandartCards != null)
            return _TherapistStandartCards;
        else
            _TherapistStandartCards = new List<Card>();

        for (int i = 0; i < 5; i++)
        {
            _TherapistStandartCards.Add(new Card("Helplessness", CardTypes.Skill, Affects.Weakness(2), "Weakens the enemy (2)", 1, Rarity.Common, CardUIType.TherapistCard));
        }

        for (int i = 0; i < 4; i++)
        {
            _TherapistStandartCards.Add(new Card("Support", CardTypes.Skill, Affects.DoubleNextAttack(), "Doubles the next attack", 1, Rarity.Common, CardUIType.TherapistCard));//???ЖЕНЯ
        }

        _TherapistStandartCards.Add(new Card("Steel wall", CardTypes.Skill, Affects.Armor(3f), "Add Armor by 3 points", 2, Rarity.Common, CardUIType.TherapistCard));

        return _TherapistStandartCards;
    }


    private static List<Card> _PatientStandartCards;
    public static List<Card> PatientStandartCards()
    {
        if (_PatientStandartCards != null)
            return _PatientStandartCards;
        else
            _PatientStandartCards = new List<Card>();

        for (int i = 0; i < 5; i++)
        {
            _PatientStandartCards.Add(new Card("Hit", CardTypes.Attack, Affects.Attack(5), "Deal 5 damage", 1, Rarity.Common, CardUIType.PatientCard));
        }

        for (int i = 0; i < 4; i++)
        {
            _PatientStandartCards.Add(new Card("Shield", CardTypes.Skill,Affects.Block(5), "Gain 5 block", 1, Rarity.Common, CardUIType.PatientCard));
        }

        _PatientStandartCards.Add(new Card("Strong beat", CardTypes.Attack, Affects.Attack(8) + Affects.Vulnerablity(2), $"Deal 8 damage and 2 vulnerablity", 2, Rarity.Common, CardUIType.PatientCard));

        return _PatientStandartCards;
    }


    private static Card _Psychosis;
    public static Card Psychosis
    {
        get
        {
            if (_Psychosis == null)
                _Psychosis = new Card("Psychosis", CardTypes.Curse, Affects.Exhaust(), "Psychosis", 1, Rarity.Common,CardUIType.defaultCard);

            return _Psychosis;
        }
    }


    public static void SortDiscards(bool forTherapist)
    {
        string forWho = forTherapist ? "Therapist" : "Patient";
        Debug.Log($"<color=lightblue>SortDiscardCalled for {forWho}</color>");
        List<Card> patientDiscard = new List<Card>(PatientManager.instance.discard);
        foreach (Card card in patientDiscard)
        {
            if (card.cardBelonging == CardUIType.PatientCard && !forTherapist)
            {
                PatientManager.instance.deck.Add(card);
                PatientManager.instance.discard.Remove(card);
            }
            else if (card.cardBelonging == CardUIType.TherapistCard && forTherapist)
            {
                TherapistManager.instance.deck.Add(card);
                PatientManager.instance.discard.Remove(card);
            }
        }

        List<Card> therapistDiscard = new List<Card>(TherapistManager.instance.discard);
        foreach (Card card in therapistDiscard)
        {
            if (card.cardBelonging == CardUIType.PatientCard && !forTherapist)
            {
                PatientManager.instance.deck.Add(card);
                TherapistManager.instance.discard.Remove(card);
            }
            else if (card.cardBelonging == CardUIType.TherapistCard && forTherapist)
            {
                TherapistManager.instance.deck.Add(card);
                TherapistManager.instance.discard.Remove(card);
            }
        }
    }
}
