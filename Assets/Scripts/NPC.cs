using UnityEngine;
using UnityEngine.Events;

public class NPC : InPhobiaBahaviour
{
    public string Name;
    public float Health;
    public float AttackForce;

    public void SetAttackForce(float force)
    {
        AttackForce = force;
        Debug.Log($"<color=teal>NPC:</color> attackForce =  {force}");
    }
}
