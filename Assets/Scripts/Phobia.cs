using System.Collections;
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

    [SerializeField] private PhobiaPhase phobiaPhase;
    [SerializeField] private float maxHealth;
    [SerializeField] private bool isFirstStepAtPhaseTwo = true;
    [SerializeField] private int attackCountInAStep = 0;


    public void InitializePhobia()
    {
        MakeInstance();

        isFirstStepAtPhaseTwo = true;
        phobiaPhase = PhobiaPhase.FirstPhase;
        Health = 120;
        maxHealth = Health;
        UpdateHealthBar();

        PrepareAttack();

        vulnerablityCount = 0;
        weaknessStack = 0;
    }

    public void StartTurn()
    {
        if (IStartTurnHelper == null)
        {
            IStartTurnHelper = StartCoroutine(IStartTurn());
        }
    }

    public void MakeTheDamage(float damage)
    {
        if (vulnerablityCount > 0)
        {
            damage += damage * 0.5f;
        }
        Health -= damage;
        UpdateHealthBar();
    }

    public void AddVulnerablity(int value)
    {
        if (vulnerablityCount <= 0)
        {
            Debug.Log($"<color=#ffa500ff>phobia's</color> vulnerablity count is less or equal to 0 ");
            vulnerablityCount = 0;
            return;
        }
        vulnerablityCount += value;
    }

    public bool IsPhobiaHaveVulnerablity()
    {
        if (vulnerablityCount > 0)
            return true;
        else
            return false;
    }

    public void AddWeakness(int value)
    {
        if (weaknessStack <= 0)
        {
            Debug.Log($"<color=#ffa500ff>phobia's</color> weakness stack is less or equal to 0 ");
            weaknessStack = 0;
            return;
        }
        weaknessStack += value;
    }



    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = Health / maxHealth;
        healthTxtp.text = Mathf.RoundToInt(Health).ToString();
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

    private void AttackATime()
    {
        Debug.Log($"<color=orange>PHOBIA: </color>Attackpatient with {AttackForce} attack force");
        Patient.instance.MakeTheDamage(AttackForce);
    }


    private Coroutine IStartTurnHelper;
    private IEnumerator IStartTurn()
    {
        Debug.Log($"<color=orange>Turn Started</color>");
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < attackCountInAStep; i++)
        {
            AttackATime();
            yield return new WaitForSeconds(0.5f);
        }
        IStartTurnHelper = null;
    }


    private void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
}
