using UnityEngine.Events;

public static class Affects
{
    private static Affect affect;


    /// <summary>
    /// ---Существа с уязвимостью получают +50% урона․
    /// ---Примичение.---В конце хода снижается на 1․
    /// </summary>
    public static Affect Vulnerablity(Phobia phobia)//Patient Affect
    {
        ResetAffect();
        //$"Effect: Vulnerable creatures take +50% damage\nNote: Decrease by 1 at end of turn";
        affect.inPhobia.OnStepStart = () => phobia.vulnerablityCount++;
        affect.inPhobia.OnTurnEnd = () => //Decrease by 1 at end of turn
        {
            if (phobia.vulnerablityCount > 0)
            {
                phobia.vulnerablityCount--;
            }
        };
        return affect;
    }


    /// <summary>
    /// ---Существа со слабостью наносят каждой атакой - x урона․
    /// ---Примичение.---Уменьшается на 1 каждый раз когда наносишь урон․ 
    /// </summary>
    public static Affect Weakness(int weaknesStack, Phobia phobia)//Patient Affect //стак слабости
    {
        ResetAffect();

        affect.inPhobia.OnStepStart = () => phobia.weaknessStack += weaknesStack;

        affect.inPhobia.OnDefense = () =>
        {
            if (phobia.weaknessStack > 0)
            {
                phobia.weaknessStack--;
            }
        };
        return affect;
    }


    /// <summary>
    /// ---Снижает урон на свое количество.
    /// ---Примичение.---Полностью пропадает в начале хода.
    /// </summary>
    public static Affect Block(float block, Patient patient)//??
    {
        ResetAffect();
        affect.inPhobia.OnStepStart = () => patient.AddBlock(block);
        affect.inPhobia.OnTurnStart = () => patient.AddBlock(-block);
        return affect;
    }


    /// <summary>
    /// ---Снижает урон на свое количество.
    /// ---Примичение.---Снижается на 1 каждый раз, когда получаешь урон по хп.
    /// </summary>
    public static Affect Armor(float block, Phobia phobia)
    {
        ResetAffect();
        //affect.inPhobia.OnTurnStart = () => patient.AddBlock(block);
        affect.inPhobia.OnStepEnd = () => { if (block > 0) block--; };
        return affect;
    }


    /// <summary>
    /// ---Карта пропадает из игры до конца боя.
    /// </summary>
    public static Affect Exhaust(Patient patient,Card card)
    {
        ResetAffect();
        affect.inPhobia.OnStepEnd = () => patient.RemoveCardFromDeck(card);
        return affect;
    }


    /// <summary>
    /// ---Увеличивает урон на x, где x - значение силы.
    /// </summary>
    public static Affect Power(float damage, Patient patient)
    {
        ResetAffect();
        //affect.inPhobia.OnStep = () => patient.AttackForce += damage;
        return affect;
    }

    //Phobia

    /// <summary>
    /// PHOBIA---Увеличивает урон на x, где x - значение силы.
    /// </summary>
    public static Affect PhobiaAttack(float damage, Phobia phobia)
    {
        ResetAffect();
        //affect.inPhobia.OnStep = () => phobia.AttackForce = damage;
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
