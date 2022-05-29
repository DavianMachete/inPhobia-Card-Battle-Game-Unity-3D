using UnityEngine.Events;
using System;
using UnityEngine;

[Serializable]
public class InPhobiaAction
{
    [SerializeField] private UnityAction onAction;
    [SerializeField] private bool saveAction;
    [SerializeField] private string id;

    private bool invoked;

    public InPhobiaAction(string id, UnityAction onAction, bool saveAction)
    {
        this.id = id;
        this.onAction = onAction;
        this.saveAction = saveAction;
    }


    public void Invoke()
    {
        onAction();
        invoked = true;
    }

    public string ID
    {
        get { return id; }
    }

    public UnityAction OnAction
    {
        get { return onAction; }
    }

    public bool Invoked
    {
        get { return invoked; }
    }

    public bool SaveAction
    {
        get { return saveAction; }
    }

    private bool EqualsP(InPhobiaAction obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (this.GetHashCode() != obj.GetHashCode())
        {
            return false;
        }

        System.Diagnostics.Debug.Assert(
            base.GetType() != typeof(object));

        if (!base.Equals(obj))
        {
            return false;
        }

        return (this.id.Equals(obj.id));
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (this.GetType() != obj.GetType())
        {
            return false;
        }
        return EqualsP((InPhobiaAction)obj);
    }
    public static bool operator ==(InPhobiaAction a, InPhobiaAction b)
    {
        if (a == null || b == null)
            return false;
        return a.id == b.id;
    }
    public static bool operator !=(InPhobiaAction a, InPhobiaAction b)
    {
        return a.id != b.id;
    }
    public override int GetHashCode()
    {
        char[] idc = id.ToCharArray();
        int intId = 0;
        for (int i = 0; i < idc.Length; i++)
        {
            intId += idc[i];
        }
        return intId;
    }
}

