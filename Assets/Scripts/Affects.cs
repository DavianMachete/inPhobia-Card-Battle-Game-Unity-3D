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
        affect.OnStepStart.Add(new InPhobiaAction($"Vulnerablity_{count}".ToLower(), () => Phobia.instance.AddVulnerablity(count), false));
        affect.OnTurnEnd.Add(new InPhobiaAction($"Vulnerablity_{-1}".ToLower(), () => Phobia.instance.AddVulnerablity(-1), true));//Decrease by 1 at end of turn
        return affect;
    }


    /// <summary>
    /// ---Существа со слабостью наносят каждой атакой - x урона․
    /// ---Примичение.---Уменьшается на 1 каждый раз когда наносишь урон․ 
    /// </summary>
    public static Affect Weakness(int weaknesStack)
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"Weakness_{weaknesStack}".ToLower(),() => Phobia.instance.AddWeakness(weaknesStack), false));
        affect.OnDefense.Add(new InPhobiaAction($"Weakness_{-1}".ToLower(), () => Phobia.instance.AddWeakness(-1), true));//Decrease by 1 at end of turn
        return affect;
    }


    /// <summary>
    /// ---Снижает урон на свое количество.
    /// ---Примичение.---Полностью пропадает в начале хода.
    /// </summary>
    public static Affect Block(float block)//??
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"Block_{block}".ToLower(), () => Patient.instance.AddBlock(block), false));
        affect.OnTurnEnd.Add(new InPhobiaAction($"Block_{-block}".ToLower(), () => Patient.instance.AddBlock(-block), false));
        return affect;
    }


    /// <summary>
    /// ---Снижает урон на свое количество.
    /// ---Примичение.---Снижается на 1 каждый раз, когда получаешь урон по хп.
    /// </summary>
    public static Affect Armor(float armor)
    {
        Affect affect = new Affect();
        affect.OnTurnEnd.Add(new InPhobiaAction($"Armor_{armor}".ToLower(), () => Patient.instance.AddArmor(armor), false));
        affect.OnDefense.Add(new InPhobiaAction($"Armor_{-1}".ToLower(), () => Patient.instance.AddArmor(-1), true));//
        return affect;
    }


    /// <summary>
    /// ---Карта пропадает из игры до конца боя.
    /// </summary>
    public static Affect Exhaust()
    {
        Affect affect = new Affect();
        affect.OnStepEnd.Add(new InPhobiaAction($"Exhaust".ToLower(), () => Patient.instance.RemoveCardFromDeck(), false));
        return affect;
    }


    /// <summary>
    /// ---Увеличивает урон на x, где x - значение силы.
    /// </summary>
    public static Affect Power(float damage)
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"Power_{damage}".ToLower(), () => Patient.instance.AttackForce += damage, false));
        return affect;
    }

    public static Affect SaveBlock()
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"SaveBlock".ToLower(), () => Patient.instance.SaveBlock(), false));
        return affect;
    }
    public static Affect AttackWhenGetBlock()
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"AttackWhenGetBlock".ToLower(), () => Patient.instance.ActivateAttackWhenGetBlock(), false));
        return affect;
    }

    public static Affect SteelBlock(float value)
    {
        Affect affect = new Affect();
        affect.OnTurnEnd.Add(new InPhobiaAction($"SteelBlock_{value}".ToLower(), () => Patient.instance.AddBlock(value), true));
        return affect;
    }
    public static Affect Discard()
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"Discard".ToLower(), () => Patient.instance.Discard(), false));
        return affect;
    }

    public static Affect PullCard(int count)
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"PullCard_{count}".ToLower(), () => Patient.instance.PullCard(count),false));
        return affect;
    }

    public static Affect AddActionPoint(int value)
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"AddActionPoint_{value}".ToLower(), () => Patient.instance.SetActionPoint(Patient.instance.patientCurrentAP + value, Patient.instance.patientMaxAP),false));
        return affect;
    }

    public static Affect Attack(float attackForce)
    {
        Affect affect = new Affect();
        affect.OnAttack.Add(new InPhobiaAction($"Attack_{attackForce}".ToLower(), () => Patient.instance.SetAttackForce(attackForce), false));
        return affect;
    }

    public static Affect DropKickWithouAttack()
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"DropKickWithoutAttack".ToLower(), () => { if (Phobia.instance.IsPhobiaHaveVulnerablity()) { Patient.instance.patientCurrentAP++; Patient.instance.PullCard(1); } }, false));
        return affect;
    }

    public static Affect AttackOnDefense(float attackForce)
    {
        Affect affect = new Affect();
        affect.OnDefense.Add(new InPhobiaAction($"AttackOnDefense_{attackForce}".ToLower(), () => { Patient.instance.ActivateAttackWhenDamaged(); Patient.instance.SetAttackForce(attackForce); }, false));
        return affect;
    }

    public static Affect DoubleNextAttack()
    {
        Affect affect = new Affect();
        affect.OnStepStart.Add(new InPhobiaAction($"DoubleNextAttack".ToLower(), () => Patient.instance.DoubleNextAttack(), false));
        return affect;
    }
}
