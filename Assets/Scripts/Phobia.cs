using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Phobia : NPC
{
    public static Phobia instance;

    public int vulnerablityCount = 0;
    public int weaknessStack = 0;

    [SerializeField]
    private TMP_Text nextAction;
    [SerializeField]
    private TMP_Text healthTxtp;



    public void InitializePhobia()
    {
        MakeInstance();

        vulnerablityCount = 0;
        weaknessStack = 0;
    }



    private void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
