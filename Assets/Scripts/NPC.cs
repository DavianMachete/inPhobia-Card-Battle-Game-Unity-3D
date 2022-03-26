using UnityEngine;
using UnityEngine.Events;

public class NPC : MonoBehaviour
{
    public string Name = "George";
    public int _AP = 12;
    public float health = 800f;
    public float damage = 40f;

    public UnityAction OnDefense;
    public UnityAction AfterDefenses;

    public UnityAction OnAttack;
    public UnityAction AfterAttack;

    public UnityAction OnEveryStepStart;
    public UnityAction OnEveryStepEnd;
}
