using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[Serializable]
public class Diolog 
{
    public string Persion;
    public string text;
    public bool hasQuestion;
    public List<Question> questions;
}

[Serializable]
public class Question
{
    public string question;
    public UnityEvent onQuestion;
}
