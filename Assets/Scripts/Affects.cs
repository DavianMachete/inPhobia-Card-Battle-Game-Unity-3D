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
        affect.OnMainAction = () => //+50 % damage
        {
            if (phobia.hasVulnerablity)
            {
                patient.damage += patient.damage * 0.5f;
            }
        };
        affect.OnAdditionalAction = () => //Decrease by 1 at end of turn
        {
            //ЖЕНЯЯЯЯЯЯЯ
        };
        return affect;
    }

    /// <summary>
    /// ---Существа со слабостью наносят каждой атакой - x урона․
    /// ---Примичение.---Уменьшается на 1 каждый раз когда наносишь урон․ 
    /// </summary>
    public static Affect Weakness(int weaknesStack,Patient patient, Phobia phobia) //стак слабости
    {
        affect.OnMainAction = () => 
        {
            if (phobia.hasWeakness)
            {
                phobia.damage -= weaknesStack;
            }
        };
        affect.OnAdditionalAction = () => 
        {

        };
        return affect;
    }


    /// <summary>
    /// ---Снижает урон на свое количество.
    /// ---Примичение.---Полностью пропадает в начале хода.
    /// </summary>
    public static Affect Block(float block, Patient patient)//??
    {
        affect.OnMainAction = () =>
        {
            patient.block += block;
        };
        affect.OnAdditionalAction = () =>
        {
            patient.block -= block;
        };
        return affect;
    }


    /// <summary>
    /// ---Снижает урон на свое количество.
    /// ---Примичение.---Снижается на 1 каждый раз, когда получаешь урон по хп.
    /// </summary>
    public static Affect Armor(Patient patient, Phobia phobia)
    {
        affect.OnMainAction = () =>
        {

        };
        affect.OnAdditionalAction = () =>
        {

        };
        return affect;
    }


    /// <summary>
    /// ---Карта пропадает из игры до конца боя.
    /// </summary>
    public static Affect Exhaust(Patient patient, Phobia phobia)
    {
        affect.OnMainAction = () =>
        {

        };
        affect.OnAdditionalAction = () =>
        {

        };
        return affect;
    }


    /// <summary>
    /// ---Увеличивает урон на x, где x - значение силы.
    /// </summary>
    public static Affect Power(Patient patient, Phobia phobia)
    {
        affect.OnMainAction = () =>
        {

        };
        affect.OnAdditionalAction = () =>
        {

        };
        return affect;
    }

}
