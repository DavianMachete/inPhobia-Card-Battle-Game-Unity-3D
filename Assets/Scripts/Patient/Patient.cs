using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Patient", menuName = "ScriptableObjects/Patient", order = 1)]
[Serializable]
public class Patient : ScriptableObject
{
    public string Name;
    public Sprite image;
    public float maximumHealth;
    public float health;
    public float attackForce;
    public int patientMaximumActionPoints;
    public int patientActionPoints;
    public Phobia phobia;

    [TextArea(20,200)]
    public string info;

    public void Initialize()
    {
        patientMaximumActionPoints = 3;
        patientActionPoints = 3;

        health = maximumHealth;
    }
}
