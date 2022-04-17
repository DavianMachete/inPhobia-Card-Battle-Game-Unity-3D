using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Phobia : NPC
{
    public static Phobia instance;

    public int vulnerablityCount = 0;
    public int weaknessStack = 0;
    public int power = 0;

    [SerializeField]
    private TMP_Text healthTxtp;

    [SerializeField]
    private Image healthBarImage;

    [SerializeField]
    private TMP_Text phobiaNextAction;

    private PhobiaPhase phobiaPhase;
    private float maxHealth;
    private bool isFirstStepAtPhaseTwo = true;
    private int attackCountInAStep = 0;


    public void InitializePhobia()
    {
        MakeInstance();
        isFirstStepAtPhaseTwo = true;
        phobiaPhase = PhobiaPhase.FirstPhase;
        Health = 120;
        maxHealth = Health;
        healthBarImage.fillAmount = Health / maxHealth;
        healthTxtp.text = Mathf.RoundToInt(Health).ToString();

        PrepareAttack();

        vulnerablityCount = 0;
        weaknessStack = 0;
    }


    private void PrepareAttack()
    {
        int r = Random.Range(0, 4);
        if(phobiaPhase == PhobiaPhase.FirstPhase)
        {
            power++;
            switch (r)
            {
                case 0:
                    {
                        AttackForce = 4;
                        attackCountInAStep = 6;
                        //меч и 10x3
                        phobiaNextAction.text = "sword + 6x4";
                    }
                    break;
                case 1:
                case 2:
                case 3:
                    {
                        AttackForce = 20;
                        attackCountInAStep = 1;
                        phobiaNextAction.text = "sword + 1x20";
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            if (isFirstStepAtPhaseTwo)
            {
                AttackForce = 40;
                attackCountInAStep = 1;
                isFirstStepAtPhaseTwo = false;
                phobiaNextAction.text = "sword + 1x40";
            }
            else
            {
                switch (r)
                {
                    case 0:
                    case 1:
                        {
                            AttackForce = 3;
                            attackCountInAStep = 10;
                            phobiaNextAction.text = "sword + 10x3";
                        }
                        break;
                    case 2:
                    case 3:
                        {
                            AttackForce = 18;
                            attackCountInAStep = 1;
                            //UIController.instance.AddPsychosisToPatient();
                            phobiaNextAction.text = "sword + 1x18 + ?";
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
