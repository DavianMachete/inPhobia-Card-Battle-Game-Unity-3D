using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

[Serializable]
public class Dialog 
{
    public string Persion;
    [TextArea()]
    public string text;

    public List<Question> questions;
    public UnityEvent onPointsHavePassedTheLimit;
    public UnityEvent onNext;
}


[Serializable]
public class Question
{
    public string question;
    public UnityEvent onQuestion;
}
