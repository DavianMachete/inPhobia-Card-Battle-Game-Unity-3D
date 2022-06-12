using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Phobia", menuName = "ScriptableObjects/Phobia", order = 1)]
public class Phobia : ScriptableObject
{
    public string Name;
    public Sprite image;
    public float maximumHealth;
    public float health;
    public float attackForce;
    public int attackCountInAStep;
    public int vulnerablityCount;
    public int weaknessStack;
    public int power = -1;
    public PhobiaPhase phobiaPhase;

    private bool isFirstStepAtPhaseTwo = false;


    public void Initialize()
    {
        power = -1;
        vulnerablityCount = 0;
        weaknessStack = 0;
        phobiaPhase = PhobiaPhase.FirstPhase;
        isFirstStepAtPhaseTwo = true;
        maximumHealth = 120;
        health = maximumHealth;
    }

    public void PrepareAttack()
    {
        int r = Random.Range(0, 4);
        if (phobiaPhase == PhobiaPhase.FirstPhase)
        {
            power++;
            switch (r)
            {
                case 0:
                    {
                        attackForce = 4;
                        attackCountInAStep = 6;
                    }
                    break;
                case 1:
                case 2:
                case 3:
                    {
                        attackForce = 20;
                        attackCountInAStep = 1;
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
                attackForce = 40;
                attackCountInAStep = 1;
                isFirstStepAtPhaseTwo = false;
            }
            else
            {
                switch (r)
                {
                    case 0:
                    case 1:
                        {
                            attackForce = 3;
                            attackCountInAStep = 10;
                        }
                        break;
                    case 2:
                    case 3:
                        {
                            attackForce = 18;
                            attackCountInAStep = 1;
                            CardManager.instance.AddPsychosisToPatient();
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
