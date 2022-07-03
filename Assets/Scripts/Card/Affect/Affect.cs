using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
[CreateAssetMenu(fileName = "New Affect", menuName = "ScriptableObjects/Affect", order = 1)]
public class Affect : ScriptableObject
{
    public InPhobiaAction OnTurnStart;//The act before patient play first card
    public InPhobiaAction OnTurnEnd;//The act after patient play the last card
    public InPhobiaAction OnStepStart;//The act before Patient play card
    public InPhobiaAction OnStepEnd;//The act after Patient play card
    public InPhobiaAction OnAttack;//The act before Patient attack
    public InPhobiaAction OnDefense;//The act before patient attacted by enamy

    private string name;
    private int index;

    //public void Update()
    //{
    //    //Debug.Log($"<color=green>Affect: </color>Affect Update Started");

    //    name = "";
    //    index = 0;

    //    UpdateActionList(OnTurnStart);
    //    UpdateActionList(OnTurnEnd);
    //    UpdateActionList(OnStepStart);
    //    UpdateActionList(OnStepEnd);
    //    UpdateActionList(OnAttack);
    //    UpdateActionList(OnDefense);


    //    //Debug.Log($"<color=green>Affect: </color>Affect Updated {name}");
    //}

    //public void Clear()
    //{
    //    OnTurnStart.Clear();
    //    OnTurnEnd.Clear();
    //    OnStepStart.Clear();
    //    OnStepEnd.Clear();
    //    OnAttack.Clear();
    //    OnDefense.Clear();
    //}

    public void Invoke(InPhobiaEventType type)
    {
        switch (type)
        {
            case InPhobiaEventType.OnTurnStart:
                OnTurnStart.Invoke();
                break;
            case InPhobiaEventType.OnTurnEnd:
                OnTurnEnd.Invoke();
                break;
            case InPhobiaEventType.OnStepStart:
                OnStepStart.Invoke();
                break;
            case InPhobiaEventType.OnStepEnd:
                OnStepEnd.Invoke();
                break;
            case InPhobiaEventType.OnAttack:
                OnAttack.Invoke();
                break;
            case InPhobiaEventType.OnDefense:
                OnDefense.Invoke();
                break;
            default:
                break;
        }
    }

    private void UpdateActionList(List<InPhobiaAction> inPhobiaActions)
    {
        int length = inPhobiaActions.Count;

        //remove nulls
        for (int i = 0; i < length; i++)
        {
            if (string.IsNullOrEmpty(inPhobiaActions[i].id))
            {
                inPhobiaActions.RemoveAt(i);
                i--;
                length--;
            }
        }

        //remove invoked actions that not set to save
        for (int i = 0; i < length; i++)
        {
            //if (inPhobiaActions[i].Invoked)
            if (inPhobiaActions[i].invoked)
            {
                inPhobiaActions.RemoveAt(i);
                i--;
                length--;
            }
        }
        //remove similars
        for (int i = 0; i < length-1; i++)
        {
            for (int j = i+1; j < length; j++)
            {
                if(inPhobiaActions[i]== inPhobiaActions[j])
                {
                    inPhobiaActions.RemoveAt(i);
                    i--;
                    j--;
                    length--;
                }
            }
        }


        //Debug.Log($"<color=green>Affect: </color>{GetActionsListName(index)} Updated. Actions count {inPhobiaActions.Count}");
        index++;
        foreach (InPhobiaAction action in inPhobiaActions)//this is only for debug
        {
            //name += action.ID + " ";
            name += action.id + " ";
        }
    }


    private string GetActionsListName(int index)
    {
        switch (index)
        {
            case 0:
                return "OnTurnStart";
            case 1:
                return "OnTurnEnd";
            case 2:
                return "OnStepStart";
            case 3:
                return "OnStepEnd";
            case 4:
                return "OnAttack";
            case 5:
                return "OnDefense";
            default:
                return "Wrong index";
        }
    }
}
