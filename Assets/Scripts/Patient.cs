using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Patient : NPC
{
    public float block;


    private float prevBlock;
    private bool blockSaved;

    private void Start()
    {
        
    }

    public void SuggestPullCard(int count) 
    {

    }

    public void PullCard(int count)
    {

    }

    public void SaveBlock() 
    {
        block = prevBlock;
        blockSaved = true;
    }


}
