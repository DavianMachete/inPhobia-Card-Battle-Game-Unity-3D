using UnityEngine;
using UnityEngine.Events;

public class InPhobiaBahaviour : MonoBehaviour
{
    public UnityAction OnStepStart;
    public UnityAction OnStepEnd;

    public UnityAction OnEveryStepStart;
    public UnityAction OnEveryStepEnd;


    private protected InPhobiaBahaviour(){}
    private InPhobiaBahaviour(
        UnityAction OnStepStart, UnityAction OnStepEnd,
        UnityAction OnEveryStepStart, UnityAction OnEveryStepEnd)
    {
        this.OnStepStart = OnStepStart;
        this.OnStepEnd = OnStepEnd;
        this.OnEveryStepStart = OnEveryStepStart;
        this.OnEveryStepEnd = OnEveryStepEnd;
    }

    public static InPhobiaBahaviour operator +(InPhobiaBahaviour first, InPhobiaBahaviour second) =>
        new InPhobiaBahaviour(
            first.OnStepStart + second.OnStepStart, first.OnStepEnd + second.OnStepEnd,
            first.OnEveryStepStart + second.OnEveryStepStart, first.OnEveryStepEnd + second.OnEveryStepEnd);
}
