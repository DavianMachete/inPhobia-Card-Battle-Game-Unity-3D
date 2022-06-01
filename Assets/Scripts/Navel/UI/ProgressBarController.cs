using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressBarController : MonoBehaviour
{
    //[SerializeField] private TMP_Text barText;
    //[SerializeField] private Image bar;

    [SerializeField] private List<Transform> pointsGO;

    //[SerializeField] private int maxPointsCount;

    private int maxPointCount;
    private int currentPointsCoint;

    private void Awake()
    {
        InitializeProgressBar();
    }

    private void InitializeProgressBar()
    {
        maxPointCount = pointsGO.Count;
        currentPointsCoint = pointsGO.Count;
    }

    public void AddPoint(int value)
    {
        currentPointsCoint += value;

        if (currentPointsCoint > maxPointCount)
        {
            currentPointsCoint = maxPointCount;
            return;
        }
        if (currentPointsCoint < 0)
        {
            currentPointsCoint = 0;
            return;
        }

        for (int i = 0; i < pointsGO.Count; i++)
        {
            if (i < currentPointsCoint)
                pointsGO[i].GetChild(0).gameObject.SetActive(true);
            else
                pointsGO[i].GetChild(0).gameObject.SetActive(false);
        }
    }
}
