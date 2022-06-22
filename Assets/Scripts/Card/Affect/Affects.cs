public static class Affects
{
    /// <summary>
    /// ---Существа с уязвимостью получают +50% урона․
    /// ---Примичение.---В конце хода снижается на 1․
    /// </summary>
    public static Affect Vulnerablity(int count)
    {
        Affect affect = new Affect();
        //$"Effect: Vulnerable creatures take +50% damage\nNote: Decrease by 1 at end of turn";
        affect.OnStepStart.Add(new InPhobiaAction($"Vulnerablity_{count}".ToLower(), () => PhobiaManager.instance.AddVulnerablity(count), false));
        affect.OnTurnEnd.Add(new InPhobiaAction($"Vulnerablity_{-1}".ToLower(), () => PhobiaManager.instance.AddVulnerablity(-1), true));//Decrease by 1 at end of turn
        return affect;
    }


    /// <summary>
    /// ---Существа со слабостью наносят каждой атакой - x урона․
    /// ---Примичение.---Уменьшается на 1 каждый раз когда наносишь урон․ 
    /// </summary>
    public static Affect Weakness(int weaknesStack)
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"Weakness_{weaknesStack}".ToLower(),() => PhobiaManager.instance.AddWeakness(weaknesStack), false));
        affect.OnDefense.Add(new InPhobiaAction($"Weakness_{-1}".ToLower(), () => PhobiaManager.instance.AddWeakness(-1), true));//Decrease by 1 at end of turn
        return affect;
    }


    /// <summary>
    /// ---Снижает урон на свое количество.
    /// ---Примичение.---Полностью пропадает в начале хода.
    /// </summary>
    public static Affect AddBlock(float block)//??
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"Add Block_{block}".ToLower(), () => PatientManager.instance.AddBlock(block), false));
        //affect.OnTurnEnd.Add(new InPhobiaAction($"Block_{-block}".ToLower(), () => PatientManager.instance.AddBlock(-block), false));
        return affect;
    }

    public static Affect MultiplyBlock(float multiplier)
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"Multiply Block_{multiplier}".ToLower(), () => PatientManager.instance.AddBlock(PatientManager.instance.GetBlock() * (multiplier - 1)), false));
        //affect.OnTurnEnd.Add(new InPhobiaAction($"Block_{-block}".ToLower(), () => PatientManager.instance.AddBlock(-block), false));
        return affect;
    }

    /// <summary>
    /// ---Снижает урон на свое количество.
    /// ---Примичение.---Снижается на 1 каждый раз, когда получаешь урон по хп.
    /// </summary>
    public static Affect Armor(float armor)
    {
        Affect affect = new Affect();
        affect.OnTurnEnd.Add(new InPhobiaAction($"Armor_{armor}".ToLower(), () => PatientManager.instance.AddArmor(armor), false));
        affect.OnDefense.Add(new InPhobiaAction($"Armor_{-1}".ToLower(), () => PatientManager.instance.AddArmor(-1), true));//
        return affect;
    }


    /// <summary>
    /// ---Карта пропадает из игры до конца боя.
    /// </summary>
    public static Affect Exhaust()
    {
        Affect affect = new Affect();
        affect.OnStepEnd.Add(new InPhobiaAction($"Exhaust".ToLower(), () => PatientManager.instance.RemoveCardFromDeck(), false));
        return affect;
    }


    /// <summary>
    /// ---Увеличивает урон на x, где x - значение силы.
    /// </summary>
    public static Affect Power(float damage)
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"Power_{damage}".ToLower(), () => PatientManager.instance.patient.attackForce += damage, false));
        return affect;
    }

    public static Affect SaveBlock()
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"SaveBlock".ToLower(), () => PatientManager.instance.SaveBlock(), true));
        return affect;
    }
    public static Affect AttackWhenGetBlock()
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"AttackWhenGetBlock".ToLower(), () => PatientManager.instance.ActivateAttackWhenGetBlock(), true));
        return affect;
    }

    public static Affect SteelBlock(float value)
    {
        Affect affect = new Affect();
        affect.OnTurnEnd.Add(new InPhobiaAction($"SteelBlock_{value}".ToLower(), () => PatientManager.instance.AddBlock(value), true));
        return affect;
    }
    public static Affect Discard()
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"Discard".ToLower(), () => PatientManager.instance.Discard(), false));
        return affect;
    }

    public static Affect PullCard(int count)
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"PullCard_{count}".ToLower(), () => PatientManager.instance.PullCard(count),false));
        return affect;
    }

    public static Affect AddActionPoint(int value)
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"AddActionPoint_{value}".ToLower(), () => { PatientManager.instance.patient.patientActionPoints += value; }, false)) ;
        return affect;
    }

    public static Affect Attack(float attackForce)
    {
        Affect affect = new Affect();
        affect.OnAttack.Add(new InPhobiaAction($"Attack_{attackForce}".ToLower(), () => PatientManager.instance.SetAttackForce(attackForce), false));
        return affect;
    }

    public static Affect DropKickWithouAttack()
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"DropKickWithoutAttack".ToLower(), () => { if (PhobiaManager.instance.IsPhobiaHaveVulnerablity()) { PatientManager.instance.patient.patientActionPoints++; PatientManager.instance.PullCard(1); } }, false));
        return affect;
    }

    public static Affect AttackOnDefense(float attackForce)
    {
        Affect affect = new Affect();
        affect.OnDefense.Add(new InPhobiaAction($"AttackOnDefense_{attackForce}".ToLower(), () => { PatientManager.instance.ActivateAttackWhenDamaged(); PatientManager.instance.SetAttackForce(attackForce); }, false));
        return affect;
    }

    public static Affect DoubleNextEffect()
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"DoubleNextAttack".ToLower(), () => PatientManager.instance.DoubleNextEffect(), false));
        return affect;
    }
}
