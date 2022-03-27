using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Patient : NPC
{
    public Phobia enemy;

    private float block;
    private bool blockSaved;

    private bool blockGot;
    private bool attackWhenGetBlock;


    private void Start()
    {
        
    }

    public void PullCard(int count)
    {
        //need add functionality
    }

    public void SaveBlock() 
    {
        blockSaved = true;
    }

    public void ActivateAttackWhenGetBlock()
    {
        attackWhenGetBlock = true;
    }

    public void Attack(int damage, int countInStep = 1)
    {
        if (countInStep < 1)
        {
            countInStep = 1;
        }

        for (int i = 0; i < countInStep; i++)
        {
            OnEveryAttack.Invoke();
            enemy.OnEveryDefense.Invoke();
            enemy.Health -= damage;
        }
    }

    public void AddBlock(float block)
    {
        if (block < 0 && blockSaved)
            return;
        this.block += block;
        if (attackWhenGetBlock)
        {
            float newAttackForce = AttackForce;
            AttackForce = 5;
            //Attack();
            AttackForce = newAttackForce;
        }
    }
}
