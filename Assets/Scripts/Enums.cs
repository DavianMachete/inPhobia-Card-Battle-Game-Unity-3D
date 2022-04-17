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
    OnStepStart,
    OnStepEnd,
    OnEveryStepStart,
    OnEveryStepEnd,
    OnEnemyStepStart,
    OnEnemyStepEnd,
    OnEnemyEveryStepStart,
    OnEnemyEveryStepEnd,
    OnEveryAttack,
    OnEveryDefense
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

public enum PhobiaPhase
{
    FirstPhase,
    SecondPhase
}
