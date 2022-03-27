using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Patient : NPC
{
    private float block;
    private bool blockSaved;

    private bool blockGot;
    private bool attackWhenGetBlock;

    private void Start()
    {
        
    }

    public void SuggestPullCard(int count) 
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

    public void AddBlock(float block)
    {
        if (block < 0 && blockSaved)
            return;
        this.block += block;
        if (attackWhenGetBlock)
        {
            float newAttackForce = AttackForce;
            AttackForce = 5;
            Attack();
            AttackForce = newAttackForce;
        }
    }
}
