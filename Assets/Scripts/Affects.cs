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
                phobia.AttackForce -= weaknesStack;
            }
        };
        affect.inPhobia.OnEveryStepEnd = () =>
        {
            if (phobia.weaknessStack > 0)
            {
                phobia.AttackForce += weaknesStack;
                weaknesStack--;
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
        affect.inPhobia.OnStepEnd = () => patient.AddBlock(-block);
        return affect;
    }


    /// <summary>
    /// ---Снижает урон на свое количество.
    /// ---Примичение.---Снижается на 1 каждый раз, когда получаешь урон по хп.
    /// </summary>
    public static Affect Armor(Patient patient, Phobia phobia)
    {
        ResetAffect();
        return affect;
    }


    /// <summary>
    /// ---Карта пропадает из игры до конца боя.
    /// </summary>
    public static Affect Exhaust(Deck deck,Card card)
    {
        ResetAffect();
        affect.inPhobia.OnStepEnd = () => deck.RemoveCardFromDeck(card);
        return affect;
    }


    /// <summary>
    /// ---Увеличивает урон на x, где x - значение силы.
    /// </summary>
    public static Affect Power(Patient patient, Phobia phobia)
    {
        ResetAffect();
        return affect;
    }

    private static void ResetAffect()
    {
        affect.inPhobia.OnStepStart = null;
        affect.inPhobia.OnStepEnd = null;
        affect.inPhobia.OnEveryStepStart = null;
        affect.inPhobia.OnEveryStepEnd = null;
    }
}
