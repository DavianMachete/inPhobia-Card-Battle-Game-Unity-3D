using System.Collections.Generic;

public static class Cards 
{
    private static Card cardInspiration;
    private static Card cardRage;

    private static List<Card> _TherapistCardsToSelect;
    public static List<Card> TherapistCardsToSelect()
    {
        if (_TherapistCardsToSelect != null)
            return _TherapistCardsToSelect;
        else
            _TherapistCardsToSelect = new List<Card>();

        _TherapistCardsToSelect.Add(new Card("Ram", CardTypes.Attack, Affects.Block(Phobia.instance.AttackForce), "Deal damage equal to your block", 1, Rarity.Rare));//"Deal damage equal to your block"

        _TherapistCardsToSelect.Add(new Card("Entrench", CardTypes.Skill, 2 * Affects.Block(Phobia.instance.AttackForce), "Double your current block", 1, Rarity.Rare));//"Double your current block"

        _TherapistCardsToSelect.Add(new Card("Shrug it off", CardTypes.Skill,
            Affects.Block(8) +
            new Affect(() => Patient.instance.PullCard(1), InPhobiaEventType.OnStepStart),
            "Gain 8 block, draw 1 card", 0, Rarity.Rare));//"Gain 8 block, draw 1 card"

        _TherapistCardsToSelect.Add(new Card("Barricade", CardTypes.Equipment, new Affect(() => Patient.instance.SaveBlock(), InPhobiaEventType.OnStepEnd), "Block no longer expires at the start of your turn", 0, Rarity.Equipment));//"Block no longer expires at the start of your turn"

        _TherapistCardsToSelect.Add(new Card("Juggernaut", CardTypes.Equipment, new Affect(() => Patient.instance.ActivateAttackWhenGetBlock(), InPhobiaEventType.OnTurnStart), "Each time you gain block - deal 5 damage", 0, Rarity.Equipment));//"Each time you gain block - deal 5 damage"

        _TherapistCardsToSelect.Add(new Card("Wall of Steel", CardTypes.Equipment, new Affect(() => Patient.instance.AddBlock(3), InPhobiaEventType.OnStepEnd), "At the end of your turn: Gain 3 block", 0, Rarity.Equipment));//"At the end of your turn: Gain 3 block"

        //NEed to change
        _TherapistCardsToSelect.Add(new Card("Second Wind", CardTypes.Skill, Affects.Block(5)/*new Affect(())*/, "Discard your hand. For each card discarded: Gain 5 block", 1, Rarity.Common));//"Discard your hand. For each card discarded: Gain 5 block"

        cardInspiration = new Card("Inspiration", CardTypes.Skill, Affects.Exhaust(cardInspiration) + new Affect(() => { Patient.instance.patientCurrentAP++; Patient.instance.PullCard(1); }, InPhobiaEventType.OnStepStart), "Exhaust. Gain 1 AP, Draw 1 card", 0, Rarity.Common);
        _TherapistCardsToSelect.Add(cardInspiration);//"Exhaust. Gain 1 AP, Draw 1 card"

        _TherapistCardsToSelect.Add(new Card("Parry", CardTypes.Attack, Affects.Block(5) + new Affect(() => Patient.instance.SetAttackForce(5f), InPhobiaEventType.OnStepStart), $" Deal 5 damage \n Gain 5 block", 1, Rarity.Common));//$" Deal 5 damage \n Gain 5 block"

        _TherapistCardsToSelect.Add(new Card("Dropkick", CardTypes.Attack, new Affect(() => Patient.instance.SetAttackForce(5f), InPhobiaEventType.OnStepStart) + new Affect(() => { if (Phobia.instance.IsPhobiaHaveVulnerablity()) { Patient.instance.patientCurrentAP++; Patient.instance.PullCard(1); } }, InPhobiaEventType.OnStepStart), "Deal damage equal to your block", 1, Rarity.Common));//"Deal damage equal to your block"

        _TherapistCardsToSelect.Add(new Card("Wall of Fire", CardTypes.Skill, Affects.Block(12) + new Affect(() => Patient.instance.SetAttackForce(4f), InPhobiaEventType.OnDefense), "Gain 12 Block. This round: Every time an enemy deals damage, deal 4 damage", 2, Rarity.Common));//"Gain 12 Block. This round: Every time an enemy deals damage, deal 4 damage"

        cardRage = new Card("Rage", CardTypes.Skill, Affects.Exhaust(cardRage) + new Affect(() => Patient.instance.patientCurrentAP += 2, InPhobiaEventType.OnStepStart), "Exhaust. Gain 2 AP", 1, Rarity.Common);
        _TherapistCardsToSelect.Add(cardRage);//"Exhaust. Add 2 AP"

        _TherapistCardsToSelect.Add(new Card("Invulnerability", CardTypes.Skill, Affects.Block(30), "Exhaust. Gain 30 block", 0, Rarity.Common));//"Exhaust. Gain 30 block"

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
            _TherapistStandartCards.Add(new Card("Helplessness", CardTypes.Skill, Affects.Weakness(2), "Weakens the enemy", 1, Rarity.Common));
        }

        for (int i = 0; i < 4; i++)
        {
            _TherapistStandartCards.Add(new Card("Support", CardTypes.Skill, new Affect(() => Patient.instance.DoubleNextAttack(), InPhobiaEventType.OnStepStart), "Doubles the next attack", 1, Rarity.Common));//???ЖЕНЯ
        }

        _TherapistStandartCards.Add(new Card("Steel wall", CardTypes.Skill, Affects.Armor(3f), "Add Armor by 3 points", 2, Rarity.Common));

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
            _PatientStandartCards.Add(new Card("Hit", CardTypes.Attack, new Affect(() => Patient.instance.SetAttackForce(5f), InPhobiaEventType.OnAttack), "Deal 5 damage", 1, Rarity.Common));
        }

        for (int i = 0; i < 4; i++)
        {
            _PatientStandartCards.Add(new Card("Shield", CardTypes.Skill,Affects.Block(5), "Gain 5 block", 1, Rarity.Common));
        }

        _PatientStandartCards.Add(new Card("Strong beat", CardTypes.Attack, new Affect(() => Patient.instance.SetAttackForce(8f), InPhobiaEventType.OnStepStart) + (Affects.Vulnerablity() * 2), $"Deal 8 damage and 2 vulnerablity", 2, Rarity.Common));

        return _PatientStandartCards;
    }


    private static Card _Psychosis;
    public static Card Psychosis
    {
        get
        {
            if (_Psychosis == null)
                _Psychosis = new Card("Psychosis", CardTypes.Curse, null, "Psychosis", 1, Rarity.Common);

            return _Psychosis;
        }
    }
}
