using UnityEngine;
using UnityEngine.Events;

public class InPhobiaBahaviour : MonoBehaviour
{
    public UnityAction OnTurnStart;//For example before Patient play the first card
    public bool saveOnTurnStart = false;

    public UnityAction OnTurnEnd;//For example after Patient play the last card
    public bool saveOnTurnEnd = false;

    public UnityAction OnStepStart;//For example before Patient play any card
    public bool saveOnStepStart = false;

    public UnityAction OnStepEnd;//For example after Patient play any card
    public bool saveOnStepEnd = false;

    public UnityAction OnAttack;//When NPC attacks enamy (Called before main action)
    public bool saveOnAttack = false;

    public UnityAction OnDefense;//When NPC attacked(do defense) (Called before main action)
    public bool saveOnDefense = false;




    public InPhobiaBahaviour(
        UnityAction OnTurnStart = null, //bool saveOnTurnStart = false,
        UnityAction OnTurnEnd = null, //bool saveOnTurnEnd = false,
        UnityAction OnStepStart = null, //bool saveOnStepStart = false,
        UnityAction OnStepEnd = null, //bool saveOnStepEnd = false,
        UnityAction OnAttack = null, //bool saveOnAttack = false,
        UnityAction OnDefense = null) //, bool saveOnDefense = false)
    {
        if (OnTurnStart != null)
        {
            //if (!saveOnTurnStart)
            //    this.OnTurnStart = null;
            this.OnTurnStart = OnTurnStart;
        }
        if (OnTurnEnd != null)
        {
            //if (!saveOnTurnEnd)
            //    this.OnTurnEnd = null;
            this.OnTurnEnd = OnTurnEnd;
        }
        if (OnStepStart != null)
        {
            //if (!saveOnStepStart)
            //    this.OnStepStart = null;
            this.OnStepStart = OnStepStart;
        }
        if (OnStepEnd != null)
        {
            //if (!saveOnStepEnd)
            //    this.OnStepEnd = null;
            this.OnStepEnd = OnStepEnd;
        }
        if (OnAttack != null)
        {
            //if (!saveOnAttack)
            //    this.OnAttack = null;
            this.OnAttack = OnAttack;
        }
        if (OnDefense != null)
        {
            //if (!saveOnDefense)
            //    this.OnDefense = null;
            this.OnDefense = OnDefense;
        }
    }

    public static InPhobiaBahaviour operator +(InPhobiaBahaviour first, InPhobiaBahaviour second) =>
        new InPhobiaBahaviour(
            first.OnTurnStart + second.OnTurnStart, //first.saveOnTurnStart || second.saveOnTurnStart,
            first.OnTurnEnd + second.OnTurnEnd, //first.saveOnTurnEnd || second.saveOnTurnEnd,
            first.OnStepStart + second.OnStepStart, //first.saveOnStepStart || second.saveOnStepStart,
            first.OnStepEnd + second.OnStepEnd, //first.saveOnStepEnd || second.saveOnStepEnd,
            first.OnAttack + second.OnAttack, //first.saveOnAttack || second.saveOnAttack,
            first.OnDefense + second.OnDefense);//, first.saveOnDefense || second.saveOnDefense);
}
