using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[Serializable]
public class Affect
{
    public List<InPhobiaAction> OnTurnStart;//The act before patient play first card
    public List<InPhobiaAction> OnTurnEnd;//The act after patient play the last card
    public List<InPhobiaAction> OnStepStart;//The act before Patient play card
    public List<InPhobiaAction> OnStepEnd;//The act after Patient play card
    public List<InPhobiaAction> OnAttack;//The act before Patient attack
    public List<InPhobiaAction> OnDefense;//The act before patient attacted by enamy

    private string name;
    private int index;

    public Affect()
    {
        CheckForNulls();
    }

    public void Update()
    {
        //Debug.Log($"<color=green>Affect: </color>Affect Update Started");

        name = "";
        index = 0;

        UpdateActionList(OnTurnStart);
        UpdateActionList(OnTurnEnd);
        UpdateActionList(OnStepStart);
        UpdateActionList(OnStepEnd);
        UpdateActionList(OnAttack);
        UpdateActionList(OnDefense);


        //Debug.Log($"<color=green>Affect: </color>Affect Updated {name}");
    }

    public void Clear()
    {
        OnTurnStart.Clear();
        OnTurnEnd.Clear();
        OnStepStart.Clear();
        OnStepEnd.Clear();
        OnAttack.Clear();
        OnDefense.Clear();
    }

    public void Invoke(InPhobiaEventType type)
    {
        switch (type)
        {
            case InPhobiaEventType.OnTurnStart:
                Invoke(OnTurnStart);
                break;
            case InPhobiaEventType.OnTurnEnd:
                Invoke(OnTurnEnd);
                break;
            case InPhobiaEventType.OnStepStart:
                Invoke(OnStepStart);
                break;
            case InPhobiaEventType.OnStepEnd:
                Invoke(OnStepEnd);
                break;
            case InPhobiaEventType.OnAttack:
                Invoke(OnAttack);
                break;
            case InPhobiaEventType.OnDefense:
                Invoke(OnDefense);
                break;
            default:
                break;
        }
    }

    public void Invoke(List<InPhobiaAction> inPhobiaActions)
    {
        foreach (InPhobiaAction action in inPhobiaActions)
        {
            action.Invoke();
        }
    }

    private void UpdateActionList(List<InPhobiaAction> inPhobiaActions)
    {
        int length = inPhobiaActions.Count;

        //remove nulls
        for (int i = 0; i < length; i++)
        {
            //if (string.IsNullOrEmpty(inPhobiaActions[i].ID)) 
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
        //inPhobiaActions.Distinct().ToList();
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

    private void CheckForNulls()
    {
        if (OnTurnStart == null)
            OnTurnStart = new List<InPhobiaAction>();
        if (OnTurnEnd == null)
            OnTurnEnd = new List<InPhobiaAction>();
        if (OnStepStart == null)
            OnStepStart = new List<InPhobiaAction>();
        if (OnStepEnd == null)
            OnStepEnd = new List<InPhobiaAction>();
        if (OnAttack == null)
            OnAttack = new List<InPhobiaAction>();
        if (OnDefense == null)
            OnDefense = new List<InPhobiaAction>();
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

    public static Affect operator +(Affect a, Affect b)
    {
        a.OnTurnStart.AddRange(b.OnTurnStart);
        a.OnTurnEnd.AddRange(b.OnTurnEnd);
        a.OnStepStart.AddRange(b.OnStepStart);
        a.OnStepEnd.AddRange(b.OnStepEnd);
        a.OnAttack.AddRange(b.OnAttack);
        a.OnDefense.AddRange(b.OnDefense);

        return a;
    }
}
