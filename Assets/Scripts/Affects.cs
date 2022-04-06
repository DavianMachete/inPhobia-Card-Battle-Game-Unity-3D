using UnityEngine.Events;

public static class Affects
{
    private static Affect affect;


    /// <summary>
    /// ---Существа с уязвимостью получают +50% урона․
    /// ---Примичение.---В конце хода снижается на 1․
    /// </summary>
    public static Affect Vulnerablity(Patient patient,Phobia phobia) 
    {
        ResetAffect();
        //$"Effect: Vulnerable creatures take +50% damage\nNote: Decrease by 1 at end of turn";
        affect.inPhobia.OnStepStart = () => //+50 % damage
        {
            if (phobia.vulnerablityCount>0)
            {
                patient.AttackForce += patient.AttackForce * 0.5f;
            }
        };
        affect.inPhobia.OnEveryStepEnd = () => //Decrease by 1 at end of turn
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
    public static Affect Weakness(int weaknesStack, Phobia phobia) //стак слабости
    {
        ResetAffect();
        affect.inPhobia.OnEveryStepStart = () =>
        {
            if (phobia.weaknessStack > 0)
            {
                phobia.weaknessStack = weaknesStack+1;
            }
        };
        affect.inPhobia.OnEveryAttack = () =>
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
        affect.inPhobia.OnStepStart = () => patient.AddBlock(-block);
        affect.inPhobia.OnStepEnd = () => patient.AddBlock(block);
        return affect;
    }


    /// <summary>
    /// ---Снижает урон на свое количество.
    /// ---Примичение.---Снижается на 1 каждый раз, когда получаешь урон по хп.
    /// </summary>
    public static Affect Armor(float block, Patient patient)
    {
        ResetAffect();
        affect.inPhobia.OnStepEnd = () => patient.AddBlock(block);
        affect.inPhobia.OnEveryDefense = () => { if (block > 0) block--; };
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
        affect.inPhobia.OnStepStart = () => patient.AttackForce += damage;
        return affect;
    }


    private static void ResetAffect()
    {
        if (affect != null)
            affect = null;
        affect = new Affect();
    }
}
