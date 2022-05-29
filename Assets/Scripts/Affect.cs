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

    public Affect()
    {
        CheckNullables();
    }

    public void Update()
    {
        OnTurnStart = Update(OnTurnStart);
        OnTurnEnd = Update(OnTurnEnd);
        OnStepStart = Update(OnStepStart);
        OnStepEnd = Update(OnStepEnd);
        OnAttack = Update(OnAttack);
        OnDefense = Update(OnDefense);
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

    public void Invoke(List<InPhobiaAction> inPhobiaActions)
    {
        foreach (InPhobiaAction action in inPhobiaActions)
        {
            action.Invoke();
        }
    }

    private List<InPhobiaAction> Update(List<InPhobiaAction> inPhobiaActions)
    {
        List<InPhobiaAction> newInPhobiaActions = new List<InPhobiaAction>();

        //remove nulls
        foreach (InPhobiaAction action in inPhobiaActions)
        {
            if (!string.IsNullOrEmpty(action.ID))
                newInPhobiaActions.Add(action);
        }
        inPhobiaActions = new List<InPhobiaAction>(newInPhobiaActions);
        newInPhobiaActions.Clear();


        //remove invoked actions that not set to save
        foreach (InPhobiaAction action in inPhobiaActions)
        {
            if (action.SaveAction || !action.Invoked)
                newInPhobiaActions.Add(action);
        }
        inPhobiaActions = new List<InPhobiaAction>(newInPhobiaActions);
        newInPhobiaActions.Clear();

        //remove similars
        newInPhobiaActions = inPhobiaActions.Distinct().ToList();

        return newInPhobiaActions;
    }

    private void CheckNullables()
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
