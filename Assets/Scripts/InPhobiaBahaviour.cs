using UnityEngine;
using UnityEngine.Events;

public class InPhobiaBahaviour : MonoBehaviour
{
    public UnityAction OnTurnStart;//For example before Patient play the first card

    public UnityAction OnStepStart;//For example before Patient play any card

    public UnityAction OnStepEnd;//For example after Patient play any card

    public UnityAction OnTurnEnd;//For example after Patient play the last card

    public UnityAction OnAttack;//When NPC attacks enamy (Called before main action)

    public UnityAction OnDefense;//When NPC attacked(do defense) (Called before main action)


    public InPhobiaBahaviour(){}
    private InPhobiaBahaviour(
        UnityAction OnTurnStart, UnityAction OnTurnEnd,
        UnityAction OnStepStart, UnityAction OnStepEnd,
        UnityAction OnAttack, UnityAction OnDefense)
    {
        this.OnTurnStart = OnTurnStart;
        this.OnTurnEnd = OnTurnEnd;
        this.OnStepStart = OnStepStart;
        this.OnStepEnd = OnStepEnd;
        this.OnAttack = OnAttack;
        this.OnDefense = OnDefense;
    }

    public static InPhobiaBahaviour operator +(InPhobiaBahaviour first, InPhobiaBahaviour second) =>
        new InPhobiaBahaviour(
            first.OnTurnStart + second.OnTurnStart, first.OnTurnEnd + second.OnTurnEnd,
            first.OnStepStart + second.OnStepStart, first.OnStepEnd + second.OnStepEnd,
            first.OnAttack + second.OnAttack, first.OnDefense + second.OnDefense);
}
