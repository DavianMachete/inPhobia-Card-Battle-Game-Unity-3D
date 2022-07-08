public enum CardTypes
{
    Attack = 0,
    Skill = 1,
    Equipment = 2,
    Curse = 3
}

public enum Rarity
{
    Rare,
    Equipment,
    Common
}

public enum InPhobiaEventType
{
    OnTurnStart,
    OnTurnEnd,
    OnStepStart,
    OnStepEnd,
    OnAttack,
    OnDefense
}

public enum CardUIType
{
    TherapistCard,
    PatientCard,
    defaultCard
}
public enum ScreenPart
{
    PatientHand,
    Middle,
    Therapist
}

public enum CurveType
{
    Parabola,
    Bezier
}

public enum AffectType
{
    AddActionPoints =0,
    AddBlock =1,
    Armor,
    Attack,
    AttackOnDefense,
    Discard,
    DoubleNextAffect,
    DropKickWithoutAttack,
    Exhaust,
    GiveEnemyWeaknessOnHit,
    MultiplyBlock,
    Power,
    PullCard,
    SaveBlock,
    SteelBlock,
    Vulnerability,
    Weakness
}
