public static class Affects
{
    private static Affect affect;


    /// <summary>
    /// ---Существа с уязвимостью получают +50% урона․
    /// ---Примичение.---В конце хода снижается на 1․
    /// </summary>
    public static Affect Vulnerablity()//Patient Affect
    {
        ResetAffect();
        //$"Effect: Vulnerable creatures take +50% damage\nNote: Decrease by 1 at end of turn";
        affect.inPhobia.OnStepStart = () => Phobia.instance.AddVulnerablity(1);
        affect.inPhobia.OnTurnEnd = () => Phobia.instance.AddVulnerablity(-1);//Decrease by 1 at end of turn
        return affect;
    }


    /// <summary>
    /// ---Существа со слабостью наносят каждой атакой - x урона․
    /// ---Примичение.---Уменьшается на 1 каждый раз когда наносишь урон․ 
    /// </summary>
    public static Affect Weakness(int weaknesStack)//Patient Affect //стак слабости
    {
        ResetAffect();

        affect.inPhobia.OnStepStart = () => Phobia.instance.AddWeakness(weaknesStack);
        affect.inPhobia.OnDefense = () => Phobia.instance.AddWeakness(-1);
        return affect;
    }


    /// <summary>
    /// ---Снижает урон на свое количество.
    /// ---Примичение.---Полностью пропадает в начале хода.
    /// </summary>
    public static Affect Block(float block)//??
    {
        ResetAffect();
        affect.inPhobia.OnStepStart = () => Patient.instance.AddBlock(block);
        affect.inPhobia.OnTurnEnd = () => Patient.instance.AddBlock(-block);
        return affect;
    }


    /// <summary>
    /// ---Снижает урон на свое количество.
    /// ---Примичение.---Снижается на 1 каждый раз, когда получаешь урон по хп.
    /// </summary>
    public static Affect Armor(float block)
    {
        ResetAffect();
        affect.inPhobia.OnTurnStart = () => Patient.instance.AddBlock(block);
        affect.inPhobia.OnStepEnd = () => Patient.instance.AddBlock(-1);//
        return affect;
    }


    /// <summary>
    /// ---Карта пропадает из игры до конца боя.
    /// </summary>
    public static Affect Exhaust(Card card)
    {
        ResetAffect();
        affect.inPhobia.OnStepEnd = () => Patient.instance.RemoveCardFromDeck(card);
        return affect;
    }


    /// <summary>
    /// ---Увеличивает урон на x, где x - значение силы.
    /// </summary>
    public static Affect Power(float damage)
    {
        ResetAffect();
        affect.inPhobia.OnStepStart = () => Patient.instance.AttackForce += damage;
        return affect;
    }

    //Common
    private static void ResetAffect()
    {
        if (affect != null)
            affect = null;
        affect = new Affect();
    }
}
