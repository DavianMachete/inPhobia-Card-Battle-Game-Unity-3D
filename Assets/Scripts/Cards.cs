using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cards 
{
    private static Card cardInspiration;
    private static Card cardRage;

    public static List<Card> TherapistCardsToSelect()
    {
        List<Card> cards = new List<Card>();

        cards.Add(new Card("Ram", CardTypes.Attack, Affects.Block(Phobia.instance.AttackForce, Patient.instance), "Deal damage equal to your block", 1, Rarity.Rare));//"Deal damage equal to your block"

        cards.Add(new Card("Entrench", CardTypes.Skill, 2 * Affects.Block(Phobia.instance.AttackForce, Patient.instance), "Double your current block", 1, Rarity.Rare));//"Double your current block"

        cards.Add(new Card("Shrug it off", CardTypes.Skill,
            Affects.Block(8, Patient.instance) +
            new Affect(() => Patient.instance.PullCard(1), InPhobiaEventType.OnEveryStepStart),
            "Gain 8 block, draw 1 card", 0, Rarity.Rare));//"Gain 8 block, draw 1 card"

        cards.Add(new Card("Barricade", CardTypes.Equipment, new Affect(() => Patient.instance.SaveBlock(), InPhobiaEventType.OnStepEnd), "Block no longer expires at the start of your turn", 0, Rarity.Equipment));//"Block no longer expires at the start of your turn"

        cards.Add(new Card("Juggernaut", CardTypes.Equipment, new Affect(() => Patient.instance.ActivateAttackWhenGetBlock(), InPhobiaEventType.OnEveryStepStart), "Each time you gain block - deal 5 damage", 0, Rarity.Equipment));//"Each time you gain block - deal 5 damage"

        cards.Add(new Card("Wall of Steel", CardTypes.Equipment, new Affect(() => Patient.instance.AddBlock(3), InPhobiaEventType.OnEveryStepEnd), "At the end of your turn: Gain 3 block", 0, Rarity.Equipment));//"At the end of your turn: Gain 3 block"

        //NEed to change
        cards.Add(new Card("Second Wind", CardTypes.Skill, Affects.Block(5, Patient.instance)/*new Affect(())*/, "Discard your hand. For each card discarded: Gain 5 block", 1, Rarity.Common));//"Discard your hand. For each card discarded: Gain 5 block"

        cardInspiration = new Card("Inspiration", CardTypes.Skill, Affects.Exhaust(Patient.instance, cardInspiration) + new Affect(() => { Patient.instance.patientCurrentAP++; Patient.instance.PullCard(1); }, InPhobiaEventType.OnStepStart), "Exhaust. Gain 1 AP, Draw 1 card", 0, Rarity.Common);
        cards.Add(cardInspiration);//"Exhaust. Gain 1 AP, Draw 1 card"

        cards.Add(new Card("Parry", CardTypes.Attack, Affects.Block(5, Patient.instance) + new Affect(() => Patient.instance.Attack(5), InPhobiaEventType.OnStepStart), $" Deal 5 damage \n Gain 5 block", 1, Rarity.Common));//$" Deal 5 damage \n Gain 5 block"

        cards.Add(new Card("Dropkick", CardTypes.Attack, new Affect(() => Patient.instance.Attack(5), InPhobiaEventType.OnStepStart) + new Affect(() => { if (Phobia.instance.vulnerablityCount > 0) { Patient.instance.patientCurrentAP++; Patient.instance.PullCard(1); } }, InPhobiaEventType.OnStepStart), "Deal damage equal to your block", 1, Rarity.Common));//"Deal damage equal to your block"

        cards.Add(new Card("Wall of Fire", CardTypes.Skill, Affects.Block(12, Patient.instance) + new Affect(() => Patient.instance.Attack(4), InPhobiaEventType.OnEveryDefense), "Gain 12 Block. This round: Every time an enemy deals damage, deal 4 damage", 2, Rarity.Common));//"Gain 12 Block. This round: Every time an enemy deals damage, deal 4 damage"

        cardRage = new Card("Rage", CardTypes.Skill, Affects.Exhaust(Patient.instance, cardRage) + new Affect(() => Patient.instance.patientCurrentAP += 2, InPhobiaEventType.OnStepStart), "Exhaust. Gain 2 AP", 1, Rarity.Common);
        cards.Add(cardRage);//"Exhaust. Add 2 AP"

        cards.Add(new Card("Invulnerability", CardTypes.Skill, Affects.Block(30, Patient.instance), "Exhaust. Gain 30 block", 0, Rarity.Common));//"Exhaust. Gain 30 block"

        return cards;
    }

    public static List<Card> TherapistStandartCards()
    {
        List<Card> cards = new List<Card>();

        for (int i = 0; i < 5; i++)
        {
            cards.Add(new Card("Helplessness", CardTypes.Skill, Affects.Weakness(2, Phobia.instance), "Weakens the enemy", 1, Rarity.Common));
        }

        for (int i = 0; i < 4; i++)
        {
            cards.Add(new Card("Support", CardTypes.Skill, new Affect(()=> { if (Patient.instance.nextCard.cardType == CardTypes.Attack) Patient.instance.nextCard.affect *= 2; }, InPhobiaEventType.OnStepStart), "Doubles the next attack", 1, Rarity.Common));//???ЖЕНЯ
        }

        cards.Add(new Card("Steel wall", CardTypes.Skill, Affects.Armor(3, Patient.instance), "Add Armor by 3 points", 2, Rarity.Common));

        return cards;
    }

    public static List<Card> PatientStandartCards()
    {
        List<Card> cards = new List<Card>();

        for (int i = 0; i < 5; i++)
        {
            cards.Add(new Card("Hit", CardTypes.Attack, new Affect(() => Patient.instance.Attack(5),InPhobiaEventType.OnStepStart), "Deal 5 damage", 1, Rarity.Common));
        }

        for (int i = 0; i < 4; i++)
        {
            cards.Add(new Card("Shield", CardTypes.Skill,Affects.Block(5, Patient.instance), "Gain 5 block", 1, Rarity.Common));
        }

        cards.Add(new Card("Strong beat", CardTypes.Attack, new Affect(() => Patient.instance.Attack(8), InPhobiaEventType.OnStepStart)+(Affects.Vulnerablity(Patient.instance, Phobia.instance) *2), $"Deal 5 damage \n and 2 vulnerablity", 2, Rarity.Common));

        return cards;
    }

    public static Card Psychosis
    {
        get
        {
            Card card = new Card("Psychosis", CardTypes.Curse, null, "Psychosis", 1, Rarity.Common);

            return card;
        }
    }
}
