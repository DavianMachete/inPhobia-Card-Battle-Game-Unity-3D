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

        _TherapistCardsToSelect.Add(new Card("Деперсонализация", CardTypes.Attack, Affects.AddBlock(PhobiaManager.instance.phobia.attackForce), "Deal damage equal to your block", 1, Rarity.Rare, CardUIType.defaultCard));//"Deal damage equal to your block"

        _TherapistCardsToSelect.Add(new Card("Личные границы", CardTypes.Skill, Affects.MultiplyBlock(2), "Double your current block", 1, Rarity.Rare, CardUIType.defaultCard));//"Double your current block"

        _TherapistCardsToSelect.Add(new Card("Shrug it off", CardTypes.Skill, Affects.AddBlock(8) + Affects.PullCard(1), "Gain 8 block, draw 1 card", 0, Rarity.Rare, CardUIType.TherapistCard));//"Gain 8 block, draw 1 card"

        _TherapistCardsToSelect.Add(new Card("Гипноз", CardTypes.Skill, Affects.AddActionPoints(1,0) + Affects.PullCard(1), "Gain 1 AP, Draw 1 card", 0, Rarity.Rare, CardUIType.defaultCard));//"Gain 1 AP, Draw 1 card"

        _TherapistCardsToSelect.Add(new Card("Осозннаность", CardTypes.Attack, Affects.Attack(1, 5), "Deal 1 damage 5 times", 0, Rarity.Rare, CardUIType.defaultCard));

        _TherapistCardsToSelect.Add(new Card("Самовнушение", CardTypes.Equipment,Affects.SaveBlock() /*+ Affects.Exhaust()*/, "Block no longer expires at the start of your turn", 0, Rarity.Equipment, CardUIType.defaultCard));//"Block no longer expires at the start of your turn"

        //_TherapistCardsToSelect.Add(new Card("Juggernaut", CardTypes.Equipment, Affects.AttackWhenGetBlock() /*+ Affects.Exhaust()*/, "Each time you gain block - deal 5 damage", 0, Rarity.Equipment, CardUIType.TherapistCard));//"Each time you gain block - deal 5 damage"

        _TherapistCardsToSelect.Add(new Card("Отзеркаливание", CardTypes.Equipment, Affects.GiveEnemyWeaknessOnHit() /*+ Affects.Exhaust()*/, "Each hit gives the enemy 1 stack of weakness", 0, Rarity.Equipment, CardUIType.defaultCard));//"Each time you gain block - deal 5 damage"

        //_TherapistCardsToSelect.Add(new Card("Steel", CardTypes.Equipment, Affects.SteelBlock(3) /*+ Affects.Exhaust()*/, "At the end of your turn: Gain 3 block", 0, Rarity.Equipment, CardUIType.TherapistCard));//"At the end of your turn: Gain 3 block"

        _TherapistCardsToSelect.Add(new Card("Компенсация", CardTypes.Equipment, Affects.AddActionPoints(1, 1) /*+ Affects.Exhaust()*/, "Added 1 action point to maximum action points", 0, Rarity.Equipment, CardUIType.defaultCard));//"At the end of your turn: Gain 3 block"

        _TherapistCardsToSelect.Add(new Card("Трансформация", CardTypes.Skill, Affects.AddBlock(5 * CardManager.instance.patientCardsInHand.Count) + Affects.Discard(), "Discard your hand. For each card discarded: Gain 5 block", 2, Rarity.Common, CardUIType.defaultCard));//"Discard your hand. For each card discarded: Gain 5 block"

        _TherapistCardsToSelect.Add(new Card("Когниция", CardTypes.Skill, Affects.AddActionPoints(1,0) + Affects.PullCard(1) + Affects.Exhaust(), "Exhaust. Gain 1 AP, Draw 1 card", 0, Rarity.Common, CardUIType.defaultCard));//"Exhaust. Gain 1 AP, Draw 1 card"

        _TherapistCardsToSelect.Add(new Card("Невроз", CardTypes.Attack, Affects.AddBlock(5) + Affects.Attack(5,1), $" Deal 5 damage \n Gain 5 block", 1, Rarity.Common, CardUIType.defaultCard));//$" Deal 5 damage \n Gain 5 block"

        _TherapistCardsToSelect.Add(new Card("Гиперактивность", CardTypes.Attack, Affects.Attack(2, 3), "Deal 2 damage 3 times", 1, Rarity.Common, CardUIType.defaultCard));

        _TherapistCardsToSelect.Add(new Card("Wall of Fire", CardTypes.Skill, Affects.AddBlock(12) + Affects.AttackOnDefense(4), "Gain 12 Block. This round: Every time an enemy deals damage, deal 4 damage", 2, Rarity.Common, CardUIType.defaultCard));//"Gain 12 Block. This round: Every time an enemy deals damage, deal 4 damage"

        _TherapistCardsToSelect.Add(new Card("Аффект", CardTypes.Skill, Affects.Exhaust() + Affects.AddActionPoints(2,0), "Exhaust. Gain 2 AP", 1, Rarity.Common, CardUIType.defaultCard));//"Exhaust. Add 2 AP"

        _TherapistCardsToSelect.Add(new Card("Мобилизация", CardTypes.Skill, Affects.Exhaust() + Affects.AddBlock(30), "Exhaust. Gain 30 block", 1, Rarity.Common, CardUIType.defaultCard));//"Exhaust. Gain 30 block"

        _TherapistCardsToSelect.Add(new Card("Внутренний ресурс", CardTypes.Attack, Affects.Attack(5,1) + Affects.PullCard(1), $" Deal 5 damage \n Draw 1 card", 0, Rarity.Common, CardUIType.defaultCard));//$" Deal 5 damage \n Draw 1 card"

        _TherapistCardsToSelect.Add(new Card("Решительность", CardTypes.Attack, Affects.Attack(10,1) + Affects.Power(2), $" Deal 10 damage \n Gain 2 power", 2, Rarity.Common, CardUIType.defaultCard));//$" Deal 10 damage \n Gain 2 power"

        _TherapistCardsToSelect.Add(new Card("Психосоматика", CardTypes.Attack, Affects.Attack(7,1) + Affects.Weakness(1), $" Deal 7 damage \n Weakens the enemy (1)", 1, Rarity.Common, CardUIType.defaultCard));//$" Deal 7 damage \n Weakens the enemy (1)"

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
            _TherapistStandartCards.Add(new Card("Беспомощность", CardTypes.Skill, Affects.Weakness(2), "Weakens the enemy (2)", 1, Rarity.Common, CardUIType.defaultCard));
        }

        for (int i = 0; i < 4; i++)
        {
            _TherapistStandartCards.Add(new Card("Support", CardTypes.Skill, Affects.DoubleNextEffect(), "Doubles the next attack", 1, Rarity.Common, CardUIType.defaultCard));//???ЖЕНЯ
        }

        _TherapistStandartCards.Add(new Card("Независимость", CardTypes.Skill, Affects.Armor(3f), "Add Armor by 3 points", 2, Rarity.Common, CardUIType.defaultCard));

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
            _PatientStandartCards.Add(new Card("Аффирмация", CardTypes.Attack, Affects.Attack(5,1), "Deal 5 damage", 1, Rarity.Common, CardUIType.defaultCard));
        }

        for (int i = 0; i < 4; i++)
        {
            _PatientStandartCards.Add(new Card("Самолюбие", CardTypes.Skill, Affects.AddBlock(5), "Gain 5 block", 1, Rarity.Common, CardUIType.defaultCard));
        }

        _PatientStandartCards.Add(new Card("Сепарация", CardTypes.Attack, Affects.Attack(8,1) + Affects.Weakness(3), $"Deal 8 damage and 3 weakness", 2, Rarity.Common, CardUIType.defaultCard));

        return _PatientStandartCards;
    }


    private static Card _Psychosis;
    public static Card Psychosis
    {
        get
        {
            if (_Psychosis == null)
                _Psychosis = new Card("Psychosis", CardTypes.Curse, Affects.Exhaust(), "Psychosis", 1, Rarity.Common,CardUIType.defaultCard);////????????????????????????

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
