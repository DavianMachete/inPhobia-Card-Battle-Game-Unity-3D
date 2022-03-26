using UnityEngine.Events;

public class Affect
{
    public UnityAction OnMainAction;
    public UnityAction OnAdditionalAction;


    public Affect(UnityAction OnMainAction, UnityAction OnAdditionalAction)
    {
        this.OnMainAction = OnMainAction;
        this.OnAdditionalAction = OnAdditionalAction;
    }

    private Affect(Affect affect, int multiplier)
    {
        for (int i = 0; i < multiplier; i++)
        {
            affect += affect;
        }
        this.OnMainAction = affect.OnMainAction;
        this.OnAdditionalAction = affect.OnAdditionalAction;
    }


    public static Affect operator +(Affect firstAffect, Affect secondAffect) =>
        new Affect(firstAffect.OnMainAction + secondAffect.OnMainAction, firstAffect.OnAdditionalAction + secondAffect.OnAdditionalAction);
    public static Affect operator *(Affect affect, int number) => 
        new Affect(affect, number);
    public static Affect operator *(int number, Affect affect) =>
        new Affect(affect, number);

}
