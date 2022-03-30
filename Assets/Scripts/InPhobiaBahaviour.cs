using UnityEngine;
using UnityEngine.Events;

public class InPhobiaBahaviour : MonoBehaviour
{
    public UnityAction OnStepStart;
    public UnityAction OnStepEnd;

    public UnityAction OnEveryStepStart;
    public UnityAction OnEveryStepEnd;

    public UnityAction OnEnemyStepStart;
    public UnityAction OnEnemyStepEnd;

    public UnityAction OnEnemyEveryStepStart;
    public UnityAction OnEnemyEveryStepEnd;

    public UnityAction OnEveryAttack;//happens before every Attack
    public UnityAction OnEveryDefense;//happens before every defence


    public InPhobiaBahaviour(){}
    private InPhobiaBahaviour(
        UnityAction OnStepStart, UnityAction OnStepEnd,
        UnityAction OnEveryStepStart, UnityAction OnEveryStepEnd,
        UnityAction OnEnemyStepStart, UnityAction OnEnemyStepEnd,
        UnityAction OnEnemyEveryStepStart, UnityAction OnEnemyEveryStepEnd,
        UnityAction OnEveryAttack, UnityAction OnEveryDefense)
    {
        this.OnStepStart = OnStepStart;
        this.OnStepEnd = OnStepEnd;
        this.OnEveryStepStart = OnEveryStepStart;
        this.OnEveryStepEnd = OnEveryStepEnd;

        this.OnEnemyStepStart = OnEnemyStepStart;
        this.OnEnemyStepEnd = OnEnemyStepEnd;
        this.OnEnemyEveryStepStart = OnEnemyEveryStepStart;
        this.OnEnemyEveryStepEnd = OnEnemyEveryStepEnd;

        this.OnEveryAttack = OnEveryAttack;
        this.OnEveryDefense = OnEveryDefense;
    }

    public static InPhobiaBahaviour operator +(InPhobiaBahaviour first, InPhobiaBahaviour second) =>
        new InPhobiaBahaviour(
            first.OnStepStart + second.OnStepStart, first.OnStepEnd + second.OnStepEnd,
            first.OnEveryStepStart + second.OnEveryStepStart, first.OnEveryStepEnd + second.OnEveryStepEnd,
            first.OnEnemyStepStart + second.OnEnemyStepStart, first.OnEnemyStepEnd + second.OnEnemyStepEnd,
            first.OnEnemyEveryStepStart + second.OnEnemyEveryStepStart, first.OnEnemyEveryStepEnd + second.OnEnemyEveryStepEnd,
            first.OnEveryAttack + second.OnEveryAttack, first.OnEveryDefense + second.OnEveryDefense);
}
